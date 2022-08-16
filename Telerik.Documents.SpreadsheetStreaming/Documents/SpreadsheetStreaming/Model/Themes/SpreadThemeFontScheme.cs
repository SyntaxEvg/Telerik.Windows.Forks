using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Themes
{
	class SpreadThemeFontScheme : NamedObjectBase
	{
		public SpreadThemeFontScheme(string name, string latinMajorFontName = null, string latinMinorFontName = null, string eastAsianMajorFontName = null, string eastAsianMinorFontName = null, string complexScriptMajorFontName = null, string complexScriptMinorFontName = null)
			: base(name)
		{
			this.themeFontTypeToFont = new Dictionary<SpreadThemeFontType, SpreadThemeFonts>();
			this.themeFontTypeToFont.Add(SpreadThemeFontType.Major, new SpreadThemeFonts(latinMajorFontName, eastAsianMajorFontName, complexScriptMajorFontName));
			this.themeFontTypeToFont.Add(SpreadThemeFontType.Minor, new SpreadThemeFonts(latinMinorFontName, eastAsianMinorFontName, complexScriptMinorFontName));
		}

		public SpreadThemeFonts this[SpreadThemeFontType fontType]
		{
			get
			{
				return this.themeFontTypeToFont[fontType];
			}
		}

		public override bool Equals(object obj)
		{
			SpreadThemeFontScheme spreadThemeFontScheme = obj as SpreadThemeFontScheme;
			if (spreadThemeFontScheme == null || base.Name != spreadThemeFontScheme.Name)
			{
				return false;
			}
			foreach (KeyValuePair<SpreadThemeFontType, SpreadThemeFonts> keyValuePair in this.themeFontTypeToFont)
			{
				if (!ObjectExtensions.EqualsOfT<SpreadThemeFonts>(keyValuePair.Value, spreadThemeFontScheme[keyValuePair.Key]))
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
				foreach (KeyValuePair<SpreadThemeFontType, SpreadThemeFonts> keyValuePair in this.themeFontTypeToFont)
				{
					this.hashCode = new int?(ObjectExtensions.CombineHashCodes(this.hashCode.Value, keyValuePair.Value.GetHashCode()));
				}
			}
			return this.hashCode.Value;
		}

		readonly Dictionary<SpreadThemeFontType, SpreadThemeFonts> themeFontTypeToFont;

		int? hashCode;
	}
}
