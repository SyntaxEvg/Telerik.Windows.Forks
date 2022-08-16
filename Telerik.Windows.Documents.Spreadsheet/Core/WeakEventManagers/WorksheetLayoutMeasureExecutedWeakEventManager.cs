using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Layout;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorksheetLayoutMeasureExecutedWeakEventManager : WeakEventManager
	{
		public static WorksheetLayoutMeasureExecutedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorksheetLayoutMeasureExecutedWeakEventManager);
				WorksheetLayoutMeasureExecutedWeakEventManager worksheetLayoutMeasureExecutedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorksheetLayoutMeasureExecutedWeakEventManager;
				if (worksheetLayoutMeasureExecutedWeakEventManager == null)
				{
					worksheetLayoutMeasureExecutedWeakEventManager = new WorksheetLayoutMeasureExecutedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, worksheetLayoutMeasureExecutedWeakEventManager);
				}
				return worksheetLayoutMeasureExecutedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorksheetLayoutMeasureExecutedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorksheetLayoutMeasureExecutedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			RadWorksheetLayout radWorksheetLayout = (RadWorksheetLayout)source;
			radWorksheetLayout.MeasureExecuted += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			RadWorksheetLayout radWorksheetLayout = (RadWorksheetLayout)source;
			radWorksheetLayout.MeasureExecuted -= base.DeliverEvent;
		}
	}
}
