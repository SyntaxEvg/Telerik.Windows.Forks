using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	abstract class WorkbookCommandBase<T> : IWorkbookCommand where T : WorkbookCommandContextBase
	{
		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(this.name))
				{
					this.name = base.GetType().Name;
				}
				return this.name;
			}
		}

		internal WorkbookCommandBase()
		{
		}

		public bool AffectsLayout(WorkbookCommandContextBase context)
		{
			Guard.ThrowExceptionIfNull<WorkbookCommandContextBase>(context, "context");
			return this.AffectsLayoutOverride((T)((object)context));
		}

		protected abstract bool AffectsLayoutOverride(T context);

		public bool CanExecute(WorkbookCommandContextBase context)
		{
			Guard.ThrowExceptionIfNull<WorkbookCommandContextBase>(context, "context");
			return this.CanExecuteOverride((T)((object)context));
		}

		protected virtual bool CanExecuteOverride(T context)
		{
			return true;
		}

		public bool Execute(WorkbookCommandContextBase context)
		{
			Guard.ThrowExceptionIfNull<WorkbookCommandContextBase>(context, "context");
			if (!this.CanExecute(context))
			{
				return false;
			}
			CommandExecutingEventArgs commandExecutingEventArgs = new CommandExecutingEventArgs(this, context, false);
			context.Workbook.OnCommandExecuting(commandExecutingEventArgs);
			if (commandExecutingEventArgs.Canceled)
			{
				return false;
			}
			context.OnExecuting();
			this.OnExecuting(context);
			try
			{
				this.ExecuteOverride((T)((object)context));
			}
			catch (Exception exception)
			{
				CommandErrorEventArgs args = new CommandErrorEventArgs(exception, this, context, false);
				context.Workbook.OnCommandError(args);
				return false;
			}
			context.OnExecuted();
			context.Workbook.OnCommandExecuted(new CommandExecutedEventArgs(this, context, false));
			return true;
		}

		protected virtual void OnExecuting(WorkbookCommandContextBase context)
		{
		}

		protected abstract void ExecuteOverride(T context);

		string name;
	}
}
