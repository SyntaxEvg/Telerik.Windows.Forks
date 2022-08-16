using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorksheetProtectedChangedWeakEventManager : WeakEventManager
	{
		public static WorksheetProtectedChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorksheetProtectedChangedWeakEventManager);
				WorksheetProtectedChangedWeakEventManager worksheetProtectedChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorksheetProtectedChangedWeakEventManager;
				if (worksheetProtectedChangedWeakEventManager == null)
				{
					worksheetProtectedChangedWeakEventManager = new WorksheetProtectedChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, worksheetProtectedChangedWeakEventManager);
				}
				return worksheetProtectedChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorksheetProtectedChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorksheetProtectedChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Worksheet worksheet = (Worksheet)source;
			worksheet.IsProtectedChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			Worksheet worksheet = (Worksheet)source;
			worksheet.IsProtectedChanged -= base.DeliverEvent;
		}
	}
}
