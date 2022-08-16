using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	abstract class ConsecutivePartReaderBase<T> : PartBase<T> where T : ConsecutiveElementBase, new()
	{
		public ConsecutivePartReaderBase(PartContext context)
			: base(context)
		{
		}

		public void BeginRead()
		{
			T rootElement = base.RootElement;
			rootElement.BeginReadElement();
		}
	}
}
