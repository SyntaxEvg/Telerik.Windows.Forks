using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class MergeCellsCommand : UndoableWorksheetCommandBase<MergeCellsCommandContext>
	{
		protected override bool AffectsLayoutOverride(MergeCellsCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(MergeCellsCommandContext context)
		{
			context.OldMergedCellRanges = context.Worksheet.Cells.MergedCellRanges.GetIntersectingMergedRanges(context.NewMergedCellRange);
		}

		protected override bool CanExecuteOverride(MergeCellsCommandContext context)
		{
			return context.NewMergedCellRange.RowCount > 1 || context.NewMergedCellRange.ColumnCount > 1;
		}

		protected override void ExecuteOverride(MergeCellsCommandContext context)
		{
			context.Worksheet.Cells.MergedCellRanges.AddMergedRange(context.NewMergedCellRange);
		}

		protected override void UndoOverride(MergeCellsCommandContext context)
		{
			MergedCellRanges mergedCellRanges = context.Worksheet.Cells.MergedCellRanges;
			using (new UpdateScope(new Action(mergedCellRanges.BeginUpdate), new Action(mergedCellRanges.EndUpdate)))
			{
				mergedCellRanges.RemoveMergedRange(context.NewMergedCellRange);
				mergedCellRanges.AddMergedRanges(context.OldMergedCellRanges);
			}
		}
	}
}
