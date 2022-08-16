using System;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Common.Tables
{
	class TableCellData
	{
		public TableCellData()
		{
			this.Type = TableCellType.Empty;
		}

		public TableCellData(TableCellType type, TableCell cell)
		{
			this.Type = type;
			this.Cell = cell;
		}

		public TableCellType Type { get; set; }

		public TableCell Cell { get; set; }
	}
}
