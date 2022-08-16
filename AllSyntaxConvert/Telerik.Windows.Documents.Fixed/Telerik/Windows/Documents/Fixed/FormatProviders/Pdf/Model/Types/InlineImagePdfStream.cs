using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class InlineImagePdfStream : PdfStreamBase
	{
		public InlineImagePdfStream(IPdfImportContext context, Stream stream, PdfDictionary dictionary, PdfInt length, long offset)
			: base(context, stream, dictionary, length, offset)
		{
		}

		protected override byte[] ReadRawPdfDataOverride()
		{
			byte[] data = base.CalculateStreamLengthAndReadData();
			return base.Context.DecryptStream(base.CurrentIndirectReference, data);
		}

		protected override int CalculateLength(Reader reader)
		{
			return ImageDataKeyword.CalculateStreamLength(reader);
		}
	}
}
