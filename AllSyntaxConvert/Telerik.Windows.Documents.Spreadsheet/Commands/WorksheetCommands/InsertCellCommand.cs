using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class InsertCellCommand : UndoableWorksheetCommandBase<InsertCellCommandContext>
	{
		protected override bool AffectsLayoutOverride(InsertCellCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(InsertCellCommandContext context)
		{
		}

		protected override bool CanExecuteOverride(InsertCellCommandContext context)
		{
			return context.Worksheet.Cells.CanInsert(context.CellRange, context.ShiftType);
		}

		protected override void ExecuteOverride(InsertCellCommandContext context)
		{
			context.Worksheet.Cells.InsertInternal(context.CellRange, context.ShiftType, null);
		}

		protected override void UndoOverride(InsertCellCommandContext context)
		{
			RemoveShiftType shiftType = context.ShiftType.ReverseShiftType();
			context.Worksheet.Cells.RemoveInternal(context.CellRange, shiftType);
		}
	}
}
