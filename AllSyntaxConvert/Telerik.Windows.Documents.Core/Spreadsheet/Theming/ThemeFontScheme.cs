using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Theming
{
	public class ThemeFontScheme : NamedObjectBase
	{
		public ThemeFontScheme(string name, string latinMajorFontName = null, string latinMinorFontName = null, string eastAsianMajorFontName = null, string eastAsianMinorFontName = null, string complexScriptMajorFontName = null, string complexScriptMinorFontName = null)
			: base(name)
		{
			this.themeFontTypeToFont = new Dictionary<ThemeFontType, ThemeFonts>();
			this.themeFontTypeToFont.Add(ThemeFontType.Major, new ThemeFonts(latinMajorFontName, eastAsianMajorFontName, complexScriptMajorFontName));
			this.themeFontTypeToFont.Add(ThemeFontType.Minor, new ThemeFonts(latinMinorFontName, eastAsianMinorFontName, complexScriptMinorFontName));
		}

		internal ThemeFontScheme(string name, Dictionary<ThemeFontType, ThemeFonts> themeFontTypeToFont)
			: base(name)
		{
			Guard.ThrowExceptionIfNull<Dictionary<ThemeFontType, ThemeFonts>>(themeFontTypeToFont, "themeFontTypeToFont");
			this.themeFontTypeToFont = themeFontTypeToFont;
		}

		public ThemeFonts this[ThemeFontType fontType]
		{
			get
			{
				return this.themeFontTypeToFont[fontType];
			}
		}

		public ThemeFontScheme Clone()
		{
			Dictionary<ThemeFontType, ThemeFonts> dictionary = new Dictionary<ThemeFontType, ThemeFonts>();
			foreach (KeyValuePair<ThemeFontType, ThemeFonts> keyValuePair in this.themeFontTypeToFont)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value.Clone());
			}
			return new ThemeFontScheme(base.Name, dictionary);
		}

		public override bool Equals(object obj)
		{
			ThemeFontScheme themeFontScheme = obj as ThemeFontScheme;
			if (themeFontScheme == null || base.Name != themeFontScheme.Name)
			{
				return false;
			}
			foreach (KeyValuePair<ThemeFontType, ThemeFonts> keyValuePair in this.themeFontTypeToFont)
			{
				if (!ObjectExtensions.EqualsOfT<ThemeFonts>(keyValuePair.Value, themeFontScheme[keyValuePair.Key]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			if (this.hashCode == null)
			{
				this.hashCode = new int?(base.Name.GetHashCode());
				foreach (KeyValuePair<ThemeFontType, ThemeFonts> keyValuePair in this.themeFontTypeToFont)
				{
					this.hashCode = new int?(ObjectExtensions.CombineHashCodes(this.hashCode.Value, keyValuePair.Value.GetHashCode()));
				}
			}
			return this.hashCode.Value;
		}

		readonly Dictionary<ThemeFontType, ThemeFonts> themeFontTypeToFont;

		int? hashCode;
	}
}
