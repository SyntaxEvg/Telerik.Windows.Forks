using System;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting
{
	static class FractionParser
	{
		public static bool TryParse(string value, FormatStringType? forceParsingAsSpecificType, out double result, out string format)
		{
			format = FormatHelper.GeneralFormatString;
			int denominatorLength = -1;
			result = 0.0;
			bool flag = FractionParser.FractionsRegex.IsMatch(value);
			if (flag)
			{
				Match match = FractionParser.FractionsRegex.Match(value);
				double num = (double)int.Parse(match.Groups[FractionParser.WholeGroupName].Value);
				double num2 = (double)int.Parse(match.Groups[FractionParser.NumeratorGroupName].Value);
				double num3 = (double)int.Parse(match.Groups[FractionParser.DenominatorGroupName].Value);
				denominatorLength = num3.ToString().Length;
				result = num + num2 / num3;
				if (match.Groups[FractionParser.MinusSignGroupName].Success)
				{
					result *= -1.0;
				}
			}
			else if (!flag && forceParsingAsSpecificType == FormatStringType.Number)
			{
				flag = FractionParser.FractionsShortRegex.IsMatch(value);
				if (flag)
				{
					Match match2 = FractionParser.FractionsShortRegex.Match(value);
					double num4 = (double)int.Parse(match2.Groups[FractionParser.NumeratorGroupName].Value);
					double num5 = (double)int.Parse(match2.Groups[FractionParser.DenominatorGroupName].Value);
					denominatorLength = num5.ToString().Length;
					result = num4 / num5;
					if (match2.Groups[FractionParser.MinusSignGroupName].Success)
					{
						result *= -1.0;
					}
				}
			}
			if (flag)
			{
				format = FractionParser.GetFractionFormat(denominatorLength);
			}
			return flag;
		}

		static string GetFractionFormat(int denominatorLength)
		{
			switch (denominatorLength)
			{
			case 1:
				return FractionParser.DefaultFractionsUpToOneDigitFormat;
			case 2:
				return FractionParser.DefaultFractionsUpToTwoDigitFormat;
			default:
				return FractionParser.DefaultFractionsUpToTwoDigitFormat;
			}
		}

		static readonly string WholeGroupName = "whole";

		static readonly string NumeratorGroupName = "numerator";

		static readonly string DenominatorGroupName = "denominator";

		static readonly string MinusSignGroupName = "minusSign";

		static readonly Regex FractionsRegex = new Regex(string.Format("^(?<{0}>-)?\\s*(?<{1}>\\d+) (?<{2}>\\d+)/(?<{3}>\\d+)$", new object[]
		{
			FractionParser.MinusSignGroupName,
			FractionParser.WholeGroupName,
			FractionParser.NumeratorGroupName,
			FractionParser.DenominatorGroupName
		}));

		static readonly Regex FractionsShortRegex = new Regex(string.Format("^(?<{0}>-)?\\s*(?<{1}>\\d+)/(?<{2}>\\d+)$", FractionParser.MinusSignGroupName, FractionParser.NumeratorGroupName, FractionParser.DenominatorGroupName));

		static readonly string DefaultFractionsUpToOneDigitFormat = "# ?/?";

		static readonly string DefaultFractionsUpToTwoDigitFormat = "# ??/??";
	}
}
