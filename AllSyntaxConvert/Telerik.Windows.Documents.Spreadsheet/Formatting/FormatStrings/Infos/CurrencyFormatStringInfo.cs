using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Infos
{
	public class CurrencyFormatStringInfo : ICategorizedFormatStringInfo
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

		public IList<string> FormatStrings
		{
			get
			{
				return this.formatStrings;
			}
		}

		public CurrencyFormatStringInfo(int decimalPlaces, CurrencyInfo currencyInfo, IEnumerable<string> formatStrings)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, decimalPlaces, "decimalPlaces");
			Guard.ThrowExceptionIfNull<CurrencyInfo>(currencyInfo, "currencyInfo");
			Guard.ThrowExceptionIfNull<IEnumerable<string>>(formatStrings, "formatStrings");
			this.decimalPlaces = decimalPlaces;
			this.currencyInfo = currencyInfo;
			this.formatStrings = new List<string>(formatStrings);
		}

		internal static bool TryCreate(string formatString, out CurrencyFormatStringInfo formatStringInfo)
		{
			int num;
			if (FormatHelper.TryGetDecimalPlacesForFormatString(formatString, FormatHelper.DefaultSpreadsheetCulture.CultureInfo, out num))
			{
				foreach (CurrencyInfo currencyInfo in CurrencyFormatCategoryManager.CurrencyInfos)
				{
					List<string> list = CurrencyFormatStringBuilder.BuildFormatStrings(currencyInfo, num);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].CompareIgnoringSpecialSymbols(formatString))
						{
							formatStringInfo = new CurrencyFormatStringInfo(num, currencyInfo, list);
							return true;
						}
					}
				}
			}
			formatStringInfo = null;
			return false;
		}

		readonly int decimalPlaces;

		readonly CurrencyInfo currencyInfo;

		readonly List<string> formatStrings;
	}
}
