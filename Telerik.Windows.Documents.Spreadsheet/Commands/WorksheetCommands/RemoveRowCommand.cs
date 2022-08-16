using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveRowCommand : UndoableWorksheetCommandBase<RemoveRowCommandContext>
	{
		protected override bool AffectsLayoutOverride(RemoveRowCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(RemoveRowCommandContext context)
		{
			context.OldRowProperties = new RowsPropertyBag();
			context.OldRowProperties.CopyPropertiesFrom(context.Worksheet.Rows.PropertyBag, (long)context.FromIndex, (long)context.ToIndex);
			context.OldCellProperties = new CellsPropertyBag();
			context.OldCellProperties.CopyPropertiesFrom(context.Worksheet.Cells.PropertyBag, CellRange.FromRowRange(context.FromIndex, context.ToIndex));
		}

		protected override void ExecuteOverride(RemoveRowCommandContext context)
		{
			context.Worksheet.Rows.PropertyBag.Remove(context.FromIndex, context.ItemCount);
			context.Worksheet.Cells.RemoveRow(context.FromIndex, context.ItemCount);
		}

		protected override void UndoOverride(RemoveRowCommandContext context)
		{
			context.Worksheet.Rows.PropertyBag.Insert(context.FromIndex, context.ItemCount);
			context.Worksheet.Rows.PropertyBag.CopyPropertiesFrom(context.OldRowProperties, (long)context.FromIndex, (long)context.ToIndex);
			context.Worksheet.Cells.InsertRow(context.FromIndex, context.ItemCount, context.OldCellProperties);
		}
	}
}
