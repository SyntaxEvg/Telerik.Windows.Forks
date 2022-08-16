using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders
{
	public static class AccountingFormatStringBuilder
	{
		static AccountingFormatStringBuilder()
		{
			AccountingFormatStringBuilder.formatStringHandlers.Add(new Func<CurrencyInfo, int, string>(AccountingFormatStringBuilder.DefaultAccountingFormatStringsHandler));
			AccountingFormatStringBuilder.formatStringHandlers.Add(new Func<CurrencyInfo, int, string>(AccountingFormatStringBuilder.CurrencyWithCultureFormatStringsHandler));
		}

		public static string BuildFormatString(CurrencyInfo currencyInfo, int decimalPlaces)
		{
			string text = null;
			foreach (Func<CurrencyInfo, int, string> func in AccountingFormatStringBuilder.formatStringHandlers)
			{
				text = func(currencyInfo, decimalPlaces);
				if (text != null)
				{
					break;
				}
			}
			return text;
		}

		static string DefaultAccountingFormatStringsHandler(CurrencyInfo currencyInfo, int decimalPlaces)
		{
			if (TelerikHelper.EqualsOfT<CurrencyInfo>(currencyInfo, CurrencyInfo.None))
			{
				CultureInfo cultureInfo = FormatHelper.DefaultSpreadsheetCulture.CultureInfo;
				string currencySymbol = BuildersHelper.MakeCurrencySymbolInvisible(cultureInfo.NumberFormat.CurrencySymbol);
				return AccountingFormatStringBuilder.GetAccountingStrignsForCurrentCulture(cultureInfo, currencySymbol, decimalPlaces);
			}
			if (TelerikHelper.EqualsOfT<CurrencyInfo>(currencyInfo, CurrencyInfo.Symbol))
			{
				CultureInfo cultureInfo2 = FormatHelper.DefaultSpreadsheetCulture.CultureInfo;
				string escapedCurrencySymbol = currencyInfo.EscapedCurrencySymbol;
				return AccountingFormatStringBuilder.GetAccountingStrignsForCurrentCulture(cultureInfo2, escapedCurrencySymbol, decimalPlaces);
			}
			return null;
		}

		static string CurrencyWithCultureFormatStringsHandler(CurrencyInfo currencyInfo, int decimalPlaces)
		{
			string currencySymbol = CurrencyFormatCategoryManager.CultureInfoToCurrencyCode[currencyInfo.Culture];
			return AccountingFormatStringBuilder.GetAccountingStrignsForCurrentCulture(currencyInfo.Culture, currencySymbol, decimalPlaces);
		}

		static string GetAccountingStrignsForCurrentCulture(CultureInfo cultureInfo, string currencySymbol, int decimalPlaces)
		{
			if (cultureInfo == null)
			{
				return null;
			}
			string valuePatternConsiderDecimalPlaces = FormatHelper.GetValuePatternConsiderDecimalPlaces(decimalPlaces, true);
			int currencyNegativePattern = cultureInfo.NumberFormat.CurrencyNegativePattern;
			string positivePatternSystem = FormatHelper.PositivePatternStrings[cultureInfo.NumberFormat.CurrencyPositivePattern];
			string text = FormatHelper.NegativePatternStrings[currencyNegativePattern];
			string start;
			string middle;
			string end;
			AccountingFormatStringBuilder.GetStartMiddleEnd(text, out start, out middle, out end);
			string positivePattern = AccountingFormatStringBuilder.GetPositivePattern(positivePatternSystem, start, middle, end, currencySymbol, valuePatternConsiderDecimalPlaces);
			string negativePattern = AccountingFormatStringBuilder.GetNegativePattern(text, start, end, currencySymbol, valuePatternConsiderDecimalPlaces);
			string zeroPattern = AccountingFormatStringBuilder.GetZeroPattern(positivePatternSystem, start, middle, end, currencySymbol, decimalPlaces);
			string textPattern = AccountingFormatStringBuilder.GetTextPattern(start, end);
			return string.Format("{0};{1};{2};{3}", new object[] { positivePattern, negativePattern, zeroPattern, textPattern });
		}

		static string GetPositivePattern(string positivePatternSystem, string start, string middle, string end, string currencySymbol, string valuePattern)
		{
			int num = positivePatternSystem.IndexOf('$');
			string text;
			if (num > positivePatternSystem.IndexOf('n'))
			{
				text = positivePatternSystem.Insert(0, AccountingFormatStringBuilder.ExpandSymbol);
			}
			else
			{
				text = positivePatternSystem.Insert(num + 1, AccountingFormatStringBuilder.ExpandSymbol);
			}
			text = text.Insert(0, string.Format("_{0}", start));
			text += string.Format("_{0}", end);
			text = text.Insert(text.IndexOf('n') + 1, string.IsNullOrEmpty(middle) ? string.Empty : string.Format("_{0}", middle));
			text = text.Replace("n", valuePattern);
			return text.Replace("$", currencySymbol);
		}

		static string GetNegativePattern(string negativePatternSystem, string start, string end, string currencySymbol, string valuePattern)
		{
			int num = negativePatternSystem.IndexOf('$');
			string text;
			if (num > negativePatternSystem.IndexOf('n'))
			{
				int startIndex = negativePatternSystem.IndexOf('n');
				text = negativePatternSystem.Insert(startIndex, AccountingFormatStringBuilder.ExpandSymbol);
			}
			else
			{
				text = negativePatternSystem.Insert(num + 1, AccountingFormatStringBuilder.ExpandSymbol);
			}
			if (text.Contains('('))
			{
				text = text.Replace("(", string.Empty);
				text = text.Replace(")", string.Empty);
				int num2 = text.IndexOf('n');
				text = text.Insert(num2 + 1, ")");
				text = text.Insert(num2, "(");
			}
			if (!string.IsNullOrEmpty(start) && text.First<char>() != start[0])
			{
				text = text.Insert(0, string.Format("_{0}", start));
			}
			if (!string.IsNullOrEmpty(end) && text.Last<char>() != end[0])
			{
				text += string.Format("_{0}", end);
			}
			text = text.Replace("n", valuePattern);
			return text.Replace("$", currencySymbol);
		}

		static string GetZeroPattern(string positivePatternSystem, string start, string middle, string end, string currencySymbol, int decimalPlaces)
		{
			int num = positivePatternSystem.IndexOf('$');
			string text;
			if (num > positivePatternSystem.IndexOf('n'))
			{
				text = positivePatternSystem.Insert(0, AccountingFormatStringBuilder.ExpandSymbol);
			}
			else
			{
				text = positivePatternSystem.Insert(num + 1, AccountingFormatStringBuilder.ExpandSymbol);
			}
			text = text.Insert(0, string.Format("_{0}", start));
			text += string.Format("_{0}", end);
			text = text.Insert(text.IndexOf('n') + 1, string.IsNullOrEmpty(middle) ? string.Empty : string.Format("_{0}", middle));
			text = text.Replace("n", string.Format(AccountingFormatStringBuilder.ZeroText, string.Concat<char>(Enumerable.Repeat<char>('?', decimalPlaces))));
			return text.Replace("$", currencySymbol);
		}

		static string GetTextPattern(string start, string end)
		{
			return string.Format("_{0}@_{1}", start, end);
		}

		static void GetStartMiddleEnd(string negativePattern, out string start, out string middle, out string end)
		{
			start = string.Empty;
			middle = string.Empty;
			end = string.Empty;
			if (negativePattern.Contains("(") && negativePattern.IndexOf('$') > negativePattern.IndexOf('n'))
			{
				string text;
				end = (text = " ");
				start = text;
				middle = ")";
				return;
			}
			if (negativePattern.Contains("("))
			{
				start = "(";
				end = ")";
				return;
			}
			if (negativePattern[0] == '-' || negativePattern.Last<char>() == '-')
			{
				string text2;
				end = (text2 = "-");
				start = text2;
				return;
			}
			string text3;
			end = (text3 = " ");
			start = text3;
		}

		static readonly string ExpandSymbol = "* ";

		static readonly string ZeroText = "\"-\"{0}";

		static readonly List<Func<CurrencyInfo, int, string>> formatStringHandlers = new List<Func<CurrencyInfo, int, string>>();
	}
}
