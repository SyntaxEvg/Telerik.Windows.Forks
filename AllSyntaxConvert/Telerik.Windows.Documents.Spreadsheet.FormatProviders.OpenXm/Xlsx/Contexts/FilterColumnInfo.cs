using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class FilterColumnInfo
	{
		public FilterColumnInfo(int columnId)
		{
			this.ColumnId = columnId;
		}

		public int ColumnId { get; set; }

		public IFilterInfo FilterInfo { get; set; }
	}
}
