using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class CustomFiltersInfo : IFilterInfo
	{
		public CustomFiltersInfo()
		{
			this.CustomFilters = new List<CustomFilterCriteriaInfo>();
		}

		public bool IsAnd { get; set; }

		public List<CustomFilterCriteriaInfo> CustomFilters { get; set; }
	}
}
