using System;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class FormulaCellValue : CellValueBase<RadExpression>, ICopyable<FormulaCellValue>
	{
		public override string RawValue
		{
			get
			{
				return this.valueAsString;
			}
		}

		public override CellValueType ValueType
		{
			get
			{
				return CellValueType.Formula;
			}
		}

		public override CellValueType ResultValueType
		{
			get
			{
				return FormulaCellValue.InterpretEmptyExpression(base.Value.GetValueAsConstantExpression()).GetCellValueType();
			}
		}

		internal int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		internal int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		internal bool IsSheetNameDependent
		{
			get
			{
				return this.inputStringCollection.IsSheetNameDependent;
			}
		}

		internal bool IsWorkbookNameDependent
		{
			get
			{
				return this.inputStringCollection.IsWorkbookNameDependent;
			}
		}

		internal bool IsCellsNameDependent
		{
			get
			{
				return this.inputStringCollection.IsCellsNameDependent;
			}
		}

		internal bool ContainsSpreadsheetName
		{
			get
			{
				return this.inputStringCollection.ContainsSpreadsheetName;
			}
		}

		internal FormulaCellValue(InputStringCollection translatableString, RadExpression value, CellIndex cellIndex)
			: this(translatableString, value, cellIndex.RowIndex, cellIndex.ColumnIndex)
		{
		}

		internal FormulaCellValue(InputStringCollection translatableString, RadExpression value, int rowIndex, int columnIndex)
			: base(value)
		{
			Guard.ThrowExceptionIfNull<InputStringCollection>(translatableString, "translatableString");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			this.inputStringCollection = translatableString;
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.valueAsString = SpreadsheetCultureHelper.PrepareFormulaValue(this.inputStringCollection.BuildString());
		}

		public override string GetResultValueAsString(CellValueFormat format)
		{
			return this.GetResultValueAsCellValue().GetResultValueAsString(format);
		}

		protected override string GetValueAsStringOverride(CellValueFormat format = null)
		{
			return this.RawValue;
		}

		public ICellValue GetResultValueAsCellValue()
		{
			ConstantExpression constantExpression = FormulaCellValue.InterpretEmptyExpression(base.Value.GetValueAsConstantExpression());
			if (constantExpression is NumberExpression)
			{
				return new NumberCellValue(constantExpression.NumberValue());
			}
			ErrorCellValue result;
			if (constantExpression is ErrorExpression && ErrorCellValue.TryGetErrorCellValue(constantExpression.GetValueAsString(), out result))
			{
				return result;
			}
			return new TextCellValue(constantExpression.GetValueAsString());
		}

		internal string GetExportString()
		{
			return this.inputStringCollection.BuildStringInCulture(SpreadsheetCultureHelper.InvariantSpreadsheetCultureInfo);
		}

		internal FormulaCellValue CloneAndTranslate(Worksheet targetWorksheet, CellIndex cellIndex, bool onOverflowStarOver = false)
		{
			return this.CloneAndTranslate(targetWorksheet, cellIndex.RowIndex, cellIndex.ColumnIndex, onOverflowStarOver);
		}

		internal FormulaCellValue CloneAndTranslate(Worksheet targetWorksheet, int targetRowIndex, int targetColumnIndex, bool onOverflowStartOver = false)
		{
			InputStringCollection inputStringCollection = this.inputStringCollection.Clone();
			ExpressionCloneAndTranslateContext context = new ExpressionCloneAndTranslateContext(targetWorksheet, targetRowIndex, targetColumnIndex, this.inputStringCollection, inputStringCollection, onOverflowStartOver);
			RadExpression value = base.Value.CloneAndTranslate(context);
			return new FormulaCellValue(inputStringCollection, value, targetRowIndex, targetColumnIndex);
		}

		internal FormulaCellValue CloneAndTranslate(Worksheet worksheet, int rowIndex, int columnIndex, CellRangeInsertedOrRemovedEventArgs eventArgs, bool onOverflowStarOver = false)
		{
			InputStringCollection inputStringCollection = this.inputStringCollection.Clone();
			bool isHorizontal = eventArgs.RangeType == RangeType.Columns || eventArgs.RangeType == RangeType.CellsInRow;
			ExpressionCloneAndTranslateContext context = new ExpressionCloneAndTranslateContext(worksheet, rowIndex, columnIndex, this.inputStringCollection, inputStringCollection, isHorizontal, eventArgs.Range, eventArgs.IsRemove, onOverflowStarOver);
			RadExpression radExpression = base.Value.CloneAndTranslate(context);
			if (radExpression.Equals(base.Value))
			{
				return this;
			}
			return new FormulaCellValue(inputStringCollection, radExpression, this.rowIndex, this.columnIndex);
		}

		internal void Translate(Worksheet worksheet)
		{
			ExpressionTranslateContext context = new ExpressionTranslateContext(worksheet);
			base.Value.Translate(context);
			this.valueAsString = SpreadsheetCultureHelper.PrepareFormulaValue(this.inputStringCollection.BuildString());
		}

		internal void Translate(Workbook workbook)
		{
			ExpressionTranslateContext context = new ExpressionTranslateContext(workbook);
			base.Value.Translate(context);
			this.valueAsString = SpreadsheetCultureHelper.PrepareFormulaValue(this.inputStringCollection.BuildString());
		}

		internal void Translate(string oldName, string newName)
		{
			ExpressionTranslateContext context = new ExpressionTranslateContext(oldName, newName);
			base.Value.Translate(context);
			this.valueAsString = SpreadsheetCultureHelper.PrepareFormulaValue(this.inputStringCollection.BuildString());
		}

		internal static ConstantExpression InterpretEmptyExpression(ConstantExpression expressionValue)
		{
			if (expressionValue is EmptyExpression)
			{
				return NumberExpression.Zero;
			}
			return expressionValue;
		}

		internal bool IsValueCyclicReferenceError()
		{
			return base.Value.GetValueAsConstantExpression().Equals(ErrorExpressions.CyclicReference);
		}

		FormulaCellValue ICopyable<FormulaCellValue>.Copy(CopyContext context)
		{
			string value = this.valueAsString.Replace(context.SourceWorksheet.Name, context.TargetWorksheet.Name);
			return (FormulaCellValue)CellValueFactory.CreateFormulaCellValue(value, context.TargetWorksheet, this.RowIndex, this.ColumnIndex);
		}

		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.Value.GetHashCode();
		}

		string valueAsString;

		readonly InputStringCollection inputStringCollection;

		readonly int rowIndex;

		readonly int columnIndex;
	}
}
