using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting
{
	static class DateTimeParser
	{
		static string DefaultDayMonthYearFormat
		{
			get
			{
				return FormatHelper.ShortDatePattern.ToLower();
			}
		}

		static string DefaultDateTimeFormat
		{
			get
			{
				return string.Format("{0} {1}", DateTimeParser.DefaultDayMonthYearFormat, DateTimeParser.DefaultHourMinuteFormat);
			}
		}

		public static bool TryParse(string value, out DateTime result, out string format)
		{
			format = FormatHelper.GeneralFormatString;
			DateTime dateTime = FormatHelper.StartDate.AddDays(-1.0);
			bool flag = DateTimeParser.TryParseMonthYearDateTime(value, out result) || DateTimeParser.TryParseMonthNameYearDateTime(value, out result);
			bool flag2 = false;
			if (!flag)
			{
				flag2 = FormatHelper.DefaultSpreadsheetCulture.TryParseDateTime(value, out result);
				flag = flag2;
			}
			if (flag && result.Date == DateTime.MinValue)
			{
				result = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, result.Hour, result.Minute, result.Second, result.Millisecond);
				flag &= result > dateTime;
				if (flag)
				{
					result = result.RoundMilliseconds(1);
					format = DateTimeParser.GetDateTimeFormat(new DateTime?(result), value, false);
				}
			}
			TimeSpan value2;
			bool flag3 = FormatHelper.DefaultSpreadsheetCulture.TryParseTimeSpan(value, out value2);
			if (!flag && !flag3)
			{
				flag3 = DateTimeParser.TryParseMinuteSecondMillisecondTimeSpan(value, out value2) || DateTimeParser.TryParseMinuteSecondMillisecondAmPmTimeSpan(value, out value2);
			}
			if ((!flag || (flag && flag3)) && flag3)
			{
				result = dateTime;
				result = result.Add(value2);
				flag3 &= result >= dateTime;
				if (flag3)
				{
					result = result.RoundMilliseconds(1);
					format = DateTimeParser.GetDateTimeFormat(new DateTime?(result), value, false);
				}
				return flag3;
			}
			flag &= result >= dateTime;
			if (flag)
			{
				result = result.RoundMilliseconds(1);
				format = DateTimeParser.GetDateTimeFormat(new DateTime?(result), value, flag2);
			}
			return flag;
		}

		static bool TryParseMinuteSecondMillisecondAmPmTimeSpan(string value, out TimeSpan parsedTimeSpan)
		{
			parsedTimeSpan = TimeSpan.MinValue;
			if (!DateTimeParser.TimeMillisecondsAmPmRegex.IsMatch(value))
			{
				return false;
			}
			Match match = DateTimeParser.TimeMillisecondsAmPmRegex.Match(value);
			string value2 = match.Groups[DateTimeParser.MinuteGroupName].Value;
			string value3 = match.Groups[DateTimeParser.SecondGroupName].Value;
			string value4 = match.Groups[DateTimeParser.MillisecondGroupName].Value;
			string value5 = match.Groups[DateTimeParser.AmPmGroupName].Value;
			string value6 = string.Format("{0}:{1}.{2}", value2, value3, value4);
			bool flag = DateTimeParser.TryParseMinuteSecondMillisecondTimeSpan(value6, out parsedTimeSpan);
			if (flag && value5 == "PM")
			{
				parsedTimeSpan = parsedTimeSpan.Add(TimeSpan.FromHours(12.0));
			}
			return flag;
		}

		static bool TryParseMinuteSecondMillisecondTimeSpan(string value, out TimeSpan parsedTimeSpan)
		{
			parsedTimeSpan = TimeSpan.Zero;
			if (!DateTimeParser.TimeMillisecondsRegex.IsMatch(value))
			{
				return false;
			}
			Match match = DateTimeParser.TimeMillisecondsRegex.Match(value);
			string value2 = match.Groups[DateTimeParser.MinuteGroupName].Value;
			string value3 = match.Groups[DateTimeParser.SecondGroupName].Value;
			string value4 = match.Groups[DateTimeParser.MillisecondGroupName].Value;
			string format = string.Concat(new string[]
			{
				string.Concat<char>(Enumerable.Repeat<char>('m', value2.Length)),
				"\\:",
				string.Concat<char>(Enumerable.Repeat<char>('s', value3.Length)),
				"\\.",
				string.Concat<char>(Enumerable.Repeat<char>('f', value4.Length))
			});
			return TimeSpan.TryParseExact(value, format, null, out parsedTimeSpan);
		}

		static bool TryParseMonthYearDateTime(string value, out DateTime parsedDateTime)
		{
			string text;
			return DateTimeParser.TryParseMonthYearDateTime(value, out parsedDateTime, out text);
		}

		static bool TryParseMonthYearDateTime(string value, out DateTime parsedDateTime, out string newFormat)
		{
			parsedDateTime = DateTime.MinValue;
			DateTime startDate = FormatHelper.StartDate;
			newFormat = FormatHelper.GeneralFormatString;
			if (!DateTimeParser.DateRegex.IsMatch(value))
			{
				return false;
			}
			Match match = DateTimeParser.DateRegex.Match(value);
			int num = int.Parse(match.Groups[DateTimeParser.MonthGoupName].Value);
			int num2 = int.Parse(match.Groups[DateTimeParser.DayGroupName].Value);
			if (num > 12 || num < 1 || (num2 > 99 && num2 < startDate.Year) || num2 > DateTimeParser.MaxYearValue)
			{
				return false;
			}
			bool flag = num2 > 0 && num2 <= DateTimeParser.GetNumberOfDaysByMonth(num);
			if (flag && match.Groups[DateTimeParser.DayGroupName].Value.Length > 2)
			{
				return false;
			}
			if (num2 >= startDate.Year)
			{
				parsedDateTime = new DateTime(num2, num, startDate.Day);
				newFormat = DateTimeParser.DefaultMonthYearFormat;
			}
			else if (flag)
			{
				parsedDateTime = new DateTime(DateTime.Now.Year, num, num2);
				newFormat = DateTimeParser.DefaultDayMonthFormat;
			}
			else
			{
				parsedDateTime = new DateTime(startDate.Year + num2, num, 1);
				newFormat = DateTimeParser.DefaultMonthYearFormat;
			}
			return true;
		}

		static bool TryParseMonthNameYearDateTime(string value, out DateTime parsedDateTime)
		{
			string text;
			return DateTimeParser.TryParseMonthNameYearDateTime(value, out parsedDateTime, out text);
		}

		static bool TryParseMonthNameYearDateTime(string value, out DateTime parsedDateTime, out string newFormat)
		{
			parsedDateTime = DateTime.MinValue;
			DateTime startDate = FormatHelper.StartDate;
			newFormat = FormatHelper.GeneralFormatString;
			if (!DateTimeParser.DateRegex2.IsMatch(value))
			{
				return false;
			}
			Match match = DateTimeParser.DateRegex2.Match(value);
			string value2 = match.Groups[DateTimeParser.MonthGoupName].Value;
			int monthNumberByName = DateTimeParser.GetMonthNumberByName(value2);
			if (monthNumberByName == -1)
			{
				return false;
			}
			int num = int.Parse(match.Groups[DateTimeParser.DayGroupName].Value);
			if (num > 9999 || num < 0)
			{
				return false;
			}
			bool flag = num <= DateTimeParser.GetNumberOfDaysByMonth(monthNumberByName);
			if (flag)
			{
				parsedDateTime = new DateTime(DateTime.Now.Year, monthNumberByName, num);
				newFormat = DateTimeParser.DefaultDayMonthFormat;
			}
			else
			{
				if (num.ToString().Length == 2)
				{
					parsedDateTime = new DateTime(startDate.Year + num, monthNumberByName, 1);
				}
				else
				{
					parsedDateTime = new DateTime(num, monthNumberByName, 1);
				}
				newFormat = DateTimeParser.DefaultMonthYearFormat;
			}
			return true;
		}

		static string GetDateTimeFormat(DateTime? dateTime, string value, bool systemParsed)
		{
			string generalFormatString = FormatHelper.GeneralFormatString;
			if (DateTimeParser.TryParseUsingRegex(value, out generalFormatString))
			{
				return generalFormatString;
			}
			DateTime startDate = FormatHelper.StartDate;
			bool flag = dateTime.Value.Year != 1 || systemParsed;
			bool flag2 = dateTime.Value.Month != startDate.Month || systemParsed;
			bool flag3 = dateTime.Value.Day != startDate.Day || systemParsed;
			bool flag4 = dateTime.Value.Hour != startDate.Hour;
			bool flag5 = dateTime.Value.Minute != startDate.Minute;
			bool flag6 = dateTime.Value.Second != startDate.Second;
			bool flag7 = dateTime.Value.Millisecond != startDate.Millisecond;
			if ((flag || flag2 || flag3) && (flag4 || flag5 || flag6))
			{
				return DateTimeParser.DefaultDateTimeFormat;
			}
			if (flag && flag2 && flag3)
			{
				return DateTimeParser.DefaultDayMonthYearFormat;
			}
			if (flag && flag2)
			{
				return DateTimeParser.DefaultMonthYearFormat;
			}
			if (flag2 && flag3)
			{
				return DateTimeParser.DefaultDayMonthFormat;
			}
			if ((flag4 || flag5 || flag6) && flag7)
			{
				return DateTimeParser.DefaultMinuteSecondsMillisecondsFormat;
			}
			if (flag4 && flag5 && flag6)
			{
				return DateTimeParser.DefaultHourMinuteSecondsFormat;
			}
			if (flag4 && flag5)
			{
				return DateTimeParser.DefaultHourMinuteFormat;
			}
			return DateTimeParser.DefaultDateTimeFormat;
		}

		static bool TryParseUsingRegex(string value, out string newFormat)
		{
			newFormat = FormatHelper.GeneralFormatString;
			DateTime dateTime;
			if (DateTimeParser.TryParseMonthYearDateTime(value, out dateTime, out newFormat) || DateTimeParser.TryParseMonthNameYearDateTime(value, out dateTime, out newFormat))
			{
				return true;
			}
			foreach (Tuple<Regex, string> tuple in DateTimeParser.GetPredefinedDateTimeRegexes())
			{
				if (DateTimeParser.TryParseUsingPattern(value, tuple.Item1, tuple.Item2, out newFormat))
				{
					return true;
				}
			}
			return false;
		}

		static List<Tuple<Regex, string>> GetPredefinedDateTimeRegexes()
		{
			return new List<Tuple<Regex, string>>
			{
				new Tuple<Regex, string>(DateTimeParser.FullDateRegex, DateTimeParser.DefaultMinuteSecondsMillisecondsFormat),
				new Tuple<Regex, string>(DateTimeParser.TimeRegex, DateTimeParser.DefaultHourMinuteFormat),
				new Tuple<Regex, string>(DateTimeParser.TimeRegex1, DateTimeParser.DefaultHourMinuteSecondsFormat),
				new Tuple<Regex, string>(DateTimeParser.TimeMillisecondsRegex, DateTimeParser.DefaultMinuteSecondsMillisecondsFormat),
				new Tuple<Regex, string>(DateTimeParser.TimeMillisecondsRegex1, DateTimeParser.DefaultMinuteSecondsMillisecondsFormat),
				new Tuple<Regex, string>(DateTimeParser.TimeRegex2, DateTimeParser.DefaultHourMinuteFormat1),
				new Tuple<Regex, string>(DateTimeParser.TimeRegex3, DateTimeParser.DefaultHourMinuteSecondsFormat1),
				new Tuple<Regex, string>(DateTimeParser.TimeMillisecondsAmPmRegex, DateTimeParser.DefaultMinuteSecondsMillisecondsFormat),
				new Tuple<Regex, string>(DateTimeParser.TimeMillisecondsAmPmRegex1, DateTimeParser.DefaultMinuteSecondsMillisecondsFormat),
				new Tuple<Regex, string>(DateTimeParser.DateRegex1, DateTimeParser.DefaultDayMonthFormat),
				new Tuple<Regex, string>(DateTimeParser.DateRegex3, DateTimeParser.DefaultDayMonthYearFormat1)
			};
		}

		static bool TryParseUsingPattern(string value, Regex regex, string newFormatSuggestedValue, out string newFormatValue)
		{
			newFormatValue = FormatHelper.GeneralFormatString;
			if (regex.IsMatch(value))
			{
				newFormatValue = newFormatSuggestedValue;
				return true;
			}
			return false;
		}

		static int GetMonthNumberByName(string monthName)
		{
			string key;
			switch (key = monthName.ToLower())
			{
			case "jan":
			case "january":
				return 1;
			case "feb":
			case "february":
				return 2;
			case "mar":
			case "march":
				return 3;
			case "apr":
			case "april":
				return 4;
			case "may":
				return 5;
			case "jun":
			case "june":
				return 6;
			case "jul":
			case "july":
				return 7;
			case "aug":
			case "august":
				return 8;
			case "sep":
			case "september":
				return 9;
			case "oct":
			case "october":
				return 10;
			case "nov":
			case "november":
				return 11;
			case "dec":
			case "december":
				return 12;
			}
			return -1;
		}

		static int GetNumberOfDaysByMonth(int month)
		{
			switch (month)
			{
			case 1:
			case 3:
			case 5:
			case 7:
			case 8:
			case 10:
			case 12:
				return 31;
			case 2:
				if (DateTime.IsLeapYear(DateTime.Now.Year))
				{
					return 29;
				}
				return 28;
			case 4:
			case 6:
			case 9:
			case 11:
				return 30;
			default:
				throw new ArgumentException("Invalid month.");
			}
		}

		static readonly int MaxYearValue = 9999;

		static readonly string YearGroupName = "year";

		static readonly string MonthGoupName = "month";

		static readonly string DayGroupName = "day";

		static readonly string HourGroupName = "hour";

		static readonly string MinuteGroupName = "minute";

		static readonly string SecondGroupName = "second";

		static readonly string MillisecondGroupName = "millisecond";

		static readonly string AmPmGroupName = "ampm";

		static readonly Regex FullDateRegex = new Regex(string.Format("^(?<{0}>\\d+)/(?<{1}>\\d+)/(?<{2}>\\d+)\\s+(?<{3}>\\d+):(?<{4}>\\d+):(?<{5}>\\d+).(?<{6}>\\d+)$", new object[]
		{
			DateTimeParser.YearGroupName,
			DateTimeParser.MonthGoupName,
			DateTimeParser.DayGroupName,
			DateTimeParser.HourGroupName,
			DateTimeParser.MinuteGroupName,
			DateTimeParser.SecondGroupName,
			DateTimeParser.MillisecondGroupName
		}));

		static readonly Regex DateRegex = new Regex(string.Format("^(?<{0}>\\d{{1,2}})[-\\s/](?<{1}>(\\d{{1,2}}|\\d{{4}}))$", DateTimeParser.MonthGoupName, DateTimeParser.DayGroupName));

		static readonly Regex DateRegex1 = new Regex(string.Format("^(?<{0}>\\d+)[-\\s/](?<{1}>\\D{{3,9}})$", DateTimeParser.DayGroupName, DateTimeParser.MonthGoupName));

		static readonly Regex DateRegex2 = new Regex(string.Format("^(?<{0}>\\D{{3,9}})[-\\s/](?<{1}>\\d+)$", DateTimeParser.MonthGoupName, DateTimeParser.DayGroupName));

		static readonly Regex DateRegex3 = new Regex(string.Format("^(?<{0}>\\d+)-(?<{1}>\\D{{3,9}})-(?<{2}>\\d{{1,2}}|\\d{{4}})$", DateTimeParser.DayGroupName, DateTimeParser.MonthGoupName, DateTimeParser.YearGroupName));

		static readonly Regex TimeRegex = new Regex(string.Format("^(?<{0}>\\d+):(?<{1}>\\d+)\\s*$", DateTimeParser.HourGroupName, DateTimeParser.MinuteGroupName));

		static readonly Regex TimeRegex1 = new Regex(string.Format("^(?<{0}>\\d+):(?<{1}>\\d+):(?<{2}>\\d+)$\\s*", DateTimeParser.HourGroupName, DateTimeParser.MinuteGroupName, DateTimeParser.SecondGroupName));

		static readonly Regex TimeRegex2 = new Regex(string.Format("^(?<{0}>\\d+):(?<{1}>\\d+)\\s+[AP]M\\s*$", DateTimeParser.HourGroupName, DateTimeParser.MinuteGroupName));

		static readonly Regex TimeRegex3 = new Regex(string.Format("^(?<{0}>\\d+):(?<{1}>\\d+):(?<{2}>\\d+)\\s+[AP]M\\s*$", DateTimeParser.HourGroupName, DateTimeParser.MinuteGroupName, DateTimeParser.SecondGroupName));

		static readonly Regex TimeMillisecondsRegex = new Regex(string.Format("^(?<{0}>\\d+):(?<{1}>\\d+).(?<{2}>\\d+)\\s*$", DateTimeParser.MinuteGroupName, DateTimeParser.SecondGroupName, DateTimeParser.MillisecondGroupName));

		static readonly Regex TimeMillisecondsAmPmRegex = new Regex(string.Format("^(?<{0}>\\d+):(?<{1}>\\d+).(?<{2}>\\d+)\\s+(?<{3}>[AP]M)?\\s*$", new object[]
		{
			DateTimeParser.MinuteGroupName,
			DateTimeParser.SecondGroupName,
			DateTimeParser.MillisecondGroupName,
			DateTimeParser.AmPmGroupName
		}));

		static readonly Regex TimeMillisecondsRegex1 = new Regex(string.Format("^(?<{0}>\\d+):(?<{1}>\\d+):(?<{2}>\\d+).(?<{3}>\\d+)$", new object[]
		{
			DateTimeParser.HourGroupName,
			DateTimeParser.MinuteGroupName,
			DateTimeParser.SecondGroupName,
			DateTimeParser.MillisecondGroupName
		}));

		static readonly Regex TimeMillisecondsAmPmRegex1 = new Regex(string.Format("^(?<{0}>\\d+):(?<{1}>\\d+):(?<{2}>\\d+).(?<{3}>\\d+)\\s+(?<{4}>[AP]M)?\\s*$", new object[]
		{
			DateTimeParser.HourGroupName,
			DateTimeParser.MinuteGroupName,
			DateTimeParser.SecondGroupName,
			DateTimeParser.MillisecondGroupName,
			DateTimeParser.AmPmGroupName
		}));

		static readonly string DefaultDayMonthYearFormat1 = "d-mmm-yy";

		static readonly string DefaultDayMonthFormat = "d-mmm";

		static readonly string DefaultMonthYearFormat = "mmm-yy";

		static readonly string DefaultHourMinuteFormat = "h:mm";

		static readonly string DefaultHourMinuteFormat1 = "h:mm AM/PM";

		static readonly string DefaultHourMinuteSecondsFormat = "h:mm:ss";

		static readonly string DefaultHourMinuteSecondsFormat1 = "h:mm:ss AM/PM";

		static readonly string DefaultMinuteSecondsMillisecondsFormat = "mm:ss.0";
	}
}
