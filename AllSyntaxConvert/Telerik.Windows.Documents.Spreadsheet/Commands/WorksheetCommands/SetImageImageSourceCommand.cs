using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetImageImageSourceCommand : UndoableWorksheetCommandBase<SetImageImageSourceCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetImageImageSourceCommandContext context)
		{
			context.OldImageSource = context.Image.ImageSource;
		}

		protected override void UndoOverride(SetImageImageSourceCommandContext context)
		{
			context.Image.Image.ImageSource = context.OldImageSource;
		}

		protected override bool AffectsLayoutOverride(SetImageImageSourceCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(SetImageImageSourceCommandContext context)
		{
			context.Image.Image.ImageSource = context.NewImageSource;
		}
	}
}
