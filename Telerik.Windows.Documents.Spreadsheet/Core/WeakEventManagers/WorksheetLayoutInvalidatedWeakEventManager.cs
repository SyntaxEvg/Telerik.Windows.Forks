using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorksheetLayoutInvalidatedWeakEventManager : WeakEventManager
	{
		public static WorksheetLayoutInvalidatedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorksheetLayoutInvalidatedWeakEventManager);
				WorksheetLayoutInvalidatedWeakEventManager worksheetLayoutInvalidatedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorksheetLayoutInvalidatedWeakEventManager;
				if (worksheetLayoutInvalidatedWeakEventManager == null)
				{
					worksheetLayoutInvalidatedWeakEventManager = new WorksheetLayoutInvalidatedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, worksheetLayoutInvalidatedWeakEventManager);
				}
				return worksheetLayoutInvalidatedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorksheetLayoutInvalidatedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorksheetLayoutInvalidatedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Worksheet worksheet = (Worksheet)source;
			worksheet.LayoutInvalidated += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			Worksheet worksheet = (Worksheet)source;
			worksheet.LayoutInvalidated -= base.DeliverEvent;
		}
	}
}
