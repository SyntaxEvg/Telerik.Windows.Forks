using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class AutoFilterFilterRangeChangedWeakEventManager : WeakEventManager
	{
		public static AutoFilterFilterRangeChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(AutoFilterFilterRangeChangedWeakEventManager);
				AutoFilterFilterRangeChangedWeakEventManager autoFilterFilterRangeChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as AutoFilterFilterRangeChangedWeakEventManager;
				if (autoFilterFilterRangeChangedWeakEventManager == null)
				{
					autoFilterFilterRangeChangedWeakEventManager = new AutoFilterFilterRangeChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, autoFilterFilterRangeChangedWeakEventManager);
				}
				return autoFilterFilterRangeChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			AutoFilterFilterRangeChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			AutoFilterFilterRangeChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			AutoFilter autoFilter = (AutoFilter)source;
			autoFilter.FilterRangeChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			AutoFilter autoFilter = (AutoFilter)source;
			autoFilter.FilterRangeChanged -= base.DeliverEvent;
		}
	}
}
