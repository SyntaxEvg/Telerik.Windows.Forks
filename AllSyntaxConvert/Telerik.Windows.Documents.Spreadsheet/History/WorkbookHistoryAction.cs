using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.History
{
	public class WorkbookHistoryAction
	{
		public string DisplayText { get; protected set; }

		internal object Context { get; set; }

		public IEnumerable<WorkbookChange> Changes
		{
			get
			{
				return this.changes;
			}
		}

		public Sheet ActiveSheet
		{
			get
			{
				return this.activeSheet;
			}
		}

		internal WorkbookHistoryAction(IEnumerable<WorkbookChange> changes, string displayText)
		{
			this.changes = new List<WorkbookChange>(changes);
			this.activeSheet = this.changes[0].CommandContext.ActiveSheet;
			this.DisplayText = displayText;
		}

		internal bool Undo()
		{
			bool flag = true;
			WorkbookChange workbookChange = this.Changes.FirstOrDefault<WorkbookChange>();
			if (workbookChange != null)
			{
				Workbook workbook = workbookChange.CommandContext.Workbook;
				using (new UpdateScope(new Action(workbook.SuspendLayoutUpdate), new Action(workbook.ResumeLayoutUpdate)))
				{
					foreach (WorkbookChange workbookChange2 in this.Changes.Reverse<WorkbookChange>())
					{
						flag |= workbookChange2.Undo();
					}
				}
			}
			return flag;
		}

		internal bool Redo()
		{
			bool flag = true;
			WorkbookChange workbookChange = this.Changes.FirstOrDefault<WorkbookChange>();
			if (workbookChange != null)
			{
				Workbook workbook = workbookChange.CommandContext.Workbook;
				using (new UpdateScope(new Action(workbook.SuspendLayoutUpdate), new Action(workbook.ResumeLayoutUpdate)))
				{
					foreach (WorkbookChange workbookChange2 in this.Changes)
					{
						flag |= workbookChange2.Redo();
					}
				}
			}
			return flag;
		}

		internal bool Merge()
		{
			return false;
		}

		readonly List<WorkbookChange> changes;

		readonly Sheet activeSheet;
	}
}
