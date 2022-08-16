using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.History;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class HistoryUndoExecutedWeakEventManager : WeakEventManager
	{
		public static HistoryUndoExecutedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(HistoryUndoExecutedWeakEventManager);
				HistoryUndoExecutedWeakEventManager historyUndoExecutedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as HistoryUndoExecutedWeakEventManager;
				if (historyUndoExecutedWeakEventManager == null)
				{
					historyUndoExecutedWeakEventManager = new HistoryUndoExecutedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, historyUndoExecutedWeakEventManager);
				}
				return historyUndoExecutedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			HistoryUndoExecutedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			HistoryUndoExecutedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.UndoExecuted += new EventHandler<WorkbookHistoryEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.UndoExecuted -= new EventHandler<WorkbookHistoryEventArgs>(base.DeliverEvent);
		}
	}
}
