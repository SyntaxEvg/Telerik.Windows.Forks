using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Theming
{
	public class ThemeFonts
	{
		public ThemeFonts(string latinFontName = null, string eastAsianFontName = null, string complexScriptFontName = null)
		{
			this.fontLanguageTypeToFont = new Dictionary<FontLanguageType, ThemeFont>();
			this.AddThemeFont(latinFontName, FontLanguageType.Latin);
			this.AddThemeFont(eastAsianFontName, FontLanguageType.EastAsian);
			this.AddThemeFont(complexScriptFontName, FontLanguageType.ComplexScript);
		}

		internal ThemeFonts(Dictionary<FontLanguageType, ThemeFont> fontLanguageTypeToFont)
		{
			Guard.ThrowExceptionIfNull<Dictionary<FontLanguageType, ThemeFont>>(fontLanguageTypeToFont, "fontLanguageTypeToFont");
			this.fontLanguageTypeToFont = fontLanguageTypeToFont;
		}

		public ThemeFont this[FontLanguageType fontLanguageType]
		{
			get
			{
				return this.fontLanguageTypeToFont[fontLanguageType];
			}
		}

		public ThemeFonts Clone()
		{
			Dictionary<FontLanguageType, ThemeFont> dictionary = new Dictionary<FontLanguageType, ThemeFont>();
			foreach (KeyValuePair<FontLanguageType, ThemeFont> keyValuePair in this.fontLanguageTypeToFont)
			{
				if (keyValuePair.Value != null)
				{
					dictionary.Add(keyValuePair.Key, new ThemeFont(keyValuePair.Value.FontFamily, keyValuePair.Value.FontLanguageType));
				}
				else
				{
					dictionary.Add(keyValuePair.Key, null);
				}
			}
			return new ThemeFonts(dictionary);
		}

		public override bool Equals(object obj)
		{
			ThemeFonts themeFonts = obj as ThemeFonts;
			foreach (KeyValuePair<FontLanguageType, ThemeFont> keyValuePair in this.fontLanguageTypeToFont)
			{
				if (!ObjectExtensions.EqualsOfT<ThemeFont>(keyValuePair.Value, themeFonts[keyValuePair.Key]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 0;
			foreach (KeyValuePair<FontLanguageType, ThemeFont> keyValuePair in this.fontLanguageTypeToFont)
			{
				num = ObjectExtensions.CombineHashCodes(num, keyValuePair.Value.GetHashCode());
			}
			return num;
		}

		void AddThemeFont(string fontName, FontLanguageType fontLanguageType)
		{
			ThemeFont value = null;
			if (!string.IsNullOrEmpty(fontName))
			{
				value = new ThemeFont(fontName, fontLanguageType);
			}
			this.fontLanguageTypeToFont.Add(fontLanguageType, value);
		}

		readonly Dictionary<FontLanguageType, ThemeFont> fontLanguageTypeToFont;
	}
}
