using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class PathWriter : MarkableContentElementWriter<Path>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, Path element)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Path>(element, "element");
			using (ContentStreamOperators.PushGraphicsState(writer, context))
			{
				ContentStreamOperators.ConcatMatrixOperator.Write(writer, context, element.Position);
				ContentElementWriters.GeometryPropertiesWriter.Write(writer, context, element.GeometryProperties);
				ContentElementWriterBase.WriteElement(writer, context, element.Geometry);
				PathGeometry pathGeometry = element.Geometry as PathGeometry;
				if (element.IsFilled && element.IsStroked)
				{
					if (pathGeometry == null || pathGeometry.FillRule == FillRule.Nonzero)
					{
						ContentStreamOperators.FillAndStrokePathOperator.Write(writer, context);
					}
					else
					{
						ContentStreamOperators.FillAndStrokePathEvenOddOperator.Write(writer, context);
					}
				}
				else if (element.IsFilled)
				{
					if (pathGeometry == null || pathGeometry.FillRule == FillRule.Nonzero)
					{
						ContentStreamOperators.FillPathOperator.Write(writer, context);
					}
					else
					{
						ContentStreamOperators.FillPathEvenOddOperator.Write(writer, context);
					}
				}
				else if (element.IsStroked)
				{
					ContentStreamOperators.StrokePathOperator.Write(writer, context);
				}
				else
				{
					ContentStreamOperators.EndPathOperator.Write(writer, context);
				}
			}
		}
	}
}
