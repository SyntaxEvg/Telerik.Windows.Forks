using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	class RtfStructureRowStackItem : RtfStructureStackItem
	{
		public RtfStructureRowStackItem(TableRow element, int nestingLevel, TableRowStyle style)
			: base(element, nestingLevel)
		{
			this.RowStyle = style;
		}

		public List<TableCell> CellsToBeAdded { get; set; }

		public TableRowStyle RowStyle { get; set; }

		public TableRow Row
		{
			get
			{
				return (TableRow)base.Element;
			}
		}
	}
}
