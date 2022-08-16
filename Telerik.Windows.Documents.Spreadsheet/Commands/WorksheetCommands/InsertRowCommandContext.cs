using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class InsertRowCommandContext : WorksheetCommandContextBase
	{
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		public int ItemCount
		{
			get
			{
				return this.itemCount;
			}
		}

		public InsertRowCommandContext(Worksheet worksheet, int index, int itemCount)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(index);
			Guard.ThrowExceptionIfLessThan<int>(0, itemCount, "itemCount");
			this.index = index;
			this.itemCount = itemCount;
		}

		readonly int index;

		readonly int itemCount;
	}
}
