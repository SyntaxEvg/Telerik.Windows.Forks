using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class WorksheetCellIndex
	{
		public CellIndex CellIndex
		{
			get
			{
				return this.cellIndex;
			}
		}

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		public WorksheetCellIndex(Worksheet worksheet, int rowIndex, int columnIndex)
			: this(worksheet, new CellIndex(rowIndex, columnIndex))
		{
		}

		public WorksheetCellIndex(Worksheet worksheet, CellIndex cellIndex)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<Workbook>(worksheet.Workbook, "worksheet.Workbook");
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			this.worksheet = worksheet;
			this.cellIndex = cellIndex;
		}

		public override bool Equals(object obj)
		{
			WorksheetCellIndex worksheetCellIndex = obj as WorksheetCellIndex;
			return worksheetCellIndex != null && this.CellIndex.Equals(worksheetCellIndex.CellIndex) && this.Worksheet.Equals(worksheetCellIndex.Worksheet);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.CellIndex.GetHashCode(), this.Worksheet.GetHashCode());
		}

		readonly CellIndex cellIndex;

		readonly Worksheet worksheet;
	}
}
