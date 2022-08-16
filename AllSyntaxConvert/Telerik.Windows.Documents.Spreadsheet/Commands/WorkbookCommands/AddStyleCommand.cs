using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class AddStyleCommand : UndoableWorkbookCommandBase<AddRemoveStyleCommandContext>
	{
		protected override bool AffectsLayoutOverride(AddRemoveStyleCommandContext context)
		{
			return true;
		}

		protected override bool CanExecuteOverride(AddRemoveStyleCommandContext context)
		{
			return !context.Workbook.Styles.Contains(context.CellStyle.Name);
		}

		protected override void PreserveStateBeforeExecute(AddRemoveStyleCommandContext context)
		{
		}

		protected override void ExecuteOverride(AddRemoveStyleCommandContext context)
		{
			context.Workbook.Styles.AddInternal(context.CellStyle);
		}

		protected override void UndoOverride(AddRemoveStyleCommandContext context)
		{
			context.Workbook.Styles.RemoveInternal(context.CellStyle);
		}
	}
}
