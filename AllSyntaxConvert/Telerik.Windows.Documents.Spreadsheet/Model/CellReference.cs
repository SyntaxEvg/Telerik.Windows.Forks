using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CellReference
	{
		public int ActualRowIndex
		{
			get
			{
				return this.rowReference.ActualIndex;
			}
		}

		public int ActualColumnIndex
		{
			get
			{
				return this.columnReference.ActualIndex;
			}
		}

		public bool IsRowAbsolute
		{
			get
			{
				return this.RowReference.IsAbsolute;
			}
		}

		public bool IsColumnAbsolute
		{
			get
			{
				return this.ColumnReference.IsAbsolute;
			}
		}

		internal RowColumnReference RowReference
		{
			get
			{
				return this.rowReference;
			}
			set
			{
				this.rowReference = value;
			}
		}

		internal RowColumnReference ColumnReference
		{
			get
			{
				return this.columnReference;
			}
			set
			{
				this.columnReference = value;
			}
		}

		internal CellReference(RowColumnReference rowReference, RowColumnReference columnReference)
		{
			Guard.ThrowExceptionIfNull<RowColumnReference>(rowReference, "rowReference");
			Guard.ThrowExceptionIfNull<RowColumnReference>(columnReference, "columnReference");
			this.rowReference = rowReference;
			this.columnReference = columnReference;
		}

		public override bool Equals(object obj)
		{
			CellReference cellReference = obj as CellReference;
			return cellReference != null && this.RowReference.Equals(cellReference.RowReference) && this.ColumnReference.Equals(cellReference.ColumnReference);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.RowReference.GetHashCode(), this.ColumnReference.GetHashCode());
		}

		internal CellReference CloneAndTranslate(int rowIndex, int columnIndex, bool onOverflowStartOver)
		{
			return new CellReference(this.rowReference.CloneAndTranslate(rowIndex, onOverflowStartOver, SpreadsheetDefaultValues.RowCount), this.columnReference.CloneAndTranslate(columnIndex, onOverflowStartOver, SpreadsheetDefaultValues.ColumnCount));
		}

		internal CellReference CloneAndTranslateUponInsertRemoveCells(bool isHorizontal, int indexShift, int offsetShift, bool onOverflowStartOver)
		{
			if (isHorizontal)
			{
				return new CellReference(this.rowReference, this.columnReference.CloneAndTranslateUponInsertRemoveCells(indexShift, offsetShift, onOverflowStartOver, SpreadsheetDefaultValues.ColumnCount));
			}
			return new CellReference(this.rowReference.CloneAndTranslateUponInsertRemoveCells(indexShift, offsetShift, onOverflowStartOver, SpreadsheetDefaultValues.RowCount), this.columnReference);
		}

		internal static CellReference GetTopLeftCellReference(CellReference inputFromCellReference, CellReference inputToCellReference)
		{
			RowColumnReference rowColumnReference = inputFromCellReference.RowReference;
			if (inputFromCellReference.ActualRowIndex > inputToCellReference.ActualRowIndex)
			{
				rowColumnReference = inputToCellReference.RowReference;
			}
			RowColumnReference rowColumnReference2 = inputFromCellReference.ColumnReference;
			if (inputFromCellReference.ActualColumnIndex > inputToCellReference.ActualColumnIndex)
			{
				rowColumnReference2 = inputToCellReference.ColumnReference;
			}
			return new CellReference(rowColumnReference, rowColumnReference2);
		}

		internal static CellReference GetBottomRightCellReference(CellReference inputFromCellReference, CellReference inputToCellReference)
		{
			RowColumnReference rowColumnReference = inputToCellReference.RowReference;
			if (inputFromCellReference.ActualRowIndex > inputToCellReference.ActualRowIndex)
			{
				rowColumnReference = inputFromCellReference.RowReference;
			}
			RowColumnReference rowColumnReference2 = inputToCellReference.ColumnReference;
			if (inputFromCellReference.ActualColumnIndex > inputToCellReference.ActualColumnIndex)
			{
				rowColumnReference2 = inputFromCellReference.ColumnReference;
			}
			return new CellReference(rowColumnReference, rowColumnReference2);
		}

		internal static CellReference CreateFromCellName(string cellName, CellIndex cellIndex)
		{
			return CellReference.CreateFromCellName(cellName, cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		internal static CellReference CreateFromCellName(string cellName, int row, int column)
		{
			Guard.ThrowExceptionIfNull<string>(cellName, "cellName");
			Guard.ThrowExceptionIfInvalidRowIndex(row);
			Guard.ThrowExceptionIfInvalidColumnIndex(column);
			bool isAbsolute;
			int index;
			bool isAbsolute2;
			int index2;
			if (NameConverter.TryConvertCellNameToIndex(cellName, out isAbsolute, out index, out isAbsolute2, out index2))
			{
				RowColumnReference rowColumnReference = RowColumnReference.CreateRowColumnReference(isAbsolute, index, row, false, SpreadsheetDefaultValues.RowCount);
				RowColumnReference rowColumnReference2 = RowColumnReference.CreateRowColumnReference(isAbsolute2, index2, column, false, SpreadsheetDefaultValues.ColumnCount);
				return new CellReference(rowColumnReference, rowColumnReference2);
			}
			return null;
		}

		RowColumnReference rowReference;

		RowColumnReference columnReference;
	}
}
