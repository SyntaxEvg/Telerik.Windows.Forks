using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Tables
{
	public class TableCellBorders
	{
		public TableCellBorders()
			: this(null)
		{
		}

		public TableCellBorders(Border all)
			: this(all, all, all, all, all, all)
		{
		}

		public TableCellBorders(Border left, Border top, Border right, Border bottom)
			: this(left, top, right, bottom, null, null)
		{
		}

		public TableCellBorders(Border left, Border top, Border right, Border bottom, Border diagonalUp, Border diagonalDown)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
			this.diagonalUp = diagonalUp;
			this.diagonalDown = diagonalDown;
		}

		public Border Left
		{
			get
			{
				return this.left;
			}
		}

		public Border Top
		{
			get
			{
				return this.top;
			}
		}

		public Border Right
		{
			get
			{
				return this.right;
			}
		}

		public Border Bottom
		{
			get
			{
				return this.bottom;
			}
		}

		public Border DiagonalUp
		{
			get
			{
				return this.diagonalUp;
			}
		}

		public Border DiagonalDown
		{
			get
			{
				return this.diagonalDown;
			}
		}

		internal Thickness Thickness
		{
			get
			{
				return new Thickness(Border.GetThickness(this.Left), Border.GetThickness(this.Top), Border.GetThickness(this.Right), Border.GetThickness(this.Bottom));
			}
		}

		internal void Draw(FixedContentEditor editor, Rect middleLinesRect)
		{
			Thickness thickness = this.Thickness;
			double x = middleLinesRect.Left + thickness.Left / 2.0;
			double y = middleLinesRect.Top + thickness.Top / 2.0;
			double x2 = middleLinesRect.Left + middleLinesRect.Width - thickness.Right / 2.0;
			double y2 = middleLinesRect.Top + middleLinesRect.Height - thickness.Bottom / 2.0;
			editor.GraphicProperties.IsFilled = false;
			editor.GraphicProperties.IsStroked = true;
			TableBorders.DrawBorder(editor, this.DiagonalUp, new Point(x, y2), new Point(x2, y));
			TableBorders.DrawBorder(editor, this.DiagonalDown, new Point(x, y), new Point(x2, y2));
			TableBorders.DrawBorders(editor, middleLinesRect, this.Left, this.Top, this.Right, this.Bottom);
		}

		readonly Border left;

		readonly Border top;

		readonly Border right;

		readonly Border bottom;

		readonly Border diagonalUp;

		readonly Border diagonalDown;
	}
}
