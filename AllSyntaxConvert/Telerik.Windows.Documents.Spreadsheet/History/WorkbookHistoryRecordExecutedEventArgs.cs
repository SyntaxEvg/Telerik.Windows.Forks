using System;

namespace Telerik.Windows.Documents.Spreadsheet.History
{
	public class WorkbookHistoryRecordExecutedEventArgs : WorkbookHistoryEventArgs
	{
		public bool IsMerged { get; set; }

		public WorkbookHistoryRecordExecutedEventArgs(WorkbookHistoryAction historyAction, bool isMerged)
			: base(historyAction)
		{
			this.IsMerged = isMerged;
		}
	}
}
