using System;
using System.Globalization;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Formatting
{
	static class DateTimeFormatter
	{
		public static string FormatDate(DateTime date, string formatString)
		{
			if (string.IsNullOrEmpty(formatString))
			{
				formatString = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
			}
			return DateTimeFormatter.FormatDateTime(date, formatString);
		}

		public static string FormatTime(DateTime date, string formatString)
		{
			if (string.IsNullOrEmpty(formatString))
			{
				formatString = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
			}
			return DateTimeFormatter.FormatDateTime(date, formatString);
		}

		static string FormatDateTime(DateTime date, string formatString)
		{
			formatString = formatString.Replace("am/pm", "tt").Replace("AM/PM", "tt");
			return date.ToString(formatString, CultureInfo.InvariantCulture);
		}
	}
}
