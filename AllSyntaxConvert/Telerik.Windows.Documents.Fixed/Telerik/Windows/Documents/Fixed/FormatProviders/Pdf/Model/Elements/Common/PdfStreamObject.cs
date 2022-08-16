using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common
{
	class PdfStreamObject : PdfStreamObjectBase
	{
		public byte[] Data { get; set; }

		protected override void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			this.Data = stream.ReadDecodedPdfData();
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			return this.Data;
		}
	}
}
