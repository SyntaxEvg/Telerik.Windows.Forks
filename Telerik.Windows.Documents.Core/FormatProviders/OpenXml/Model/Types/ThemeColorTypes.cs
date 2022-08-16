using System;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	static class ThemeColorTypes
	{
		public static ThemeColorType GetThemeColorTypeByIndex(int index)
		{
			return (ThemeColorType)index;
		}
	}
}
