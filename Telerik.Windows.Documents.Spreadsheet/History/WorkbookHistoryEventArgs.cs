using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.History
{
	public class WorkbookHistoryEventArgs : EventArgs
	{
		public WorkbookHistoryAction HistoryAction { get; set; }

		public WorkbookHistoryEventArgs(WorkbookHistoryAction historyAction)
		{
			Guard.ThrowExceptionIfNull<WorkbookHistoryAction>(historyAction, "historyAction");
			this.HistoryAction = historyAction;
		}
	}
}
