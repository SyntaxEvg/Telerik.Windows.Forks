using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Editing.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Tables
{
	public class TableRow
	{
		internal TableRow()
		{
			this.cells = new TableCellCollection();
			this.allIntersectingCells = new List<TableCell>();
		}

		public TableCellCollection Cells
		{
			get
			{
				return this.cells;
			}
		}

		internal double Height { get; set; }

		internal double Top { get; set; }

		internal double Bottom
		{
			get
			{
				return this.Top + this.Height;
			}
		}

		internal List<TableCell> AllIntersectingCells
		{
			get
			{
				return this.allIntersectingCells;
			}
		}

		public override string ToString()
		{
			return string.Format("Row Top:{0}, Height:{1}", this.Top, this.Height);
		}

		internal TableRow Copy()
		{
			TableRow tableRow = new TableRow();
			tableRow.Cells.AddRange(this.Cells);
			tableRow.AllIntersectingCells.AddRange(this.AllIntersectingCells);
			return tableRow;
		}

		readonly TableCellCollection cells;

		readonly List<TableCell> allIntersectingCells;
	}
}
