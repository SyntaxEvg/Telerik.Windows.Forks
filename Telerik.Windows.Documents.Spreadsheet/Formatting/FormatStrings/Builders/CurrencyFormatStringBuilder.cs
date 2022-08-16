using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders
{
	public static class CurrencyFormatStringBuilder
	{
		static CurrencyFormatStringBuilder()
		{
			CurrencyFormatStringBuilder.formatStringHandlers.Add(new Func<CurrencyInfo, int, List<string>>(CurrencyFormatStringBuilder.DefaultCurrencyFormatStringsHandler));
			CurrencyFormatStringBuilder.formatStringHandlers.Add(new Func<CurrencyInfo, int, List<string>>(CurrencyFormatStringBuilder.CurrencyWithCultureFormatStringsHandler));
		}

		public static List<string> CurrencyWithCultureFormatStringsHandler(CurrencyInfo currencyInfo, int decimalPlaces)
		{
			if (currencyInfo.Culture == null)
			{
				return null;
			}
			string currencyCode = CurrencyFormatCategoryManager.CultureInfoToCurrencyCode[currencyInfo.Culture];
			return BuildersHelper.GetCurrencyStringsForCurrentCulture(currencyInfo.Culture, currencyCode, decimalPlaces, true);
		}

		public static List<string> BuildFormatStrings(CurrencyInfo currencyInfo, int decimalPlaces)
		{
			List<string> list = null;
			foreach (Func<CurrencyInfo, int, List<string>> func in CurrencyFormatStringBuilder.formatStringHandlers)
			{
				list = func(currencyInfo, decimalPlaces);
				if (list != null)
				{
					break;
				}
			}
			return list;
		}

		static List<string> DefaultCurrencyFormatStringsHandler(CurrencyInfo currencyInfo, int decimalPlaces)
		{
			if (TelerikHelper.EqualsOfT<CurrencyInfo>(currencyInfo, CurrencyInfo.None))
			{
				string currencySymbol = FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol;
				return BuildersHelper.GetCurrencyStringsForCurrentCulture(FormatHelper.DefaultSpreadsheetCulture.CultureInfo, BuildersHelper.MakeCurrencySymbolInvisible(currencySymbol), decimalPlaces, true);
			}
			if (TelerikHelper.EqualsOfT<CurrencyInfo>(currencyInfo, CurrencyInfo.Symbol))
			{
				return BuildersHelper.GetCurrencyStringsForCurrentCulture(FormatHelper.DefaultSpreadsheetCulture.CultureInfo, currencyInfo.EscapedCurrencySymbol, decimalPlaces, true);
			}
			return null;
		}

		static readonly List<Func<CurrencyInfo, int, List<string>>> formatStringHandlers = new List<Func<CurrencyInfo, int, List<string>>>();
	}
}
