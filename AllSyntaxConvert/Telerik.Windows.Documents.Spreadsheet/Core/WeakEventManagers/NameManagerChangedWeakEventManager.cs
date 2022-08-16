using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class NameManagerChangedWeakEventManager : WeakEventManager
	{
		public static NameManagerChangedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(NameManagerChangedWeakEventManager);
				NameManagerChangedWeakEventManager nameManagerChangedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as NameManagerChangedWeakEventManager;
				if (nameManagerChangedWeakEventManager == null)
				{
					nameManagerChangedWeakEventManager = new NameManagerChangedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, nameManagerChangedWeakEventManager);
				}
				return nameManagerChangedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			NameManagerChangedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			NameManagerChangedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			NameManager nameManager = (NameManager)source;
			nameManager.Changed += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			NameManager nameManager = (NameManager)source;
			nameManager.Changed -= base.DeliverEvent;
		}
	}
}
