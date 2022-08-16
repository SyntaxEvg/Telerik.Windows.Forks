using System;
using Telerik.Documents.SpreadsheetStreaming.Core;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	class ElementChildManager : ChildEntityManagerBase<ElementBase>
	{
		public override TResult GetRegisteredChild<TResult>()
		{
			return base.GetRegisteredChild<TResult>(true);
		}

		protected override void EnsureEndUsingChild(ElementBase previousInstance)
		{
			ConsecutiveElementBase consecutiveElementBase = previousInstance as ConsecutiveElementBase;
			if (consecutiveElementBase != null)
			{
				consecutiveElementBase.EnsureWritingEnded();
			}
		}
	}
}
