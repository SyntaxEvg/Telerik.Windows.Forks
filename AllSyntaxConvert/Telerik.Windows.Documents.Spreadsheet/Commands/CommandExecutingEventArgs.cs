using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	public class CommandExecutingEventArgs : CommandExecuteEventArgsBase
	{
		public bool Canceled { get; set; }

		internal CommandExecutingEventArgs(IWorkbookCommand command, WorkbookCommandContextBase commandContext, bool isInUndo = false)
			: base(command, commandContext, isInUndo)
		{
		}

		public void Cancel()
		{
			this.Canceled = true;
		}
	}
}
