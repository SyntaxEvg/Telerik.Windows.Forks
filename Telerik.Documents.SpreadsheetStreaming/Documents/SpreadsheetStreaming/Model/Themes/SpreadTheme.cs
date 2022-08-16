using System;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Themes
{
	class SpreadTheme : NamedObjectBase
	{
		public SpreadTheme(string name, SpreadThemeColorScheme colorScheme, SpreadThemeFontScheme fontScheme)
			: base(name)
		{
			this.colorScheme = colorScheme;
			this.fontScheme = fontScheme;
		}

		public SpreadThemeColorScheme ColorScheme
		{
			get
			{
				return this.colorScheme;
			}
		}

		public SpreadThemeFontScheme FontScheme
		{
			get
			{
				return this.fontScheme;
			}
		}

		public override bool Equals(object obj)
		{
			SpreadTheme spreadTheme = obj as SpreadTheme;
			return spreadTheme != null && (base.Name.Equals(spreadTheme.Name) && this.ColorScheme.Equals(spreadTheme.ColorScheme)) && this.FontScheme.Equals(spreadTheme.FontScheme);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(base.Name.GetHashCode(), this.ColorScheme.GetHashCode(), this.FontScheme.GetHashCode());
		}

		readonly SpreadThemeColorScheme colorScheme;

		readonly SpreadThemeFontScheme fontScheme;
	}
}
