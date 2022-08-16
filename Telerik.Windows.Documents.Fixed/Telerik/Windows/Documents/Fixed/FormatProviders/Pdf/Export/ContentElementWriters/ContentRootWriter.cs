using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class ContentRootWriter : ContentElementWriter<IContentRootElement>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, IContentRootElement element)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IContentRootElement>(element, "element");
			Guard.ThrowExceptionIfNotEqual<IContentRootElement>(context.ContentRoot, element, "context.ContentRoot");
			ContentStreamOperators.ConcatMatrixOperator.Write(writer, context, context.RootDipToPointTransformation);
			foreach (ContentElementBase contentElementBase in context.GetRootElements())
			{
				Clipping clipping = contentElementBase as Clipping;
				if (clipping != null)
				{
					ContentElementWriterBase.WriteElement(writer, context, clipping);
				}
				else
				{
					ContentElementWriterBase.WriteElement(writer, context, contentElementBase);
				}
			}
			MarkedContentExportInfo currentContentMarkerExportInfo = context.GetCurrentContentMarkerExportInfo(null);
			if (currentContentMarkerExportInfo.ShouldExportMarkedContentEnd)
			{
				ContentStreamOperators.EndMarkedContentOperator.Write(writer, context.Owner);
			}
		}
	}
}
