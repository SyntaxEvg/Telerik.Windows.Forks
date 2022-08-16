using System;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Validation
{
	class RepairRule<T> : IRepairRule where T : DocumentElementBase
	{
		public RepairRule(Action<T> repairAction)
		{
			Guard.ThrowExceptionIfNull<Action<T>>(repairAction, "repairAction");
			this.repairAction = repairAction;
		}

		public void Repair(DocumentElementBase documentElement)
		{
			Guard.ThrowExceptionIfNull<DocumentElementBase>(documentElement, "documentElement");
			this.repairAction((T)((object)documentElement));
		}

		readonly Action<T> repairAction;
	}
}
