using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class ShapeCollectionChangedWeakEventManager : WeakEventManager
	{
		public static ShapeCollectionChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(ShapeCollectionChangedWeakEventManager);
				ShapeCollectionChangedWeakEventManager shapeCollectionChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as ShapeCollectionChangedWeakEventManager;
				if (shapeCollectionChangedWeakEventManager == null)
				{
					shapeCollectionChangedWeakEventManager = new ShapeCollectionChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, shapeCollectionChangedWeakEventManager);
				}
				return shapeCollectionChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			ShapeCollectionChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			ShapeCollectionChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			ShapeCollection shapeCollection = (ShapeCollection)source;
			shapeCollection.Changed += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			ShapeCollection shapeCollection = (ShapeCollection)source;
			shapeCollection.Changed -= base.DeliverEvent;
		}
	}
}
