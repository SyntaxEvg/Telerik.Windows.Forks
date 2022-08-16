using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	public class TableBorders
	{
		public TableBorders()
			: this(null)
		{
		}

		public TableBorders(Border all)
			: this(all, all, all, all)
		{
		}

		public TableBorders(Border left, Border top, Border right, Border bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
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

		internal Thickness Thickness
		{
			get
			{
				return new Thickness(Border.GetThickness(this.Left), Border.GetThickness(this.Top), Border.GetThickness(this.Right), Border.GetThickness(this.Bottom));
			}
		}

		internal virtual void Draw(FixedContentEditor editor, Rect middleLinesRect)
		{
			TableBorders.DrawBorders(editor, middleLinesRect, this.Left, this.Top, this.Right, this.Bottom);
		}

		internal static void DrawBorders(FixedContentEditor editor, Rect middleLinesRect, Border leftBorder, Border topBorder, Border rightBorder, Border bottomBorder)
		{
			Thickness thickness = new Thickness(Border.GetThickness(leftBorder), Border.GetThickness(topBorder), Border.GetThickness(rightBorder), Border.GetThickness(bottomBorder));
			double num = middleLinesRect.Left;
			double num2 = middleLinesRect.Top;
			double num3 = num + middleLinesRect.Width;
			double num4 = num2 + middleLinesRect.Height;
			editor.GraphicProperties.IsFilled = false;
			editor.GraphicProperties.IsStroked = true;
			TableBorders.DrawBorder(editor, leftBorder, new Point(num, num2 - thickness.Top / 2.0), new Point(num, num4 + thickness.Bottom / 2.0));
			TableBorders.DrawBorder(editor, topBorder, new Point(num - thickness.Left / 2.0, num2), new Point(num3 + thickness.Right / 2.0, num2));
			TableBorders.DrawBorder(editor, rightBorder, new Point(num3, num2 - thickness.Top / 2.0), new Point(num3, num4 + thickness.Bottom / 2.0));
			TableBorders.DrawBorder(editor, bottomBorder, new Point(num - thickness.Left / 2.0, num4), new Point(num3 + thickness.Right / 2.0, num4));
		}

		internal static void DrawBorder(FixedContentEditor editor, Border border, Point start, Point end)
		{
			if (border != null && border.BorderStyle != BorderStyle.None)
			{
				editor.GraphicProperties.StrokeThickness = border.Thickness;
				editor.GraphicProperties.StrokeColor = border.Color;
				editor.DrawLine(start, end);
			}
		}

		readonly Border left;

		readonly Border top;

		readonly Border right;

		readonly Border bottom;
	}
}
