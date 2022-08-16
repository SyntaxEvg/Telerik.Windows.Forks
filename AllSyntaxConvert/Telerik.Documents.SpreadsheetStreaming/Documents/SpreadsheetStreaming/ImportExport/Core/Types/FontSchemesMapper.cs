using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class FontSchemesMapper
	{
		static FontSchemesMapper()
		{
			FontSchemesMapper.nameToValueMapper.DefaultFromValue = new ValueBox<string>(FontSchemesMapper.None);
			FontSchemesMapper.nameToValueMapper.AddPair(FontSchemesMapper.MajorFont, SpreadThemeFontType.Major);
			FontSchemesMapper.nameToValueMapper.AddPair(FontSchemesMapper.MinorFont, SpreadThemeFontType.Minor);
		}

		public static string GetFontSchemeName(SpreadThemeFontType scheme)
		{
			return FontSchemesMapper.nameToValueMapper.GetFromValue(scheme);
		}

		public static SpreadThemeFontType? GetFontScheme(string name)
		{
			if (name.Equals(FontSchemesMapper.None, StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return new SpreadThemeFontType?(FontSchemesMapper.nameToValueMapper.GetToValue(name));
		}

		public static readonly string MajorFont = "major";

		public static readonly string MinorFont = "minor";

		public static readonly string None = "none";

		static readonly ValueMapper<string, SpreadThemeFontType> nameToValueMapper = new ValueMapper<string, SpreadThemeFontType>();
	}
}
