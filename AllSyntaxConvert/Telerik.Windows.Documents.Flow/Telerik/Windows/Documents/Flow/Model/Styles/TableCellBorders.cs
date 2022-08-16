using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class TableCellBorders
	{
		public TableCellBorders()
			: this(Border.DefaultBorder)
		{
		}

		public TableCellBorders(Border all)
			: this(all, all, all, all)
		{
			Guard.ThrowExceptionIfNull<Border>(all, "all");
		}

		public TableCellBorders(Border leftBorder, Border topBorder, Border rightBorder, Border bottomBorder)
			: this(leftBorder, topBorder, rightBorder, bottomBorder, null, null, null, null)
		{
		}

		public TableCellBorders(Border leftBorder, Border topBorder, Border rightBorder, Border bottomBorder, Border insideHorizontalBorder, Border insideVerticalBorder, Border diagonalDownBorder, Border diagonalUpBorder)
		{
			this.left = leftBorder ?? Border.DefaultBorder;
			this.top = topBorder ?? Border.DefaultBorder;
			this.right = rightBorder ?? Border.DefaultBorder;
			this.bottom = bottomBorder ?? Border.DefaultBorder;
			this.insideHorizontal = insideHorizontalBorder ?? Border.DefaultBorder;
			this.insideVertical = insideVerticalBorder ?? Border.DefaultBorder;
			this.diagonalDown = diagonalDownBorder ?? Border.EmptyBorder;
			this.diagonalUp = diagonalUpBorder ?? Border.EmptyBorder;
		}

		public TableCellBorders(TableCellBorders source, Border leftBorder = null, Border topBorder = null, Border rightBorder = null, Border bottomBorder = null, Border insideHorizontalBorder = null, Border insideVerticalBorder = null, Border diagonalDownBorder = null, Border diagonalUpBorder = null)
		{
			this.left = leftBorder ?? source.Left;
			this.top = topBorder ?? source.Top;
			this.right = rightBorder ?? source.Right;
			this.bottom = bottomBorder ?? source.Bottom;
			this.insideHorizontal = insideHorizontalBorder ?? source.InsideHorizontal;
			this.insideVertical = insideVerticalBorder ?? source.InsideVertical;
			this.diagonalDown = diagonalDownBorder ?? source.DiagonalDown;
			this.diagonalUp = diagonalUpBorder ?? source.DiagonalUp;
		}

		public Border Top
		{
			get
			{
				return this.top;
			}
		}

		public Border Bottom
		{
			get
			{
				return this.bottom;
			}
		}

		public Border Left
		{
			get
			{
				return this.left;
			}
		}

		public Border Right
		{
			get
			{
				return this.right;
			}
		}

		public Border InsideHorizontal
		{
			get
			{
				return this.insideHorizontal;
			}
		}

		public Border InsideVertical
		{
			get
			{
				return this.insideVertical;
			}
		}

		public Border DiagonalDown
		{
			get
			{
				return this.diagonalDown;
			}
		}

		public Border DiagonalUp
		{
			get
			{
				return this.diagonalUp;
			}
		}

		public static bool operator ==(TableCellBorders a, TableCellBorders b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
		}

		public static bool operator !=(TableCellBorders a, TableCellBorders b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			TableCellBorders tableCellBorders = obj as TableCellBorders;
			return !(tableCellBorders == null) && (this.Left.Equals(tableCellBorders.Left) && this.Top.Equals(tableCellBorders.Top) && this.Right.Equals(tableCellBorders.Right) && this.Bottom.Equals(tableCellBorders.Bottom) && this.InsideHorizontal.Equals(tableCellBorders.InsideHorizontal) && this.InsideVertical.Equals(tableCellBorders.InsideVertical) && this.DiagonalDown.Equals(tableCellBorders.DiagonalDown)) && this.DiagonalUp.Equals(tableCellBorders.DiagonalUp);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Left.GetHashCode(), this.Top.GetHashCode(), this.Right.GetHashCode(), this.Bottom.GetHashCode(), this.InsideHorizontal.GetHashCode(), this.InsideVertical.GetHashCode(), this.DiagonalDown.GetHashCode(), this.DiagonalUp.GetHashCode());
		}

		readonly Border top;

		readonly Border bottom;

		readonly Border left;

		readonly Border right;

		readonly Border insideHorizontal;

		readonly Border insideVertical;

		readonly Border diagonalDown;

		readonly Border diagonalUp;
	}
}
