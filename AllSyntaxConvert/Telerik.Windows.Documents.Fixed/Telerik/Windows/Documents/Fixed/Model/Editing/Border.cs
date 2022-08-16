using System;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	public class Border
	{
		public Border()
			: this(FixedDocumentDefaults.StrokeThickness, FixedDocumentDefaults.DefaultBorderStyle, FixedDocumentDefaults.Color)
		{
		}

		public Border(BorderStyle borderStyle)
			: this(FixedDocumentDefaults.StrokeThickness, borderStyle, FixedDocumentDefaults.Color)
		{
		}

		public Border(double thickness, ColorBase color)
			: this(thickness, FixedDocumentDefaults.DefaultBorderStyle, color)
		{
		}

		public Border(double thickness, BorderStyle borderStyle, ColorBase color)
		{
			this.borderStyle = borderStyle;
			this.color = color;
			this.thickness = thickness;
		}

		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
		}

		public double Thickness
		{
			get
			{
				return this.thickness;
			}
		}

		public ColorBase Color
		{
			get
			{
				return this.color;
			}
		}

		internal static double GetThickness(Border border)
		{
			if (border == null || border.BorderStyle == BorderStyle.None)
			{
				return 0.0;
			}
			return border.Thickness;
		}

		readonly double thickness;

		readonly ColorBase color;

		readonly BorderStyle borderStyle;
	}
}
