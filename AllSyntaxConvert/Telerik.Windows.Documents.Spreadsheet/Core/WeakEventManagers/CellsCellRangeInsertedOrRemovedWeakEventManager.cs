using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class CellsCellRangeInsertedOrRemovedWeakEventManager : WeakEventManager
	{
		public static CellsCellRangeInsertedOrRemovedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(CellsCellRangeInsertedOrRemovedWeakEventManager);
				CellsCellRangeInsertedOrRemovedWeakEventManager cellsCellRangeInsertedOrRemovedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as CellsCellRangeInsertedOrRemovedWeakEventManager;
				if (cellsCellRangeInsertedOrRemovedWeakEventManager == null)
				{
					cellsCellRangeInsertedOrRemovedWeakEventManager = new CellsCellRangeInsertedOrRemovedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, cellsCellRangeInsertedOrRemovedWeakEventManager);
				}
				return cellsCellRangeInsertedOrRemovedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			CellsCellRangeInsertedOrRemovedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			CellsCellRangeInsertedOrRemovedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Cells cells = (Cells)source;
			cells.CellRangeInsertedOrRemoved += new EventHandler<CellRangeInsertedOrRemovedEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			Cells cells = (Cells)source;
			cells.CellRangeInsertedOrRemoved -= new EventHandler<CellRangeInsertedOrRemovedEventArgs>(base.DeliverEvent);
		}
	}
}
