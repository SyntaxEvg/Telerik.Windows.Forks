using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class SetWorkbookThemeCommand : SetWorkbookPropertyCommand<DocumentTheme>
	{
		protected override bool AffectsLayoutOverride(SetWorkbookPropertyCommandContext<DocumentTheme> context)
		{
			return true;
		}

		protected override DocumentTheme GetPropertyValue(Workbook workbook)
		{
			return workbook.Theme;
		}

		protected override void SetPropertyValue(Workbook workbook, DocumentTheme value)
		{
			workbook.SetThemeInternal(value);
		}
	}
}
