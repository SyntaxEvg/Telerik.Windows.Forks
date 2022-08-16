using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing.Collections;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Tables
{
	public class Table : IBlockElement
	{
		public Table()
			: this(0)
		{
		}

		Table(int firstRowIndex)
		{
			this.firstRowIndex = firstRowIndex;
			this.rows = new TableRowCollection();
			this.pendingRows = new List<TableRow>();
			this.fixedColumnWidths = new Dictionary<int, double>();
			this.finalColumnWidths = new List<double>();
			this.minColumnWidths = new Dictionary<int, double>();
			this.Borders = new TableBorders();
			this.Background = FixedDocumentDefaults.BackgroundColor;
			this.DefaultCellProperties = new CellProperties
			{
				Padding = FixedDocumentDefaults.DefaultCellPadding,
				Background = FixedDocumentDefaults.BackgroundColor
			};
			this.LayoutType = FixedDocumentDefaults.DefaultLayoutType;
			this.BorderSpacing = FixedDocumentDefaults.DefaultBorderSpacing;
			this.BorderCollapse = FixedDocumentDefaults.DefaultBorderCollapse;
			this.IsLastRowSplit = false;
		}

		Table(Table other, int firstRowIndex)
			: this(firstRowIndex)
		{
			this.Borders = other.Borders;
			this.Margin = other.Margin;
			this.Background = other.Background;
			this.DefaultCellProperties = other.DefaultCellProperties;
			this.BorderSpacing = other.BorderSpacing;
			this.BorderCollapse = other.BorderCollapse;
			this.fixedColumnWidths = other.fixedColumnWidths;
			this.minColumnWidths = other.minColumnWidths;
			this.PreferredWidth = other.PreferredWidth;
		}

		public Thickness Margin { get; set; }

		public TableLayoutType LayoutType { get; set; }

		public TableBorders Borders
		{
			get
			{
				return this.borders;
			}
			set
			{
				Guard.ThrowExceptionIfNull<TableBorders>(value, "value");
				this.borders = value;
			}
		}

		public ColorBase Background { get; set; }

		public CellProperties DefaultCellProperties { get; set; }

		public Size DesiredSize { get; set; }

		public double BorderSpacing
		{
			get
			{
				return this.borderSpacing;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<double>(0.0, value, "value");
				this.borderSpacing = value;
			}
		}

		public BorderCollapse BorderCollapse { get; set; }

		public TableRowCollection Rows
		{
			get
			{
				return this.rows;
			}
		}

		public bool HasPendingContent
		{
			get
			{
				return this.pendingRows.Any<TableRow>();
			}
		}

		internal double? PreferredWidth { get; set; }

		internal IEnumerable<TableCell> Cells
		{
			get
			{
				for (int i = 0; i < this.rows.Count; i++)
				{
					IEnumerable<TableCell> ownedCells;
					if (i == 0)
					{
						ownedCells = this.rows[i].AllIntersectingCells;
					}
					else
					{
						ownedCells = this.rows[i].Cells;
					}
					foreach (TableCell cell in ownedCells)
					{
						yield return cell;
					}
				}
				yield break;
			}
		}

		Rect BordersLayoutRectangle
		{
			get
			{
				Thickness thickness = this.Borders.Thickness;
				double num = -this.BorderSpacing / 2.0;
				double num2 = -this.BorderSpacing / 2.0;
				double width = this.DesiredSize.Width - this.Margin.Left - this.Margin.Right - (thickness.Left + thickness.Right) / 2.0;
				double height = this.DesiredSize.Height - this.Margin.Top - this.Margin.Bottom - (thickness.Top + thickness.Bottom) / 2.0;
				if (this.BorderCollapse == BorderCollapse.Separate)
				{
					num -= thickness.Left / 2.0;
					num2 -= thickness.Top / 2.0;
				}
				return new Rect(num, num2, width, height);
			}
		}

		double WidthMargins
		{
			get
			{
				Thickness thickness = this.Borders.Thickness;
				double num = this.Margin.Left + this.Margin.Right + this.BorderSpacing;
				if (this.BorderCollapse == BorderCollapse.Collapse)
				{
					num += (thickness.Left + thickness.Right) / 2.0;
				}
				else
				{
					num += thickness.Left + thickness.Right;
				}
				return num;
			}
		}

		double HeightMargins
		{
			get
			{
				Thickness thickness = this.Borders.Thickness;
				double num = this.Margin.Top + this.Margin.Bottom + this.BorderSpacing;
				if (this.BorderCollapse == BorderCollapse.Collapse)
				{
					num += (thickness.Top + thickness.Bottom) / 2.0;
				}
				else
				{
					num += thickness.Top + thickness.Bottom;
				}
				return num;
			}
		}

		bool IsSplitTable { get; set; }

		bool IsLastRowSplit { get; set; }

		public Size Measure()
		{
			return this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
		}

		public Size Measure(Size availableSize)
		{
			this.OnBeforeMeasure();
			if (this.TryCalculateDesiredSizeOnMinimalSizesScenarios(availableSize))
			{
				return this.DesiredSize;
			}
			double widthMargins = this.WidthMargins;
			double heightMargins = this.HeightMargins;
			availableSize = new Size(Math.Max(0.0, availableSize.Width - widthMargins), Math.Max(0.0, availableSize.Height - heightMargins));
			if (!this.IsSplitTable)
			{
				this.MeasureInfiniteWidthLayout();
				double tableFitWidth;
				if (this.ShouldMeasureFiniteWidthLayout(availableSize.Width, out tableFitWidth))
				{
					this.MeasureFiniteWidthLayout(tableFitWidth);
				}
				this.ExpandAccordingToTablePreferredWidth();
				this.CalculateColumnLefts();
			}
			this.CalculateRowHeights();
			this.CalculateRowTopsAndSplit(availableSize.Height);
			foreach (TableCell cell in this.Cells)
			{
				this.CalculateCellLayoutRectangle(cell, this.rows.Count - 1);
			}
			TableRow tableRow = this.rows[this.rows.Count - 1];
			double num = tableRow.Top + tableRow.Height;
			double num2 = this.columnLefts[this.columnLefts.Length - 1] + this.finalColumnWidths[this.finalColumnWidths.Count - 1];
			this.DesiredSize = new Size(num2 + widthMargins, num + heightMargins);
			return this.DesiredSize;
		}

		void ExpandAccordingToTablePreferredWidth()
		{
			if (this.PreferredWidth != null)
			{
				double totalColumnWidth = this.GetTotalColumnWidth();
				double num = totalColumnWidth;
				double? preferredWidth = this.PreferredWidth;
				if (num < preferredWidth.GetValueOrDefault() && preferredWidth != null)
				{
					IEnumerable<int> enumerable = this.DetermineColumnsToExpand();
					double num2 = this.PreferredWidth.Value - totalColumnWidth;
					double num3 = num2 / (double)enumerable.Count<int>();
					foreach (int index in enumerable)
					{
						double value = this.finalColumnWidths[index] + num3;
						this.finalColumnWidths[index] = value;
					}
				}
			}
		}

		IEnumerable<int> DetermineColumnsToExpand()
		{
			if (this.fixedColumnWidths.Count == this.finalColumnWidths.Count)
			{
				return this.fixedColumnWidths.Keys;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < this.finalColumnWidths.Count; i++)
			{
				if (!this.fixedColumnWidths.ContainsKey(i))
				{
					list.Add(i);
				}
			}
			return list;
		}

		bool ShouldMeasureFiniteWidthLayout(double availableWidth, out double newAvailableWidth)
		{
			bool flag = this.PreferredWidth != null || (!double.IsInfinity(availableWidth) && !double.IsNaN(availableWidth));
			newAvailableWidth = ((this.PreferredWidth != null) ? this.PreferredWidth.Value : availableWidth);
			if (flag)
			{
				if (this.LayoutType == TableLayoutType.FixedWidth)
				{
					return true;
				}
				if (this.LayoutType == TableLayoutType.AutoFit)
				{
					double num = 0.0;
					for (int i = 0; i < this.finalColumnWidths.Count; i++)
					{
						num += this.finalColumnWidths[i];
					}
					return num > newAvailableWidth;
				}
			}
			return false;
		}

		double GetTotalColumnWidth()
		{
			double num = 0.0;
			for (int i = 0; i < this.finalColumnWidths.Count; i++)
			{
				num += this.finalColumnWidths[i];
			}
			return num;
		}

		bool TryCalculateDesiredSizeOnMinimalSizesScenarios(Size availableSize)
		{
			if (!this.hasCells)
			{
				this.DesiredSize = new Size(this.WidthMargins, this.HeightMargins);
				return true;
			}
			if (availableSize == RadFixedDocumentEditor.MinimalMeasureSize)
			{
				this.DesiredSize = this.MeasureMinimalSplitSize();
				return true;
			}
			return false;
		}

		public void Draw(FixedContentEditor editor, Rect boundingRect)
		{
			Guard.ThrowExceptionIfNull<FixedContentEditor>(editor, "editor");
			this.Measure(new Size(boundingRect.Width, boundingRect.Height));
			using (editor.SavePosition())
			{
				using (editor.SaveProperties())
				{
					Thickness thickness = this.Borders.Thickness;
					double num = boundingRect.X + this.Margin.Left + this.BorderSpacing / 2.0;
					double num2 = boundingRect.Y + this.Margin.Top + this.BorderSpacing / 2.0;
					if (this.BorderCollapse == BorderCollapse.Collapse)
					{
						num += thickness.Left / 2.0;
						num2 += thickness.Top / 2.0;
					}
					else
					{
						num += thickness.Left;
						num2 += thickness.Top;
					}
					num += editor.Position.Matrix.OffsetX;
					num2 += editor.Position.Matrix.OffsetY;
					editor.Position.Translate(num, num2);
					Rect bordersLayoutRectangle = this.BordersLayoutRectangle;
					this.DrawBackground(editor, this.Background, bordersLayoutRectangle, this.Borders.Thickness);
					foreach (TableCell tableCell in this.Cells)
					{
						ColorBase background = tableCell.Background ?? this.DefaultCellProperties.Background;
						this.DrawBackground(editor, background, tableCell.BordersLayoutRectangle, tableCell.BordersThickness);
					}
					this.Borders.Draw(editor, bordersLayoutRectangle);
					foreach (TableCell tableCell2 in this.Cells)
					{
						this.DrawCellContent(editor, tableCell2);
						tableCell2.ActualBorders.Draw(editor, tableCell2.BordersLayoutRectangle);
					}
				}
			}
		}

		public Table Split()
		{
			int num = this.firstRowIndex + (this.IsLastRowSplit ? (this.rows.Count - 1) : this.rows.Count);
			Table table = new Table(this, num);
			table.rows.AddRange(this.pendingRows);
			table.hasCells = this.hasCells && table.rows.Count > 0;
			table.fixedColumnWidths = this.fixedColumnWidths;
			table.finalColumnWidths = this.finalColumnWidths;
			table.minColumnWidths = this.minColumnWidths;
			table.columnLefts = this.columnLefts;
			this.FixSharedMergedCells(table);
			table.IsSplitTable = true;
			return table;
		}

		IBlockElement IBlockElement.Split()
		{
			return this.Split();
		}

		Size MeasureMinimalSplitSize()
		{
			IEnumerable<TableCell> enumerable;
			if (this.IsSplitTable)
			{
				enumerable = this.rows[0].AllIntersectingCells;
			}
			else
			{
				enumerable = this.rows[0].Cells;
			}
			double height = 0.0;
			double num = 0.0;
			foreach (TableCell tableCell in enumerable)
			{
				double fixedWidth = this.GetFixedWidth(tableCell);
				num = Math.Max(num, tableCell.Measure(new Size(fixedWidth, height)).Height);
			}
			double num2 = this.GetTotalFixedWidth();
			num2 += this.WidthMargins;
			num += this.HeightMargins;
			return new Size(num2, num);
		}

		double GetTotalFixedWidth()
		{
			double num = 0.0;
			foreach (KeyValuePair<int, double> keyValuePair in this.fixedColumnWidths)
			{
				num += keyValuePair.Value;
			}
			return num;
		}

		double GetFixedWidth(TableCell cell)
		{
			double num = 0.0;
			for (int i = cell.FromColumn; i <= cell.ToColumn; i++)
			{
				double num2;
				if (this.fixedColumnWidths.TryGetValue(i, out num2))
				{
					num += num2;
				}
			}
			return num;
		}

		int GetRelativeRowIndex(int absoluteRowIndex)
		{
			return absoluteRowIndex - this.firstRowIndex;
		}

		int GetAbsoluteRowIndex(int relativeRowIndex)
		{
			return relativeRowIndex + this.firstRowIndex;
		}

		void CalculateRowHeights()
		{
			foreach (TableCell tableCell in this.Cells)
			{
				int num = Math.Max(0, this.GetRelativeRowIndex(tableCell.FromRow));
				int relativeRowIndex = this.GetRelativeRowIndex(tableCell.ToRow);
				double rowsHeight = this.GetRowsHeight(num, relativeRowIndex);
				this.CalculateCellLayoutRectangle(tableCell, this.rows.Count - 1);
				tableCell.Measure(new Size(tableCell.LayoutRectangle.Width, double.MaxValue));
				double height = tableCell.DesiredSize.Height;
				if (height > rowsHeight)
				{
					double num2 = (height - rowsHeight) / (double)(relativeRowIndex - num + 1);
					for (int i = num; i <= relativeRowIndex; i++)
					{
						this.rows[i].Height += num2;
					}
				}
			}
		}

		void OnBeforeMeasure()
		{
			IEnumerable<TableRow> range = (this.IsLastRowSplit ? this.pendingRows.Skip(1) : this.pendingRows);
			this.rows.AddRange(range);
			this.pendingRows.Clear();
			if (this.IsSplitTable)
			{
				using (IEnumerator<TableRow> enumerator = this.rows.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TableRow tableRow = enumerator.Current;
						tableRow.Height = 0.0;
						tableRow.Top = 0.0;
					}
					return;
				}
			}
			foreach (TableRow tableRow2 in this.rows)
			{
				tableRow2.Height = 0.0;
				tableRow2.Top = 0.0;
				tableRow2.AllIntersectingCells.Clear();
			}
			this.columnLefts = null;
			this.minColumnWidths.Clear();
			this.fixedColumnWidths.Clear();
			this.finalColumnWidths.Clear();
			this.PrepareCellsAndArrangeColumns();
		}

		void FixSharedMergedCells(Table splitTable)
		{
			if (this.rows.Count == 0 || splitTable.rows.Count == 0)
			{
				return;
			}
			if (this.IsLastRowSplit)
			{
				this.FixSharedMergedCellsWhenIsLastRowSplit(splitTable);
				return;
			}
			this.FixSharedMergedCellsWhenNoRowIsSplit(splitTable);
		}

		void FixSharedMergedCellsWhenNoRowIsSplit(Table splitTable)
		{
			int num = this.rows.Count - 1;
			TableRow tableRow = this.rows[num];
			int absoluteRowIndex = this.GetAbsoluteRowIndex(num);
			int num2 = -1;
			Dictionary<int, TableCell> dictionary = new Dictionary<int, TableCell>();
			foreach (TableCell tableCell in tableRow.AllIntersectingCells)
			{
				if (tableCell.ToRow > absoluteRowIndex)
				{
					TableCell tableCell2 = tableCell.Split();
					dictionary.Add(tableCell2.FromColumn, tableCell2);
					num2 = Math.Max(num2, tableCell.ToRow);
				}
			}
			if (!dictionary.Any<KeyValuePair<int, TableCell>>())
			{
				return;
			}
			int num3 = num2 - absoluteRowIndex;
			for (int i = 0; i < num3; i++)
			{
				TableRow tableRow2 = splitTable.rows[i].Copy();
				splitTable.Rows[i] = tableRow2;
				for (int j = 0; j < tableRow2.AllIntersectingCells.Count; j++)
				{
					TableCell tableCell3 = tableRow2.AllIntersectingCells[j];
					if (tableCell3.FromRow <= absoluteRowIndex)
					{
						tableRow2.AllIntersectingCells[j] = dictionary[tableCell3.FromColumn];
					}
				}
			}
		}

		void FixSharedMergedCellsWhenIsLastRowSplit(Table splitTable)
		{
			TableRow tableRow = splitTable.rows[0];
			int num = splitTable.firstRowIndex;
			Dictionary<int, TableCell> dictionary = new Dictionary<int, TableCell>();
			int num2 = -1;
			foreach (TableCell tableCell in tableRow.AllIntersectingCells)
			{
				if (tableCell.ToRow > num)
				{
					dictionary.Add(tableCell.FromColumn, tableCell);
					num2 = Math.Max(num2, tableCell.ToRow);
				}
			}
			if (!dictionary.Any<KeyValuePair<int, TableCell>>())
			{
				return;
			}
			int num3 = num2 - num;
			for (int i = 1; i <= num3; i++)
			{
				TableRow tableRow2 = splitTable.rows[i].Copy();
				splitTable.rows[i] = tableRow2;
				for (int j = 0; j < tableRow2.AllIntersectingCells.Count; j++)
				{
					TableCell tableCell2 = tableRow2.AllIntersectingCells[j];
					if (tableCell2.FromRow <= num)
					{
						tableRow2.AllIntersectingCells[j] = dictionary[tableCell2.FromColumn];
					}
				}
			}
		}

		void DrawCellContent(FixedContentEditor editor, TableCell cell)
		{
			Guard.ThrowExceptionIfNull<FixedContentEditor>(editor, "editor");
			Guard.ThrowExceptionIfNull<TableCell>(cell, "cell");
			cell.Draw(editor);
		}

		void DrawBackground(FixedContentEditor editor, ColorBase background, Rect bordersMiddleLinesRectangle, Thickness bordersThickness)
		{
			if (background != null)
			{
				editor.GraphicProperties.IsFilled = true;
				editor.GraphicProperties.IsStroked = false;
				editor.GraphicProperties.FillColor = background;
				double x = bordersMiddleLinesRectangle.Left + bordersThickness.Left / 2.0;
				double y = bordersMiddleLinesRectangle.Top + bordersThickness.Top / 2.0;
				double width = Math.Max(0.0, bordersMiddleLinesRectangle.Width - (bordersThickness.Left + bordersThickness.Right) / 2.0);
				double height = Math.Max(0.0, bordersMiddleLinesRectangle.Height - (bordersThickness.Top + bordersThickness.Bottom) / 2.0);
				editor.DrawRectangle(new Rect(x, y, width, height));
			}
		}

		void MeasureFiniteWidthLayout(double tableFitWidth)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			for (int i = 0; i < this.finalColumnWidths.Count; i++)
			{
				double num4;
				if (this.fixedColumnWidths.TryGetValue(i, out num4))
				{
					num2 += num4;
					num3 += Math.Max(num4, this.minColumnWidths[i]);
				}
				else
				{
					num += this.finalColumnWidths[i];
				}
			}
			if (num3 > tableFitWidth)
			{
				double num5 = num + num2;
				for (int j = 0; j < this.finalColumnWidths.Count; j++)
				{
					double num6;
					double num7;
					if (this.fixedColumnWidths.TryGetValue(j, out num6))
					{
						num7 = num6 / num5;
					}
					else
					{
						num7 = this.finalColumnWidths[j] / num5;
					}
					this.finalColumnWidths[j] = num7 * tableFitWidth;
				}
				return;
			}
			double num8 = tableFitWidth - num3;
			for (int k = 0; k < this.finalColumnWidths.Count; k++)
			{
				if (!this.fixedColumnWidths.ContainsKey(k))
				{
					double num9 = this.finalColumnWidths[k] / num;
					this.finalColumnWidths[k] = num9 * num8;
				}
			}
		}

		void MeasureInfiniteWidthLayout()
		{
			foreach (KeyValuePair<int, double> keyValuePair in this.fixedColumnWidths)
			{
				this.finalColumnWidths[keyValuePair.Key] = keyValuePair.Value;
			}
			for (int i = 0; i < this.finalColumnWidths.Count; i++)
			{
				this.finalColumnWidths[i] = Math.Max(this.finalColumnWidths[i], this.minColumnWidths[i]);
			}
			foreach (TableCell cell in this.Cells)
			{
				this.FitCellWidth(cell);
			}
		}

		void PrepareCellsAndArrangeColumns()
		{
			this.hasCells = false;
			for (int i = 0; i < this.rows.Count; i++)
			{
				TableRow tableRow = this.rows[i];
				Stack<TableCell> stack = new Stack<TableCell>(from cell in tableRow.AllIntersectingCells
					orderby cell.FromColumn descending
					select cell);
				tableRow.AllIntersectingCells.Clear();
				int num = 0;
				foreach (TableCell tableCell in tableRow.Cells)
				{
					this.hasCells = true;
					this.SetDefaultProperties(tableCell);
					num = Table.MoveToNextFreeColumn(num, tableRow, stack);
					tableCell.FromRow = i;
					tableCell.FromColumn = num;
					num = tableCell.ToColumn + 1;
					tableRow.AllIntersectingCells.Add(tableCell);
					this.MarkUsedCellRange(tableCell);
					if (tableCell.ColumnSpan <= 1)
					{
						this.SetFixedColumnWidths(tableCell);
					}
				}
				tableRow.AllIntersectingCells.AddRange(stack);
			}
			foreach (TableCell tableCell2 in this.Cells)
			{
				if (tableCell2.ColumnSpan > 1)
				{
					this.SetFixedColumnWidths(tableCell2);
				}
				this.SetMinimumColumnWidths(tableCell2);
			}
		}

		void SetMinimumColumnWidths(TableCell cell)
		{
			double num = cell.CalculateNonSplittableCellWidth() / (double)cell.ColumnSpan;
			for (int i = cell.FromColumn; i <= cell.ToColumn; i++)
			{
				this.minColumnWidths[i] = (this.minColumnWidths.ContainsKey(i) ? Math.Max(num, this.minColumnWidths[i]) : num);
			}
		}

		void SetDefaultProperties(TableCell cell)
		{
			cell.DefaultCellProperties = this.DefaultCellProperties;
			cell.BorderSpacing = this.BorderSpacing;
			cell.BorderCollapse = this.BorderCollapse;
		}

		static int MoveToNextFreeColumn(int currentColumn, TableRow row, Stack<TableCell> mergedCells)
		{
			TableCell tableCell = ((mergedCells.Count > 0) ? mergedCells.Peek() : null);
			while (tableCell != null && tableCell.FromColumn <= currentColumn)
			{
				currentColumn = Math.Max(currentColumn, tableCell.ToColumn + 1);
				row.AllIntersectingCells.Add(mergedCells.Pop());
				tableCell = ((mergedCells.Count > 0) ? mergedCells.Peek() : null);
			}
			return currentColumn;
		}

		void CalculateCellLayoutRectangle(TableCell cell, int maxBottomRowIndex)
		{
			int index = Math.Max(0, this.GetRelativeRowIndex(cell.FromRow));
			int index2 = System.Math.Min(maxBottomRowIndex, this.GetRelativeRowIndex(cell.ToRow));
			double num = this.columnLefts[cell.FromColumn];
			double top = this.rows[index].Top;
			double width = this.columnLefts[cell.ToColumn] + this.finalColumnWidths[cell.ToColumn] - num;
			double height = this.rows[index2].Bottom - top;
			cell.LayoutRectangle = new Rect(num, top, width, height);
		}

		void CalculateRowTopsAndSplit(double maxTableHeight)
		{
			int i = 0;
			TableRow tableRow = this.rows[i];
			tableRow.Top = 0.0;
			if (tableRow.Bottom <= maxTableHeight)
			{
				for (i = 1; i < this.rows.Count; i++)
				{
					tableRow = this.rows[i];
					tableRow.Top = this.rows[i - 1].Bottom;
					if (tableRow.Bottom > maxTableHeight)
					{
						break;
					}
				}
			}
			if (i < this.rows.Count)
			{
				this.SplitAndMoveToPending(i, maxTableHeight);
			}
		}

		void SplitAndMoveToPending(int splitRowIndex, double maxTableHeight)
		{
			TableRow item;
			if (this.TrySplitRow(splitRowIndex, maxTableHeight, out item))
			{
				this.pendingRows.Add(item);
				this.MoveRowsToPending(splitRowIndex + 1);
				return;
			}
			if (splitRowIndex == 0)
			{
				this.MoveRowsToPending(splitRowIndex + 1);
				return;
			}
			this.MoveRowsToPending(splitRowIndex);
		}

		bool TrySplitRow(int rowIndex, double maxTableHeight, out TableRow splitRow)
		{
			Guard.ThrowExceptionIfLessThan<double>(0.0, maxTableHeight, "maxTableHeight");
			splitRow = null;
			this.IsLastRowSplit = this.CheckIfRowCanBeSplit(rowIndex, maxTableHeight);
			if (this.IsLastRowSplit)
			{
				TableRow tableRow = this.rows[rowIndex];
				tableRow.Height = 0.0;
				splitRow = new TableRow();
				using (List<TableCell>.Enumerator enumerator = tableRow.AllIntersectingCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TableCell tableCell = enumerator.Current;
						double val = tableCell.DesiredSize.Height - (tableRow.Top - tableCell.LayoutRectangle.Top);
						tableRow.Height = Math.Max(tableRow.Height, val);
						TableCell item = tableCell.Split();
						splitRow.AllIntersectingCells.Add(item);
					}
					goto IL_DD;
				}
			}
			if (rowIndex > 0)
			{
				this.MeasureLastRowMergedCells(rowIndex - 1);
			}
			IL_DD:
			return this.IsLastRowSplit;
		}

		void MeasureLastRowMergedCells(int lastRowIndex)
		{
			TableRow tableRow = this.rows[lastRowIndex];
			foreach (TableCell tableCell in tableRow.AllIntersectingCells)
			{
				if (tableCell.ToRow >= lastRowIndex + 1)
				{
					this.CalculateCellLayoutRectangle(tableCell, lastRowIndex);
					Rect layoutRectangle = tableCell.LayoutRectangle;
					tableCell.Measure(new Size(layoutRectangle.Width, layoutRectangle.Height));
				}
			}
		}

		bool CheckIfRowCanBeSplit(int splitRowIndex, double maxTableHeight)
		{
			bool result = true;
			TableRow tableRow = this.rows[splitRowIndex];
			foreach (TableCell tableCell in tableRow.AllIntersectingCells)
			{
				this.CalculateCellLayoutRectangle(tableCell, splitRowIndex);
				double num = maxTableHeight - tableCell.LayoutRectangle.Top;
				if (tableCell.Measure(new Size(tableCell.LayoutRectangle.Width, num)).Height > num)
				{
					result = false;
				}
			}
			return result;
		}

		void MoveRowsToPending(int firstRowIndex)
		{
			for (int i = firstRowIndex; i < this.rows.Count; i++)
			{
				this.pendingRows.Add(this.rows[i]);
			}
			int num = this.rows.Count - firstRowIndex;
			if (num > 0)
			{
				this.rows.RemoveRange(firstRowIndex, num);
			}
		}

		void CalculateColumnLefts()
		{
			this.columnLefts = new double[this.finalColumnWidths.Count];
			for (int i = 1; i < this.finalColumnWidths.Count; i++)
			{
				this.columnLefts[i] = this.columnLefts[i - 1] + this.finalColumnWidths[i - 1];
			}
		}

		void SetFixedColumnWidths(TableCell cell)
		{
			if (cell.PreferredWidth != null)
			{
				double num = 0.0;
				int num2 = 0;
				for (int i = cell.FromColumn; i <= cell.ToColumn; i++)
				{
					double num3;
					if (this.fixedColumnWidths.TryGetValue(i, out num3))
					{
						num += num3;
					}
					else
					{
						num2++;
					}
				}
				double num4 = Math.Max(0.0, cell.PreferredWidth.Value - num);
				if (num4 > 0.0)
				{
					if (num2 > 0)
					{
						double value = num4 / (double)num2;
						for (int j = cell.FromColumn; j <= cell.ToColumn; j++)
						{
							if (!this.fixedColumnWidths.ContainsKey(j))
							{
								this.fixedColumnWidths[j] = value;
							}
						}
						return;
					}
					double num5 = num4 / (double)cell.ColumnSpan;
					for (int k = cell.FromColumn; k <= cell.ToColumn; k++)
					{
						Dictionary<int, double> dictionary;
						int key;
						(dictionary = this.fixedColumnWidths)[key = k] = dictionary[key] + num5;
					}
				}
			}
		}

		void FitCellWidth(TableCell cell)
		{
			double num;
			double num2;
			int floatingColumnsCount = this.GetFloatingColumnsCount(cell.FromColumn, cell.ToColumn, out num, out num2);
			double width = ((floatingColumnsCount == 0) ? num : double.PositiveInfinity);
			Size size = cell.Measure(new Size(width, double.PositiveInfinity));
			if (floatingColumnsCount > 0)
			{
				double num3 = num + num2;
				if (size.Width > num3)
				{
					double num4 = (size.Width - num3) / (double)floatingColumnsCount;
					for (int i = cell.FromColumn; i <= cell.ToColumn; i++)
					{
						if (!this.fixedColumnWidths.ContainsKey(i))
						{
							List<double> list;
							int index;
							(list = this.finalColumnWidths)[index = i] = list[index] + num4;
						}
					}
				}
			}
		}

		double GetRowsHeight(int fromRelativeRowIndex, int toRelativeRowIndex)
		{
			double num = 0.0;
			for (int i = fromRelativeRowIndex; i <= toRelativeRowIndex; i++)
			{
				num += this.rows[i].Height;
			}
			return num;
		}

		int GetFloatingColumnsCount(int fromColumnIndex, int toColumnIndex, out double fixedWidth, out double floatingWidth)
		{
			fixedWidth = 0.0;
			floatingWidth = 0.0;
			int num = 0;
			for (int i = fromColumnIndex; i <= toColumnIndex; i++)
			{
				double num2;
				if (this.fixedColumnWidths.TryGetValue(i, out num2))
				{
					fixedWidth += num2;
				}
				else
				{
					floatingWidth += this.finalColumnWidths[i];
					num++;
				}
			}
			return num;
		}

		void MarkUsedCellRange(TableCell cell)
		{
			this.AddMissingRows(cell.ToRow);
			this.AddMissingColumns(cell.ToColumn);
			for (int i = cell.FromRow + 1; i <= cell.ToRow; i++)
			{
				this.rows[i].AllIntersectingCells.Add(cell);
			}
		}

		void AddMissingColumns(int usedColumnIndex)
		{
			while (this.finalColumnWidths.Count <= usedColumnIndex)
			{
				this.finalColumnWidths.Add(0.0);
			}
		}

		void AddMissingRows(int usedRowIndex)
		{
			while (this.rows.Count <= usedRowIndex)
			{
				this.Rows.AddTableRow();
			}
		}

		readonly int firstRowIndex;

		readonly TableRowCollection rows;

		readonly List<TableRow> pendingRows;

		Dictionary<int, double> fixedColumnWidths;

		Dictionary<int, double> minColumnWidths;

		List<double> finalColumnWidths;

		double[] columnLefts;

		double borderSpacing;

		TableBorders borders;

		bool hasCells;
	}
}
