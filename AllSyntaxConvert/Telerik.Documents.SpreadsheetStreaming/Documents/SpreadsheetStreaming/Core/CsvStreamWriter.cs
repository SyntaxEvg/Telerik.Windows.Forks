using System;
using System.IO;
using System.Text;

namespace Telerik.Documents.SpreadsheetStreaming.Core
{
	class CsvStreamWriter : StreamWriter
	{
		public CsvStreamWriter(Stream stream, Encoding encoding)
			: base(stream, encoding)
		{
		}

		public CsvStreamWriter(Stream stream)
			: this(stream, Encoding.UTF8, true)
		{
		}

		public CsvStreamWriter(Stream stream, Encoding encoding, bool shouldDisposeStream)
			: base(stream, encoding)
		{
			this.shouldDisposeStream = shouldDisposeStream;
		}

		protected override void Dispose(bool disposeManaged)
		{
			base.Dispose(this.shouldDisposeStream);
		}

		readonly bool shouldDisposeStream;
	}
}
