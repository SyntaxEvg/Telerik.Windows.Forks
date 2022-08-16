using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorkbookContentChangedWeakEventManager : WeakEventManager
	{
		public static WorkbookContentChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorkbookContentChangedWeakEventManager);
				WorkbookContentChangedWeakEventManager workbookContentChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorkbookContentChangedWeakEventManager;
				if (workbookContentChangedWeakEventManager == null)
				{
					workbookContentChangedWeakEventManager = new WorkbookContentChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, workbookContentChangedWeakEventManager);
				}
				return workbookContentChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorkbookContentChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorkbookContentChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.WorkbookContentChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.WorkbookContentChanged -= base.DeliverEvent;
		}
	}
}
