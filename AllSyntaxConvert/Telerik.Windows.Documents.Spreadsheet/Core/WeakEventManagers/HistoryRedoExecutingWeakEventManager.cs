using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.History;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class HistoryRedoExecutingWeakEventManager : WeakEventManager
	{
		public static HistoryRedoExecutingWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(HistoryRedoExecutingWeakEventManager);
				HistoryRedoExecutingWeakEventManager historyRedoExecutingWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as HistoryRedoExecutingWeakEventManager;
				if (historyRedoExecutingWeakEventManager == null)
				{
					historyRedoExecutingWeakEventManager = new HistoryRedoExecutingWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, historyRedoExecutingWeakEventManager);
				}
				return historyRedoExecutingWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			HistoryRedoExecutingWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			HistoryRedoExecutingWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.RedoExecuting += new EventHandler<WorkbookHistoryEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.RedoExecuting -= new EventHandler<WorkbookHistoryEventArgs>(base.DeliverEvent);
		}
	}
}
