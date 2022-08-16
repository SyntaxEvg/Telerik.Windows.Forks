using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class UnmergeCellsCommand : UndoableWorksheetCommandBase<UnmergeCellsCommandContext>
	{
		protected override bool AffectsLayoutOverride(UnmergeCellsCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(UnmergeCellsCommandContext context)
		{
			context.OldMergedCellRanges = context.Worksheet.Cells.MergedCellRanges.GetIntersectingMergedRanges(context.CellRangeToUnmerge);
		}

		protected override void ExecuteOverride(UnmergeCellsCommandContext context)
		{
			context.Worksheet.Cells.MergedCellRanges.RemoveMergedRanges(context.OldMergedCellRanges);
		}

		protected override void UndoOverride(UnmergeCellsCommandContext context)
		{
			context.Worksheet.Cells.MergedCellRanges.AddMergedRanges(context.OldMergedCellRanges);
		}
	}
}
