using System;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Infos
{
	public class ScientificFormatStringInfo
	{
		public int DecimalPlaces
		{
			get
			{
				return this.decimalPlaces;
			}
		}

		public ScientificFormatStringInfo(int decimalPlaces)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, decimalPlaces, "decimalPlaces");
			this.decimalPlaces = decimalPlaces;
		}

		internal static bool TryCreate(string formatString, out ScientificFormatStringInfo formatStringInfo)
		{
			Match match = ScientificFormatStringInfo.ScientificRegex.Match(formatString);
			if (match.Success)
			{
				int length = match.Groups[FormatHelper.ZeroCountGroupName].Value.Length;
				formatStringInfo = new ScientificFormatStringInfo(length);
				return true;
			}
			formatStringInfo = null;
			return false;
		}

		static readonly string ScientificPattern = string.Format("^0(\\.(?<{0}>0*))?E\\+00$", FormatHelper.ZeroCountGroupName);

		static readonly Regex ScientificRegex = new Regex(ScientificFormatStringInfo.ScientificPattern);

		readonly int decimalPlaces;
	}
}
