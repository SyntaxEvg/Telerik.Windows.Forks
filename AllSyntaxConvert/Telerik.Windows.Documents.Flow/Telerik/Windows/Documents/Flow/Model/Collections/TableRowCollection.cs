using System;

namespace Telerik.Windows.Documents.Flow.Model.Collections
{
	public sealed class TableRowCollection : DocumentElementCollection<TableRow, Table>
	{
		internal TableRowCollection(Table parent)
			: base(parent)
		{
		}

		public TableRow AddTableRow()
		{
			TableRow tableRow = new TableRow(base.Owner.Document);
			base.Add(tableRow);
			return tableRow;
		}
	}
}
