using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeFlipCommand : UndoableWorksheetCommandBase<SetShapeFlipCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetShapeFlipCommandContext context)
		{
			context.OldHorizontalFlip = context.Shape.IsHorizontallyFlipped;
			context.OldVerticalFlip = context.Shape.IsVerticallyFlipped;
		}

		protected override void UndoOverride(SetShapeFlipCommandContext context)
		{
			context.Shape.Shape.IsHorizontallyFlipped = context.OldHorizontalFlip;
			context.Shape.Shape.IsVerticallyFlipped = context.OldVerticalFlip;
		}

		protected override bool AffectsLayoutOverride(SetShapeFlipCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(SetShapeFlipCommandContext context)
		{
			context.Shape.Shape.IsHorizontallyFlipped = context.HorizontalFlip;
			context.Shape.Shape.IsVerticallyFlipped = context.VerticalFlip;
		}
	}
}
