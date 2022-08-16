using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	public abstract class CommandExecuteEventArgsBase : EventArgs
	{
		internal IWorkbookCommand Command { get; set; }

		internal WorkbookCommandContextBase CommandContext { get; set; }

		public bool IsInUndo { get; set; }

		public string CommandName
		{
			get
			{
				return this.Command.Name;
			}
		}

		internal CommandExecuteEventArgsBase(IWorkbookCommand command, WorkbookCommandContextBase commandContext, bool isInUndo)
		{
			Guard.ThrowExceptionIfNull<IWorkbookCommand>(command, "command");
			Guard.ThrowExceptionIfNull<WorkbookCommandContextBase>(commandContext, "commandContext");
			this.Command = command;
			this.CommandContext = commandContext;
			this.IsInUndo = isInUndo;
		}
	}
}
