using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class SetStylePropertyCommand : UndoableWorkbookCommandBase<SetStylePropertyCommandContext>
	{
		protected override bool AffectsLayoutOverride(SetStylePropertyCommandContext context)
		{
			return context.StyleProperty.AffectsLayout;
		}

		protected override bool CanExecuteOverride(SetStylePropertyCommandContext context)
		{
			return !TelerikHelper.EqualsOfT<object>(context.OldValue, context.NewValue);
		}

		protected override void PreserveStateBeforeExecute(SetStylePropertyCommandContext context)
		{
		}

		protected override void ExecuteOverride(SetStylePropertyCommandContext context)
		{
			context.StyleProperty.SetValueInternal(context.NewValue);
		}

		protected override void UndoOverride(SetStylePropertyCommandContext context)
		{
			context.StyleProperty.SetValueInternal(context.OldValue);
		}
	}
}
