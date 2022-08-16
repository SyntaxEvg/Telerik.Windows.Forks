using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetSortCommand : UndoableWorksheetCommandBase<SetSortCommandContext>
	{
		protected override bool AffectsLayoutOverride(SetSortCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(SetSortCommandContext context)
		{
			int rowCount = context.SortRange.RowCount;
			int[] array = new int[rowCount];
			int[] array2 = new int[rowCount];
			for (int i = 0; i < rowCount; i++)
			{
				array[i] = context.SortedIndexes[i];
				array2[i] = i;
			}
			Array.Sort<int, int>(array, array2);
			int[] array3 = new int[rowCount];
			for (int j = 0; j < rowCount; j++)
			{
				array3[j] = array2[j];
			}
			context.OldSortedIndexes = array3;
		}

		protected override void UndoOverride(SetSortCommandContext context)
		{
			Worksheet worksheet = context.Worksheet;
			worksheet.SortState.RemoveFirst();
			worksheet.Cells.PropertyBag.Sort(worksheet, context.SortRange, context.OldSortedIndexes);
			worksheet.SortState.SetSortRangeInternal(context.SortRange);
		}

		protected override void ExecuteOverride(SetSortCommandContext context)
		{
			Worksheet worksheet = context.Worksheet;
			worksheet.Cells.PropertyBag.Sort(worksheet, context.SortRange, context.SortedIndexes);
			worksheet.SortState.AddFirstInternal(context.SortCondition);
		}
	}
}
