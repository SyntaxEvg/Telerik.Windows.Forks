using System;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Collections
{
	public class TableRowCollection : CollectionBase<TableRow>
	{
		public TableRow AddTableRow()
		{
			TableRow tableRow = new TableRow();
			base.Add(tableRow);
			return tableRow;
		}
	}
}
