using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	interface IWorkbookCommand
	{
		string Name { get; }

		bool AffectsLayout(WorkbookCommandContextBase context);

		bool CanExecute(WorkbookCommandContextBase context);

		bool Execute(WorkbookCommandContextBase context);
	}
}
