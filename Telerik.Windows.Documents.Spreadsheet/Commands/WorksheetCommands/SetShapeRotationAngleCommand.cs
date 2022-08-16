using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeRotationAngleCommand : UndoableWorkbookCommandBase<SetShapeRotationAngleCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetShapeRotationAngleCommandContext context)
		{
			context.OldAngle = context.FloatingShape.Shape.RotationAngle;
		}

		protected override void UndoOverride(SetShapeRotationAngleCommandContext context)
		{
			context.FloatingShape.SetRotationAngleInternal(context.OldAngle, context.AdjustCellindex);
		}

		protected override bool AffectsLayoutOverride(SetShapeRotationAngleCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(SetShapeRotationAngleCommandContext context)
		{
			context.FloatingShape.SetRotationAngleInternal(context.Angle, context.AdjustCellindex);
		}
	}
}
