using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class Border : IComparable<Border>
	{
		public Border(Border border)
		{
			this.thickness = border.Thickness;
			this.style = border.Style;
			this.color = border.Color;
			this.shadow = border.Shadow;
			this.frame = border.Frame;
			this.spacing = border.Spacing;
		}

		public Border(BorderStyle style)
			: this(0.0, style, new ThemableColor(Colors.Transparent), false, false, 0.0)
		{
		}

		public Border(double thickness, BorderStyle style, ThemableColor color)
			: this(thickness, style, color, false, false, 0.0)
		{
		}

		public Border(double thickness, BorderStyle style, ThemableColor color, bool shadow, bool frame, double spacing)
		{
			this.thickness = thickness;
			this.style = style;
			this.shadow = shadow;
			this.frame = frame;
			this.spacing = spacing;
			if (color == (ThemableColor)null && (this.Style == BorderStyle.None || this.Style == BorderStyle.Inherit))
				color = Border.DefaultBorder.Color;
			//this.color = color;


			//if (color == null && (this.Style == BorderStyle.None || this.Style == BorderStyle.Inherit))
			//{
			//	color = Border.DefaultBorder.Color;
			//}
			this.color = color;
		}

		public ThemableColor Color
		{
			get
			{
				return this.color;
			}
		}

		public bool Shadow
		{
			get
			{
				return this.shadow;
			}
		}

		public bool Frame
		{
			get
			{
				return this.frame;
			}
		}

		public double Spacing
		{
			get
			{
				return this.spacing;
			}
		}

		public BorderStyle Style
		{
			get
			{
				return this.style;
			}
		}

		public double Thickness
		{
			get
			{
				return this.thickness;
			}
		}

		public static bool operator ==(Border first, Border second)
		{
			return object.ReferenceEquals(first, second) || (!object.ReferenceEquals(first, null) && !object.ReferenceEquals(second, null) && first.Equals(second));
		}

		public static bool operator !=(Border first, Border second)
		{
			return !(first == second);
		}

		public override bool Equals(object obj)
		{
			Border border = obj as Border;
			if (border == null)
			{
				return false;
			}
			int num = this.Thickness.CompareTo(border.Thickness);
			if (num != 0)
			{
				return false;
			}
			int num2 = this.Style - border.Style;
			return num2 == 0 && ((this.Color == null && border.Color == null) || ((!(this.Color != null) || !(border.Color == null)) && (!(this.Color == null) || !(border.Color != null)) && this.Color.Equals(border.Color) && this.Spacing == border.Spacing && this.Shadow == border.Shadow && this.Frame == border.Frame));
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Style.GetHashCode(), this.Color.GetHashCode(), this.Thickness.GetHashCode());
		}

		public int CompareTo(Border other)
		{
			if (other == null)
			{
				return 1;
			}
			int num = this.Thickness.CompareTo(other.Thickness);
			if (num != 0)
			{
				return num;
			}
			int num2 = this.Style - other.Style;
			if (num2 != 0)
			{
				return num2;
			}
			if (!this.Color.IsFromTheme && !other.Color.IsFromTheme)
			{
				Color localValue = this.Color.LocalValue;
				Color localValue2 = other.Color.LocalValue;
				int num3 = (int)(localValue.R + localValue.B + 2 * localValue.G - (localValue2.R + localValue2.B + 2 * localValue2.G));
				if (num3 != 0)
				{
					return num3;
				}
				num3 = (int)(localValue.B + 2 * localValue.G - (localValue2.B + 2 * localValue2.G));
				if (num3 != 0)
				{
					return num3;
				}
				num3 = (int)(localValue.G - localValue2.G);
				if (num3 != 0)
				{
					return num3;
				}
			}
			return 0;
		}

		internal static Border Max(Border x, Border y)
		{
			if (x == null)
			{
				return y;
			}
			if (y == null)
			{
				return x;
			}
			if (x.CompareTo(y) <= 0)
			{
				return y;
			}
			return x;
		}

		internal static readonly Border DefaultBorder = new Border(0.0, BorderStyle.Inherit, new ThemableColor(Colors.Black));

		internal static readonly Border EmptyBorder = new Border(0.0, BorderStyle.None, new ThemableColor(Colors.Black));

		readonly ThemableColor color;

		readonly bool shadow;

		readonly bool frame;

		readonly double spacing;

		readonly BorderStyle style;

		readonly double thickness;
	}
}
