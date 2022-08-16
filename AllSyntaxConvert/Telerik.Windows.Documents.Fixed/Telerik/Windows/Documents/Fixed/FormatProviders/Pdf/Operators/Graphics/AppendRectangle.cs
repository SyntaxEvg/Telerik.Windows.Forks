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
	class AppendRectangle : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "re";
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
			MoveTo.Execute(interpreter, last4, last3);
			LineTo.Execute(interpreter, new PdfReal(last4.Value + last2.Value), last3);
			LineTo.Execute(interpreter, new PdfReal(last4.Value + last2.Value), new PdfReal(last3.Value + last.Value));
			LineTo.Execute(interpreter, last4, new PdfReal(last3.Value + last.Value));
			ClosePath.Execute(interpreter);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, RectangleGeometry rectangleGeometry)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<RectangleGeometry>(rectangleGeometry, "rectangleGeometry");
			Point point = new Point(rectangleGeometry.Rect.Left, rectangleGeometry.Rect.Bottom);
			double width = rectangleGeometry.Rect.Width;
			double height = rectangleGeometry.Rect.Height;
			point.Y -= height;
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				point.X.ToPdfReal(),
				point.Y.ToPdfReal(),
				width.ToPdfReal(),
				height.ToPdfReal()
			});
		}
	}
}
