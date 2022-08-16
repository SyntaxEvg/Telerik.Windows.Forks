using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.Writers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps
{
	class CMapObject : PdfStreamObjectBase
	{
		public CMapObject()
			: this(new CMapEncoding())
		{
		}

		public CMapObject(CMapEncoding encoding)
		{
			this.encoding = encoding;
		}

		public CMapEncoding Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			byte[] result;
			using (Stream stream = new MemoryStream())
			{
				PdfWriter writer = new PdfWriter(stream);
				PdfElementWriters.CMapWriter.Write(writer, context, this.Encoding);
				result = stream.ReadAllBytes();
			}
			return result;
		}

		protected override void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfStreamBase>(stream, "stream");
			using (Stream stream2 = new MemoryStream(stream.ReadDecodedPdfData()))
			{
				CMapEncodingInterpreter cmapEncodingInterpreter = new CMapEncodingInterpreter(stream2, context, this.Encoding);
				cmapEncodingInterpreter.Execute();
			}
		}

		readonly CMapEncoding encoding;
	}
}
