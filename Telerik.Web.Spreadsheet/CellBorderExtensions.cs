using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Web.Spreadsheet
{
	public static class CellBorderExtensions
	{
		public static BorderStyle ToBorderStyle(this CellBorder border, DocumentTheme theme)
		{
			return new BorderStyle
			{
				Color = border.Color.GetActualValue(theme).ToHex(),
				Size = new double?(border.Thickness)
			};
		}
	}
}
