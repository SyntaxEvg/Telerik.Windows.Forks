using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.TextMeasurer;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders;
using Telerik.Windows.Documents.Spreadsheet.Measurement;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting
{
	public static class FormatHelper
	{
		internal static string ShortDatePattern
		{
			get
			{
				return FormatHelper.DefaultSpreadsheetCulture.CultureInfo.DateTimeFormat.ShortDatePattern;
			}
		}

		internal static string LongTimePattern
		{
			get
			{
				return FormatHelper.DefaultSpreadsheetCulture.CultureInfo.DateTimeFormat.LongTimePattern;
			}
		}

		internal static string ShortDateLongTimePattern
		{
			get
			{
				return string.Format("{0} {1}", FormatHelper.ShortDatePattern, FormatHelper.LongTimePattern);
			}
		}

		internal static SpreadsheetCultureHelper DefaultSpreadsheetCulture { get; set; } = new SpreadsheetCultureHelper();

		public static DateTime? ConvertDoubleToDateTime(double doubleValue)
		{
			if (doubleValue < 0.0)
			{
				return null;
			}
			double num = doubleValue;
			if (num <= 60.0)
			{
				num += 1.0;
			}
			long ticks;
			DateTime? result;
			if (FormatHelper.TryConvertDoubleDateToTicks(num, out ticks))
			{
				result = new DateTime?(new DateTime(ticks, DateTimeKind.Unspecified));
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static double ConvertDateTimeToDouble(DateTime dateTime)
		{
			double num = dateTime.ToOADate();
			if (num <= 60.0 && num >= 1.0)
			{
				num -= 1.0;
			}
			return num;
		}

		public static DateTime RoundMinutes(this DateTime dateTimeValue)
		{
			int millisecond = dateTimeValue.Millisecond;
			DateTime result = new DateTime(dateTimeValue.Year, dateTimeValue.Month, dateTimeValue.Day, dateTimeValue.Hour, dateTimeValue.Minute, dateTimeValue.Second);
			if (millisecond >= 500)
			{
				result = result.AddSeconds(1.0);
			}
			return result;
		}

		public static DateTime RoundMilliseconds(this DateTime dateTimeValue, int precision)
		{
			int millisecond = dateTimeValue.Millisecond;
			string text = millisecond.ToString();
			DateTime result = dateTimeValue;
			if (precision > 0 && text.Length > precision)
			{
				string format = "0." + string.Concat<char>(Enumerable.Repeat<char>('0', precision));
				string text2 = ((double)millisecond / Math.Pow(10.0, (double)text.Length)).ToString(format);
				int num = int.Parse(text2.Substring(2));
				int num2 = text.Length - num.ToString().Length;
				num *= (int)Math.Pow(10.0, (double)num2);
				result = new DateTime(result.Year, result.Month, result.Day, result.Hour, result.Minute, result.Second, num);
			}
			return result;
		}

		public static DateTime? ToDateTime(this NumberCellValue numberCellValue)
		{
			return FormatHelper.ConvertDoubleToDateTime(numberCellValue.Value);
		}

		internal static Color? GetForegroundOrDefault(this FormatDescriptor formatDescriptor)
		{
			if (formatDescriptor == null)
			{
				return FormatHelper.DefaultFormatForeground;
			}
			return formatDescriptor.Foreground;
		}

		internal static CultureInfo GetCultureOrDefault(this FormatDescriptor formatDescriptor)
		{
			if (formatDescriptor == null)
			{
				return FormatHelper.DefaultFormatCulture;
			}
			return formatDescriptor.Culture;
		}

		internal static Predicate<double> GetConditionOrDefault(this FormatDescriptor formatDescriptor)
		{
			if (formatDescriptor == null)
			{
				return FormatHelper.DefaultFormatCondition;
			}
			return formatDescriptor.Condition;
		}

		internal static double Round(double value)
		{
			if (value > 0.0)
			{
				return Math.Floor(value);
			}
			return Math.Ceiling(value);
		}

		internal static string ReplaceCultureSpecificSeparatorsWithDefaults(string formatString)
		{
			return FormatHelper.ReplaceSeparators(formatString, (char c) => SpreadsheetCultureHelper.IsCharEqualTo(c, new string[]
			{
				FormatHelper.DefaultSpreadsheetCulture.NumberGroupSeparator,
				FormatHelper.DefaultSpreadsheetCulture.CurrencyGroupSeparator
			}), (char c) => SpreadsheetCultureHelper.IsCharEqualTo(c, new string[]
			{
				FormatHelper.DefaultSpreadsheetCulture.NumberDecimalSeparator,
				FormatHelper.DefaultSpreadsheetCulture.CurrencyDecimalSeparator
			}), CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator, CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
		}

		internal static string ReplaceDefaultSeparatorsWithCultureSpecific(string formatString)
		{
			return FormatHelper.ReplaceSeparators(formatString, (char c) => SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator }), (char c) => SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator }), FormatHelper.DefaultSpreadsheetCulture.NumberGroupSeparator, FormatHelper.DefaultSpreadsheetCulture.NumberDecimalSeparator);
		}

		static string ReplaceSeparators(string formatString, Predicate<char> shouldReplaceWithGroupSeparator, Predicate<char> shouldReplaceWithDecimalSeparator, string newGroupSeparator, string newDecimalSeparator)
		{
			string currencySymbol = FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol;
			string text = BuildersHelper.MakeCurrencySymbolInvisible(currencySymbol);
			int[] startIndeces = FormatHelper.FindAllOccurrences(formatString, currencySymbol);
			int[] startIndeces2 = FormatHelper.FindAllOccurrences(formatString, text);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < formatString.Length; i++)
			{
				char c = formatString[i];
				if (FormatHelper.CheckIfIsCurrencySymbolPart(startIndeces, currencySymbol, i) || FormatHelper.CheckIfIsCurrencySymbolPart(startIndeces2, text, i))
				{
					stringBuilder.Append(c);
				}
				else if (c == '"')
				{
					flag2 = !flag2;
					stringBuilder.Append(c);
				}
				else if (!flag2)
				{
					if (c == '[')
					{
						flag = true;
					}
					else if (c == ']')
					{
						flag = false;
					}
					if (!flag && shouldReplaceWithGroupSeparator(c))
					{
						stringBuilder.Append(newGroupSeparator);
					}
					else if (!flag && shouldReplaceWithDecimalSeparator(c))
					{
						stringBuilder.Append(newDecimalSeparator);
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		internal static bool CheckIfIsCurrencySymbolPart(int[] startIndeces, string currencySymbol, int currentCharIndex)
		{
			bool result = false;
			foreach (int num in startIndeces)
			{
				int num2 = num + currencySymbol.Length - 1;
				if (num <= currentCharIndex && currentCharIndex <= num2)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		internal static int[] FindAllOccurrences(string value, string part)
		{
			List<int> list = new List<int>();
			for (int i = value.IndexOf(part, StringComparison.Ordinal); i >= 0; i = value.IndexOf(part))
			{
				list.Add(list.Count * part.Length + i);
				value = value.Remove(i, part.Length);
			}
			return list.ToArray();
		}

		internal static CultureInfo GetCurrentCulture()
		{
			return Thread.CurrentThread.CurrentCulture;
		}

		internal static DateTime? RoundUpValue(string formatString, DateTime? dateTimeValue)
		{
			DateTime? result = dateTimeValue;
			if (result == null || result.Value.Millisecond == 0)
			{
				return result;
			}
			if (!formatString.Contains(FormatHelper.MilliSecondsChar))
			{
				result = new DateTime?(result.Value.RoundMinutes());
			}
			else
			{
				int precision = (from c in formatString
					where c == FormatHelper.MilliSecondsChar
					select c).Count<char>();
				result = new DateTime?(result.Value.RoundMilliseconds(precision));
			}
			return result;
		}

		internal static string ExpandSymbol(string symbol, double availableWidth, global::Telerik.Windows.Documents.Spreadsheet.Model.FontProperties fontProperties)
		{
			global::Telerik.Windows.Documents.Core.TextMeasurer.TextMeasurementInfo textMeasurementInfo = global::Telerik.Windows.Documents.Spreadsheet.Measurement.RadTextMeasurer.Measure(symbol, fontProperties, null);
			double width = textMeasurementInfo.Size.Width;
			int count = (int)(availableWidth / width);
			string text = string.Concat(global::System.Linq.Enumerable.Repeat<string>(symbol, count));
			double width2 = global::Telerik.Windows.Documents.Spreadsheet.Measurement.RadTextMeasurer.Measure(text, fontProperties, null).Size.Width;
			while (width2 - availableWidth > 0.1 && text.Length > 0)
			{
				text = text.Substring(0, text.Length - 1);
				width2 = global::Telerik.Windows.Documents.Spreadsheet.Measurement.RadTextMeasurer.Measure(text, fontProperties, null).Size.Width;
			}
			return text;
		}

		internal static string StripUnderscores(string text)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			while (i < text.Length)
			{
				if (text[i] != '_' || i == text.Length - 1)
				{
					stringBuilder.Append(text[i]);
					i++;
				}
				else
				{
					i += 2;
				}
			}
			return stringBuilder.ToString();
		}

		internal static string StripBrackets(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			text = text.Trim();
			int num = text.IndexOf('[');
			if (num != -1)
			{
				int num2 = text.IndexOf(']', num);
				while (num != -1 && num < num2)
				{
					text = text.Remove(num, num2 - num + 1);
					text = text.Trim();
					num = text.IndexOf('[');
					if (num != -1)
					{
						num2 = text.IndexOf(']', num);
					}
				}
			}
			return text;
		}

		internal static string StripCurrencyEscapingsAndSlashes(string text)
		{
			string currencySymbol = FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol;
			string currencySymbol2 = BuildersHelper.MakeCurrencySymbolInvisible(currencySymbol);
			text = FormatHelper.RemoveCurrencySymbolEscapingsAndSlashes(text, currencySymbol);
			text = FormatHelper.RemoveCurrencySymbolEscapingsAndSlashes(text, currencySymbol2);
			return text;
		}

		static string RemoveCurrencySymbolEscapingsAndSlashes(string text, string currencySymbol)
		{
			for (int i = text.IndexOf(currencySymbol, StringComparison.Ordinal); i != -1; i = text.IndexOf(currencySymbol, StringComparison.Ordinal))
			{
				int num = currencySymbol.Length;
				while (i > 0)
				{
					if (text[i - 1] != '\\' && text[i - 1] != '"')
					{
						break;
					}
					i--;
					num++;
				}
				while (i + num + 1 < text.Length && text[i + num] == '"')
				{
					num++;
				}
				text = text.Remove(i, num);
			}
			return text;
		}

		internal static bool ContainsTotalHoursMinutesSecondsModifiers(string formatString)
		{
			StringBuilder stringBuilder = new StringBuilder(formatString);
			if (stringBuilder.Length == 0)
			{
				return false;
			}
			stringBuilder.Replace(stringBuilder.ToString(), stringBuilder.ToString().Trim());
			int num;
			while ((num = stringBuilder.ToString().IndexOf('[')) != -1)
			{
				int num2 = stringBuilder.ToString().IndexOf(']', num);
				if (num2 == -1)
				{
					break;
				}
				string a = stringBuilder.ToString().Substring(num + 1, num2 - num - 1);
				if (a == "h" || a == "m" || a == "s")
				{
					return true;
				}
				stringBuilder.Remove(num, num2 - num + 1);
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Replace(stringBuilder.ToString(), stringBuilder.ToString().Trim());
				}
			}
			return false;
		}

		internal static bool TryGetDecimalPlacesForFormatString(string formatString, CultureInfo cultureInfo, out int decimalPlaces)
		{
			bool flag;
			return FormatHelper.TryGetDecimalPlacesForFormatString(formatString, cultureInfo, out decimalPlaces, out flag);
		}

		internal static bool TryGetDecimalPlacesForFormatString(string formatString, CultureInfo cultureInfo, out int decimalPlaces, out bool useThousandsSeparator)
		{
			string pattern = string.Format("(?<{0}>#{1}##)?0({2}(?<{3}>0+))?", new object[]
			{
				FormatHelper.UseThousandsSeparatorGroupName,
				cultureInfo.NumberFormat.NumberGroupSeparator,
				cultureInfo.NumberFormat.NumberDecimalSeparator,
				FormatHelper.ZeroCountGroupName
			});
			decimalPlaces = -1;
			useThousandsSeparator = false;
			Regex regex = new Regex(pattern);
			MatchCollection matchCollection = regex.Matches(formatString);
			if (matchCollection.Count > 0)
			{
				Match match = matchCollection[0];
				foreach (object obj in matchCollection)
				{
					Match match2 = (Match)obj;
					if (match2.Length > match.Length)
					{
						match = match2;
					}
				}
				decimalPlaces = match.Groups[FormatHelper.ZeroCountGroupName].Value.Length;
				useThousandsSeparator = match.Groups[FormatHelper.UseThousandsSeparatorGroupName].Success;
				return true;
			}
			return false;
		}

		internal static string GetValuePatternConsiderDecimalPlaces(int decimalPlaces, bool useThousandsSeparator = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (useThousandsSeparator)
			{
				stringBuilder.Append(string.Format("#{0}##", FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.NumberGroupSeparator));
			}
			stringBuilder.Append("0");
			if (decimalPlaces > 0)
			{
				stringBuilder.Append(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.NumberDecimalSeparator);
				for (int i = 0; i < decimalPlaces; i++)
				{
					stringBuilder.Append("0");
				}
			}
			return stringBuilder.ToString();
		}

		internal static ICellValue GetCellValueRespectFormat(CellValueFormat format, ICellValue currentCellValue)
		{
			ICellValue result = currentCellValue;
			string text;
			double doubleValue;
			if (format.FormatStringInfo.FormatType == FormatStringType.Text)
			{
				NumberCellValue numberCellValue = currentCellValue as NumberCellValue;
				if (numberCellValue != null)
				{
					result = new TextCellValue(numberCellValue.Value.ToString(FormatHelper.DefaultSpreadsheetCulture.CultureInfo));
				}
				BooleanCellValue booleanCellValue = currentCellValue as BooleanCellValue;
				if (booleanCellValue != null)
				{
					result = new TextCellValue(booleanCellValue.GetResultValueAsString(CellValueFormat.GeneralFormat).ToString());
				}
				FormulaCellValue formulaCellValue = currentCellValue as FormulaCellValue;
				if (formulaCellValue != null)
				{
					result = new TextCellValue(formulaCellValue.GetValueAsString(format));
				}
			}
			else if (currentCellValue.ValueType == CellValueType.Text && NumberValueParser.TryParse(currentCellValue.GetValueAsString(format), null, null, out text, out doubleValue))
			{
				result = new NumberCellValue(doubleValue);
			}
			return result;
		}

		static bool TryConvertDoubleDateToTicks(double value, out long ticks)
		{
			ticks = -1L;
			if (value >= 2958466.0 || value <= -657435.0)
			{
				return false;
			}
			double num = value * 86400000.0;
			double num2;
			if (value >= 0.0)
			{
				num2 = 0.5;
			}
			else
			{
				num2 = -0.5;
			}
			long num3 = (long)(num + num2);
			if (num3 < 0L)
			{
				num3 -= num3 % 86400000L * 2L;
			}
			num3 += 59926435200000L;
			if (num3 < 0L || num3 >= 315537897600000L)
			{
				return false;
			}
			ticks = num3 * 10000L;
			return true;
		}

		internal static readonly string[] PositivePatternStrings = new string[] { "$n", "n$", "$ n", "n $" };

		internal static readonly string[] NegativePatternStrings = new string[]
		{
			"($n)", "-$n", "$-n", "$n-", "(n$)", "-n$", "n-$", "n$-", "-n $", "-$ n",
			"n $-", "$ n-", "$ -n", "n- $", "($ n)", "(n $)"
		};

		internal static readonly string ZeroCountGroupName = "zeroCount";

		internal static readonly string UseThousandsSeparatorGroupName = "useThousandsSeparator";

		public static readonly DateTime StartDate = new DateTime(1900, 1, 1, 0, 0, 0);

		internal static readonly string IsRtlRegExPattern = "[\\p{IsArabic}\\p{IsHebrew}]";

		internal static readonly string GeneralFormatString = string.Empty;

		internal static readonly string GeneralFormatName = "General";

		internal static readonly string TextPlaceholder = "@";

		internal static readonly double MinimumScientificValue = 100000000000.0;

		internal static readonly char FractionsInvisibleChar = '1';

		internal static readonly char MilliSecondsChar = 'f';

		internal static readonly string InvisibleDivider = "q";

		internal static readonly double InvisibleDividerFontsize = 0.01;

		internal static readonly char[] NumberChars = new char[] { '0', '#' };

		internal static readonly char[] DateChars = new char[] { 'y', 'm', 'd' };

		internal static readonly char[] TimeChars = new char[] { 'h', 's' };

		internal static readonly double MaxDoubleValueTranslatableToDateTime = 2958465.0;

		internal static readonly Color? DefaultFormatForeground = null;

		internal static readonly CultureInfo DefaultFormatCulture = null;

		internal static readonly Predicate<double> DefaultFormatCondition = null;

		internal static readonly double MaxGeneralNumber = 1E+21;

		internal static readonly double MinGeneralNumber = 1E-19;

		internal static readonly string GeneralNumberEditFormatString = "0.###################";

		internal static readonly string GeneralNumberScientificEditFormatString = "0.##############E+00";
	}
}
