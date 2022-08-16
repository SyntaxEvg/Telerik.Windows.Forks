using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class SetWorkbookPropertyCommandContext<T> : WorkbookCommandContextBase
	{
		public T OldValue
		{
			get
			{
				return this.oldValue;
			}
			internal set
			{
				this.oldValue = value;
			}
		}

		public T NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		public SetWorkbookPropertyCommandContext(Workbook workbook, T newValue)
			: base(workbook)
		{
			this.newValue = newValue;
		}

		T oldValue;

		readonly T newValue;
	}
}
