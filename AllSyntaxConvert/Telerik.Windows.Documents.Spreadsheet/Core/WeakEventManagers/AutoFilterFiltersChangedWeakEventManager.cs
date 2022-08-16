using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class AutoFilterFiltersChangedWeakEventManager : WeakEventManager
	{
		public static AutoFilterFiltersChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(AutoFilterFiltersChangedWeakEventManager);
				AutoFilterFiltersChangedWeakEventManager autoFilterFiltersChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as AutoFilterFiltersChangedWeakEventManager;
				if (autoFilterFiltersChangedWeakEventManager == null)
				{
					autoFilterFiltersChangedWeakEventManager = new AutoFilterFiltersChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, autoFilterFiltersChangedWeakEventManager);
				}
				return autoFilterFiltersChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			AutoFilterFiltersChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			AutoFilterFiltersChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			AutoFilter autoFilter = (AutoFilter)source;
			autoFilter.FiltersChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			AutoFilter autoFilter = (AutoFilter)source;
			autoFilter.FiltersChanged -= base.DeliverEvent;
		}
	}
}
