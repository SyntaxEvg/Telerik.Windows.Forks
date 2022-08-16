using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class TableBorders
	{
		public TableBorders()
			: this(Border.DefaultBorder)
		{
		}

		public TableBorders(Border all)
		{
			Guard.ThrowExceptionIfNull<Border>(all, "all");
			this.left = all;
			this.top = all;
			this.right = all;
			this.bottom = all;
			this.insideHorizontal = all;
			this.insideVertical = all;
		}

		public TableBorders(Border leftBorder, Border topBorder, Border rightBorder, Border bottomBorder)
			: this(leftBorder, topBorder, rightBorder, bottomBorder, null, null)
		{
		}

		public TableBorders(Border leftBorder, Border topBorder, Border rightBorder, Border bottomBorder, Border insideHorizontalBorder, Border insideVerticalBorder)
		{
			this.left = leftBorder ?? Border.DefaultBorder;
			this.top = topBorder ?? Border.DefaultBorder;
			this.right = rightBorder ?? Border.DefaultBorder;
			this.bottom = bottomBorder ?? Border.DefaultBorder;
			this.insideHorizontal = insideHorizontalBorder ?? Border.DefaultBorder;
			this.insideVertical = insideVerticalBorder ?? Border.DefaultBorder;
		}

		public TableBorders(TableBorders source, Border leftBorder = null, Border topBorder = null, Border rightBorder = null, Border bottomBorder = null, Border insideHorizontalBorder = null, Border insideVerticalBorder = null)
		{
			this.left = leftBorder ?? source.Left;
			this.top = topBorder ?? source.Top;
			this.right = rightBorder ?? source.Right;
			this.bottom = bottomBorder ?? source.Bottom;
			this.insideHorizontal = insideHorizontalBorder ?? source.InsideHorizontal;
			this.insideVertical = insideVerticalBorder ?? source.InsideVertical;
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

		public static bool operator ==(TableBorders a, TableBorders b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
		}

		public static bool operator !=(TableBorders a, TableBorders b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			TableBorders tableBorders = obj as TableBorders;
			return !(tableBorders == null) && (this.Left.Equals(tableBorders.Left) && this.Top.Equals(tableBorders.Top) && this.Right.Equals(tableBorders.Right) && this.Bottom.Equals(tableBorders.Bottom) && this.InsideHorizontal.Equals(tableBorders.InsideHorizontal)) && this.InsideVertical.Equals(tableBorders.InsideVertical);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Left.GetHashCode(), this.Top.GetHashCode(), this.Right.GetHashCode(), this.Bottom.GetHashCode(), this.InsideHorizontal.GetHashCode(), this.InsideVertical.GetHashCode());
		}

		readonly Border top;

		readonly Border bottom;

		readonly Border left;

		readonly Border right;

		readonly Border insideHorizontal;

		readonly Border insideVertical;
	}
}
