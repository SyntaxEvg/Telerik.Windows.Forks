using System;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CellReferenceRange
	{
		public CellReference FromCellReference
		{
			get
			{
				return this.cellAreaReference.FromCellReference;
			}
		}

		public CellReference ToCellReference
		{
			get
			{
				return this.cellAreaReference.ToCellReference;
			}
		}

		public int RowsCount
		{
			get
			{
				return this.cellAreaReference.RowsCount;
			}
		}

		public int ColumnsCount
		{
			get
			{
				return this.cellAreaReference.ColumnsCount;
			}
		}

		public bool IsInRange
		{
			get
			{
				return this.cellAreaReference != null && this.cellAreaReference.IsValid;
			}
		}

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheetReference.Worksheet;
			}
		}

		internal CellAreaReference CellAreaReference
		{
			get
			{
				return this.cellAreaReference;
			}
		}

		internal bool IsWorksheetAbsolute
		{
			get
			{
				return this.worksheetReference.IsWorksheetAbsolute;
			}
		}

		internal bool IsValid
		{
			get
			{
				return this.IsInRange && this.Worksheet != null;
			}
		}

		internal string WorksheetName
		{
			get
			{
				return this.worksheetReference.WorksheetName;
			}
		}

		internal Workbook Workbook
		{
			get
			{
				return this.worksheetReference.Workbook;
			}
		}

		internal WorksheetReference WorksheetReference
		{
			get
			{
				return this.worksheetReference;
			}
		}

		internal CellReferenceRange(Workbook workbook, string worksheetName, bool isWorksheetAbsolute, CellAreaReference cellReferenceRange)
			: this(new WorksheetReference(workbook, worksheetName, isWorksheetAbsolute), cellReferenceRange)
		{
		}

		internal CellReferenceRange(WorksheetReference worksheetReference, CellAreaReference cellReferenceRange)
		{
			Guard.ThrowExceptionIfNull<WorksheetReference>(worksheetReference, "worksheetReference");
			this.worksheetReference = worksheetReference;
			this.cellAreaReference = cellReferenceRange;
		}

		public override bool Equals(object obj)
		{
			CellReferenceRange cellReferenceRange = obj as CellReferenceRange;
			return cellReferenceRange != null && TelerikHelper.EqualsOfT<WorksheetReference>(this.worksheetReference, cellReferenceRange.worksheetReference) && TelerikHelper.EqualsOfT<CellAreaReference>(this.cellAreaReference, cellReferenceRange.cellAreaReference);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.worksheetReference.GetHashCode(), this.cellAreaReference.GetHashCodeOrZero());
		}

		public override string ToString()
		{
			return this.BuildToString();
		}

		internal string BuildToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.worksheetReference.ToString());
			if (this.cellAreaReference == null || !this.cellAreaReference.IsValid)
			{
				stringBuilder.Append(ErrorExpressions.ReferenceError.Value);
			}
			else
			{
				stringBuilder.Append(this.cellAreaReference.ToString());
			}
			return stringBuilder.ToString();
		}

		internal CellReferenceRange CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			CellAreaReference cellAreaReference = null;
			if (this.cellAreaReference != null)
			{
				if (context.Range != null)
				{
					return this.CloneAndTranslateUponInsertRemoveCells(context);
				}
				cellAreaReference = this.cellAreaReference.CloneAndTranslate(context.RowIndex, context.ColumnIndex, context.OnOverflowStartOver);
				cellAreaReference = (cellAreaReference.IsValid ? cellAreaReference : null);
			}
			return new CellReferenceRange(this.worksheetReference.CloneAndTranslate(context.Worksheet), cellAreaReference);
		}

		internal void Translate()
		{
			this.worksheetReference.Translate();
		}

		internal bool ShouldTranslateUponInsertRemoveCells(ExpressionCloneAndTranslateContext context)
		{
			if (this.cellAreaReference != null)
			{
				if (context.IsHorizontal)
				{
					if (context.Range.FromIndex.RowIndex <= this.FromCellReference.ActualRowIndex && this.ToCellReference.ActualRowIndex <= context.Range.ToIndex.RowIndex && context.Range.FromIndex.ColumnIndex <= this.ToCellReference.ActualColumnIndex)
					{
						return true;
					}
					if (context.Range.FromIndex.RowIndex <= context.RowIndex && context.RowIndex <= context.Range.ToIndex.RowIndex && context.Range.FromIndex.ColumnIndex <= context.ColumnIndex)
					{
						return true;
					}
				}
				else
				{
					if (context.Range.FromIndex.ColumnIndex <= this.FromCellReference.ActualColumnIndex && this.ToCellReference.ActualColumnIndex <= context.Range.ToIndex.ColumnIndex && context.Range.FromIndex.RowIndex <= this.ToCellReference.ActualRowIndex)
					{
						return true;
					}
					if (context.Range.FromIndex.ColumnIndex <= context.ColumnIndex && context.ColumnIndex <= context.Range.ToIndex.ColumnIndex && context.Range.FromIndex.RowIndex <= context.RowIndex)
					{
						return true;
					}
				}
			}
			return false;
		}

		CellReferenceRange CloneAndTranslateUponInsertRemoveCells(ExpressionCloneAndTranslateContext context)
		{
			if (this.cellAreaReference != null)
			{
				bool translateTarget;
				bool translateSource;
				int rangeFromIndex;
				int rangeToIndex;
				int referenceFromIndex;
				int referenceToIndex;
				if (context.IsHorizontal)
				{
					translateTarget = context.Range.FromIndex.RowIndex <= this.FromCellReference.ActualRowIndex && this.ToCellReference.ActualRowIndex <= context.Range.ToIndex.RowIndex && context.Range.FromIndex.ColumnIndex <= this.ToCellReference.ActualColumnIndex;
					translateSource = context.Range.FromIndex.RowIndex <= context.RowIndex && context.RowIndex <= context.Range.ToIndex.RowIndex && context.Range.FromIndex.ColumnIndex <= context.ColumnIndex;
					rangeFromIndex = context.Range.FromIndex.ColumnIndex;
					rangeToIndex = context.Range.ToIndex.ColumnIndex;
					referenceFromIndex = this.FromCellReference.ActualColumnIndex;
					referenceToIndex = this.ToCellReference.ActualColumnIndex;
				}
				else
				{
					translateTarget = context.Range.FromIndex.ColumnIndex <= this.FromCellReference.ActualColumnIndex && this.ToCellReference.ActualColumnIndex <= context.Range.ToIndex.ColumnIndex && context.Range.FromIndex.RowIndex <= this.ToCellReference.ActualRowIndex;
					translateSource = context.Range.FromIndex.ColumnIndex <= context.ColumnIndex && context.ColumnIndex <= context.Range.ToIndex.ColumnIndex && context.Range.FromIndex.RowIndex <= context.RowIndex;
					rangeFromIndex = context.Range.FromIndex.RowIndex;
					rangeToIndex = context.Range.ToIndex.RowIndex;
					referenceFromIndex = this.FromCellReference.ActualRowIndex;
					referenceToIndex = this.ToCellReference.ActualRowIndex;
				}
				return new CellReferenceRange(this.worksheetReference, this.cellAreaReference.CloneAndTranslateUponInsertRemoveCells(context, rangeFromIndex, rangeToIndex, referenceFromIndex, referenceToIndex, translateSource, translateTarget));
			}
			return null;
		}

		readonly WorksheetReference worksheetReference;

		readonly CellAreaReference cellAreaReference;
	}
}
