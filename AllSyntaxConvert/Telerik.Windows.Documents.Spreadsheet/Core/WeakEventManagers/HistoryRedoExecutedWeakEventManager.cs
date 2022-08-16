using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.History;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class HistoryRedoExecutedWeakEventManager : WeakEventManager
	{
		public static HistoryRedoExecutedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(HistoryRedoExecutedWeakEventManager);
				HistoryRedoExecutedWeakEventManager historyRedoExecutedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as HistoryRedoExecutedWeakEventManager;
				if (historyRedoExecutedWeakEventManager == null)
				{
					historyRedoExecutedWeakEventManager = new HistoryRedoExecutedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, historyRedoExecutedWeakEventManager);
				}
				return historyRedoExecutedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			HistoryRedoExecutedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			HistoryRedoExecutedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.RedoExecuted += new EventHandler<WorkbookHistoryEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.RedoExecuted -= new EventHandler<WorkbookHistoryEventArgs>(base.DeliverEvent);
		}
	}
}
