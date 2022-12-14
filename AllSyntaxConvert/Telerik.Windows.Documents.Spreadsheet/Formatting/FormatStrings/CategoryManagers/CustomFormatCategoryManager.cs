using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers
{
	public static class CustomFormatCategoryManager
	{
		public static IList<string> FormatStrings
		{
			get
			{
				return CustomFormatCategoryManager.formatStrings;
			}
		}

		static readonly List<string> formatStrings = new List<string>
		{
			FormatHelper.GeneralFormatName,
			"0",
			"0.00",
			"#,##0",
			"#,##0.00",
			"#,##0_);(#,##0)",
			"#,##0_);[Red](#,##0)",
			"#,##0.00_);(#,##0.00)",
			"#,##0.00_);[Red](#,##0.00)",
			"$#,##0_);($#,##0)",
			"$#,##0_);[Red]($#,##0)",
			"$#,##0.00_);($#,##0.00)",
			"$#,##0.00_);[Red]($#,##0.00)",
			"0%",
			"0.00%",
			"0.00E+00",
			"##0.0E+0",
			"# ?/?",
			"# ??/??",
			"m/d/yyyy",
			"d-mmm-yy",
			"d-mmm",
			"mmm-yy",
			"h:mm AM/PM",
			"h:mm:ss AM/PM",
			"h:mm",
			"h:mm:ss",
			"m/d/yyyy h:mm",
			"mm:ss",
			"mm:ss.0",
			"@",
			"[h]:mm:ss",
			"_($* #,##0_);_($* (#,##0);_($* \"-\"_);_(@_)",
			"_(* #,##0_);_(* (#,##0);_(* \"-\"_);_(@_)",
			"_($* #,##0.00_);_($* (#,##0.00);_($* \"-\"??_);_(@_)",
			"_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)"
		};
	}
}
