using System;
using System.IO;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class PdfSingleLineWriter : PdfWriter
	{
		public PdfSingleLineWriter(Stream output)
			: base(output)
		{
		}

		public override void WriteLine()
		{
			base.WriteSeparator();
		}
	}
}
