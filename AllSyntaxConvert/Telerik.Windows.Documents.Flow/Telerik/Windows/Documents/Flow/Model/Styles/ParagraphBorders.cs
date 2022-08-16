using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class ParagraphBorders
	{
		public ParagraphBorders()
			: this(Border.DefaultBorder)
		{
		}

		public ParagraphBorders(Border all)
		{
			Guard.ThrowExceptionIfNull<Border>(all, "all");
			this.left = all;
			this.top = all;
			this.right = all;
			this.bottom = all;
			this.between = all;
		}

		public ParagraphBorders(Border leftBorder, Border topBorder, Border rightBorder, Border bottomBorder)
			: this(leftBorder, topBorder, rightBorder, bottomBorder, null)
		{
		}

		public ParagraphBorders(Border leftBorder, Border topBorder, Border rightBorder, Border bottomBorder, Border between)
		{
			this.left = leftBorder ?? Border.DefaultBorder;
			this.top = topBorder ?? Border.DefaultBorder;
			this.right = rightBorder ?? Border.DefaultBorder;
			this.bottom = bottomBorder ?? Border.DefaultBorder;
			this.between = between ?? Border.DefaultBorder;
		}

		public ParagraphBorders(ParagraphBorders source, Border leftBorder = null, Border topBorder = null, Border rightBorder = null, Border bottomBorder = null, Border between = null)
		{
			this.left = leftBorder ?? source.Left;
			this.top = topBorder ?? source.Top;
			this.right = rightBorder ?? source.Right;
			this.bottom = bottomBorder ?? source.Bottom;
			this.between = between ?? source.Between;
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

		public Border Between
		{
			get
			{
				return this.between;
			}
		}

		public static bool operator ==(ParagraphBorders a, ParagraphBorders b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
		}

		public static bool operator !=(ParagraphBorders a, ParagraphBorders b)
		{
			return !(a == b);
		}

		public ParagraphBorders SetLeft(Border left)
		{
			return new ParagraphBorders(left, this.Top, this.Right, this.Bottom, this.Between);
		}

		public ParagraphBorders SetTop(Border top)
		{
			return new ParagraphBorders(this.Left, top, this.Right, this.Bottom, this.Between);
		}

		public ParagraphBorders SetRight(Border right)
		{
			return new ParagraphBorders(this.Left, this.Top, right, this.Bottom, this.Between);
		}

		public ParagraphBorders SetBottom(Border bottom)
		{
			return new ParagraphBorders(this.Left, this.Top, this.Right, bottom, this.Between);
		}

		public ParagraphBorders SetBetween(Border between)
		{
			return new ParagraphBorders(this.Left, this.Top, this.Right, this.Bottom, between);
		}

		public override bool Equals(object obj)
		{
			ParagraphBorders paragraphBorders = obj as ParagraphBorders;
			return !(paragraphBorders == null) && (this.Left.Equals(paragraphBorders.Left) && this.Top.Equals(paragraphBorders.Top) && this.Right.Equals(paragraphBorders.Right) && this.Bottom.Equals(paragraphBorders.Bottom)) && this.Between.Equals(paragraphBorders.Between);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Left.GetHashCode(), this.Top.GetHashCode(), this.Right.GetHashCode(), this.Bottom.GetHashCode(), this.Between.GetHashCode());
		}

		readonly Border top;

		readonly Border bottom;

		readonly Border left;

		readonly Border right;

		readonly Border between;
	}
}
