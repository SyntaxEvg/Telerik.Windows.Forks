using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RearrangeFilterRowsCommand : UndoableWorksheetCommandBase<RearrangeFilterRowsCommandContext>
	{
		protected override void PreserveStateBeforeExecute(RearrangeFilterRowsCommandContext context)
		{
		}

		protected override void UndoOverride(RearrangeFilterRowsCommandContext context)
		{
			ShiftType shiftType;
			if (context.ShiftType == ShiftType.Down)
			{
				shiftType = ShiftType.Up;
			}
			else
			{
				if (context.ShiftType != ShiftType.Up)
				{
					throw new NotSupportedException();
				}
				shiftType = ShiftType.Down;
			}
			context.Worksheet.Filter.RearrangeFilterRows(context.AbsoluteIndex, context.Delta, shiftType);
		}

		protected override bool AffectsLayoutOverride(RearrangeFilterRowsCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(RearrangeFilterRowsCommandContext context)
		{
			context.Worksheet.Filter.RearrangeFilterRows(context.AbsoluteIndex, context.Delta, context.ShiftType);
		}
	}
}
