using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class SetStylePropertyGroupIncludedCommand : UndoableWorkbookCommandBase<SetStylePropertyGroupIncludedCommandContext>
	{
		protected override bool AffectsLayoutOverride(SetStylePropertyGroupIncludedCommandContext context)
		{
			return true;
		}

		protected override bool CanExecuteOverride(SetStylePropertyGroupIncludedCommandContext context)
		{
			return !TelerikHelper.EqualsOfT<bool>(context.OldValue, context.NewValue);
		}

		protected override void PreserveStateBeforeExecute(SetStylePropertyGroupIncludedCommandContext context)
		{
		}

		protected override void ExecuteOverride(SetStylePropertyGroupIncludedCommandContext context)
		{
			context.Style.SetIsStylePropertyGroupIncludedInternal(context.StylePropertyGroup, context.NewValue);
		}

		protected override void UndoOverride(SetStylePropertyGroupIncludedCommandContext context)
		{
			context.Style.SetIsStylePropertyGroupIncludedInternal(context.StylePropertyGroup, context.OldValue);
		}
	}
}
