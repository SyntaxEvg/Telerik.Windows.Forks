using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Infos
{
	public class NumberFormatStringInfo : ICategorizedFormatStringInfo
	{
		public int DecimalPlaces
		{
			get
			{
				return this.decimalPlaces;
			}
		}

		public bool UseThousandSeparator
		{
			get
			{
				return this.useThousandSeparator;
			}
		}

		public NumberFormatStringInfo(int decimalPlaces, bool useThousandSeparator)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, decimalPlaces, "decimalPlaces");
			this.decimalPlaces = decimalPlaces;
			this.useThousandSeparator = useThousandSeparator;
		}

		internal static bool TryCreate(string formatString, out NumberFormatStringInfo formatStringInfo)
		{
			int num;
			bool flag;
			if (FormatHelper.TryGetDecimalPlacesForFormatString(formatString, FormatHelper.DefaultSpreadsheetCulture.CultureInfo, out num, out flag))
			{
				List<string> list = NumberFormatStringBuilder.BuildFormatStrings(num, flag);
				if (list.Contains(formatString))
				{
					formatStringInfo = new NumberFormatStringInfo(num, flag);
					return true;
				}
			}
			formatStringInfo = null;
			return false;
		}

		readonly int decimalPlaces;

		readonly bool useThousandSeparator;
	}
}
