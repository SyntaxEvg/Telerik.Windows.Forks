using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Themes
{
	class SpreadThemeFont
	{
		public SpreadThemeFont(string fontFamily, SpreadFontLanguageType fontLanguageType)
		{
			this.fontFamily = fontFamily;
			this.fontLanguageType = fontLanguageType;
		}

		public string FontFamily
		{
			get
			{
				return this.fontFamily;
			}
		}

		public override bool Equals(object obj)
		{
			SpreadThemeFont spreadThemeFont = obj as SpreadThemeFont;
			return spreadThemeFont != null && ObjectExtensions.EqualsOfT<string>(this.fontFamily, spreadThemeFont.fontFamily) && this.fontLanguageType == spreadThemeFont.fontLanguageType;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.fontFamily.GetHashCode(), this.fontLanguageType.GetHashCode());
		}

		readonly string fontFamily;

		readonly SpreadFontLanguageType fontLanguageType;
	}
}
