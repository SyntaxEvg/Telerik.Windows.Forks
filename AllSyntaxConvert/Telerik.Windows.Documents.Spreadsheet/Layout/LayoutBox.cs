using System;
using System.Windows;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	public abstract class LayoutBox
	{
		public double Left
		{
			get
			{
				return this.left;
			}
		}

		public double Top
		{
			get
			{
				return this.top;
			}
		}

		public double Width
		{
			get
			{
				return this.width;
			}
		}

		public double Height
		{
			get
			{
				return this.height;
			}
		}

		public Size Size
		{
			get
			{
				return new Size(this.Width, this.Height);
			}
		}

		public Rect BoundingRectangle
		{
			get
			{
				if (this.boundingRectangle == null)
				{
					this.boundingRectangle = new Rect?(new Rect(Math.Round(this.Left, 3), Math.Round(this.Top, 3), Math.Round(this.Width, 3), Math.Round(this.Height, 3)));
				}
				return this.boundingRectangle.Value;
			}
		}

		internal LayoutBox(Rect rect)
		{
			this.left = rect.Left;
			this.top = rect.Top;
			this.width = rect.Width;
			this.height = rect.Height;
		}

		public override string ToString()
		{
			return string.Format("BoundingRectangle:{0}", this.BoundingRectangle);
		}

		double top;

		double left;

		double width;

		double height;

		Rect? boundingRectangle;
	}
}
