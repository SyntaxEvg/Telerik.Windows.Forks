using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Common.Tables;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types
{
	class TableImporter
	{
		public Table FillTableFromTableCellsData(Table tableToFill, List<TableRowData> tableRowDatasList)
		{
			Guard.ThrowExceptionIfNull<List<TableRowData>>(tableRowDatasList, "tableCellDatasList");
			this.tableCellDataGrid = new TableCellDataGrid();
			this.tableCellDataGrid.IsInitializing = true;
			this.currentRowIndex = 0;
			foreach (TableRowData tableRowData in tableRowDatasList)
			{
				this.currentColumnIndex = 0;
				TableRow row = tableRowData.Row;
				tableToFill.Rows.Add(row);
				foreach (TableCellData tableCellData in tableRowData.TableCellDatas)
				{
					bool flag = false;
					while (this.tableCellDataGrid[this.currentRowIndex, this.currentColumnIndex].Type != TableCellType.Empty)
					{
						this.currentColumnIndex++;
					}
					int columnSpan = tableCellData.Cell.ColumnSpan;
					if (tableCellData.Type == TableCellType.VerticallyMerged && this.currentRowIndex > 0 && this.tableCellDataGrid[this.currentRowIndex - 1, this.currentColumnIndex].Type != TableCellType.Empty)
					{
						TableCellData tableCellData2 = this.tableCellDataGrid[this.currentRowIndex - 1, this.currentColumnIndex];
						this.AddCellsToGrid(columnSpan, tableCellData2);
						int maxRowSpanForCell = this.GetMaxRowSpanForCell(tableCellData2, tableRowDatasList.Count);
						if (tableCellData2.Cell.RowSpan < maxRowSpanForCell)
						{
							tableCellData2.Cell.RowSpan++;
						}
						flag = true;
					}
					else if (columnSpan > 1)
					{
						this.AddCellsToGrid(columnSpan, tableCellData);
					}
					else if (tableCellData.Type == TableCellType.HorizontalyMerged && this.currentColumnIndex > 0 && this.tableCellDataGrid[this.currentRowIndex, this.currentColumnIndex - 1].Type != TableCellType.Empty)
					{
						TableCellData tableCellData3 = this.tableCellDataGrid[this.currentRowIndex, this.currentColumnIndex - 1];
						this.AddCellsToGrid(columnSpan, tableCellData3);
						tableCellData3.Cell.ColumnSpan++;
						flag = true;
					}
					else
					{
						this.AddCellsToGrid(columnSpan, tableCellData);
					}
					if (!flag)
					{
						row.Cells.Add(tableCellData.Cell);
					}
				}
				this.currentRowIndex++;
			}
			return tableToFill;
		}

		int GetMaxRowSpanForCell(TableCellData tableCellData, int rowsCount)
		{
			bool condition = false;
			int num = 0;
			for (int i = 0; i < rowsCount; i++)
			{
				if (this.tableCellDataGrid[i, this.currentColumnIndex - tableCellData.Cell.ColumnSpan] == tableCellData)
				{
					condition = true;
					num = i;
					break;
				}
			}
			Guard.ThrowExceptionIfFalse(condition, "The start index of the cell could not miss.");
			return rowsCount - num;
		}

		void AddCellsToGrid(int gridSpan, TableCellData tableCellData)
		{
			int num = this.currentColumnIndex + gridSpan;
			while (this.currentColumnIndex < num)
			{
				this.tableCellDataGrid[this.currentRowIndex, this.currentColumnIndex] = tableCellData;
				this.currentColumnIndex++;
			}
		}

		int currentRowIndex;

		int currentColumnIndex;

		TableCellDataGrid tableCellDataGrid;
	}
}
