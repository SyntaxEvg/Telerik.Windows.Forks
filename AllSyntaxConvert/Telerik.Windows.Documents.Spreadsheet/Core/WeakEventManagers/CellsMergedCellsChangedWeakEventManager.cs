using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class CellsMergedCellsChangedWeakEventManager : WeakEventManager
	{
		public static CellsMergedCellsChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(CellsMergedCellsChangedWeakEventManager);
				CellsMergedCellsChangedWeakEventManager cellsMergedCellsChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as CellsMergedCellsChangedWeakEventManager;
				if (cellsMergedCellsChangedWeakEventManager == null)
				{
					cellsMergedCellsChangedWeakEventManager = new CellsMergedCellsChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, cellsMergedCellsChangedWeakEventManager);
				}
				return cellsMergedCellsChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			CellsMergedCellsChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			CellsMergedCellsChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Cells cells = (Cells)source;
			cells.MergedCellsChanged += new EventHandler<MergedCellRangesChangedEventArgs>(base.DeliverEvent);
		}

		protected override void StopListening(object source)
		{
			Cells cells = (Cells)source;
			cells.MergedCellsChanged -= new EventHandler<MergedCellRangesChangedEventArgs>(base.DeliverEvent);
		}
	}
}
