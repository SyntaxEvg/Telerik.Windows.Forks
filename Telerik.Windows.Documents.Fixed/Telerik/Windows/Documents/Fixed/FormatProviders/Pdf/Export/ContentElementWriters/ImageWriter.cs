using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class ImageWriter : MarkableContentElementWriter<Image>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, Image element)
		{
			using (ContentStreamOperators.PushGraphicsState(writer, context))
			{
				if (element.StencilColor != null)
				{
					ContentElementWriters.ColorWriter.Write(writer, context, element.StencilColor);
				}
				Matrix matrix = new Matrix(element.Width, 0.0, 0.0, -element.Height, 0.0, element.Height);
				matrix = matrix.MultiplyBy(element.Position.Matrix);
				MatrixPosition position = new MatrixPosition(matrix);
				ResourceEntry resource = context.GetResource(element.ImageSource);
				ContentStreamOperators.ConcatMatrixOperator.Write(writer, context, position);
				ContentStreamOperators.PaintOperator.Write(writer, context, resource.ResourceKey);
			}
		}
	}
}
