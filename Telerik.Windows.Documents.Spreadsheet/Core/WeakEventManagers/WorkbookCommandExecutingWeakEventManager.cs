using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Commands;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorkbookCommandExecutingWeakEventManager : WeakEventManager
	{
		public static WorkbookCommandExecutingWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorkbookCommandExecutingWeakEventManager);
				WorkbookCommandExecutingWeakEventManager workbookCommandExecutingWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorkbookCommandExecutingWeakEventManager;
				if (workbookCommandExecutingWeakEventManager == null)
				{
					workbookCommandExecutingWeakEventManager = new WorkbookCommandExecutingWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, workbookCommandExecutingWeakEventManager);
				}
				return workbookCommandExecutingWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorkbookCommandExecutingWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorkbookCommandExecutingWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.CommandExecuting += new EventHandler<CommandExecutingEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.CommandExecuting -= new EventHandler<CommandExecutingEventArgs>(base.DeliverEvent);
		}
	}
}
