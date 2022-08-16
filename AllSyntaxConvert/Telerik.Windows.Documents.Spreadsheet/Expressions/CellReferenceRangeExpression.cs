using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class CellReferenceRangeExpression : RadExpression, IWeakEventListener
	{
		public CellReferenceRange CellReferenceRange
		{
			get
			{
				return this.firstCellReferenceRange;
			}
		}

		public ReadOnlyCollection<CellReferenceRange> CellReferenceRanges
		{
			get
			{
				return this.cellReferenceRanges;
			}
		}

		internal Worksheet Worksheet
		{
			get
			{
				if (!this.IsValid)
				{
					return null;
				}
				return this.firstCellReferenceRange.Worksheet;
			}
		}

		internal bool IsValid
		{
			get
			{
				return this.isValid && this.AreCellReferenceRangesInOneWorksheet();
			}
		}

		internal bool IsInRange
		{
			get
			{
				return this.isInRange;
			}
		}

		internal bool HasSingleRange
		{
			get
			{
				return this.hasSingleRange;
			}
		}

		internal ArrayExpression FirstRangeArray
		{
			get
			{
				ArrayExpression arrayExpression = (ArrayExpression)base.GetValue();
				return (ArrayExpression)arrayExpression.Value[0, 0];
			}
		}

		internal CellReferenceRangeExpression(Workbook workbook, string worksheetName, bool isWorksheetAbsolute, CellAreaReference cellReferenceRange)
			: this(new CellReferenceRange(workbook, worksheetName, isWorksheetAbsolute, cellReferenceRange))
		{
		}

		internal CellReferenceRangeExpression(CellReferenceRange cellReferenceRange)
			: this(new List<CellReferenceRange> { cellReferenceRange })
		{
		}

		internal CellReferenceRangeExpression(IEnumerable<CellReferenceRange> cellReferenceRanges)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<CellReferenceRange>>(cellReferenceRanges, "cellReferenceRanges");
			Guard.ThrowExceptionIfLessThan<int>(1, cellReferenceRanges.Count<CellReferenceRange>(), "cellReferenceRanges");
			this.cellReferenceRanges = new ReadOnlyCollection<CellReferenceRange>(cellReferenceRanges.ToList<CellReferenceRange>());
			this.firstCellReferenceRange = this.cellReferenceRanges[0];
			this.hasSingleRange = this.cellReferenceRanges.Count == 1;
			this.isValid = this.GetIsValid();
			this.isInRange = this.GetIsInRange();
			this.AttachWeekEventPropertyChanged();
		}

		protected override RadExpression GetValueOverride()
		{
			RadExpression[,] array = new RadExpression[1, this.cellReferenceRanges.Count];
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				RadExpression valueOfRange = this.GetValueOfRange(this.cellReferenceRanges[i]);
				array[0, i] = valueOfRange;
			}
			return new ArrayExpression(array);
		}

		internal RadExpression GetValueOfRange(CellReferenceRange range)
		{
			if (!range.IsValid)
			{
				return ErrorExpressions.ReferenceError;
			}
			int rowsCount = range.CellAreaReference.RowsCount;
			int columnsCount = range.CellAreaReference.ColumnsCount;
			RadExpression[,] array = new RadExpression[rowsCount, columnsCount];
			int actualRowIndex = range.CellAreaReference.FromCellReference.ActualRowIndex;
			int actualColumnIndex = range.CellAreaReference.FromCellReference.ActualColumnIndex;
			for (int i = 0; i < range.CellAreaReference.RowsCount; i++)
			{
				int j = 0;
				while (j < range.CellAreaReference.ColumnsCount)
				{
					long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(i + actualRowIndex, j + actualColumnIndex);
					ICellValue propertyValue = range.Worksheet.Cells.PropertyBag.GetPropertyValue<ICellValue>(CellPropertyDefinitions.ValueProperty, index);
					switch (propertyValue.ValueType)
					{
					case CellValueType.Empty:
						array[i, j] = EmptyExpression.Empty;
						break;
					case CellValueType.Number:
					{
						double value = ((NumberCellValue)propertyValue).Value;
						array[i, j] = NumberExpression.CreateValidNumberOrErrorExpression(value);
						break;
					}
					case CellValueType.Boolean:
					{
						bool value2 = ((BooleanCellValue)propertyValue).Value;
						array[i, j] = (value2 ? BooleanExpression.True : BooleanExpression.False);
						break;
					}
					case CellValueType.Formula:
					{
						RadExpression radExpression = this.InterpreteFormulaCellValue((FormulaCellValue)propertyValue);
						array[i, j] = radExpression;
						break;
					}
					case CellValueType.Text:
						array[i, j] = new StringExpression(propertyValue.GetValueAsString(CellValueFormat.GeneralFormat));
						break;
					case CellValueType.RichText:
						goto IL_174;
					case CellValueType.Error:
						array[i, j] = ErrorExpressions.FindErrorExpression(propertyValue.RawValue);
						break;
					default:
						goto IL_174;
					}
					j++;
					continue;
					IL_174:
					throw new NotImplementedException("TODO:");
				}
			}
			return new ArrayExpression(array);
		}

		public override bool Equals(object obj)
		{
			CellReferenceRangeExpression cellReferenceRangeExpression = obj as CellReferenceRangeExpression;
			if (cellReferenceRangeExpression == null || this.cellReferenceRanges.Count != cellReferenceRangeExpression.cellReferenceRanges.Count)
			{
				return false;
			}
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				if (!TelerikHelper.EqualsOfT<CellReferenceRange>(this.cellReferenceRanges[i], cellReferenceRangeExpression.cellReferenceRanges[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = this.cellReferenceRanges[0].GetHashCodeOrZero();
			for (int i = 1; i < this.cellReferenceRanges.Count; i++)
			{
				num = TelerikHelper.CombineHashCodes(num, this.cellReferenceRanges[i].GetHashCodeOrZero());
			}
			return num;
		}

		internal override bool SimilarEquals(object obj)
		{
			return this.Equals(obj);
		}

		RadExpression InterpreteFormulaCellValue(FormulaCellValue formulaCellValue)
		{
			RadExpression value = formulaCellValue.Value;
			this.DetachWeekEventValueInvalidated(value);
			this.AttachWeekEventValueInvalidated(value);
			RadExpression radExpression = value.GetValue();
			if (value is CellReferenceRangeExpression)
			{
				ArrayExpression arrayExpression = radExpression as ArrayExpression;
				if (arrayExpression != null && arrayExpression[0, 0] is EmptyExpression)
				{
					radExpression = NumberExpression.Zero;
				}
			}
			if (value is BinaryOperatorExpression<CellReferenceRangeExpression> && this.Equals(radExpression))
			{
				radExpression = ErrorExpressions.CyclicReference;
			}
			return radExpression;
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			if (context.IsInsertingRemovingCells)
			{
				for (int i = 0; i < this.cellReferenceRanges.Count; i++)
				{
					if (this.cellReferenceRanges[i].ShouldTranslateUponInsertRemoveCells(context))
					{
						return this.CloneAndTranslateInternal(context);
					}
				}
				return this;
			}
			return this.CloneAndTranslateInternal(context);
		}

		RadExpression CloneAndTranslateInternal(ExpressionCloneAndTranslateContext context)
		{
			CellReferenceRange[] array = new CellReferenceRange[this.cellReferenceRanges.Count];
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				array[i] = this.cellReferenceRanges[i].CloneAndTranslate(context);
			}
			CellReferenceRangeExpression cellReferenceRangeExpression = new CellReferenceRangeExpression(array);
			if (context.OldFormatCollection != null && context.NewFormatCollection != null)
			{
				for (int j = 0; j < context.OldFormatCollection.Count; j++)
				{
					CellReferenceInputString cellReferenceInputString = context.OldFormatCollection[j] as CellReferenceInputString;
					if (cellReferenceInputString != null && object.ReferenceEquals(this, cellReferenceInputString.Expression))
					{
						((CellReferenceInputString)context.NewFormatCollection[j]).Expression = cellReferenceRangeExpression;
					}
				}
			}
			return cellReferenceRangeExpression;
		}

		internal override void Translate(ExpressionTranslateContext context)
		{
			bool flag = false;
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				if (context.IsWorksheetRenamed && context.RenamedWorksheet.Equals(this.Worksheet))
				{
					this.cellReferenceRanges[i].Translate();
					flag = true;
				}
			}
			if (flag)
			{
				base.InvalidateValue();
			}
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return this.BuildToString();
		}

		string BuildToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				stringBuilder.Append(this.cellReferenceRanges[i].ToString());
				if (i < this.cellReferenceRanges.Count - 1)
				{
					stringBuilder.Append(FormatHelper.DefaultSpreadsheetCulture.ListSeparator);
				}
			}
			return stringBuilder.ToString();
		}

		internal static bool AreCellReferenceRangesInOneWorksheet(CellReferenceRangeExpression left, CellReferenceRangeExpression right)
		{
			return left.AreCellReferenceRangesInOneWorksheet() && right.AreCellReferenceRangesInOneWorksheet() && TelerikHelper.EqualsOfT<Worksheet>(left.firstCellReferenceRange.Worksheet, right.firstCellReferenceRange.Worksheet);
		}

		internal static bool TryCreateIntersection(CellReferenceRangeExpression left, CellReferenceRangeExpression right, out CellReferenceRangeExpression intersection)
		{
			intersection = null;
			if (!left.IsValid || !right.IsValid || !CellReferenceRangeExpression.AreCellReferenceRangesInOneWorksheet(left, right))
			{
				return false;
			}
			List<CellReferenceRange> list = new List<CellReferenceRange>();
			for (int i = 0; i < left.cellReferenceRanges.Count; i++)
			{
				for (int j = 0; j < right.cellReferenceRanges.Count; j++)
				{
					CellAreaReference cellAreaReference = CellAreaReference.CreateIntersection(left.cellReferenceRanges[i].CellAreaReference, right.cellReferenceRanges[j].CellAreaReference);
					if (cellAreaReference != null)
					{
						list.Add(new CellReferenceRange(left.cellReferenceRanges[i].WorksheetReference, cellAreaReference));
					}
				}
			}
			if (list.Count > 0)
			{
				intersection = new CellReferenceRangeExpression(list);
				return true;
			}
			return false;
		}

		internal static bool TryCreateUnion(CellReferenceRangeExpression left, CellReferenceRangeExpression right, out CellReferenceRangeExpression union)
		{
			union = null;
			if (!left.IsValid || !right.IsValid)
			{
				return false;
			}
			if (left.firstCellReferenceRange.IsInRange && right.firstCellReferenceRange.IsInRange && CellReferenceRangeExpression.AreCellReferenceRangesInOneWorksheet(left, right))
			{
				List<CellReferenceRange> list = new List<CellReferenceRange>();
				list.AddRange(left.cellReferenceRanges);
				list.AddRange(right.cellReferenceRanges);
				union = new CellReferenceRangeExpression(list);
				return true;
			}
			return false;
		}

		internal static bool TryCreateRange(CellReferenceRangeExpression left, CellReferenceRangeExpression right, out CellReferenceRangeExpression range)
		{
			range = null;
			if (!left.IsValid || !right.IsValid)
			{
				return false;
			}
			if (!CellReferenceRangeExpression.AreCellReferenceRangesInOneWorksheet(left, right) && (!left.firstCellReferenceRange.IsWorksheetAbsolute || right.firstCellReferenceRange.IsWorksheetAbsolute))
			{
				return false;
			}
			CellAreaReference rangeArea = CellReferenceRangeExpression.GetRangeArea(left, right);
			CellReferenceRange cellReferenceRange = new CellReferenceRange(left.firstCellReferenceRange.Workbook, left.firstCellReferenceRange.WorksheetName, left.firstCellReferenceRange.IsWorksheetAbsolute, rangeArea);
			range = new CellReferenceRangeExpression(cellReferenceRange);
			return true;
		}

		internal static CellReferenceRangeExpression CreateRangeWithSpecificDimensions(CellReferenceRangeExpression originalRange, int rowsCount, int columnsCount)
		{
			Guard.ThrowExceptionIfLessThan<int>(1, rowsCount, "rowsCount");
			Guard.ThrowExceptionIfLessThan<int>(1, columnsCount, "columnsCount");
			CellReference fromCellReference = originalRange.CellReferenceRange.FromCellReference;
			int targetIndex = System.Math.Min(fromCellReference.ActualRowIndex + rowsCount - 1, SpreadsheetDefaultValues.RowCount);
			int targetIndex2 = System.Math.Min(fromCellReference.ActualColumnIndex + columnsCount - 1, SpreadsheetDefaultValues.ColumnCount);
			CellReference toCellReference = new CellReference(RowColumnReference.CreateAbsoluteRowColumnReference(targetIndex), RowColumnReference.CreateAbsoluteRowColumnReference(targetIndex2));
			CellAreaReference cellReferenceRange = new CellAreaReference(fromCellReference, toCellReference, false);
			return new CellReferenceRangeExpression(originalRange.cellReferenceRanges[0].Workbook, originalRange.cellReferenceRanges[0].WorksheetName, originalRange.cellReferenceRanges[0].IsWorksheetAbsolute, cellReferenceRange);
		}

		void AttachWeekEventPropertyChanged()
		{
			if (this.IsValid)
			{
				CellsPropertyBagPropertyChangedWeakEventManager.AddListener(this.Worksheet.Cells.PropertyBag, this);
			}
		}

		void DetachWeakEventPropertyChanged()
		{
			if (this.IsValid)
			{
				CellsPropertyBagPropertyChangedWeakEventManager.RemoveListener(this.Worksheet.Cells.PropertyBag, this);
			}
		}

		~CellReferenceRangeExpression()
		{
			this.DetachWeakEventPropertyChanged();
		}

		void AttachWeekEventValueInvalidated(RadExpression expression)
		{
			RadExpressionValueChangedWeakEventManager.AddListener(expression, this);
		}

		void DetachWeekEventValueInvalidated(RadExpression expression)
		{
			RadExpressionValueChangedWeakEventManager.RemoveListener(expression, this);
		}

		public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			if (managerType == typeof(RadExpressionValueChangedWeakEventManager))
			{
				this.ExpressionCache_ValueInvalidated(sender, e);
				return true;
			}
			if (managerType == typeof(CellsPropertyBagPropertyChangedWeakEventManager))
			{
				this.Properties_PropertyChanged(sender, (CellPropertyChangedEventArgs)e);
				return true;
			}
			return false;
		}

		void ExpressionCache_ValueInvalidated(object sender, EventArgs e)
		{
			base.InvalidateValue();
		}

		void Properties_PropertyChanged(object sender, CellPropertyChangedEventArgs e)
		{
			if (e.Property != CellPropertyDefinitions.ValueProperty || !this.IsValid)
			{
				return;
			}
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				CellReference fromCellReference = this.cellReferenceRanges[i].FromCellReference;
				CellReference toCellReference = this.cellReferenceRanges[i].ToCellReference;
				int actualRowIndex = fromCellReference.ActualRowIndex;
				int actualColumnIndex = fromCellReference.ActualColumnIndex;
				int actualRowIndex2 = toCellReference.ActualRowIndex;
				int actualColumnIndex2 = toCellReference.ActualColumnIndex;
				if (e.CellRange.IntersectsWith(actualRowIndex, actualColumnIndex, actualRowIndex2, actualColumnIndex2))
				{
					base.InvalidateValue();
					return;
				}
			}
		}

		bool AreCellReferenceRangesInOneWorksheet()
		{
			if (this.cellReferenceRanges.Count < 2)
			{
				return true;
			}
			Worksheet worksheet = this.firstCellReferenceRange.Worksheet;
			for (int i = 1; i < this.cellReferenceRanges.Count; i++)
			{
				if (!TelerikHelper.EqualsOfT<Worksheet>(worksheet, this.cellReferenceRanges[i].Worksheet))
				{
					return false;
				}
			}
			return true;
		}

		bool GetIsInRange()
		{
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				if (!this.cellReferenceRanges[i].IsInRange)
				{
					return false;
				}
			}
			return true;
		}

		bool GetIsValid()
		{
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				if (!this.cellReferenceRanges[i].IsValid)
				{
					return false;
				}
			}
			return true;
		}

		CellReference GetTopLeftCellReference()
		{
			RowColumnReference rowReference = this.cellReferenceRanges[0].CellAreaReference.FromCellReference.RowReference;
			RowColumnReference columnReference = this.cellReferenceRanges[0].CellAreaReference.FromCellReference.ColumnReference;
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				if (rowReference.ActualIndex > this.cellReferenceRanges[i].CellAreaReference.FromCellReference.ActualRowIndex)
				{
					rowReference = this.cellReferenceRanges[i].CellAreaReference.FromCellReference.RowReference;
				}
				if (columnReference.ActualIndex > this.cellReferenceRanges[i].CellAreaReference.FromCellReference.ActualColumnIndex)
				{
					columnReference = this.cellReferenceRanges[i].CellAreaReference.FromCellReference.ColumnReference;
				}
			}
			return new CellReference(rowReference, columnReference);
		}

		CellReference GetBottomRightCellReference()
		{
			RowColumnReference rowReference = this.cellReferenceRanges[0].CellAreaReference.ToCellReference.RowReference;
			RowColumnReference columnReference = this.cellReferenceRanges[0].CellAreaReference.ToCellReference.ColumnReference;
			for (int i = 0; i < this.cellReferenceRanges.Count; i++)
			{
				if (rowReference.ActualIndex < this.cellReferenceRanges[i].CellAreaReference.ToCellReference.ActualRowIndex)
				{
					rowReference = this.cellReferenceRanges[i].CellAreaReference.ToCellReference.RowReference;
				}
				if (columnReference.ActualIndex < this.cellReferenceRanges[i].CellAreaReference.ToCellReference.ActualColumnIndex)
				{
					columnReference = this.cellReferenceRanges[i].CellAreaReference.ToCellReference.ColumnReference;
				}
			}
			return new CellReference(rowReference, columnReference);
		}

		static CellAreaReference GetRangeArea(CellReferenceRangeExpression left, CellReferenceRangeExpression right)
		{
			CellReference topLeftCellReference = left.GetTopLeftCellReference();
			CellReference bottomRightCellReference = left.GetBottomRightCellReference();
			CellReference topLeftCellReference2 = right.GetTopLeftCellReference();
			CellReference bottomRightCellReference2 = right.GetBottomRightCellReference();
			CellReference topLeftCellReference3 = CellReference.GetTopLeftCellReference(topLeftCellReference, topLeftCellReference2);
			CellReference bottomRightCellReference3 = CellReference.GetBottomRightCellReference(bottomRightCellReference, bottomRightCellReference2);
			return new CellAreaReference(topLeftCellReference3, bottomRightCellReference3, false);
		}

		readonly ReadOnlyCollection<CellReferenceRange> cellReferenceRanges;

		readonly CellReferenceRange firstCellReferenceRange;

		readonly bool isValid;

		readonly bool isInRange;

		readonly bool hasSingleRange;
	}
}
