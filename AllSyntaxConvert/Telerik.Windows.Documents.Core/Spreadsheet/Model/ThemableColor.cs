using System;
using System.ComponentModel;
using System.Windows.Media;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	[global::System.ComponentModel.TypeConverter(typeof(global::Telerik.Windows.Documents.Spreadsheet.Utilities.ThemableColorTypeConverter))]
	public class ThemableColor : global::Telerik.Windows.Documents.Spreadsheet.Theming.IThemableObject<global::System.Windows.Media.Color>
	{
		public ThemableColor(global::System.Windows.Media.Color color)
			: this(color, false)
		{
			this.localValue = new global::System.Windows.Media.Color?(color);
		}

		public ThemableColor(global::System.Windows.Media.Color color, bool isAutomatic)
		{
			this.isAutomatic = isAutomatic;
			this.localValue = new global::System.Windows.Media.Color?(color);
		}

		public ThemableColor(global::Telerik.Windows.Documents.Spreadsheet.Theming.ThemeColorType themeColorType, global::Telerik.Windows.Documents.Spreadsheet.Model.ColorShadeType? colorShadeType = null)
		{
			this.themeColorType = new global::Telerik.Windows.Documents.Spreadsheet.Theming.ThemeColorType?(themeColorType);
			this.colorShadeType = colorShadeType;
		}

		public ThemableColor(global::Telerik.Windows.Documents.Spreadsheet.Theming.ThemeColorType themeColorType, double tintAndShade)
			: this(themeColorType, null)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfOutOfRange<double>(-1.0, 1.0, tintAndShade, "tintAndShade");
			this.tintAndShade = new double?(tintAndShade);
		}

		public global::System.Windows.Media.Color LocalValue
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

		public global::Telerik.Windows.Documents.Spreadsheet.Theming.ThemeColorType ThemeColorType
		{
			get
			{
				return this.themeColorType.Value;
			}
		}

		public global::Telerik.Windows.Documents.Spreadsheet.Model.ColorShadeType? ColorShadeType
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

		public static global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor FromArgb(byte alfa, byte red, byte green, byte blue)
		{
			return new global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(global::System.Windows.Media.Color.FromArgb(alfa, red, green, blue));
		}

		public static bool operator ==(ThemableColor first, ThemableColor second)
		{
			return (object)first == null ? (object)second == null : first.Equals((object)second);
		}

		public static bool operator !=(ThemableColor first, ThemableColor second)
		{
			return !(first == second);
		}

		public static explicit operator global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(global::System.Windows.Media.Color value)
		{
			return new global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(value);
		}

		public global::System.Windows.Media.Color GetActualValue(global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme>(theme, "theme");
			return this.GetActualValue(theme.ColorScheme);
		}

		public global::System.Windows.Media.Color GetActualValue(global::Telerik.Windows.Documents.Spreadsheet.Theming.ThemeColorScheme colorScheme)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Spreadsheet.Theming.ThemeColorScheme>(colorScheme, "colorScheme");
			global::System.Windows.Media.Color color;
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
			return global::Telerik.Windows.Documents.Media.ColorsHelper.UpdateTint(color, tint);
		}

		public override bool Equals(object obj)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor themableColor = obj as global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor;
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
			return global::Telerik.Windows.Documents.Utilities.ObjectExtensions.CombineHashCodes(this.localValue.GetHashCode(), this.themeColorType.GetHashCode(), this.tintAndShade.GetHashCode(), this.colorShadeType.GetHashCode());
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

		private readonly global::System.Windows.Media.Color? localValue;

		private readonly global::Telerik.Windows.Documents.Spreadsheet.Theming.ThemeColorType? themeColorType;

		private readonly global::Telerik.Windows.Documents.Spreadsheet.Model.ColorShadeType? colorShadeType;

		private readonly double? tintAndShade;

		private readonly bool isAutomatic;
	}
}
