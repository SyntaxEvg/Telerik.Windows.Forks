using System;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Infos
{
	public class AccountingFormatStringInfo : ICategorizedFormatStringInfo
	{
		public int DecimalPlaces
		{
			get
			{
				return this.decimalPlaces;
			}
		}

		public CurrencyInfo CurrencyInfo
		{
			get
			{
				return this.currencyInfo;
			}
		}

		public string FormatString
		{
			get
			{
				return this.formatString;
			}
		}

		public AccountingFormatStringInfo(int decimalPlaces, CurrencyInfo currencyInfo, string formatString)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, decimalPlaces, "decimalPlaces");
			Guard.ThrowExceptionIfNull<CurrencyInfo>(currencyInfo, "currencyInfo");
			Guard.ThrowExceptionIfNull<string>(formatString, "formatString");
			this.decimalPlaces = decimalPlaces;
			this.currencyInfo = currencyInfo;
			this.formatString = formatString;
		}

		internal static bool TryCreate(string formatString, out AccountingFormatStringInfo formatStringInfo)
		{
			int num;
			if (FormatHelper.TryGetDecimalPlacesForFormatString(formatString, FormatHelper.DefaultSpreadsheetCulture.CultureInfo, out num))
			{
				foreach (CurrencyInfo currencyInfo in CurrencyFormatCategoryManager.CurrencyInfos)
				{
					string otherValue = AccountingFormatStringBuilder.BuildFormatString(currencyInfo, num);
					if (formatString.CompareIgnoringSpecialSymbols(otherValue))
					{
						formatStringInfo = new AccountingFormatStringInfo(num, currencyInfo, otherValue);
						return true;
					}
				}
			}
			formatStringInfo = null;
			return false;
		}

		readonly int decimalPlaces;

		readonly CurrencyInfo currencyInfo;

		readonly string formatString;
	}
}
