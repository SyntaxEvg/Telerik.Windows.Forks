using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Core.WeakEventManagers
{
	class WorksheetCopiedWeakEventManager : WeakEventManager
	{
		public static WorksheetCopiedWeakEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(WorksheetCopiedWeakEventManager);
				WorksheetCopiedWeakEventManager worksheetCopiedWeakEventManager = WeakEventManager.GetCurrentManager(typeFromHandle) as WorksheetCopiedWeakEventManager;
				if (worksheetCopiedWeakEventManager == null)
				{
					worksheetCopiedWeakEventManager = new WorksheetCopiedWeakEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, worksheetCopiedWeakEventManager);
				}
				return worksheetCopiedWeakEventManager;
			}
		}

		public static void AddListener(object source, IWeakEventListener listener)
		{
			WorksheetCopiedWeakEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		public static void RemoveListener(object source, IWeakEventListener listener)
		{
			WorksheetCopiedWeakEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		protected override void StartListening(object source)
		{
			Worksheet worksheet = (Worksheet)source;
			worksheet.Copied += base.DeliverEvent;
		}

		protected override void StopListening(object source)
		{
			Worksheet worksheet = (Worksheet)source;
			worksheet.Copied -= base.DeliverEvent;
		}
	}
}
