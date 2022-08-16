using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class VCurveTo : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "v";
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
			interpreter.EnsurePathFigure();
			PathSegment pathSegment = interpreter.CurrentPathFigure.Segments.LastOrDefault<PathSegment>();
			PdfReal x;
			PdfReal y;
			if (pathSegment != null)
			{
				x = new PdfReal(pathSegment.LastPoint.X);
				y = new PdfReal(pathSegment.LastPoint.Y);
			}
			else
			{
				x = new PdfReal(interpreter.CurrentPathFigure.StartPoint.X);
				y = new PdfReal(interpreter.CurrentPathFigure.StartPoint.Y);
			}
			CurveTo.Execute(interpreter, x, y, last4, last3, last2, last);
		}
	}
}
