using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class SortConditionInfo
	{
		public CellRange Range { get; set; }

		public string SortBy { get; set; }

		public string CustomList { get; set; }

		public bool Descending { get; set; }

		public int? DxfId { get; set; }
	}
}
