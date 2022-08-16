using System;
using Telerik.Windows.Documents.Flow.Model.BorderEvaluation.GridItems;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.Model.BorderEvaluation
{
	class TableBordersCalculator
	{
		internal TableBordersCalculator(Table table)
		{
			this.table = table;
		}

		internal VerticalBorderGridItem[,] VerticalBorders { get; set; }

		internal HorizontalBorderGridItem[,] HorisontalBorders { get; set; }

		internal CrossBorderGridItem[,] CrossBorders { get; set; }

		internal Table Table
		{
			get
			{
				return this.table;
			}
		}

		internal virtual void RecalculateBorders()
		{
			this.InitializeNoCellCpacing();
			this.CalculateHorizontalAndVerticalBorders();
			this.CalculateHorizontalAndVerticalInheritedBorders();
			this.MergeContinuesBorders();
			this.CalculateCrossBorders();
		}

		internal void ClearCollections()
		{
			this.VerticalBorders = null;
			this.HorisontalBorders = null;
			this.CrossBorders = null;
		}

		void InitializeNoCellCpacing()
		{
			int count = this.Table.Rows.Count;
			int gridColumnsCount = this.Table.GridColumnsCount;
			this.VerticalBorders = new VerticalBorderGridItem[count, gridColumnsCount + 1];
			for (int i = 0; i < this.Table.Rows.Count; i++)
			{
				for (int j = 0; j < gridColumnsCount + 1; j++)
				{
					this.VerticalBorders[i, j] = new VerticalBorderGridItem();
					this.VerticalBorders[i, j].ColumnIndex = j;
				}
			}
			this.HorisontalBorders = new HorizontalBorderGridItem[count + 1, gridColumnsCount];
			for (int k = 0; k < count + 1; k++)
			{
				for (int l = 0; l < gridColumnsCount; l++)
				{
					this.HorisontalBorders[k, l] = new HorizontalBorderGridItem();
					this.HorisontalBorders[k, l].RowIndex = k;
				}
			}
			this.CrossBorders = new CrossBorderGridItem[count + 1, gridColumnsCount + 1];
			for (int m = 0; m < count + 1; m++)
			{
				for (int n = 0; n < gridColumnsCount + 1; n++)
				{
					this.CrossBorders[m, n] = new CrossBorderGridItem();
					this.CrossBorders[m, n].RowIndex = m;
					this.CrossBorders[m, n].ColumnIndex = n;
				}
			}
		}

		void CalculateHorizontalAndVerticalBorders()
		{
			foreach (TableRow tableRow in this.Table.Rows)
			{
				foreach (TableCell tableCell in tableRow.Cells)
				{
					for (int i = tableCell.GridRowIndex; i < tableCell.GridRowIndex + tableCell.RowSpan; i++)
					{
						this.VerticalBorders[i, tableCell.GridColumnIndex].IsInsideCell = false;
						if (this.VerticalBorders[i, tableCell.GridColumnIndex].Border.CompareTo(tableCell.Borders.Left) < 0)
						{
							this.VerticalBorders[i, tableCell.GridColumnIndex].Border = tableCell.Borders.Left;
						}
						this.VerticalBorders[i, tableCell.GridColumnIndex + tableCell.ColumnSpan].IsInsideCell = false;
						if (this.VerticalBorders[i, tableCell.GridColumnIndex + tableCell.ColumnSpan].Border.CompareTo(tableCell.Borders.Right) < 0)
						{
							this.VerticalBorders[i, tableCell.GridColumnIndex + tableCell.ColumnSpan].Border = tableCell.Borders.Right;
						}
					}
					for (int j = tableCell.GridColumnIndex; j < tableCell.GridColumnIndex + tableCell.ColumnSpan; j++)
					{
						this.HorisontalBorders[tableCell.GridRowIndex, j].IsInsideCell = false;
						if (this.HorisontalBorders[tableCell.GridRowIndex, j].Border.CompareTo(tableCell.Borders.Top) < 0)
						{
							this.HorisontalBorders[tableCell.GridRowIndex, j].Border = tableCell.Borders.Top;
						}
						this.HorisontalBorders[tableCell.GridRowIndex + tableCell.RowSpan, j].IsInsideCell = false;
						if (this.HorisontalBorders[tableCell.GridRowIndex + tableCell.RowSpan, j].Border.CompareTo(tableCell.Borders.Bottom) < 0)
						{
							this.HorisontalBorders[tableCell.GridRowIndex + tableCell.RowSpan, j].Border = tableCell.Borders.Bottom;
						}
					}
				}
			}
		}

		void CalculateHorizontalAndVerticalInheritedBorders()
		{
			TableBorders borders = this.Table.Borders;
			Border border = new Border(BorderStyle.Inherit);
			int gridRowsCount = this.Table.GridRowsCount;
			int gridColumnsCount = this.Table.GridColumnsCount;
			for (int i = 0; i < gridRowsCount; i++)
			{
				if (this.VerticalBorders[i, 0].Border.Style == BorderStyle.Inherit)
				{
					this.VerticalBorders[i, 0].Border = borders.Left;
				}
				if (this.VerticalBorders[i, gridColumnsCount].Border.Style == BorderStyle.Inherit)
				{
					this.VerticalBorders[i, gridColumnsCount].Border = borders.Right;
				}
				for (int j = 1; j < gridColumnsCount; j++)
				{
					if (this.VerticalBorders[i, j].IsInsideCell)
					{
						this.VerticalBorders[i, j].Border = border;
					}
					else if (this.VerticalBorders[i, j].Border.Style == BorderStyle.Inherit)
					{
						this.VerticalBorders[i, j].Border = borders.InsideVertical;
					}
				}
			}
			for (int k = 0; k < gridColumnsCount; k++)
			{
				if (this.HorisontalBorders[0, k].Border.Style == BorderStyle.Inherit)
				{
					this.HorisontalBorders[0, k].Border = borders.Top;
				}
				if (this.HorisontalBorders[gridRowsCount, k].Border.Style == BorderStyle.Inherit)
				{
					this.HorisontalBorders[gridRowsCount, k].Border = borders.Bottom;
				}
				for (int l = 1; l < gridRowsCount; l++)
				{
					if (this.HorisontalBorders[l, k].IsInsideCell)
					{
						this.HorisontalBorders[l, k].Border = border;
					}
					else if (this.HorisontalBorders[l, k].Border.Style == BorderStyle.Inherit)
					{
						this.HorisontalBorders[l, k].Border = borders.InsideHorizontal;
					}
				}
			}
		}

		void MergeContinuesBorders()
		{
			for (int i = 0; i < this.Table.GridRowsCount + 1; i++)
			{
				if (this.Table.GridColumnsCount > 0)
				{
					HorizontalBorderGridItem horizontalBorderGridItem = this.HorisontalBorders[i, 0];
					horizontalBorderGridItem.ColumnIndex = 0;
					horizontalBorderGridItem.ColumnSpan = 1;
					for (int j = 1; j < this.Table.GridColumnsCount; j++)
					{
						if (this.HorisontalBorders[i, j].Border == horizontalBorderGridItem.Border)
						{
							this.HorisontalBorders[i, j] = horizontalBorderGridItem;
							horizontalBorderGridItem.ColumnSpan++;
						}
						else
						{
							horizontalBorderGridItem = this.HorisontalBorders[i, j];
							horizontalBorderGridItem.ColumnIndex = j;
							horizontalBorderGridItem.ColumnSpan = 1;
						}
					}
				}
			}
			for (int k = 0; k < this.Table.GridColumnsCount + 1; k++)
			{
				if (this.Table.GridRowsCount > 0)
				{
					VerticalBorderGridItem verticalBorderGridItem = this.VerticalBorders[0, k];
					verticalBorderGridItem.RowIndex = 0;
					verticalBorderGridItem.RowSpan = 1;
					for (int l = 1; l < this.Table.GridRowsCount; l++)
					{
						if (this.VerticalBorders[l, k].Border == verticalBorderGridItem.Border)
						{
							this.VerticalBorders[l, k] = verticalBorderGridItem;
							verticalBorderGridItem.RowSpan++;
						}
						else
						{
							verticalBorderGridItem = this.VerticalBorders[l, k];
							verticalBorderGridItem.RowIndex = l;
							verticalBorderGridItem.RowSpan = 1;
						}
					}
				}
			}
		}

		void CalculateCrossBorders()
		{
			for (int i = 0; i < this.Table.GridRowsCount + 1; i++)
			{
				for (int j = 0; j < this.Table.GridColumnsCount + 1; j++)
				{
					Border border = ((i > 0) ? this.VerticalBorders[i - 1, j].Border : null);
					Border border2 = ((i < this.Table.GridRowsCount) ? this.VerticalBorders[i, j].Border : null);
					Border border3 = Border.Max(border, border2);
					Border border4 = ((j > 0) ? this.HorisontalBorders[i, j - 1].Border : null);
					Border border5 = ((j < this.Table.GridColumnsCount) ? this.HorisontalBorders[i, j].Border : null);
					Border border6 = Border.Max(border4, border5);
					Border border7 = Border.Max(border3, border6);
					if (border6 != null)
					{
						this.CrossBorders[i, j].VerticalSize = border6.Thickness;
					}
					if (border3 != null)
					{
						this.CrossBorders[i, j].HorizontalSize = border3.Thickness;
					}
					this.CrossBorders[i, j].Border = border7;
					CrossBorderDirection crossBorderDirection = CrossBorderDirection.None;
					if (border7 == border4)
					{
						if (this.Table.FlowDirection == FlowDirection.LeftToRight)
						{
							crossBorderDirection |= CrossBorderDirection.Left;
						}
						else
						{
							crossBorderDirection |= CrossBorderDirection.Right;
						}
					}
					if (border7 == border5)
					{
						if (this.Table.FlowDirection == FlowDirection.LeftToRight)
						{
							crossBorderDirection |= CrossBorderDirection.Right;
						}
						else
						{
							crossBorderDirection |= CrossBorderDirection.Left;
						}
					}
					if (border7 == border)
					{
						crossBorderDirection |= CrossBorderDirection.Top;
					}
					if (border7 == border2)
					{
						crossBorderDirection |= CrossBorderDirection.Bottom;
					}
					this.CrossBorders[i, j].CrossBorderDirection = crossBorderDirection;
				}
			}
		}

		readonly Table table;
	}
}
