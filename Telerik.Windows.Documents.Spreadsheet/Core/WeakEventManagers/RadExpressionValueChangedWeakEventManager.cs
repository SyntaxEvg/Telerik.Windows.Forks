using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Expressions;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class RadExpressionValueChangedWeakEventManager : WeakEventManager
	{
		public static RadExpressionValueChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(RadExpressionValueChangedWeakEventManager);
				RadExpressionValueChangedWeakEventManager radExpressionValueChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as RadExpressionValueChangedWeakEventManager;
				if (radExpressionValueChangedWeakEventManager == null)
				{
					radExpressionValueChangedWeakEventManager = new RadExpressionValueChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, radExpressionValueChangedWeakEventManager);
				}
				return radExpressionValueChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			RadExpressionValueChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			RadExpressionValueChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			RadExpression radExpression = (RadExpression)source;
			radExpression.ValueInvalidated += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			RadExpression radExpression = (RadExpression)source;
			radExpression.ValueInvalidated -= base.DeliverEvent;
		}
	}
}
