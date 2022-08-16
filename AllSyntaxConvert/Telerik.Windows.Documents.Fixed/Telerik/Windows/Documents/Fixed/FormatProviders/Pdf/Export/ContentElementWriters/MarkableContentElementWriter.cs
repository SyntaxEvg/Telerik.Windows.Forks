using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	abstract class MarkableContentElementWriter<T> : ContentElementWriterBase where T : PositionContentElement
	{
		public sealed override void Write(PdfWriter writer, IPdfContentExportContext context, object element)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<object>(element, "element");
			T markableContent = (T)((object)element);
			MarkedContentExportInfo currentContentMarkerExportInfo = context.GetCurrentContentMarkerExportInfo(markableContent.Marker);
			if (currentContentMarkerExportInfo.ShouldExportMarkedContentEnd)
			{
				ContentStreamOperators.EndMarkedContentOperator.Write(writer, context.Owner);
			}
			if (currentContentMarkerExportInfo.ShouldExportMarkedContentStart)
			{
				ContentStreamOperators.ConcatMatrixOperator.Write(writer, context, context.RootDipToPointTransformation.InverseMatrix());
				ContentStreamOperators.BeginMarkedContentOperator.Write(writer, context, markableContent.Marker.Name);
				ContentStreamOperators.ConcatMatrixOperator.Write(writer, context, context.RootDipToPointTransformation);
			}
			this.WriteOverride(writer, context, markableContent);
		}

		protected abstract void WriteOverride(PdfWriter writer, IPdfContentExportContext context, T markableContent);
	}
}
