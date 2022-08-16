using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class SegmentWriter : ContentElementWriter<PathSegment>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, PathSegment element)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PathSegment>(element, "element");
			BezierSegment bezierSegment = element as BezierSegment;
			if (bezierSegment != null)
			{
				this.WriteBezierSegment(writer, context, bezierSegment);
				return;
			}
			LineSegment lineSegment = element as LineSegment;
			if (lineSegment != null)
			{
				this.WriteLineSegment(writer, context, lineSegment);
			}
		}

		void WriteLineSegment(PdfWriter writer, IPdfContentExportContext context, LineSegment lineSegment)
		{
			ContentStreamOperators.LineToOperator.Write(writer, context, lineSegment);
		}

		void WriteBezierSegment(PdfWriter writer, IPdfContentExportContext context, BezierSegment bezierSegment)
		{
			ContentStreamOperators.CurveToOperator.Write(writer, context, bezierSegment);
		}
	}
}
