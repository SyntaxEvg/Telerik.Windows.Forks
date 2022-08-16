using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	public class CommandExecutedEventArgs : CommandExecuteEventArgsBase
	{
		internal CommandExecutedEventArgs(IWorkbookCommand command, WorkbookCommandContextBase commandContext, bool isInUndo = false)
			: base(command, commandContext, isInUndo)
		{
		}
	}
}
