using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	abstract class ConsecutivePartWriterBase<T> : PartBase<T> where T : ConsecutiveElementBase, new()
	{
		public ConsecutivePartWriterBase(PartContext context)
			: base(context)
		{
			this.BeginWrite();
		}

		internal override void CompleteWriteOverride()
		{
			this.EndWrite();
			base.CompleteWriteOverride();
		}

		void BeginWrite()
		{
			T rootElement = base.RootElement;
			rootElement.BeginWriteElement();
		}

		void EndWrite()
		{
			T rootElement = base.RootElement;
			rootElement.EnsureWritingEnded();
		}
	}
}
