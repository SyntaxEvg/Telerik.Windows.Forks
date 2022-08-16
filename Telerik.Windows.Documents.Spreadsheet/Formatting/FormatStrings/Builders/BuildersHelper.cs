using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders
{
	static class BuildersHelper
	{
		public static string GetCultureDependentFormatCode(string invariantCultureFormatCode)
		{
			string currencySymbol = FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol;
			string numberDecimalSeparator = FormatHelper.DefaultSpreadsheetCulture.NumberDecimalSeparator;
			string numberGroupSeparator = FormatHelper.DefaultSpreadsheetCulture.NumberGroupSeparator;
			return invariantCultureFormatCode.Replace(".", numberDecimalSeparator).Replace(",", numberGroupSeparator).Replace("$", currencySymbol);
		}

		public static List<string> GetCurrencyStringsForCurrentCulture(CultureInfo cultureInfo, string currencyCode, int decimalPlaces, bool useThousandSeparator = true)
		{
			List<string> list = new List<string>();
			string valuePatternConsiderDecimalPlaces = FormatHelper.GetValuePatternConsiderDecimalPlaces(decimalPlaces, useThousandSeparator);
			int currencyNegativePattern = cultureInfo.NumberFormat.CurrencyNegativePattern;
			string text = FormatHelper.PositivePatternStrings[cultureInfo.NumberFormat.CurrencyPositivePattern];
			string text2 = FormatHelper.NegativePatternStrings[currencyNegativePattern];
			string text3 = text.Replace("n", valuePatternConsiderDecimalPlaces);
			text3 = text3.Replace("$", currencyCode);
			text3 = text3.Trim();
			string text4 = text2.Replace("n", valuePatternConsiderDecimalPlaces);
			text4 = text4.Replace("$", currencyCode);
			text4 = text4.Trim();
			list.Add(string.Format("{0}{1}", text3, BuildersHelper.GetAlignmentStringForPattern(currencyNegativePattern, true)));
			list.Add(string.Format("{0};[Red]{0}", text3));
			list.Add(string.Format("{0}{1};{2}", text3, BuildersHelper.GetAlignmentStringForPattern(currencyNegativePattern, false), text4));
			list.Add(string.Format("{0}{1};[Red]{2}", text3, BuildersHelper.GetAlignmentStringForPattern(currencyNegativePattern, false), text4));
			return list;
		}

		public static string MakeCurrencySymbolInvisible(string currencySymbol)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < currencySymbol.Length; i++)
			{
				stringBuilder.Append("_");
				stringBuilder.Append(currencySymbol[i]);
			}
			return stringBuilder.ToString();
		}

		static string GetAlignmentStringForPattern(int patternIndex, bool isFirstNegativePattern)
		{
			switch (patternIndex)
			{
			case 0:
			case 4:
				break;
			case 1:
			case 2:
				goto IL_5F;
			case 3:
				goto IL_59;
			default:
				switch (patternIndex)
				{
				case 7:
				case 10:
				case 11:
					goto IL_59;
				case 8:
				case 9:
				case 12:
				case 13:
					goto IL_5F;
				case 14:
				case 15:
					break;
				default:
					goto IL_5F;
				}
				break;
			}
			if (isFirstNegativePattern)
			{
				return string.Empty;
			}
			return "_)";
			IL_59:
			return "_-";
			IL_5F:
			return string.Empty;
		}

		const string InvariantCurrencySymbol = "$";

		const string InvariantNumberDecimalSeparator = ".";

		const string InvariantNumberGroupSeparator = ",";
	}
}
