using System;
using Telerik.Documents.SpreadsheetStreaming.Core;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx
{
	class ExporterChildManager : ChildEntityManagerBase<EntityBase>
	{
		public override TResult GetRegisteredChild<TResult>()
		{
			return base.GetRegisteredChild<TResult>(false);
		}

		protected override void EnsureEndUsingChild(EntityBase previousInstance)
		{
		}
	}
}
