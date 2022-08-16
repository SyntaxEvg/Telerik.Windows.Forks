using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport;
using Telerik.Documents.SpreadsheetStreaming.Model.Themes;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public class SpreadThemableColor
	{
		public SpreadThemableColor(SpreadColor color)
			: this(color, false)
		{
		}

		public SpreadThemableColor(SpreadColor color, bool isAutomatic)
		{
			this.localValue = color;
			this.isAutomatic = isAutomatic;
		}

		public SpreadThemableColor(SpreadThemeColorType themeColorType, double tintAndShade)
			: this(themeColorType)
		{
			Guard.ThrowExceptionIfOutOfRange<double>(-1.0, 1.0, tintAndShade, "tintAndShade");
			this.tintAndShade = new double?(tintAndShade);
		}

		public SpreadThemableColor(SpreadThemeColorType themeColorType)
			: this(themeColorType, null)
		{
		}

		public SpreadThemableColor(SpreadThemeColorType themeColorType, SpreadColorShadeType? colorShadeType)
		{
			this.themeColorType = new SpreadThemeColorType?(themeColorType);
			this.colorShadeType = colorShadeType;
		}

		public SpreadColor LocalValue
		{
			get
			{
				return this.localValue;
			}
		}

		public bool IsAutomatic
		{
			get
			{
				return this.isAutomatic;
			}
		}

		public SpreadThemeColorType ThemeColorType
		{
			get
			{
				return this.themeColorType.Value;
			}
		}

		public SpreadColorShadeType? ColorShadeType
		{
			get
			{
				return this.colorShadeType;
			}
		}

		public double? TintAndShade
		{
			get
			{
				return this.tintAndShade;
			}
		}

		public bool IsFromTheme
		{
			get
			{
				return this.themeColorType != null;
			}
		}

		public static SpreadThemableColor FromRgb(byte red, byte green, byte blue)
		{
			return new SpreadThemableColor(new SpreadColor(red, green, blue));
		}

		public static bool operator ==(SpreadThemableColor first, SpreadThemableColor second)
		{
			if (first == null)
			{
				return second == null;
			}
			return first.Equals(second);
		}

		public static bool operator !=(SpreadThemableColor first, SpreadThemableColor second)
		{
			return !(first == second);
		}

		public static explicit operator SpreadThemableColor(SpreadColor value)
		{
			return new SpreadThemableColor(value);
		}

		public override bool Equals(object obj)
		{
			SpreadThemableColor spreadThemableColor = obj as SpreadThemableColor;
			return !(spreadThemableColor == null) && (ObjectExtensions.EqualsOfT<SpreadColor>(this.localValue, spreadThemableColor.localValue) && ObjectExtensions.EqualsOfT<bool>(this.isAutomatic, spreadThemableColor.isAutomatic) && ObjectExtensions.EqualsOfT<SpreadThemeColorType?>(this.themeColorType, spreadThemableColor.themeColorType) && this.tintAndShade.EqualsDouble(spreadThemableColor.tintAndShade)) && ObjectExtensions.EqualsOfT<SpreadColorShadeType?>(this.colorShadeType, spreadThemableColor.colorShadeType);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.localValue.GetHashCodeOrZero(), this.themeColorType.GetHashCodeOrZero(), this.tintAndShade.GetHashCodeOrZero(), this.colorShadeType.GetHashCodeOrZero());
		}

		public override string ToString()
		{
			if (this.IsFromTheme)
			{
				return this.ThemeColorType.ToString();
			}
			if (this.isAutomatic)
			{
				return string.Format("Auto({0})", this.localValue.ToString());
			}
			return this.localValue.ToString();
		}

		internal SpreadColor GetActualValue(SpreadTheme theme)
		{
			return this.GetActualValue(theme.ColorScheme);
		}

		internal SpreadColor GetActualValue(SpreadThemeColorScheme colorScheme)
		{
			SpreadColor color;
			double tint;
			if (this.IsFromTheme)
			{
				color = colorScheme[this.ThemeColorType].Color;
				if (this.tintAndShade != null)
				{
					tint = this.tintAndShade.Value;
				}
				else if (this.colorShadeType != null)
				{
					tint = colorScheme.GetTintAndShade(this.themeColorType.Value, this.colorShadeType.Value);
				}
				else
				{
					tint = 0.0;
				}
			}
			else
			{
				color = this.localValue;
				tint = 0.0;
			}
			return ColorsHelper.UpdateTint(color, tint);
		}

		readonly SpreadColor localValue;

		readonly SpreadThemeColorType? themeColorType;

		readonly SpreadColorShadeType? colorShadeType;

		readonly double? tintAndShade;

		readonly bool isAutomatic;
	}
}
