using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Commands;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorkbookCommandErrorWeakEventManager : WeakEventManager
	{
		public static WorkbookCommandErrorWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorkbookCommandErrorWeakEventManager);
				WorkbookCommandErrorWeakEventManager workbookCommandErrorWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorkbookCommandErrorWeakEventManager;
				if (workbookCommandErrorWeakEventManager == null)
				{
					workbookCommandErrorWeakEventManager = new WorkbookCommandErrorWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, workbookCommandErrorWeakEventManager);
				}
				return workbookCommandErrorWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorkbookCommandErrorWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorkbookCommandErrorWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.CommandError += new EventHandler<CommandErrorEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.CommandError -= new EventHandler<CommandErrorEventArgs>(base.DeliverEvent);
		}
	}
}
