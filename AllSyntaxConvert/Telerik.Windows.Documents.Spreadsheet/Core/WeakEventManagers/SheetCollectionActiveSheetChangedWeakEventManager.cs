using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class SheetCollectionActiveSheetChangedWeakEventManager : WeakEventManager
	{
		public static SheetCollectionActiveSheetChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(SheetCollectionActiveSheetChangedWeakEventManager);
				SheetCollectionActiveSheetChangedWeakEventManager sheetCollectionActiveSheetChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as SheetCollectionActiveSheetChangedWeakEventManager;
				if (sheetCollectionActiveSheetChangedWeakEventManager == null)
				{
					sheetCollectionActiveSheetChangedWeakEventManager = new SheetCollectionActiveSheetChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, sheetCollectionActiveSheetChangedWeakEventManager);
				}
				return sheetCollectionActiveSheetChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			SheetCollectionActiveSheetChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			SheetCollectionActiveSheetChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			SheetCollection sheetCollection = (SheetCollection)source;
			sheetCollection.ActiveSheetChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			SheetCollection sheetCollection = (SheetCollection)source;
			sheetCollection.ActiveSheetChanged -= base.DeliverEvent;
		}
	}
}
