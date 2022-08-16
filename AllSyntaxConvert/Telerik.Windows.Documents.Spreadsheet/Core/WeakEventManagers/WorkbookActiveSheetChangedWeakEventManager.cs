using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorkbookActiveSheetChangedWeakEventManager : WeakEventManager
	{
		public static WorkbookActiveSheetChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorkbookActiveSheetChangedWeakEventManager);
				WorkbookActiveSheetChangedWeakEventManager workbookActiveSheetChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorkbookActiveSheetChangedWeakEventManager;
				if (workbookActiveSheetChangedWeakEventManager == null)
				{
					workbookActiveSheetChangedWeakEventManager = new WorkbookActiveSheetChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, workbookActiveSheetChangedWeakEventManager);
				}
				return workbookActiveSheetChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorkbookActiveSheetChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorkbookActiveSheetChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.ActiveSheetChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.ActiveSheetChanged -= base.DeliverEvent;
		}
	}
}
