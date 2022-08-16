using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Theming
{
	public class DocumentTheme : NamedObjectBase
	{
		public DocumentTheme(string name, ThemeColorScheme colorScheme, ThemeFontScheme fontScheme)
			: base(name)
		{
			Guard.ThrowExceptionIfNull<ThemeColorScheme>(colorScheme, "colorScheme");
			Guard.ThrowExceptionIfNull<ThemeFontScheme>(fontScheme, "fontScheme");
			this.colorScheme = colorScheme;
			this.fontScheme = fontScheme;
		}

		public ThemeColorScheme ColorScheme
		{
			get
			{
				return this.colorScheme;
			}
		}

		public ThemeFontScheme FontScheme
		{
			get
			{
				return this.fontScheme;
			}
		}

		public DocumentTheme Clone()
		{
			return new DocumentTheme(base.Name, this.ColorScheme.Clone(), this.FontScheme.Clone());
		}

		public override bool Equals(object obj)
		{
			DocumentTheme documentTheme = obj as DocumentTheme;
			return documentTheme != null && (base.Name.Equals(documentTheme.Name) && this.ColorScheme.Equals(documentTheme.ColorScheme)) && this.FontScheme.Equals(documentTheme.FontScheme);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(base.Name.GetHashCode(), this.ColorScheme.GetHashCode(), this.FontScheme.GetHashCode());
		}

		readonly ThemeColorScheme colorScheme;

		readonly ThemeFontScheme fontScheme;
	}
}
