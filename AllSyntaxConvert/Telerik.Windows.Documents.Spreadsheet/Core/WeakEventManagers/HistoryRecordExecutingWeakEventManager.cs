using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.History;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class HistoryRecordExecutingWeakEventManager : WeakEventManager
	{
		public static HistoryRecordExecutingWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(HistoryRecordExecutingWeakEventManager);
				HistoryRecordExecutingWeakEventManager historyRecordExecutingWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as HistoryRecordExecutingWeakEventManager;
				if (historyRecordExecutingWeakEventManager == null)
				{
					historyRecordExecutingWeakEventManager = new HistoryRecordExecutingWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, historyRecordExecutingWeakEventManager);
				}
				return historyRecordExecutingWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			HistoryRecordExecutingWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			HistoryRecordExecutingWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.RecordExecuting += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.RecordExecuting -= base.DeliverEvent;
		}
	}
}
