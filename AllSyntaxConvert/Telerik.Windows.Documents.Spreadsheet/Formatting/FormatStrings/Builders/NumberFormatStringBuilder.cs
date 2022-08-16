using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders
{
	public static class NumberFormatStringBuilder
	{
		public static List<string> BuildFormatStrings(int decimalPlaces, bool useThousandSeparator)
		{
			return BuildersHelper.GetCurrencyStringsForCurrentCulture(FormatHelper.DefaultSpreadsheetCulture.CultureInfo, string.Empty, decimalPlaces, useThousandSeparator);
		}
	}
}
