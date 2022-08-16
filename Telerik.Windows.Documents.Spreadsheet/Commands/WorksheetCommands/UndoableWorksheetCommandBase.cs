using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	abstract class UndoableWorksheetCommandBase<T> : UndoableWorkbookCommandBase<T> where T : WorksheetCommandContextBase
	{
	}
}
