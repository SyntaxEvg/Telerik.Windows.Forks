using System;
using System.ComponentModel;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class SheetPropertyChangedWeakEventManager : WeakEventManager
	{
		public static SheetPropertyChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(SheetPropertyChangedWeakEventManager);
				SheetPropertyChangedWeakEventManager sheetPropertyChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as SheetPropertyChangedWeakEventManager;
				if (sheetPropertyChangedWeakEventManager == null)
				{
					sheetPropertyChangedWeakEventManager = new SheetPropertyChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, sheetPropertyChangedWeakEventManager);
				}
				return sheetPropertyChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			SheetPropertyChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			SheetPropertyChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Sheet sheet = (Sheet)source;
			sheet.PropertyChanged += new PropertyChangedEventHandler(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			Sheet sheet = (Sheet)source;
			sheet.PropertyChanged -= new PropertyChangedEventHandler(base.DeliverEvent);
		}
	}
}
