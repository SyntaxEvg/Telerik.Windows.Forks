using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class FontSchemes
	{
		static FontSchemes()
		{
			FontSchemes.nameToValueMapper.DefaultFromValue = new ValueBox<string>(FontSchemes.None);
			FontSchemes.nameToValueMapper.AddPair(FontSchemes.MajorFont, ThemeFontType.Major);
			FontSchemes.nameToValueMapper.AddPair(FontSchemes.MinorFont, ThemeFontType.Minor);
		}

		public static string GetFontSchemeName(ThemeFontType scheme)
		{
			return FontSchemes.nameToValueMapper.GetFromValue(scheme);
		}

		public static ThemeFontType? GetFontScheme(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			if (name.Equals(FontSchemes.None, StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return new ThemeFontType?(FontSchemes.nameToValueMapper.GetToValue(name));
		}

		public static readonly string MajorFont = "major";

		public static readonly string MinorFont = "minor";

		public static readonly string None = "none";

		static readonly ValueMapper<string, ThemeFontType> nameToValueMapper = new ValueMapper<string, ThemeFontType>();
	}
}
