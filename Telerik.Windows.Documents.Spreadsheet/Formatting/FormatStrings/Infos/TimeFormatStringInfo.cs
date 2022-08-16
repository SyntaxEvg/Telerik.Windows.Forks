using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Infos
{
	public class TimeFormatStringInfo : ICategorizedFormatStringInfo
	{
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		public TimeFormatStringInfo(CultureInfo culture)
		{
			Guard.ThrowExceptionIfNull<CultureInfo>(culture, "culture");
			this.culture = culture;
		}

		internal static bool TryCreate(string formatString, out TimeFormatStringInfo formatStringInfo)
		{
			string loweredFormatString = formatString.ToLower();
			loweredFormatString = loweredFormatString.Replace("tt", "AM/PM");
			if (DateTimeFormatCategoryManager.CultureInfoToTimeFormats.ContainsKey(FormatHelper.DefaultSpreadsheetCulture.CultureInfo))
			{
				IList<string> source = DateTimeFormatCategoryManager.CultureInfoToTimeFormats[FormatHelper.DefaultSpreadsheetCulture.CultureInfo];
				if (source.Any((string x) => x.Contains(loweredFormatString) && loweredFormatString != "@"))
				{
					formatStringInfo = new TimeFormatStringInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
					return true;
				}
			}
			formatStringInfo = null;
			return false;
		}

		readonly CultureInfo culture;
	}
}
