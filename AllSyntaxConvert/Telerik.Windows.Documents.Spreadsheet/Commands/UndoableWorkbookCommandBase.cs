using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	abstract class UndoableWorkbookCommandBase<T> : WorkbookCommandBase<T>, IUndoableWorkbookCommand, IWorkbookCommand where T : WorkbookCommandContextBase
	{
		protected override void OnExecuting(WorkbookCommandContextBase context)
		{
			base.OnExecuting(context);
			if (context.IsFirstExecution && context.Workbook.History.IsEnabled)
			{
				T context2 = (T)((object)context);
				this.PreserveStateBeforeExecute(context2);
			}
		}

		protected abstract void PreserveStateBeforeExecute(T context);

		public bool CanUndo(WorkbookCommandContextBase context)
		{
			return context.Workbook.History.IsEnabled && this.CanUndoOverride((T)((object)context));
		}

		protected virtual bool CanUndoOverride(T context)
		{
			return true;
		}

		public bool Undo(WorkbookCommandContextBase context)
		{
			Guard.ThrowExceptionIfNull<WorkbookCommandContextBase>(context, "context");
			if (!this.CanUndo(context))
			{
				return false;
			}
			T context2 = (T)((object)context);
			CommandExecutingEventArgs commandExecutingEventArgs = new CommandExecutingEventArgs(this, context, true);
			context.Workbook.OnCommandExecuting(commandExecutingEventArgs);
			if (commandExecutingEventArgs.Canceled)
			{
				return false;
			}
			try
			{
				this.UndoOverride(context2);
			}
			catch (Exception exception)
			{
				CommandErrorEventArgs args = new CommandErrorEventArgs(exception, this, context, true);
				context.Workbook.OnCommandError(args);
				return false;
			}
			context.Workbook.OnCommandExecuted(new CommandExecutedEventArgs(this, context, true));
			return true;
		}

		protected abstract void UndoOverride(T context);

		public bool Redo(WorkbookCommandContextBase context)
		{
			T context2 = (T)((object)context);
			this.PrepareRedoExecute(context2);
			return base.Execute(context);
		}

		protected virtual void PrepareRedoExecute(T context)
		{
		}
	}
}
