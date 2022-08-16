using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetCellPropertyValuesCommand : UndoableWorksheetCommandBase<SetPropertyValuesCommandContext>
	{
		protected override bool AffectsLayoutOverride(SetPropertyValuesCommandContext context)
		{
			return context.Property.AffectsLayout;
		}

		protected override void PreserveStateBeforeExecute(SetPropertyValuesCommandContext context)
		{
			context.OldValues = context.Worksheet.Cells.PropertyBag.GetNonTypedPropertyValue(context.Property, context.NewValues.FromIndex, context.NewValues.ToIndex);
		}

		protected override void ExecuteOverride(SetPropertyValuesCommandContext context)
		{
			context.Worksheet.Cells.PropertyBag.SetNonTypedPropertyValue(context.Property, context.NewValues);
		}

		protected override void UndoOverride(SetPropertyValuesCommandContext context)
		{
			context.Worksheet.Cells.PropertyBag.SetNonTypedPropertyValue(context.Property, context.OldValues);
		}
	}
}
