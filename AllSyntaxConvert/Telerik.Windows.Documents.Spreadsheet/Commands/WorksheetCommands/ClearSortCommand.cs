using System;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class ClearSortCommand : UndoableWorksheetCommandBase<ClearSortCommandContext>
	{
		protected override bool AffectsLayoutOverride(ClearSortCommandContext context)
		{
			return false;
		}

		protected override void PreserveStateBeforeExecute(ClearSortCommandContext context)
		{
			Worksheet worksheet = context.Worksheet;
			context.SortRange = worksheet.SortState.SortRange;
			context.SortConditions = worksheet.SortState.SortConditions.ToArray<ISortCondition>();
		}

		protected override void UndoOverride(ClearSortCommandContext context)
		{
			SortState sortState = context.Worksheet.SortState;
			sortState.AddRange(context.SortConditions);
			sortState.SetSortRangeInternal(context.SortRange);
		}

		protected override void ExecuteOverride(ClearSortCommandContext context)
		{
			Worksheet worksheet = context.Worksheet;
			worksheet.SortState.ClearInternal();
		}
	}
}
