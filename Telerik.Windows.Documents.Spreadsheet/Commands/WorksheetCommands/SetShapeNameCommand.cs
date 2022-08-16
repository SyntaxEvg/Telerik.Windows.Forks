using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeNameCommand : UndoableWorksheetCommandBase<SetShapeNameCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetShapeNameCommandContext context)
		{
			context.OldName = context.Shape.Shape.Name;
		}

		protected override void UndoOverride(SetShapeNameCommandContext context)
		{
			context.Shape.Shape.Name = context.OldName;
		}

		protected override bool AffectsLayoutOverride(SetShapeNameCommandContext context)
		{
			return false;
		}

		protected override void ExecuteOverride(SetShapeNameCommandContext context)
		{
			context.Shape.Shape.Name = context.NewName;
		}
	}
}
