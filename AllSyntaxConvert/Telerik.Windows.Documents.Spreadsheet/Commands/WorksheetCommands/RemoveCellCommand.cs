using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveCellCommand : UndoableWorksheetCommandBase<RemoveCellCommandContext>
	{
		protected override bool AffectsLayoutOverride(RemoveCellCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(RemoveCellCommandContext context)
		{
			context.OldCellProperties = new CellsPropertyBag();
			context.OldCellProperties.CopyPropertiesFrom(context.Worksheet.Cells.PropertyBag, context.CellRange);
		}

		protected override bool CanExecuteOverride(RemoveCellCommandContext context)
		{
			return context.Worksheet.Cells.CanRemove(context.CellRange, context.ShiftType);
		}

		protected override void ExecuteOverride(RemoveCellCommandContext context)
		{
			context.Worksheet.Cells.RemoveInternal(context.CellRange, context.ShiftType);
		}

		protected override void UndoOverride(RemoveCellCommandContext context)
		{
			InsertShiftType shiftType = context.ShiftType.ReverseShiftType();
			context.Worksheet.Cells.InsertInternal(context.CellRange, shiftType, context.OldCellProperties);
		}
	}
}
