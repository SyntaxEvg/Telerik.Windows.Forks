using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorksheetViewStateUpdateNeededWeakEventManager : WeakEventManager
	{
		public static WorksheetViewStateUpdateNeededWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorksheetViewStateUpdateNeededWeakEventManager);
				WorksheetViewStateUpdateNeededWeakEventManager worksheetViewStateUpdateNeededWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorksheetViewStateUpdateNeededWeakEventManager;
				if (worksheetViewStateUpdateNeededWeakEventManager == null)
				{
					worksheetViewStateUpdateNeededWeakEventManager = new WorksheetViewStateUpdateNeededWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, worksheetViewStateUpdateNeededWeakEventManager);
				}
				return worksheetViewStateUpdateNeededWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorksheetViewStateUpdateNeededWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorksheetViewStateUpdateNeededWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Worksheet worksheet = (Worksheet)source;
			worksheet.ViewStateUpdateNeeded += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			Worksheet worksheet = (Worksheet)source;
			worksheet.ViewStateUpdateNeeded -= base.DeliverEvent;
		}
	}
}
