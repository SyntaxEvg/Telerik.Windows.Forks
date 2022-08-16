using System;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SendShapeBackwardCommand : UndoableWorksheetCommandBase<MoveShapeInDepthCommandContext>
	{
		protected override void PreserveStateBeforeExecute(MoveShapeInDepthCommandContext context)
		{
			foreach (FloatingShapeBase floatingShapeBase in context.Shapes)
			{
				int zindex = context.Worksheet.Shapes.GetZIndex(floatingShapeBase);
				context.OldShapeIndices.Add(floatingShapeBase, zindex);
			}
		}

		protected override void UndoOverride(MoveShapeInDepthCommandContext context)
		{
			context.Worksheet.Shapes.MoveShapesToIndices(context.OldShapeIndices);
		}

		protected override bool AffectsLayoutOverride(MoveShapeInDepthCommandContext context)
		{
			return true;
		}

		protected override void ExecuteOverride(MoveShapeInDepthCommandContext context)
		{
			context.Worksheet.Shapes.SendBackwardInternal(context.Shapes);
		}
	}
}
