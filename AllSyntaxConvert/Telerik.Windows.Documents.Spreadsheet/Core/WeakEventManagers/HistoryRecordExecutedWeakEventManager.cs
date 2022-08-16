using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.History;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class HistoryRecordExecutedWeakEventManager : WeakEventManager
	{
		public static HistoryRecordExecutedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(HistoryRecordExecutedWeakEventManager);
				HistoryRecordExecutedWeakEventManager historyRecordExecutedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as HistoryRecordExecutedWeakEventManager;
				if (historyRecordExecutedWeakEventManager == null)
				{
					historyRecordExecutedWeakEventManager = new HistoryRecordExecutedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, historyRecordExecutedWeakEventManager);
				}
				return historyRecordExecutedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			HistoryRecordExecutedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			HistoryRecordExecutedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.RecordExecuted += new EventHandler<WorkbookHistoryRecordExecutedEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.RecordExecuted -= new EventHandler<WorkbookHistoryRecordExecutedEventArgs>(base.DeliverEvent);
		}
	}
}
