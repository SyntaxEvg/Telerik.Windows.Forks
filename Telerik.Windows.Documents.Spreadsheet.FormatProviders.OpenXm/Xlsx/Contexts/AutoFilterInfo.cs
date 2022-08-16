using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class AutoFilterInfo
	{
		public AutoFilterInfo(CellRange range, List<FilterColumnInfo> filterColumnInfos)
		{
			this.Range = range;
			this.FilterColumnInfos = filterColumnInfos;
		}

		public CellRange Range { get; set; }

		public List<FilterColumnInfo> FilterColumnInfos { get; set; }
	}
}
