using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class CellsPropertyBagPropertyChangedWeakEventManager : WeakEventManager
	{
		public static CellsPropertyBagPropertyChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(CellsPropertyBagPropertyChangedWeakEventManager);
				CellsPropertyBagPropertyChangedWeakEventManager cellsPropertyBagPropertyChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as CellsPropertyBagPropertyChangedWeakEventManager;
				if (cellsPropertyBagPropertyChangedWeakEventManager == null)
				{
					cellsPropertyBagPropertyChangedWeakEventManager = new CellsPropertyBagPropertyChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, cellsPropertyBagPropertyChangedWeakEventManager);
				}
				return cellsPropertyBagPropertyChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			CellsPropertyBagPropertyChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			CellsPropertyBagPropertyChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			CellsPropertyBag cellsPropertyBag = (CellsPropertyBag)source;
			cellsPropertyBag.PropertyChanged += new EventHandler<CellPropertyChangedEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			CellsPropertyBag cellsPropertyBag = (CellsPropertyBag)source;
			cellsPropertyBag.PropertyChanged -= new EventHandler<CellPropertyChangedEventArgs>(base.DeliverEvent);
		}
	}
}
