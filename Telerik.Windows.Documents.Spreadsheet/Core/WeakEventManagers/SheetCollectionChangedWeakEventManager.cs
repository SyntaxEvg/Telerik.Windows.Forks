using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class SheetCollectionChangedWeakEventManager : WeakEventManager
	{
		public static SheetCollectionChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(SheetCollectionChangedWeakEventManager);
				SheetCollectionChangedWeakEventManager sheetCollectionChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as SheetCollectionChangedWeakEventManager;
				if (sheetCollectionChangedWeakEventManager == null)
				{
					sheetCollectionChangedWeakEventManager = new SheetCollectionChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, sheetCollectionChangedWeakEventManager);
				}
				return sheetCollectionChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			SheetCollectionChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			SheetCollectionChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			SheetCollection sheetCollection = (SheetCollection)source;
			sheetCollection.Changed += new EventHandler<SheetCollectionChangedEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			SheetCollection sheetCollection = (SheetCollection)source;
			sheetCollection.Changed -= new EventHandler<SheetCollectionChangedEventArgs>(base.DeliverEvent);
		}
	}
}
