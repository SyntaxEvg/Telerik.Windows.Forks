using System;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	public static class SpreadsheetCultureInfo
	{
		static SpreadsheetCultureHelper SpreadsheetCultureHelper
		{
			get
			{
				return FormatHelper.DefaultSpreadsheetCulture;
			}
		}

		public static string NumberDecimalSeparator
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.NumberDecimalSeparator;
			}
		}

		public static string NumberGroupSeparator
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.NumberGroupSeparator;
			}
		}

		public static string CurrencyDecimalSeparator
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.CurrencyDecimalSeparator;
			}
		}

		public static string CurrencyGroupSeparator
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.CurrencyGroupSeparator;
			}
		}

		public static string ListSeparator
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.ListSeparator;
			}
		}

		public static string ArrayListSeparator
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.ArrayListSeparator;
			}
		}

		public static string ArrayRowSeparator
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.ArrayRowSeparator;
			}
		}

		public static string TextQualifier
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.TextQualifier;
			}
		}

		public static string TimeSeparator
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.TimeSeparator;
			}
		}

		public static string DateSeparator
		{
			get
			{
				return SpreadsheetCultureInfo.SpreadsheetCultureHelper.DateSeparator;
			}
		}
	}
}
