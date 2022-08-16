using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapePositionCommand : UndoableWorksheetCommandBase<SetShapePositionCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetShapePositionCommandContext context)
		{
			context.OldCellIndex = context.Shape.CellIndex;
			context.OldOffsetX = context.Shape.OffsetX;
			context.OldOffsetY = context.Shape.OffsetY;
		}

		protected override void UndoOverride(SetShapePositionCommandContext context)
		{
			context.Shape.CellIndexInternal = context.OldCellIndex;
			context.Shape.OffsetXInternal = context.OldOffsetX;
			context.Shape.OffsetYInternal = context.OldOffsetY;
		}

		protected override bool AffectsLayoutOverride(SetShapePositionCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(SetShapePositionCommandContext context)
		{
			context.Shape.CellIndexInternal = context.NewCellIndex;
			context.Shape.OffsetXInternal = context.NewOffsetX;
			context.Shape.OffsetYInternal = context.NewOffsetY;
		}
	}
}
