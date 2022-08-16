using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class SortStateInfo
	{
		public CellRange Range { get; set; }

		public List<SortConditionInfo> Conditions { get; set; }
	}
}
