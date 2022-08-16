using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CellBorder
	{
		public CellBorderStyle Style
		{
			get
			{
				return this.style;
			}
		}

		public ThemableColor Color
		{
			get
			{
				return this.color;
			}
		}

		public double Thickness
		{
			get
			{
				return CellBorder.GetThicknessByBorderStyle(this.Style);
			}
		}

		public CellBorder(CellBorderStyle style, ThemableColor color)
		{
			this.style = style;
			if (CellBorder.Default != null && this.style == CellBorderStyle.None && color != new ThemableColor(Colors.Transparent))
			{
				color = CellBorder.Default.Color;
			}
			this.color = color;
		}

		public static bool operator ==(CellBorder first, CellBorder second)
		{
			return object.ReferenceEquals(first, second) || (first != null && first.Equals(second));
		}

		public static bool operator !=(CellBorder first, CellBorder second)
		{
			return !(first == second);
		}

		public override bool Equals(object obj)
		{
			CellBorder cellBorder = obj as CellBorder;
			return !(cellBorder == null) && this.style == cellBorder.style && TelerikHelper.EqualsOfT<ThemableColor>(this.color, cellBorder.color);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.style.GetHashCode(), this.color.GetHashCode());
		}

		public static CellBorder GetWithMaxPriority(CellBorder leftBorder, CellBorder rightBorder, ThemeColorScheme colorScheme)
		{
			Guard.ThrowExceptionIfNull<CellBorder>(leftBorder, "leftBorder");
			Guard.ThrowExceptionIfNull<CellBorder>(rightBorder, "rightBorder");
			Guard.ThrowExceptionIfNull<ThemeColorScheme>(colorScheme, "colorScheme");
			if (leftBorder.GetZIndex(colorScheme) <= rightBorder.GetZIndex(colorScheme))
			{
				return rightBorder;
			}
			return leftBorder;
		}

		internal int GetZIndex(ThemeColorScheme colorScheme)
		{
			if (CellBorder.Default.Equals(this))
			{
				return 0;
			}
			int num = 1275 * (int)Math.Round(this.Thickness);
			Color actualValue = this.Color.GetActualValue(colorScheme);
			return num + (int)(actualValue.R + actualValue.B * 2 + actualValue.G * 4);
		}

		public static double GetThicknessByBorderStyle(CellBorderStyle borderStyle)
		{
			switch (borderStyle)
			{
			case CellBorderStyle.None:
			case CellBorderStyle.Hair:
			case CellBorderStyle.Dotted:
			case CellBorderStyle.DashDotDot:
			case CellBorderStyle.DashDot:
			case CellBorderStyle.Dashed:
			case CellBorderStyle.Thin:
				return CellBorder.DefaultThickness;
			case CellBorderStyle.MediumDashDotDot:
			case CellBorderStyle.MediumDashDot:
			case CellBorderStyle.MediumDashed:
			case CellBorderStyle.Double:
			case CellBorderStyle.Medium:
			case CellBorderStyle.SlantDashDot:
				return CellBorder.DefaultThickness * 2.0;
			case CellBorderStyle.Thick:
				return CellBorder.DefaultThickness * 3.0;
			default:
				return CellBorder.DefaultThickness;
			}
		}

		static readonly double DefaultThickness = 1.0;

		public static readonly CellBorder Default = new CellBorder(CellBorderStyle.None, new ThemableColor(Colors.LightGray));

		readonly CellBorderStyle style;

		readonly ThemableColor color;
	}
}
