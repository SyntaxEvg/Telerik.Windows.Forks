using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	abstract class ContentElementWriter<T> : ContentElementWriterBase
	{
		public sealed override void Write(PdfWriter writer, IPdfContentExportContext context, object element)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<object>(element, "element");
			this.WriteOverride(writer, context, (T)((object)element));
		}

		protected abstract void WriteOverride(PdfWriter writer, IPdfContentExportContext context, T element);
	}
}
