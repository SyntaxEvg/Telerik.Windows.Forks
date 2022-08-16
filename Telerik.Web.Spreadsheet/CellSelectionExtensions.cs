using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Web.Spreadsheet
{
	public static class CellSelectionExtensions
	{
		public static CellSelection GetCellSelection(this Cells cells, string cellRangeRef)
		{
			string[] array = cellRangeRef.Split(new char[] { ':' });
			CellRange cellRange;
			if (array.Length == 1)
			{
				int rowIndex;
				int columnIndex;
				NameConverter.ConvertCellNameToIndex(cellRangeRef, out rowIndex, out columnIndex);
				CellIndex cellIndex = new CellIndex(rowIndex, columnIndex);
				cellRange = new CellRange(cellIndex, cellIndex);
			}
			else
			{
				int rowIndex2 = 0;
				int columnIndex2 = 0;
				int rowIndex;
				int columnIndex;
				bool flag;
				bool flag2;
				NameConverter.ConvertCellNameToIndex(array[0], out flag, out rowIndex, out flag2, out columnIndex);
				bool flag3;
				bool flag4;
				NameConverter.ConvertCellNameToIndex(array[1], out flag3, out rowIndex2, out flag4, out columnIndex2);
				cellRange = new CellRange(new CellIndex(rowIndex, columnIndex), new CellIndex(rowIndex2, columnIndex2));
			}
			return cells[cellRange];
		}
	}
}
