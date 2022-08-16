using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class ClippingGeometryWriter : GeometryWriterBase
	{
		protected override void WritePathGeometry(PdfWriter writer, IPdfContentExportContext context, PathGeometry pathGeometry)
		{
			base.WritePathGeometry(writer, context, pathGeometry);
			switch (pathGeometry.FillRule)
			{
			case FillRule.EvenOdd:
				ContentStreamOperators.ModifyClippingPathEvenOddOperator.Write(writer, context);
				break;
			case FillRule.Nonzero:
				ContentStreamOperators.ModifyClippingPathNonzeroOperator.Write(writer, context);
				break;
			default:
				throw new NotSupportedException(string.Format("Fill rule of type {0} is not supported for clipping operator.", pathGeometry.FillRule));
			}
			ContentStreamOperators.EndPathOperator.Write(writer, context);
		}

		protected override void WriteRectangleGeometry(PdfWriter writer, IPdfContentExportContext context, RectangleGeometry rectangleGeometry)
		{
			base.WriteRectangleGeometry(writer, context, rectangleGeometry);
			ContentStreamOperators.ModifyClippingPathNonzeroOperator.Write(writer, context);
			ContentStreamOperators.EndPathOperator.Write(writer, context);
		}
	}
}
