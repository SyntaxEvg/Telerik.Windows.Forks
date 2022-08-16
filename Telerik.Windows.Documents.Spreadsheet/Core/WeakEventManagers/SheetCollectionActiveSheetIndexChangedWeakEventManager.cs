using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class SheetCollectionActiveSheetIndexChangedWeakEventManager : WeakEventManager
	{
		public static SheetCollectionActiveSheetIndexChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(SheetCollectionActiveSheetIndexChangedWeakEventManager);
				SheetCollectionActiveSheetIndexChangedWeakEventManager sheetCollectionActiveSheetIndexChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as SheetCollectionActiveSheetIndexChangedWeakEventManager;
				if (sheetCollectionActiveSheetIndexChangedWeakEventManager == null)
				{
					sheetCollectionActiveSheetIndexChangedWeakEventManager = new SheetCollectionActiveSheetIndexChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, sheetCollectionActiveSheetIndexChangedWeakEventManager);
				}
				return sheetCollectionActiveSheetIndexChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			SheetCollectionActiveSheetIndexChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			SheetCollectionActiveSheetIndexChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			SheetCollection sheetCollection = (SheetCollection)source;
			sheetCollection.ActiveSheetIndexChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			SheetCollection sheetCollection = (SheetCollection)source;
			sheetCollection.ActiveSheetIndexChanged -= base.DeliverEvent;
		}
	}
}
