using System;
using System.Globalization;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting
{
	static class CurrencyParser
	{
		public static bool TryParse(string value, out double result, out string format)
		{
			format = FormatHelper.GeneralFormatString;
			bool flag = FormatHelper.DefaultSpreadsheetCulture.TryParseCurrency(value, out result);
			if (flag)
			{
				format = CurrencyParser.GetCurrencyFormat(value);
			}
			return flag;
		}

		static string GetCurrencyFormat(string value)
		{
			CultureInfo cultureInfo = FormatHelper.DefaultSpreadsheetCulture.CultureInfo;
			string text = value.Replace(cultureInfo.NumberFormat.CurrencySymbol, "");
			string arg = string.Format(CurrencyParser.DefaultCurrencyFormat, cultureInfo.NumberFormat.CurrencyGroupSeparator);
			if (text.IndexOf(cultureInfo.NumberFormat.NumberDecimalSeparator) != -1)
			{
				arg = string.Format(CurrencyParser.DefaultCurrencyDecimalFormat, cultureInfo.NumberFormat.CurrencyGroupSeparator, cultureInfo.NumberFormat.CurrencyDecimalSeparator);
			}
			int currencyPositivePattern = cultureInfo.NumberFormat.CurrencyPositivePattern;
			int currencyNegativePattern = cultureInfo.NumberFormat.CurrencyNegativePattern;
			string text2 = FormatHelper.PositivePatternStrings[currencyPositivePattern];
			string text3 = FormatHelper.NegativePatternStrings[currencyNegativePattern];
			string str = "[Red]" + text3.Replace("$", CurrencyParser.GetLiteralCurrencySymbol()).Replace("-", cultureInfo.NumberFormat.NegativeSign).Replace("n", "{0}");
			text2 = string.Format(CurrencyParser.NegativeToPositivePatternStrings[currencyNegativePattern], text2);
			string str2 = text2.Replace("$", CurrencyParser.GetLiteralCurrencySymbol()).Replace("-", cultureInfo.NumberFormat.NegativeSign).Replace("n", "{0}");
			return string.Format(str2 + ";" + str, arg);
		}

		static string GetLiteralCurrencySymbol()
		{
			string currencySymbol = FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol;
			return string.Format("\"{0}\"", currencySymbol);
		}

		static readonly string DefaultCurrencyFormat = "#{0}##0";

		static readonly string DefaultCurrencyDecimalFormat = "#{0}##0{1}00";

		static readonly string[] NegativeToPositivePatternStrings = new string[]
		{
			"{0}_)", "{0}", "{0}", "{0}_-", "{0}_)", "{0}", "{0}", "{0}_-", "{0}", "{0}",
			"{0}_-", "{0}_-", "{0}", "{0}", "{0}_)", "{0}_)"
		};
	}
}
