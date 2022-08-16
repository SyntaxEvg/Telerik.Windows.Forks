using System;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class MoveSortConditionCommand : UndoableWorksheetCommandBase<MoveSortConditionCommandContext>
	{
		protected override void PreserveStateBeforeExecute(MoveSortConditionCommandContext context)
		{
		}

		protected override void UndoOverride(MoveSortConditionCommandContext context)
		{
			context.Worksheet.SortState[context.OrderIndex] = context.Condition;
		}

		protected override bool AffectsLayoutOverride(MoveSortConditionCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(MoveSortConditionCommandContext context)
		{
			ISortCondition value = (ISortCondition)((ITranslatable)context.Condition).Copy(context.NewIndex);
			context.Worksheet.SortState[context.OrderIndex] = value;
		}
	}
}
