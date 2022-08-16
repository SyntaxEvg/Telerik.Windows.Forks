using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Commands;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorkbookCommandExecutedWeakEventManager : WeakEventManager
	{
		public static WorkbookCommandExecutedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorkbookCommandExecutedWeakEventManager);
				WorkbookCommandExecutedWeakEventManager workbookCommandExecutedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorkbookCommandExecutedWeakEventManager;
				if (workbookCommandExecutedWeakEventManager == null)
				{
					workbookCommandExecutedWeakEventManager = new WorkbookCommandExecutedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, workbookCommandExecutedWeakEventManager);
				}
				return workbookCommandExecutedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorkbookCommandExecutedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorkbookCommandExecutedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.CommandExecuted += new EventHandler<CommandExecutedEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.CommandExecuted -= new EventHandler<CommandExecutedEventArgs>(base.DeliverEvent);
		}
	}
}
