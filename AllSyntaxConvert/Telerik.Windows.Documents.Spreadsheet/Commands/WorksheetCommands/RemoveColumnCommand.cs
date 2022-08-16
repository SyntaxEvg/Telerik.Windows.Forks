using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveColumnCommand : UndoableWorksheetCommandBase<RemoveColumnCommandContext>
	{
		protected override bool AffectsLayoutOverride(RemoveColumnCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(RemoveColumnCommandContext context)
		{
			context.OldColumnProperties = new ColumnsPropertyBag();
			context.OldColumnProperties.CopyPropertiesFrom(context.Worksheet.Columns.PropertyBag, (long)context.FromIndex, (long)context.ToIndex);
			context.OldCellProperties = new CellsPropertyBag();
			context.OldCellProperties.CopyPropertiesFrom(context.Worksheet.Cells.PropertyBag, CellRange.FromColumnRange(context.FromIndex, context.ToIndex));
		}

		protected override void ExecuteOverride(RemoveColumnCommandContext context)
		{
			context.Worksheet.Columns.PropertyBag.Remove(context.FromIndex, context.ItemCount);
			context.Worksheet.Cells.RemoveColumn(context.FromIndex, context.ItemCount);
		}

		protected override void UndoOverride(RemoveColumnCommandContext context)
		{
			context.Worksheet.Columns.PropertyBag.Insert(context.FromIndex, context.ItemCount);
			context.Worksheet.Columns.PropertyBag.CopyPropertiesFrom(context.OldColumnProperties, (long)context.FromIndex, (long)context.ToIndex);
			context.Worksheet.Cells.InsertColumn(context.FromIndex, context.ItemCount, context.OldCellProperties);
		}
	}
}
