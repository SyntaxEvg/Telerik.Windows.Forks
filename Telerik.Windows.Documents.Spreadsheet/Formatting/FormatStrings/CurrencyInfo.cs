using System;
using System.Globalization;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	public class CurrencyInfo
	{
		public string CurrencySymbol
		{
			get
			{
				return this.currencySymbol;
			}
		}

		internal string EscapedCurrencySymbol
		{
			get
			{
				return this.escapedCurrencySymbol;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		public static CurrencyInfo Symbol
		{
			get
			{
				if (CurrencyInfo.symbol == null || CurrencyInfo.symbol.CurrencySymbol != FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol)
				{
					CurrencyInfo.symbol = new CurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null);
				}
				return CurrencyInfo.symbol;
			}
		}

		public CurrencyInfo(string currencySymbol, CultureInfo culture = null)
		{
			Guard.ThrowExceptionIfNull<string>(currencySymbol, "currencySymbol");
			this.currencySymbol = currencySymbol;
			this.escapedCurrencySymbol = string.Format("\"{0}\"", this.currencySymbol);
			this.culture = culture;
		}

		public override bool Equals(object obj)
		{
			CurrencyInfo currencyInfo = obj as CurrencyInfo;
			return currencyInfo != null && this.CurrencySymbol == currencyInfo.CurrencySymbol && this.Culture == currencyInfo.Culture;
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.CurrencySymbol.GetHashCode(), (this.Culture != null) ? this.Culture.GetHashCode() : 0);
		}

		public static readonly CurrencyInfo None = new CurrencyInfo("None", null);

		readonly string currencySymbol;

		readonly string escapedCurrencySymbol;

		readonly CultureInfo culture;

		static CurrencyInfo symbol;
	}
}
