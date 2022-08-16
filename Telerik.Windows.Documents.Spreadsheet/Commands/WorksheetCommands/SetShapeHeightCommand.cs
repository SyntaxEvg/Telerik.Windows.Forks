using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeHeightCommand : UndoableWorksheetCommandBase<SetShapeSizeCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetShapeSizeCommandContext context)
		{
			context.OldValue = context.Shape.Height;
		}

		protected override void UndoOverride(SetShapeSizeCommandContext context)
		{
			context.Shape.SetHeightInternal(context.RespectLockAspectRatio, context.OldValue, context.AdjustCellIndex);
		}

		protected override bool AffectsLayoutOverride(SetShapeSizeCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(SetShapeSizeCommandContext context)
		{
			context.Shape.SetHeightInternal(context.RespectLockAspectRatio, context.NewValue, context.AdjustCellIndex);
		}
	}
}
