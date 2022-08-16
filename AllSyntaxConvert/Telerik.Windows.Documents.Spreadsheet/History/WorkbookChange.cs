using System;
using Telerik.Windows.Documents.Spreadsheet.Commands;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.History
{
	public class WorkbookChange
	{
		public string CommandName
		{
			get
			{
				return this.Command.Name;
			}
		}

		internal IUndoableWorkbookCommand Command
		{
			get
			{
				return this.command;
			}
		}

		internal WorkbookCommandContextBase CommandContext
		{
			get
			{
				return this.commandContext;
			}
		}

		internal WorkbookChange(IUndoableWorkbookCommand command, WorkbookCommandContextBase commandContext)
		{
			Guard.ThrowExceptionIfNull<IUndoableWorkbookCommand>(command, "command");
			Guard.ThrowExceptionIfNull<WorkbookCommandContextBase>(commandContext, "commandContext");
			this.command = command;
			this.commandContext = commandContext;
		}

		internal bool Undo()
		{
			return this.Command.Undo(this.CommandContext);
		}

		internal bool Redo()
		{
			return this.Command.Redo(this.CommandContext);
		}

		readonly IUndoableWorkbookCommand command;

		readonly WorkbookCommandContextBase commandContext;
	}
}
