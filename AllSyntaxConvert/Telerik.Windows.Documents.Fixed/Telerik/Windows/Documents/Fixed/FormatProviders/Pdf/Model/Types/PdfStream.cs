using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfStream : PdfStreamBase
	{
		public PdfStream(IPdfImportContext context, Stream stream, PdfDictionary dictionary, PdfInt length, long offset)
			: base(context, stream, dictionary, length, offset)
		{
		}

		protected override byte[] ReadRawPdfDataOverride()
		{
			byte[] data;
			if (base.Length != null)
			{
				base.Reader.Reader.Seek(base.Offset + (long)base.Length.Value, SeekOrigin.Begin);
				if (StreamStartKeyword.TryFindPdfStreamEndOnNextRow(base.Reader.Reader))
				{
					base.Reader.Reader.Seek(base.Offset, SeekOrigin.Begin);
					data = base.Reader.Reader.Read(base.Length.Value);
				}
				else
				{
					data = base.CalculateStreamLengthAndReadData();
				}
			}
			else
			{
				data = base.CalculateStreamLengthAndReadData();
			}
			return base.Context.DecryptStream(base.CurrentIndirectReference, data);
		}

		protected override int CalculateLength(Reader reader)
		{
			return StreamStartKeyword.CalculateStreamLength(base.Reader.Reader);
		}
	}
}
