using System;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting
{
	static class NumberValueParser
	{
		public static bool TryParse(string value, FormatStringType? forceParsingAsSpecificType, SpreadsheetCultureHelper cultureInfo, out string format, out double doubleValue)
		{
			doubleValue = 0.0;
			format = FormatHelper.GeneralFormatString;
			cultureInfo = cultureInfo ?? FormatHelper.DefaultSpreadsheetCulture;
			foreach (NumberValueParser.TryParseNumberValueDelegate tryParseNumberValueDelegate in NumberValueParser.TryParseFunctions)
			{
				if (tryParseNumberValueDelegate(value, forceParsingAsSpecificType, cultureInfo, out doubleValue, out format))
				{
					return true;
				}
			}
			return false;
		}

		static bool TryParseGeneral(string value, FormatStringType? forceParsingAsSpecificType, SpreadsheetCultureHelper cultureInfo, out double parsedDoubleValue, out string format)
		{
			format = string.Empty;
			return cultureInfo.TryParseGeneral(value, out parsedDoubleValue);
		}

		static bool TryParseNumber(string value, FormatStringType? forceParsingAsSpecificType, SpreadsheetCultureHelper cultureInfo, out double result, out string format)
		{
			format = FormatHelper.GeneralFormatString;
			result = 0.0;
			bool flag = cultureInfo.TryParseNumber(value, out result);
			if (flag)
			{
				format = NumberValueParser.GetNumberFormat(value, cultureInfo);
			}
			return flag;
		}

		static bool TryParseCurrency(string value, FormatStringType? forceParsingAsSpecificType, SpreadsheetCultureHelper cultureInfo, out double result, out string format)
		{
			return CurrencyParser.TryParse(value, out result, out format);
		}

		static bool TryParseFraction(string value, FormatStringType? forceParsingAsSpecificType, SpreadsheetCultureHelper cultureInfo, out double result, out string format)
		{
			return FractionParser.TryParse(value, forceParsingAsSpecificType, out result, out format);
		}

		static bool TryParseDate(string value, FormatStringType? forceParsingAsSpecificType, SpreadsheetCultureHelper cultureInfo, out double result, out string format)
		{
			DateTime dateTime;
			if (DateTimeParser.TryParse(value, out dateTime, out format))
			{
				result = FormatHelper.ConvertDateTimeToDouble(dateTime);
				return true;
			}
			result = 0.0;
			format = FormatHelper.GeneralFormatString;
			return false;
		}

		static bool TryParsePercent(string value, FormatStringType? forceParsingAsSpecificType, SpreadsheetCultureHelper cultureInfo, out double result, out string format)
		{
			format = FormatHelper.GeneralFormatString;
			bool flag = cultureInfo.TryParsePercent(value, out result);
			if (flag)
			{
				format = NumberValueParser.GetPercentFormat(value, cultureInfo);
			}
			return flag;
		}

		static bool TryParseScientific(string value, FormatStringType? forceParsingAsSpecificType, SpreadsheetCultureHelper cultureInfo, out double result, out string format)
		{
			format = FormatHelper.GeneralFormatString;
			bool flag = cultureInfo.TryParseScientific(value, out result);
			if (flag)
			{
				format = string.Format(NumberValueParser.DefaultScientificFormat, cultureInfo.NumberDecimalSeparator);
			}
			return flag;
		}

		static string GetNumberFormat(string value, SpreadsheetCultureHelper cultureInfo)
		{
			if (value.IndexOf(cultureInfo.CultureInfo.NumberFormat.NumberDecimalSeparator) != -1)
			{
				return string.Format(NumberValueParser.DefaultNumberDecimalFormat, cultureInfo.NumberGroupSeparator, cultureInfo.NumberDecimalSeparator);
			}
			return string.Format(NumberValueParser.DefaultNumberFormat, cultureInfo.NumberGroupSeparator);
		}

		static string GetPercentFormat(string value, SpreadsheetCultureHelper cultureInfo)
		{
			int num;
			if ((num = value.IndexOf(cultureInfo.CultureInfo.NumberFormat.PercentDecimalSeparator)) != -1)
			{
				int num2 = 0;
				int num3 = num + 1;
				while (num3 < value.Length && char.IsDigit(value[num3]))
				{
					num2++;
					num3++;
				}
				return string.Format(NumberValueParser.DefaultPercentDecimalFormat, cultureInfo.NumberDecimalSeparator);
			}
			return NumberValueParser.DefaultPercentFormat;
		}

		static readonly string DefaultNumberFormat = "#{0}##0";

		static readonly string DefaultNumberDecimalFormat = "#{0}##0{1}00";

		static readonly string DefaultPercentFormat = "0%";

		static readonly string DefaultPercentDecimalFormat = "0{0}00%";

		static readonly string DefaultScientificFormat = "0{0}00E+00";

		static NumberValueParser.TryParseNumberValueDelegate[] TryParseFunctions = new NumberValueParser.TryParseNumberValueDelegate[]
		{
			new NumberValueParser.TryParseNumberValueDelegate(NumberValueParser.TryParseGeneral),
			new NumberValueParser.TryParseNumberValueDelegate(NumberValueParser.TryParseNumber),
			new NumberValueParser.TryParseNumberValueDelegate(NumberValueParser.TryParseCurrency),
			new NumberValueParser.TryParseNumberValueDelegate(NumberValueParser.TryParseFraction),
			new NumberValueParser.TryParseNumberValueDelegate(NumberValueParser.TryParseDate),
			new NumberValueParser.TryParseNumberValueDelegate(NumberValueParser.TryParsePercent),
			new NumberValueParser.TryParseNumberValueDelegate(NumberValueParser.TryParseScientific)
		};

		delegate bool TryParseNumberValueDelegate(string value, FormatStringType? forceParsingAsSpecificType, SpreadsheetCultureHelper cultureInfo, out double parsedDoubleValue, out string format);
	}
}
