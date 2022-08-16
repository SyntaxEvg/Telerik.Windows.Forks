using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class CurveTo : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "c";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			PdfReal last = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			PdfReal last2 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			PdfReal last3 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			PdfReal last4 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			PdfReal last5 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			PdfReal last6 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			CurveTo.Execute(interpreter, last6, last5, last4, last3, last2, last);
		}

		public static void Execute(ContentStreamInterpreter interpreter, PdfReal x1, PdfReal y1, PdfReal x2, PdfReal y2, PdfReal x3, PdfReal y3)
		{
			interpreter.EnsurePathFigure();
			BezierSegment bezierSegment = interpreter.CurrentPathFigure.Segments.AddBezierSegment();
			bezierSegment.Point1 = new Point(x1.Value, y1.Value);
			bezierSegment.Point2 = new Point(x2.Value, y2.Value);
			bezierSegment.Point3 = new Point(x3.Value, y3.Value);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, BezierSegment segment)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<BezierSegment>(segment, "segment");
			Point point = segment.Point1;
			Point point2 = segment.Point2;
			Point point3 = segment.Point3;
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				point.X.ToPdfReal(),
				point.Y.ToPdfReal(),
				point2.X.ToPdfReal(),
				point2.Y.ToPdfReal(),
				point3.X.ToPdfReal(),
				point3.Y.ToPdfReal()
			});
		}
	}
}
