using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing.Collections;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Tables
{
	public class TableCell
	{
		internal TableCell()
		{
			this.Borders = new TableCellBorders();
			this.blocks = new BlockCollection();
			this.pendingBlocks = new List<IBlockElement>();
			this.rowSpan = 1;
			this.columnSpan = 1;
			this.IsLastBlockSplit = false;
		}

		TableCell(TableCell other)
			: this()
		{
			this.Padding = other.Padding;
			this.Borders = other.Borders;
			this.Background = other.Background;
			this.DefaultCellProperties = other.DefaultCellProperties;
			this.BorderSpacing = other.BorderSpacing;
			this.BorderCollapse = other.BorderCollapse;
			this.FromRow = other.FromRow;
			this.FromColumn = other.FromColumn;
			this.RowSpan = other.RowSpan;
			this.ColumnSpan = other.ColumnSpan;
		}

		public int RowSpan
		{
			get
			{
				return this.rowSpan;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<int>(1, value, "value");
				this.rowSpan = value;
			}
		}

		public int ColumnSpan
		{
			get
			{
				return this.columnSpan;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<int>(1, value, "value");
				this.columnSpan = value;
			}
		}

		public double? PreferredWidth { get; set; }

		public BlockCollection Blocks
		{
			get
			{
				return this.blocks;
			}
		}

		public Thickness? Padding { get; set; }

		public TableCellBorders Borders
		{
			get
			{
				return this.borders;
			}
			set
			{
				Guard.ThrowExceptionIfNull<TableCellBorders>(value, "value");
				this.borders = value;
			}
		}

		public ColorBase Background { get; set; }

		public Size Size
		{
			get
			{
				return new Size(this.LayoutRectangle.Width, this.LayoutRectangle.Height);
			}
		}

		internal Size DesiredSize
		{
			get
			{
				double width = this.DesiredContentSize.Width + this.WidthPaddings;
				double height = this.DesiredContentSize.Height + this.HeightPaddings;
				return new Size(width, height);
			}
		}

		internal Rect LayoutRectangle { get; set; }

		internal int FromRow { get; set; }

		internal int FromColumn { get; set; }

		internal CellProperties DefaultCellProperties { get; set; }

		internal double BorderSpacing { get; set; }

		internal BorderCollapse BorderCollapse { get; set; }

		internal int ToRow
		{
			get
			{
				return this.FromRow + this.RowSpan - 1;
			}
		}

		internal int ToColumn
		{
			get
			{
				return this.FromColumn + this.ColumnSpan - 1;
			}
		}

		internal bool HasPendingContent
		{
			get
			{
				return this.pendingBlocks.Count > 0;
			}
		}

		internal Rect BordersLayoutRectangle
		{
			get
			{
				Thickness bordersThickness = this.BordersThickness;
				double num = this.LayoutRectangle.Left + this.BorderSpacing / 2.0;
				double num2 = this.LayoutRectangle.Top + this.BorderSpacing / 2.0;
				double num3 = this.LayoutRectangle.Width - this.BorderSpacing;
				double num4 = this.LayoutRectangle.Height - this.BorderSpacing;
				if (this.BorderCollapse == BorderCollapse.Separate)
				{
					num += bordersThickness.Left / 2.0;
					num2 += bordersThickness.Left / 2.0;
					num3 -= (bordersThickness.Left + bordersThickness.Right) / 2.0;
					num4 -= (bordersThickness.Top + bordersThickness.Bottom) / 2.0;
				}
				num3 = Math.Max(0.0, num3);
				num4 = Math.Max(0.0, num4);
				return new Rect(num, num2, num3, num4);
			}
		}

		internal Thickness BordersThickness
		{
			get
			{
				return this.ActualBorders.Thickness;
			}
		}

		internal TableCellBorders ActualBorders
		{
			get
			{
				Border left = this.Borders.Left ?? this.DefaultCellProperties.Borders.Left;
				Border top = this.Borders.Top ?? this.DefaultCellProperties.Borders.Top;
				Border right = this.Borders.Right ?? this.DefaultCellProperties.Borders.Right;
				Border bottom = this.Borders.Bottom ?? this.DefaultCellProperties.Borders.Bottom;
				Border diagonalUp = this.Borders.DiagonalUp ?? this.DefaultCellProperties.Borders.DiagonalUp;
				Border diagonalDown = this.Borders.DiagonalDown ?? this.DefaultCellProperties.Borders.DiagonalDown;
				return new TableCellBorders(left, top, right, bottom, diagonalUp, diagonalDown);
			}
		}

		Thickness ActualPadding
		{
			get
			{
				if (this.Padding == null)
				{
					return this.DefaultCellProperties.Padding;
				}
				return this.Padding.Value;
			}
		}

		double WidthPaddings
		{
			get
			{
				Thickness bordersThickness = this.BordersThickness;
				Thickness actualPadding = this.ActualPadding;
				double num = actualPadding.Left + actualPadding.Right + this.BorderSpacing;
				if (this.BorderCollapse == BorderCollapse.Collapse)
				{
					num += (bordersThickness.Left + bordersThickness.Right) / 2.0;
				}
				else
				{
					num += bordersThickness.Left + bordersThickness.Right;
				}
				return num;
			}
		}

		double HeightPaddings
		{
			get
			{
				Thickness bordersThickness = this.BordersThickness;
				Thickness actualPadding = this.ActualPadding;
				double num = actualPadding.Top + actualPadding.Bottom + this.BorderSpacing;
				if (this.BorderCollapse == BorderCollapse.Collapse)
				{
					num += (bordersThickness.Top + bordersThickness.Bottom) / 2.0;
				}
				else
				{
					num += bordersThickness.Top + bordersThickness.Bottom;
				}
				return num;
			}
		}

		Size DesiredContentSize { get; set; }

		bool IsLastBlockSplit { get; set; }

		internal void Draw(FixedContentEditor editor)
		{
			Guard.ThrowExceptionIfNull<FixedContentEditor>(editor, "editor");
			Thickness bordersThickness = this.BordersThickness;
			Thickness actualPadding = this.ActualPadding;
			double num = Math.Max(0.0, this.LayoutRectangle.Width - this.WidthPaddings);
			double num2 = Math.Max(0.0, this.LayoutRectangle.Height - this.HeightPaddings);
			double num3 = this.LayoutRectangle.X + actualPadding.Left + this.BorderSpacing / 2.0;
			double num4 = this.LayoutRectangle.Y + actualPadding.Top + this.BorderSpacing / 2.0;
			double width = (this.DesiredContentSize.Width.IsEqualTo(num, 1E-08) ? Math.Max(this.DesiredContentSize.Width, num) : num);
			double num5 = (this.DesiredContentSize.Height.IsEqualTo(num2, 1E-08) ? Math.Max(this.DesiredContentSize.Height, num2) : num2);
			if (this.BorderCollapse == BorderCollapse.Collapse)
			{
				num3 += bordersThickness.Left / 2.0;
				num4 += bordersThickness.Top / 2.0;
			}
			else
			{
				num3 += bordersThickness.Left;
				num4 += bordersThickness.Top;
			}
			using (editor.PushTransformedClipping(new Rect(num3, num4, width, num5)))
			{
				foreach (IBlockElement blockElement in this.blocks)
				{
					blockElement.Draw(editor, new Rect(num3, num4, width, num5));
					num4 += blockElement.DesiredSize.Height;
					num5 -= blockElement.DesiredSize.Height;
				}
			}
		}

		internal Size Measure()
		{
			return this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
		}

		internal Size Measure(Size availableSize)
		{
			double widthPaddings = this.WidthPaddings;
			double heightPaddings = this.HeightPaddings;
			availableSize = new Size(Math.Max(0.0, availableSize.Width - widthPaddings), Math.Max(0.0, availableSize.Height - heightPaddings));
			double num = 0.0;
			double num2 = 0.0;
			double num3 = availableSize.Height;
			IEnumerable<IBlockElement> range = (this.IsLastBlockSplit ? this.pendingBlocks.Skip(1) : this.pendingBlocks);
			this.blocks.AddRange(range);
			this.pendingBlocks.Clear();
			int num4 = 0;
			while (num4 < this.blocks.Count && num3 >= 0.0)
			{
				IBlockElement blockElement = this.blocks[num4];
				Size availableSize2 = new Size(availableSize.Width, num3);
				Size size = blockElement.Measure(availableSize2);
				if (num2 + size.Height > availableSize.Height || blockElement.HasPendingContent)
				{
					bool flag = num4 == 0;
					this.IsLastBlockSplit = this.TrySplitAndMoveToPending(num4, availableSize2);
					bool flag2 = this.IsLastBlockSplit || flag;
					if (!flag2)
					{
						break;
					}
				}
				if (size.Width > num)
				{
					num = size.Width;
				}
				num2 += size.Height;
				num3 = availableSize.Height - num2;
				num4++;
			}
			this.DesiredContentSize = new Size(num, num2);
			return this.DesiredSize;
		}

		internal double CalculateNonSplittableCellWidth()
		{
			double num = 0.0;
			foreach (IBlockElement blockElement in this.blocks)
			{
				blockElement.Measure(RadFixedDocumentEditor.MinimalWidthMeasureSize);
				num = Math.Max(num, blockElement.DesiredSize.Width);
			}
			return num + this.WidthPaddings;
		}

		bool TrySplitAndMoveToPending(int splitBlockIndex, Size availableSize)
		{
			IBlockElement blockElement = this.blocks[splitBlockIndex];
			bool flag = splitBlockIndex == 0;
			bool result = false;
			if (blockElement.DesiredSize.Height > availableSize.Height && !flag)
			{
				this.MoveBlocksToPending(splitBlockIndex);
			}
			else
			{
				if (blockElement.HasPendingContent)
				{
					this.pendingBlocks.Add(blockElement.Split());
					result = true;
				}
				this.MoveBlocksToPending(splitBlockIndex + 1);
			}
			return result;
		}

		void MoveBlocksToPending(int firstBlockIndex)
		{
			for (int i = firstBlockIndex; i < this.blocks.Count; i++)
			{
				this.pendingBlocks.Add(this.blocks[i]);
			}
			this.blocks.RemoveRange(firstBlockIndex, this.blocks.Count - firstBlockIndex);
		}

		internal TableCell Split()
		{
			TableCell tableCell = new TableCell(this);
			tableCell.blocks.AddRange(this.pendingBlocks);
			return tableCell;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IBlockElement blockElement in this.Blocks)
			{
				stringBuilder.AppendLine(string.Format("{0}, {1}, {2}", '{', blockElement.ToString(), '}'));
			}
			return stringBuilder.ToString();
		}

		readonly BlockCollection blocks;

		readonly List<IBlockElement> pendingBlocks;

		int rowSpan;

		int columnSpan;

		TableCellBorders borders;
	}
}
