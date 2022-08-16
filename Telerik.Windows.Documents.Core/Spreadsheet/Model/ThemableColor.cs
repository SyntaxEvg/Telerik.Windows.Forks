using System;
using System.ComponentModel;
using System.Windows.Media;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	[TypeConverter(typeof(ThemableColorTypeConverter))]
	public class ThemableColor : IThemableObject<Color>
	{
		public ThemableColor(Color color)
			: this(color, false)
		{
			this.localValue = new Color?(color);
		}

		public ThemableColor(Color color, bool isAutomatic)
		{
			this.isAutomatic = isAutomatic;
			this.localValue = new Color?(color);
		}

		public ThemableColor(ThemeColorType themeColorType, ColorShadeType? colorShadeType = null)
		{
			this.themeColorType = new ThemeColorType?(themeColorType);
			this.colorShadeType = colorShadeType;
		}

		public ThemableColor(ThemeColorType themeColorType, double tintAndShade)
			: this(themeColorType, null)
		{
			Guard.ThrowExceptionIfOutOfRange<double>(-1.0, 1.0, tintAndShade, "tintAndShade");
			this.tintAndShade = new double?(tintAndShade);
		}

		public Color LocalValue
		{
			get
			{
				return this.localValue.Value;
			}
		}

		public bool IsAutomatic
		{
			get
			{
				return this.isAutomatic;
			}
		}

		public ThemeColorType ThemeColorType
		{
			get
			{
				return this.themeColorType.Value;
			}
		}

		public ColorShadeType? ColorShadeType
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

		public static ThemableColor FromArgb(byte alfa, byte red, byte green, byte blue)
		{
			return new ThemableColor(Color.FromArgb(alfa, red, green, blue));
		}

		public static bool operator ==(ThemableColor first, ThemableColor second)
		{
			if (first == null)
			{
				return second == null;
			}
			return first.Equals(second);
		}

		public static bool operator !=(ThemableColor first, ThemableColor second)
		{
			return !(first == second);
		}

		public static explicit operator ThemableColor(Color value)
		{
			return new ThemableColor(value);
		}

		public Color GetActualValue(DocumentTheme theme)
		{
			Guard.ThrowExceptionIfNull<DocumentTheme>(theme, "theme");
			return this.GetActualValue(theme.ColorScheme);
		}

		public Color GetActualValue(ThemeColorScheme colorScheme)
		{
			Guard.ThrowExceptionIfNull<ThemeColorScheme>(colorScheme, "colorScheme");
			Color color;
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
				color = this.localValue.Value;
				tint = 0.0;
			}
			return ColorsHelper.UpdateTint(color, tint);
		}

		public override bool Equals(object obj)
		{
			ThemableColor themableColor = obj as ThemableColor;
			if (themableColor != null && this.localValue == themableColor.localValue && this.isAutomatic == themableColor.isAutomatic && this.themeColorType == themableColor.themeColorType)
			{
				double? num = this.tintAndShade;
				double? num2 = themableColor.tintAndShade;
				if (num.GetValueOrDefault() == num2.GetValueOrDefault() && num != null == (num2 != null))
				{
					return this.colorShadeType == themableColor.colorShadeType;
				}
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.localValue.GetHashCode(), this.themeColorType.GetHashCode(), this.tintAndShade.GetHashCode(), this.colorShadeType.GetHashCode());
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

		readonly Color? localValue;

		readonly ThemeColorType? themeColorType;

		readonly ColorShadeType? colorShadeType;

		readonly double? tintAndShade;

		readonly bool isAutomatic;
	}
}
