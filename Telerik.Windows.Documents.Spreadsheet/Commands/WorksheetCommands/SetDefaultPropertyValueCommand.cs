using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetDefaultPropertyValueCommand<T> : UndoableWorksheetCommandBase<SetDefaultPropertyValueCommandContext<T>>
	{
		protected override bool AffectsLayoutOverride(SetDefaultPropertyValueCommandContext<T> context)
		{
			return context.Property.AffectsLayout;
		}

		protected override void PreserveStateBeforeExecute(SetDefaultPropertyValueCommandContext<T> context)
		{
			context.OldValue = context.PropertyBag.GetDefaultPropertyValue<T>(context.Property);
		}

		protected override void ExecuteOverride(SetDefaultPropertyValueCommandContext<T> context)
		{
			context.PropertyBag.SetDefaultPropertyValue<T>(context.Property, context.NewValue);
		}

		protected override void UndoOverride(SetDefaultPropertyValueCommandContext<T> context)
		{
			context.PropertyBag.SetDefaultPropertyValue<T>(context.Property, context.OldValue);
		}
	}
}
