using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class AddShapeCommand : UndoableWorksheetCommandBase<AddRemoveShapeCommandContext>
	{
		protected override bool AffectsLayoutOverride(AddRemoveShapeCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(AddRemoveShapeCommandContext context)
		{
		}

		protected override void ExecuteOverride(AddRemoveShapeCommandContext context)
		{
			context.Worksheet.Shapes.AddInternal(context.Shape);
		}

		protected override void UndoOverride(AddRemoveShapeCommandContext context)
		{
			context.Worksheet.Shapes.RemoveInternal(context.Shape);
		}
	}
}
