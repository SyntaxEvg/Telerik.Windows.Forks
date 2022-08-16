using System;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetFilterRangeCommand : UndoableWorksheetCommandBase<SetFilterRangeCommandContext>
	{
		protected override void PreserveStateBeforeExecute(SetFilterRangeCommandContext context)
		{
			AutoFilter filter = context.Worksheet.Filter;
			context.OldRange = filter.FilterRange;
		}

		protected override void UndoOverride(SetFilterRangeCommandContext context)
		{
			AutoFilter filter = context.Worksheet.Filter;
			filter.SetFilterRangeInternal(context.OldRange);
		}

		protected override bool AffectsLayoutOverride(SetFilterRangeCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(SetFilterRangeCommandContext context)
		{
			context.Worksheet.Filter.SetFilterRangeInternal(context.NewRange);
		}
	}
}
