using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class ShapeCollectionShapeChangedWeakEventManager : WeakEventManager
	{
		public static ShapeCollectionShapeChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(ShapeCollectionShapeChangedWeakEventManager);
				ShapeCollectionShapeChangedWeakEventManager shapeCollectionShapeChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as ShapeCollectionShapeChangedWeakEventManager;
				if (shapeCollectionShapeChangedWeakEventManager == null)
				{
					shapeCollectionShapeChangedWeakEventManager = new ShapeCollectionShapeChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, shapeCollectionShapeChangedWeakEventManager);
				}
				return shapeCollectionShapeChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			ShapeCollectionShapeChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			ShapeCollectionShapeChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			ShapeCollection shapeCollection = (ShapeCollection)source;
			shapeCollection.ShapeChanged += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			ShapeCollection shapeCollection = (ShapeCollection)source;
			shapeCollection.ShapeChanged -= base.DeliverEvent;
		}
	}
}
