using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorkbookThemeChangedWeakEventManager : WeakEventManager
	{
		public static WorkbookThemeChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorkbookThemeChangedWeakEventManager);
				WorkbookThemeChangedWeakEventManager workbookThemeChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorkbookThemeChangedWeakEventManager;
				if (workbookThemeChangedWeakEventManager == null)
				{
					workbookThemeChangedWeakEventManager = new WorkbookThemeChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, workbookThemeChangedWeakEventManager);
				}
				return workbookThemeChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorkbookThemeChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorkbookThemeChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.ThemeChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			Workbook workbook = (Workbook)source;
			workbook.ThemeChanged -= base.DeliverEvent;
		}
	}
}
