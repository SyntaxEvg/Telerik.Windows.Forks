using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class ThemableFontFamily : IThemableObject<FontFamily>
	{
		public ThemableFontFamily(FontFamily fontFamily)
		{
			if (string.IsNullOrEmpty(fontFamily.Source))
			{
				this.themeFontType = new ThemeFontType?(ThemeFontType.Minor);
				return;
			}
			this.localValue = fontFamily;
		}

		public ThemableFontFamily(string familyName)
			: this(new FontFamily(familyName))
		{
		}

		public ThemableFontFamily(ThemeFontType themeFontType)
		{
			this.themeFontType = new ThemeFontType?(themeFontType);
		}

		public FontFamily LocalValue
		{
			get
			{
				return this.localValue;
			}
		}

		public ThemeFontType ThemeFontType
		{
			get
			{
				return this.themeFontType.Value;
			}
		}

		public bool IsFromTheme
		{
			get
			{
				return this.localValue == null;
			}
		}

		public static bool operator ==(ThemableFontFamily first, ThemableFontFamily second)
		{
			return (object)first == null ? (object)second == null : first.Equals((object)second);
		}

		public static bool operator !=(ThemableFontFamily first, ThemableFontFamily second)
		{
			return !(first == second);
		}

		public static explicit operator ThemableFontFamily(FontFamily value)
		{
			return new ThemableFontFamily(value);
		}

		public FontFamily GetActualValue(DocumentTheme theme)
		{
			Guard.ThrowExceptionIfNull<DocumentTheme>(theme, "theme");
			return this.GetActualValue(theme.FontScheme, FontLanguageType.Latin);
		}

		public override string ToString()
		{
			if (this.IsFromTheme)
			{
				return this.ThemeFontType.ToString();
			}
			return this.localValue.ToString();
		}

		public override bool Equals(object obj)
		{
			ThemableFontFamily themableFontFamily = obj as ThemableFontFamily;
			return !(themableFontFamily == null) && ObjectExtensions.EqualsOfT<FontFamily>(this.localValue, themableFontFamily.localValue) && this.themeFontType == themableFontFamily.themeFontType;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.localValue.GetHashCodeOrZero(), this.themeFontType.GetHashCodeOrZero());
		}

		FontFamily GetActualValue(ThemeFontScheme themeFontScheme, FontLanguageType fontLanguageType = FontLanguageType.Latin)
		{
			Guard.ThrowExceptionIfNull<ThemeFontScheme>(themeFontScheme, "themeFontScheme");
			return this.IsFromTheme ? themeFontScheme[this.ThemeFontType][fontLanguageType].FontFamily : this.localValue;
		}

		readonly FontFamily localValue;

		readonly ThemeFontType? themeFontType;
	}
}
