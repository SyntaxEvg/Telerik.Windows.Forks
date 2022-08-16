using System;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Infos
{
	public class PercentageFormatStringInfo : ICategorizedFormatStringInfo
	{
		public int DecimalPlaces
		{
			get
			{
				return this.decimalPlaces;
			}
		}

		public PercentageFormatStringInfo(int decimalPlaces)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, decimalPlaces, "decimalPlaces");
			this.decimalPlaces = decimalPlaces;
		}

		internal static bool TryCreate(string formatString, out PercentageFormatStringInfo formatStringInfo)
		{
			Match match = PercentageFormatStringInfo.PercentRegex.Match(formatString);
			if (match.Success)
			{
				int length = match.Groups[FormatHelper.ZeroCountGroupName].Value.Length;
				formatStringInfo = new PercentageFormatStringInfo(length);
				return true;
			}
			formatStringInfo = null;
			return false;
		}

		static readonly string PercentPattern = string.Format("^0(\\.(?<{0}>0+))?%$", FormatHelper.ZeroCountGroupName);

		static readonly Regex PercentRegex = new Regex(PercentageFormatStringInfo.PercentPattern);

		readonly int decimalPlaces;
	}
}
