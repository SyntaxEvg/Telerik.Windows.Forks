using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class MoveFilterCommand : UndoableWorksheetCommandBase<MoveFilterCommandContext>
	{
		protected override void PreserveStateBeforeExecute(MoveFilterCommandContext context)
		{
		}

		protected override void UndoOverride(MoveFilterCommandContext context)
		{
			context.Worksheet.Filter.MoveFilter(context.NewFilter, context.OldIndex);
		}

		protected override bool AffectsLayoutOverride(MoveFilterCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(MoveFilterCommandContext context)
		{
			context.NewFilter = context.Worksheet.Filter.MoveFilter(context.Filter, context.NewIndex);
		}
	}
}
