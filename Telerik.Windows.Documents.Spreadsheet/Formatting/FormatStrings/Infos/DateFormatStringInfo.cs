using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Infos
{
	public class DateFormatStringInfo : ICategorizedFormatStringInfo
	{
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		public DateFormatStringInfo(CultureInfo culture)
		{
			Guard.ThrowExceptionIfNull<CultureInfo>(culture, "culture");
			this.culture = culture;
		}

		internal static bool TryCreate(string formatString, out DateFormatStringInfo formatStringInfo)
		{
			string loweredFormatString = formatString.ToLower();
			if (DateTimeFormatCategoryManager.CultureInfoToDateFormats.ContainsKey(FormatHelper.DefaultSpreadsheetCulture.CultureInfo))
			{
				IList<string> source = DateTimeFormatCategoryManager.CultureInfoToDateFormats[FormatHelper.DefaultSpreadsheetCulture.CultureInfo];
				if (source.Any((string x) => x.Contains(loweredFormatString) && loweredFormatString != "@"))
				{
					formatStringInfo = new DateFormatStringInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
					return true;
				}
			}
			formatStringInfo = null;
			return false;
		}

		readonly CultureInfo culture;
	}
}
