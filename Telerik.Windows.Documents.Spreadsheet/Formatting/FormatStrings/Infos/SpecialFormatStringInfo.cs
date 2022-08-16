using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Infos
{
	public class SpecialFormatStringInfo : ICategorizedFormatStringInfo
	{
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		public SpecialFormatStringInfo(CultureInfo culture)
		{
			Guard.ThrowExceptionIfNull<CultureInfo>(culture, "culture");
			this.culture = culture;
		}

		internal static bool TryCreate(string formatString, out SpecialFormatStringInfo formatStringInfo)
		{
			foreach (KeyValuePair<CultureInfo, IList<SpecialFormatInfo>> keyValuePair in SpecialFormatCategoryManager.CultureInfoToFormatInfo)
			{
				SpecialFormatInfo specialFormatInfo = (from sfi in keyValuePair.Value
					where sfi.FormatString == formatString
					select sfi).FirstOrDefault<SpecialFormatInfo>();
				if (specialFormatInfo != null)
				{
					formatStringInfo = new SpecialFormatStringInfo(keyValuePair.Key);
					return true;
				}
			}
			formatStringInfo = null;
			return false;
		}

		readonly CultureInfo culture;
	}
}
