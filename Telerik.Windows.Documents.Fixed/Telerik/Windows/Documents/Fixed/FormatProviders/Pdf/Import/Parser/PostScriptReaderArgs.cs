using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser
{
	class PostScriptReaderArgs
	{
		public PostScriptReaderArgs(IPdfImportContext context, byte b)
		{
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			this.Context = context;
			this.Byte = b;
		}

		public byte Byte { get; set; }

		public IPdfImportContext Context { get; set; }
	}
}
