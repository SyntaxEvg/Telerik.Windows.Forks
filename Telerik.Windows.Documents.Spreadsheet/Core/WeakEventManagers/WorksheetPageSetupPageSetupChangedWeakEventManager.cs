using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorksheetPageSetupPageSetupChangedWeakEventManager : WeakEventManager
	{
		public static WorksheetPageSetupPageSetupChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorksheetPageSetupPageSetupChangedWeakEventManager);
				WorksheetPageSetupPageSetupChangedWeakEventManager worksheetPageSetupPageSetupChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorksheetPageSetupPageSetupChangedWeakEventManager;
				if (worksheetPageSetupPageSetupChangedWeakEventManager == null)
				{
					worksheetPageSetupPageSetupChangedWeakEventManager = new WorksheetPageSetupPageSetupChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, worksheetPageSetupPageSetupChangedWeakEventManager);
				}
				return worksheetPageSetupPageSetupChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorksheetPageSetupPageSetupChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorksheetPageSetupPageSetupChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			WorksheetPageSetup worksheetPageSetup = (WorksheetPageSetup)source;
			worksheetPageSetup.PageSetupChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			WorksheetPageSetup worksheetPageSetup = (WorksheetPageSetup)source;
			worksheetPageSetup.PageSetupChanged -= base.DeliverEvent;
		}
	}
}
