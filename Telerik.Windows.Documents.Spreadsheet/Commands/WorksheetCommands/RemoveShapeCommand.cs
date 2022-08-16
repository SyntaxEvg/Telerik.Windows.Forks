using System;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveShapeCommand : UndoableWorksheetCommandBase<AddRemoveShapeCommandContext>
	{
		protected override bool AffectsLayoutOverride(AddRemoveShapeCommandContext context)
		{
			return true;
		}

		protected override void PreserveStateBeforeExecute(AddRemoveShapeCommandContext context)
		{
		}

		protected override void ExecuteOverride(AddRemoveShapeCommandContext context)
		{
			context.Worksheet.Shapes.RemoveInternal(context.Shape);
		}

		protected override void UndoOverride(AddRemoveShapeCommandContext context)
		{
			context.Worksheet.Shapes.AddInternal(context.Shape);
		}

		protected override bool CanExecuteOverride(AddRemoveShapeCommandContext context)
		{
			return context.Worksheet.Shapes.Contains(context.Shape);
		}
	}
}
