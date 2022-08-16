using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class RtfGroupContext : IDisposable
	{
		public RtfGroupContext(RtfWriter writer)
		{
			this.writer = writer;
		}

		public void Dispose()
		{
			this.writer.WriteGroupEnd();
		}

		readonly RtfWriter writer;
	}
}
