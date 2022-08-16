using System;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Collections
{
	public class TableCellCollection : CollectionBase<TableCell>
	{
		public TableCell AddTableCell()
		{
			TableCell tableCell = new TableCell();
			base.Add(tableCell);
			return tableCell;
		}
	}
}
