using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	public class CommandErrorEventArgs : CommandExecuteEventArgsBase
	{
		public Exception Exception { get; set; }

		public bool Handled { get; set; }

		internal CommandErrorEventArgs(Exception exception, IWorkbookCommand command, WorkbookCommandContextBase commandContext, bool isInUndo = false)
			: base(command, commandContext, isInUndo)
		{
			Guard.ThrowExceptionIfNull<Exception>(exception, "Exception");
			this.Exception = exception;
		}
	}
}
