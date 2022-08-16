using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfName : PdfSimpleType<string>
	{
		public PdfName()
		{
		}

		public PdfName(string initialValue)
			: base(initialValue)
		{
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.PdfName;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.WritePdfName(PdfNames.EscapeName(base.Value));
		}
	}
}
