using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeLockAspectRatioCommand : UndoableWorksheetCommandBase<SetShapeLockAspectRatioCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetShapeLockAspectRatioCommandContext context)
		{
			context.OldLockAspectRatio = context.Shape.LockAspectRatio;
		}

		protected override void UndoOverride(SetShapeLockAspectRatioCommandContext context)
		{
			context.Shape.Shape.LockAspectRatio = context.OldLockAspectRatio;
		}

		protected override bool AffectsLayoutOverride(SetShapeLockAspectRatioCommandContext context)
		{
			return false;
		}

		protected override void ExecuteOverride(SetShapeLockAspectRatioCommandContext context)
		{
			context.Shape.Shape.LockAspectRatio = context.LockAspectRatio;
		}
	}
}
