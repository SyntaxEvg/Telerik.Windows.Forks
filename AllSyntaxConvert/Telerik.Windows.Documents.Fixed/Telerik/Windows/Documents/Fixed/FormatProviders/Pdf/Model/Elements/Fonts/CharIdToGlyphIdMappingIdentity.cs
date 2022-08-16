using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class CharIdToGlyphIdMappingIdentity : CharIdToGlyphIdMappingBase
	{
		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			writer.WritePdfName("Identity");
		}
	}
}
