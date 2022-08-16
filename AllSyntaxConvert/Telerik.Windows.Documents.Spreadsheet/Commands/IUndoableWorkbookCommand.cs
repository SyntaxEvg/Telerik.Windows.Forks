using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	interface IUndoableWorkbookCommand : IWorkbookCommand
	{
		bool CanUndo(WorkbookCommandContextBase context);

		bool Undo(WorkbookCommandContextBase context);

		bool Redo(WorkbookCommandContextBase context);
	}
}
