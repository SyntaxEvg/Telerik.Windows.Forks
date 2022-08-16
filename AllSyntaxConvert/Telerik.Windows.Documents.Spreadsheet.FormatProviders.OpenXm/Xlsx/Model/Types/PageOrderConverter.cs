using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class PageOrderConverter : IStringConverter<PageOrder>
	{
		public PageOrder ConvertFromString(string str)
		{
			if (string.Equals(str, "OverThenDown", StringComparison.OrdinalIgnoreCase))
			{
				return PageOrder.OverThenDown;
			}
			return PageOrder.DownThenOver;
		}

		public string ConvertToString(PageOrder pageOrder)
		{
			if (pageOrder == PageOrder.OverThenDown)
			{
				return "overThenDown";
			}
			return "downThenOver";
		}
	}
}
