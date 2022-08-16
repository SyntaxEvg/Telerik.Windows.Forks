using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Tables
{
	public class CellProperties
	{
		public CellProperties()
		{
			this.Borders = new TableCellBorders();
			this.Padding = default(Thickness);
			this.Background = null;
		}

		public TableCellBorders Borders
		{
			get
			{
				return this.borders;
			}
			set
			{
				Guard.ThrowExceptionIfNull<TableCellBorders>(value, "value");
				this.borders = value;
			}
		}

		public Thickness Padding { get; set; }

		public ColorBase Background { get; set; }

		TableCellBorders borders;
	}
}
