using System;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class ContainerWriter : ContentElementWriter<IContentRootElement>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, IContentRootElement container)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IContentRootElement>(container, "container");
			foreach (ContentElementBase element in container.Content)
			{
				ContentElementWriterBase.WriteElement(writer, context, element);
			}
		}
	}
}
