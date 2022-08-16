using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetRowPropertyValuesCommand : UndoableWorksheetCommandBase<SetPropertyValuesCommandContext>
	{
		protected override bool AffectsLayoutOverride(SetPropertyValuesCommandContext context)
		{
			return context.Property.AffectsLayout;
		}

		protected override void PreserveStateBeforeExecute(SetPropertyValuesCommandContext context)
		{
			context.OldValues = context.Worksheet.Rows.PropertyBag.GetNonTypedPropertyValue(context.Property, context.NewValues.FromIndex, context.NewValues.ToIndex);
		}

		protected override void ExecuteOverride(SetPropertyValuesCommandContext context)
		{
			context.Worksheet.Rows.PropertyBag.SetNonTypedPropertyValue(context.Property, context.NewValues);
		}

		protected override void UndoOverride(SetPropertyValuesCommandContext context)
		{
			context.Worksheet.Rows.PropertyBag.SetNonTypedPropertyValue(context.Property, context.OldValues);
		}
	}
}
