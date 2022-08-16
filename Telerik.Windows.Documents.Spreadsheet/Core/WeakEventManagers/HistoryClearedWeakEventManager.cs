using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.History;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class HistoryClearedWeakEventManager : WeakEventManager
	{
		public static HistoryClearedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(HistoryClearedWeakEventManager);
				HistoryClearedWeakEventManager historyClearedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as HistoryClearedWeakEventManager;
				if (historyClearedWeakEventManager == null)
				{
					historyClearedWeakEventManager = new HistoryClearedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, historyClearedWeakEventManager);
				}
				return historyClearedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			HistoryClearedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			HistoryClearedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.Cleared += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			WorkbookHistory workbookHistory = (WorkbookHistory)source;
			workbookHistory.Cleared -= base.DeliverEvent;
		}
	}
}
