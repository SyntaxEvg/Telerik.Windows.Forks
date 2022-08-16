using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class FiltersInfo : IFilterInfo
	{
		public FiltersInfo()
		{
			this.StringFilters = new List<string>();
			this.DateFilters = new List<DateGroupItemInfo>();
		}

		public bool Blank { get; set; }

		public List<string> StringFilters { get; set; }

		public List<DateGroupItemInfo> DateFilters { get; set; }
	}
}
