using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Common.Tables
{
	class TableCellDataGrid
	{
		public TableCellDataGrid()
		{
			this.Data = new List<TableRowData>();
		}

		public bool IsInitializing { get; set; }

		public List<TableRowData> Data { get; set; }

		public TableCellData this[int y, int x]
		{
			get
			{
				return this.Data[y].TableCellDatas[x];
			}
			set
			{
				this.Data[y].TableCellDatas[x] = value;
			}
		}

		public static TableCellDataGrid CreateFromTable(Table table)
		{
			TableCellDataGrid tableCellDataGrid = new TableCellDataGrid();
			tableCellDataGrid.IsInitializing = true;
			int num = 0;
			foreach (TableRow tableRow in table.Rows)
			{
				int num2 = 0;
				foreach (TableCell tableCell in tableRow.Cells)
				{
					while (tableCellDataGrid.EnsureIndexExistsAndGetTableCellData(num, num2, tableRow).Type != TableCellType.Empty)
					{
						num2++;
					}
					tableCellDataGrid.EnsureIndexExistsAndSetTableCellData(num, num2, tableRow, new TableCellData(TableCellType.Cell, tableCell));
					for (int i = num + 1; i < num + tableCell.RowSpan; i++)
					{
						TableCell cell = TableCellDataGrid.CreateVerticallyMergedEmptyCell(tableCell, i);
						TableRow row = table.Rows[i];
						tableCellDataGrid.EnsureIndexExistsAndSetTableCellData(i, num2, row, new TableCellData(TableCellType.VerticallyMerged, cell));
					}
					for (int j = num; j < num + tableCell.RowSpan; j++)
					{
						for (int k = num2 + 1; k < num2 + tableCell.ColumnSpan; k++)
						{
							TableRow row2 = table.Rows[j];
							tableCellDataGrid.EnsureIndexExistsAndSetTableCellData(j, k, row2, new TableCellData(TableCellType.HorizontalyMerged, tableCell));
						}
					}
					num2++;
				}
				num++;
			}
			tableCellDataGrid.IsInitializing = false;
			return tableCellDataGrid;
		}

		public static Table FillTableFromTableCellsData(Table tableToFill, List<TableRowData> tableRowDatasList)
		{
			Guard.ThrowExceptionIfNull<List<TableRowData>>(tableRowDatasList, "tableCellDatasList");
			TableCellDataGrid tableCellDataGrid = new TableCellDataGrid();
			tableCellDataGrid.IsInitializing = true;
			int num = 0;
			foreach (TableRowData tableRowData in tableRowDatasList)
			{
				int num2 = 0;
				TableRow row = tableRowData.Row;
				tableToFill.Rows.Add(row);
				foreach (TableCellData tableCellData in tableRowData.TableCellDatas)
				{
					bool flag = false;
					while (tableCellDataGrid.EnsureIndexExistsAndGetTableCellData(num, num2, row).Type != TableCellType.Empty)
					{
						num2++;
					}
					int columnSpan = tableCellData.Cell.ColumnSpan;
					if (tableCellData.Type == TableCellType.VerticallyMerged && num > 0 && tableCellDataGrid.EnsureIndexExistsAndGetTableCellData(num - 1, num2, row).Type != TableCellType.Empty)
					{
						TableCellData tableCellData2 = tableCellDataGrid.EnsureIndexExistsAndGetTableCellData(num - 1, num2, row);
						TableCellDataGrid.AddCellsToGrid(tableCellDataGrid, row, ref num2, num, columnSpan, tableCellData2);
						int maxRowSpanForCell = TableCellDataGrid.GetMaxRowSpanForCell(tableCellDataGrid, row, num2, tableCellData2, tableRowDatasList.Count);
						if (tableCellData2.Cell.RowSpan < maxRowSpanForCell)
						{
							tableCellData2.Cell.RowSpan++;
						}
						flag = true;
					}
					else if (columnSpan > 1)
					{
						TableCellDataGrid.AddCellsToGrid(tableCellDataGrid, row, ref num2, num, columnSpan, tableCellData);
					}
					else if (tableCellData.Type == TableCellType.HorizontalyMerged && num2 > 0 && tableCellDataGrid.EnsureIndexExistsAndGetTableCellData(num, num2 - 1, row).Type != TableCellType.Empty)
					{
						TableCellData tableCellData3 = tableCellDataGrid.EnsureIndexExistsAndGetTableCellData(num, num2 - 1, row);
						TableCellDataGrid.AddCellsToGrid(tableCellDataGrid, row, ref num2, num, columnSpan, tableCellData3);
						tableCellData3.Cell.ColumnSpan++;
						flag = true;
					}
					else
					{
						TableCellDataGrid.AddCellsToGrid(tableCellDataGrid, row, ref num2, num, columnSpan, tableCellData);
					}
					if (!flag)
					{
						row.Cells.Add(tableCellData.Cell);
					}
				}
				num++;
			}
			return tableToFill;
		}

		static TableCell CreateVerticallyMergedEmptyCell(TableCell cell, int gridRowIndex)
		{
			TableCell tableCell = cell.Clone();
			tableCell.Blocks.Clear();
			tableCell.Blocks.AddParagraph();
			tableCell.GridColumnIndex = cell.GridColumnIndex;
			tableCell.GridRowIndex = gridRowIndex;
			tableCell.RowSpan = 1;
			return tableCell;
		}

		static int GetMaxRowSpanForCell(TableCellDataGrid tableCellDataGrid, TableRow currentRow, int currentColumnIndex, TableCellData tableCellData, int rowsCount)
		{
			bool condition = false;
			int num = 0;
			for (int i = 0; i < rowsCount; i++)
			{
				if (tableCellDataGrid.EnsureIndexExistsAndGetTableCellData(i, currentColumnIndex - tableCellData.Cell.ColumnSpan, currentRow) == tableCellData)
				{
					condition = true;
					num = i;
					break;
				}
			}
			Guard.ThrowExceptionIfFalse(condition, "The start index of the cell could not miss.");
			return rowsCount - num;
		}

		static void AddCellsToGrid(TableCellDataGrid tableCellDataGrid, TableRow currentRow, ref int currentColumnIndex, int currentRowIndex, int gridSpan, TableCellData tableCellData)
		{
			int num = currentColumnIndex + gridSpan;
			while (currentColumnIndex < num)
			{
				tableCellDataGrid.EnsureIndexExistsAndSetTableCellData(currentRowIndex, currentColumnIndex, currentRow, tableCellData);
				currentColumnIndex++;
			}
		}

		TableCellData EnsureIndexExistsAndGetTableCellData(int y, int x, TableRow row)
		{
			this.EnsureIndexExists(y, x, row);
			return this.Data[y].TableCellDatas[x];
		}

		void EnsureIndexExistsAndSetTableCellData(int y, int x, TableRow row, TableCellData value)
		{
			this.EnsureIndexExists(y, x, row);
			this.Data[y].TableCellDatas[x] = value;
		}

		void EnsureIndexExists(int y, int x, TableRow row)
		{
			while (y >= this.Data.Count)
			{
				this.Data.Add(new TableRowData(row, new List<TableCellData>()));
			}
			while (x >= this.Data[y].TableCellDatas.Count)
			{
				this.Data[y].TableCellDatas.Add(new TableCellData());
			}
		}
	}
}
