using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	abstract class ContentElementWriterBase
	{
		public static void WriteElement(PdfWriter writer, IPdfContentExportContext context, object element)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<object>(element, "element");
			ContentElementWriterBase contentElementWriterBase;
			if (ContentElementWriters.TryGetWriter(element, out contentElementWriterBase))
			{
				contentElementWriterBase.Write(writer, context, element);
			}
		}

		public abstract void Write(PdfWriter writer, IPdfContentExportContext context, object element);
	}
}
