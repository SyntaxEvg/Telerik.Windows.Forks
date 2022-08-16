using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	abstract class SetWorkbookPropertyCommand<T> : UndoableWorkbookCommandBase<SetWorkbookPropertyCommandContext<T>>
	{
		protected override void PreserveStateBeforeExecute(SetWorkbookPropertyCommandContext<T> context)
		{
			context.OldValue = this.GetPropertyValue(context.Workbook);
		}

		protected override void ExecuteOverride(SetWorkbookPropertyCommandContext<T> context)
		{
			this.SetPropertyValue(context.Workbook, context.NewValue);
		}

		protected override void UndoOverride(SetWorkbookPropertyCommandContext<T> context)
		{
			this.SetPropertyValue(context.Workbook, context.OldValue);
		}

		protected abstract T GetPropertyValue(Workbook workbook);

		protected abstract void SetPropertyValue(Workbook workbook, T value);
	}
}
