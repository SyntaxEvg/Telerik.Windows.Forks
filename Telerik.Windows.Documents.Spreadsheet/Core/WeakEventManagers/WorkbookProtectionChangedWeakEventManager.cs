using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorkbookProtectionChangedWeakEventManager : WeakEventManager
	{
		public static WorkbookProtectionChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorkbookProtectionChangedWeakEventManager);
				WorkbookProtectionChangedWeakEventManager workbookProtectionChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorkbookProtectionChangedWeakEventManager;
				if (workbookProtectionChangedWeakEventManager == null)
				{
					workbookProtectionChangedWeakEventManager = new WorkbookProtectionChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, workbookProtectionChangedWeakEventManager);
				}
				return workbookProtectionChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorkbookProtectionChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorkbookProtectionChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.IsProtectedChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.IsProtectedChanged -= base.DeliverEvent;
		}
	}
}
