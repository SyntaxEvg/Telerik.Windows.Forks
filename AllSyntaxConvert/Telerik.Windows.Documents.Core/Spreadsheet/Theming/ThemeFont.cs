using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Theming
{
	public class ThemeFont
	{
		public ThemeFont(FontFamily fontFamily, FontLanguageType fontLanguageType)
		{
			Guard.ThrowExceptionIfNull<FontFamily>(fontFamily, "fontFamily");
			this.fontFamily = fontFamily;
			this.fontLanguageType = fontLanguageType;
		}

		public ThemeFont(string fontName, FontLanguageType fontLanguageType)
			: this(new FontFamily(fontName), fontLanguageType)
		{
		}

		public FontFamily FontFamily
		{
			get
			{
				return this.fontFamily;
			}
		}

		public FontLanguageType FontLanguageType
		{
			get
			{
				return this.fontLanguageType;
			}
		}

		public override bool Equals(object obj)
		{
			ThemeFont themeFont = obj as ThemeFont;
			return themeFont != null && ObjectExtensions.EqualsOfT<FontFamily>(this.fontFamily, themeFont.fontFamily) && this.fontLanguageType == themeFont.fontLanguageType;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.fontFamily.GetHashCode(), this.fontLanguageType.GetHashCode());
		}

		readonly FontFamily fontFamily;

		readonly FontLanguageType fontLanguageType;
	}
}
