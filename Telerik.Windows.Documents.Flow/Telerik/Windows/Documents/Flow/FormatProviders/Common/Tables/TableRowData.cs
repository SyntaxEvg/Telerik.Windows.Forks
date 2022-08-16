using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Common.Tables
{
	class TableRowData
	{
		public TableRowData(TableRow row, List<TableCellData> tableCellDatas)
		{
			this.TableCellDatas = tableCellDatas;
			this.Row = row;
		}

		public List<TableCellData> TableCellDatas { get; set; }

		public TableRow Row { get; set; }
	}
}
