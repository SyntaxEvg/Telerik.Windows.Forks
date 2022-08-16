using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.BorderEvaluation.GridItems;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.Model.BorderEvaluation
{
	class TableBorderGrid
	{
		public TableBorderGrid(Table table)
		{
			this.Table = table;
			this.isValid = false;
			this.tableBorderCalculator = new TableBordersCalculator(this.Table);
		}

		public Table Table { get; set; }

		TableBordersCalculator TableBorderCalculator
		{
			get
			{
				return this.tableBorderCalculator;
			}
		}

		VerticalBorderGridItem[,] VerticalBorders
		{
			get
			{
				return this.TableBorderCalculator.VerticalBorders;
			}
		}

		HorizontalBorderGridItem[,] HorisontalBorders
		{
			get
			{
				return this.TableBorderCalculator.HorisontalBorders;
			}
		}

		CrossBorderGridItem[,] CrossBorders
		{
			get
			{
				return this.TableBorderCalculator.CrossBorders;
			}
		}

		public VerticalBorderGridItem GetLeftVerticalBorder(int rowIndex, int columnIndex)
		{
			return this.VerticalBorders[rowIndex, columnIndex];
		}

		public HorizontalBorderGridItem GetTopHorizontalBorder(int rowIndex, int columnIndex)
		{
			return this.HorisontalBorders[rowIndex, columnIndex];
		}

		public CrossBorderGridItem GetTopLeftCrossBorder(int rowIndex, int columnIndex)
		{
			return this.CrossBorders[rowIndex, columnIndex];
		}

		public bool HasChanges()
		{
			if (this.tableFlowDirection != this.Table.FlowDirection)
			{
				return true;
			}
			if (this.hasCellCpacingHash != this.Table.HasCellSpacing)
			{
				return true;
			}
			if (this.tableBordersHash != this.Table.Borders.GetHashCode())
			{
				return true;
			}
			if (this.rowHashList == null)
			{
				return true;
			}
			using (IEnumerator<TableRow> enumerator = this.Table.Rows.GetEnumerator())
			{
				using (IEnumerator<TableRowHash> enumerator2 = this.rowHashList.GetEnumerator())
				{
					bool flag = enumerator.MoveNext();
					bool flag2 = enumerator2.MoveNext();
					while (flag && flag2)
					{
						if (enumerator.Current != enumerator2.Current.Row)
						{
							return true;
						}
						using (IEnumerator<TableCell> enumerator3 = enumerator.Current.Cells.GetEnumerator())
						{
							using (IEnumerator<TableCellHash> enumerator4 = enumerator2.Current.CellHashList.GetEnumerator())
							{
								bool flag3 = enumerator3.MoveNext();
								bool flag4 = enumerator4.MoveNext();
								while (flag3 && flag4)
								{
									if (enumerator3.Current.RowSpan != enumerator4.Current.RowSpan || enumerator3.Current.ColumnSpan != enumerator4.Current.ColumnSpan || enumerator3.Current.Borders.GetHashCode() != enumerator4.Current.BordersHash)
									{
										return true;
									}
									flag3 = enumerator3.MoveNext();
									flag4 = enumerator4.MoveNext();
								}
								if (flag3 || flag4)
								{
									return true;
								}
							}
						}
						flag = enumerator.MoveNext();
						flag2 = enumerator2.MoveNext();
					}
					if (flag || flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void CheckForChanges()
		{
			if (this.HasChanges())
			{
				this.Invalidate();
			}
		}

		public void Invalidate()
		{
			this.isValid = false;
		}

		internal void AssureValid()
		{
			if (!this.isValid)
			{
				this.RecalculateBordersGrid();
			}
		}

		void RecalculateBordersGrid()
		{
			this.ReCalculateHash();
			this.TableBorderCalculator.ClearCollections();
			if (!this.Table.HasCellSpacing)
			{
				this.TableBorderCalculator.RecalculateBorders();
			}
			this.isValid = true;
		}

		void ReCalculateHash()
		{
			this.hasCellCpacingHash = this.Table.HasCellSpacing;
			this.tableFlowDirection = this.Table.FlowDirection;
			this.tableBordersHash = this.Table.Borders.GetHashCode();
			this.rowHashList = new List<TableRowHash>();
			foreach (TableRow tableRow in this.Table.Rows)
			{
				TableRowHash tableRowHash = new TableRowHash
				{
					Row = tableRow,
					CellHashList = new List<TableCellHash>()
				};
				foreach (TableCell tableCell in tableRow.Cells)
				{
					tableRowHash.CellHashList.Add(new TableCellHash
					{
						BordersHash = tableCell.Borders.GetHashCode(),
						RowSpan = tableCell.RowSpan,
						ColumnSpan = tableCell.ColumnSpan
					});
				}
				this.rowHashList.Add(tableRowHash);
			}
		}

		readonly TableBordersCalculator tableBorderCalculator;

		List<TableRowHash> rowHashList;

		int tableBordersHash;

		bool hasCellCpacingHash;

		FlowDirection tableFlowDirection;

		bool isValid;
	}
}
