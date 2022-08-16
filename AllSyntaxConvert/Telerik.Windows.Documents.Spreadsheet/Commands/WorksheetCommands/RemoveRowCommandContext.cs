using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveRowCommandContext : WorksheetCommandContextBase
	{
		public int FromIndex
		{
			get
			{
				return this.fromIndex;
			}
		}

		public int ToIndex
		{
			get
			{
				return this.fromIndex + this.itemCount - 1;
			}
		}

		public int ItemCount
		{
			get
			{
				return this.itemCount;
			}
		}

		internal RowsPropertyBag OldRowProperties { get; set; }

		internal CellsPropertyBag OldCellProperties { get; set; }

		public RemoveRowCommandContext(Worksheet worksheet, int fromIndex, int itemCount)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(fromIndex);
			Guard.ThrowExceptionIfLessThan<int>(0, itemCount, "itemCount");
			this.fromIndex = fromIndex;
			this.itemCount = itemCount;
		}

		readonly int fromIndex;

		readonly int itemCount;
	}
}
