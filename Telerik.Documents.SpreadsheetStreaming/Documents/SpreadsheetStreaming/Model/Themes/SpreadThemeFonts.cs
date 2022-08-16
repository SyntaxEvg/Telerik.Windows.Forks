using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Themes
{
	class SpreadThemeFonts
	{
		public SpreadThemeFonts(string latinFontName = null, string eastAsianFontName = null, string complexScriptFontName = null)
		{
			this.fontLanguageTypeToFont = new Dictionary<SpreadFontLanguageType, SpreadThemeFont>();
			this.AddThemeFont(latinFontName, SpreadFontLanguageType.Latin);
			this.AddThemeFont(eastAsianFontName, SpreadFontLanguageType.EastAsian);
			this.AddThemeFont(complexScriptFontName, SpreadFontLanguageType.ComplexScript);
		}

		public SpreadThemeFont this[SpreadFontLanguageType fontLanguageType]
		{
			get
			{
				return this.fontLanguageTypeToFont[fontLanguageType];
			}
		}

		public override bool Equals(object obj)
		{
			SpreadThemeFonts spreadThemeFonts = obj as SpreadThemeFonts;
			foreach (KeyValuePair<SpreadFontLanguageType, SpreadThemeFont> keyValuePair in this.fontLanguageTypeToFont)
			{
				if (!ObjectExtensions.EqualsOfT<SpreadThemeFont>(keyValuePair.Value, spreadThemeFonts[keyValuePair.Key]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 0;
			foreach (KeyValuePair<SpreadFontLanguageType, SpreadThemeFont> keyValuePair in this.fontLanguageTypeToFont)
			{
				num = ObjectExtensions.CombineHashCodes(num, keyValuePair.Value.GetHashCode());
			}
			return num;
		}

		void AddThemeFont(string fontName, SpreadFontLanguageType fontLanguageType)
		{
			SpreadThemeFont value = null;
			if (!string.IsNullOrEmpty(fontName))
			{
				value = new SpreadThemeFont(fontName, fontLanguageType);
			}
			this.fontLanguageTypeToFont.Add(fontLanguageType, value);
		}

		readonly Dictionary<SpreadFontLanguageType, SpreadThemeFont> fontLanguageTypeToFont;
	}
}
