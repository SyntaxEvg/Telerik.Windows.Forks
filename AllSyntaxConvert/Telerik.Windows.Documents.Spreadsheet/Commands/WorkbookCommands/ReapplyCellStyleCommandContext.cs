using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class ReapplyCellStyleCommandContext : WorkbookCommandContextBase
	{
		public string OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		public string NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		public ReapplyCellStyleCommandContext(Workbook workbook, string oldValue)
			: base(workbook)
		{
			this.oldValue = oldValue;
			this.newValue = oldValue;
		}

		readonly string oldValue;

		readonly string newValue;
	}
}
