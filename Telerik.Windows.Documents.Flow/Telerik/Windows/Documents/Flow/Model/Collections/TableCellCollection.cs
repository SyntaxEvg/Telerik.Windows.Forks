using System;

namespace Telerik.Windows.Documents.Flow.Model.Collections
{
	public sealed class TableCellCollection : DocumentElementCollection<TableCell, TableRow>
	{
		internal TableCellCollection(TableRow parent)
			: base(parent)
		{
		}

		public TableCell AddTableCell()
		{
			TableCell tableCell = new TableCell(base.Owner.Document);
			base.Add(tableCell);
			return tableCell;
		}
	}
}
