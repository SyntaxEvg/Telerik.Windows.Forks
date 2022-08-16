using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities
{
	static class BuiltInNumberFormatsGenerator
	{
		public static Dictionary<string, int> GetBuiltInFormatsForCurrentCulture()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, FormatHelper.GeneralFormatString, 0);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, "0", 1);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetNumberFormat(2, false, 0), 2);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetNumberFormat(0, true, 0), 3);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetNumberFormat(2, true, 0), 4);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetCurrencyFormat(0, 2), 5);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetCurrencyFormat(0, 3), 6);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetCurrencyFormat(2, 2), 7);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetCurrencyFormat(2, 3), 8);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, "0%", 9);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetNumberFormatWithPercent(2, false, 0), 10);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetScientificFormat1(), 11);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, "# ?/?", 12);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, "# ??/??", 13);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetShortDateFormat(), 14);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetDayMonthYearFormat(), 15);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetDayMonthFormat(), 16);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetMonthYearFormat(), 17);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetHourMinuteAMPMFormat(), 18);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetHourMinuteSecondAMPMFormat(), 19);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetHourMinuteFormat(), 20);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetHourMinuteSecondFormat(), 21);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetShortDateHourMinuteFormat(), 22);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetNumberFormat(0, true, 2), 37);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetNumberFormat(0, true, 3), 38);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetNumberFormat(2, true, 2), 39);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetNumberFormat(2, true, 3), 40);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuildersHelper.GetCultureDependentFormatCode("_(*#,##0_);_(*(#,##0);_(* \"-\"_);_(@_)"), 41);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuildersHelper.GetCultureDependentFormatCode("_($*#,##0_);_($*(#,##0);_($* \"-\"_);_(@_)"), 42);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuildersHelper.GetCultureDependentFormatCode("_(*#,##0.00_);_(*(#,##0.00);_(*\"-\"??_);_(@_)"), 43);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuildersHelper.GetCultureDependentFormatCode("_($*#,##0.00_);_($*(#,##0.00);_($*\"-\"??_);_(@_)"), 44);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetMonthSecondFormat(), 45);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetTotalHoursMonthSecondFormat(), 46);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetMonthSecondMillisecondFormat(), 47);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, BuiltInNumberFormatsGenerator.GetScientificFormat2(), 48);
			BuiltInNumberFormatsGenerator.RegisterBuiltInFormat(dictionary, "@", 49);
			return dictionary;
		}

		static void RegisterBuiltInFormat(Dictionary<string, int> builtInFormats, string format, int id)
		{
			builtInFormats[format] = id;
		}

		static string GetNumberFormat(int decimalPlaces, bool useThousandSeparator, int formatNumber)
		{
			return NumberFormatStringBuilder.BuildFormatStrings(decimalPlaces, useThousandSeparator)[formatNumber];
		}

		static string GetCurrencyFormat(int decimalPlaces, int formatNumber)
		{
			string currencySymbol = FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol;
			return BuildersHelper.GetCurrencyStringsForCurrentCulture(FormatHelper.DefaultSpreadsheetCulture.CultureInfo, currencySymbol, decimalPlaces, true)[formatNumber];
		}

		static string GetNumberFormatWithPercent(int decimalPlaces, bool useThousandSeparator, int formatNumber)
		{
			return NumberFormatStringBuilder.BuildFormatStrings(decimalPlaces, useThousandSeparator)[formatNumber] + "%";
		}

		static string GetScientificFormat1()
		{
			return string.Format("0{0}00E+00", FormatHelper.DefaultSpreadsheetCulture.NumberDecimalSeparator);
		}

		static string GetShortDateFormat()
		{
			return FormatHelper.DefaultSpreadsheetCulture.CultureInfo.DateTimeFormat.ShortDatePattern.ToLower();
		}

		static string GetDayMonthYearFormat()
		{
			return string.Format("d{0}mmm{0}yy", FormatHelper.DefaultSpreadsheetCulture.DateSeparator);
		}

		static string GetDayMonthFormat()
		{
			return string.Format("d{0}mmm", FormatHelper.DefaultSpreadsheetCulture.DateSeparator);
		}

		static string GetMonthYearFormat()
		{
			return string.Format("mmm{0}yy", FormatHelper.DefaultSpreadsheetCulture.DateSeparator);
		}

		static string GetHourMinuteAMPMFormat()
		{
			return string.Format("h{0}mm AM/PM", FormatHelper.DefaultSpreadsheetCulture.TimeSeparator);
		}

		static string GetHourMinuteSecondAMPMFormat()
		{
			return string.Format("h{0}mm{0}ss AM/PM", FormatHelper.DefaultSpreadsheetCulture.TimeSeparator);
		}

		static string GetHourMinuteFormat()
		{
			return string.Format("h{0}mm", FormatHelper.DefaultSpreadsheetCulture.TimeSeparator);
		}

		static string GetHourMinuteSecondFormat()
		{
			return string.Format("h{0}mm{0}ss", FormatHelper.DefaultSpreadsheetCulture.TimeSeparator);
		}

		static string GetShortDateHourMinuteFormat()
		{
			return string.Format("{0} {1}", BuiltInNumberFormatsGenerator.GetShortDateFormat(), BuiltInNumberFormatsGenerator.GetHourMinuteFormat());
		}

		static string GetMonthSecondFormat()
		{
			return string.Format("mm{0}ss", FormatHelper.DefaultSpreadsheetCulture.TimeSeparator);
		}

		static string GetTotalHoursMonthSecondFormat()
		{
			return string.Format("[h]{0}mm{0}ss", FormatHelper.DefaultSpreadsheetCulture.TimeSeparator);
		}

		static string GetMonthSecondMillisecondFormat()
		{
			return string.Format("mm{0}ss{1}0", FormatHelper.DefaultSpreadsheetCulture.TimeSeparator, FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.NumberDecimalSeparator);
		}

		static string GetScientificFormat2()
		{
			return string.Format("##0{0}0E+0", FormatHelper.DefaultSpreadsheetCulture.NumberDecimalSeparator);
		}
	}
}
