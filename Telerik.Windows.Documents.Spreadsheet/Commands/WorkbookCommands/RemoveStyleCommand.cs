using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class RemoveStyleCommand : UndoableWorkbookCommandBase<AddRemoveStyleCommandContext>
	{
		protected override bool AffectsLayoutOverride(AddRemoveStyleCommandContext context)
		{
			return true;
		}

		protected override bool CanExecuteOverride(AddRemoveStyleCommandContext context)
		{
			return context.CellStyle.IsRemovable && context.Workbook.Styles.Contains(context.CellStyle);
		}

		protected override void PreserveStateBeforeExecute(AddRemoveStyleCommandContext context)
		{
		}

		protected override void ExecuteOverride(AddRemoveStyleCommandContext context)
		{
			context.Workbook.Styles.RemoveInternal(context.CellStyle);
		}

		protected override void UndoOverride(AddRemoveStyleCommandContext context)
		{
			context.Workbook.Styles.AddInternal(context.CellStyle);
		}
	}
}
