using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class ClosePath : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "h";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			ClosePath.Execute(interpreter);
		}

		public static void Execute(ContentStreamInterpreter interpreter)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			if (interpreter.CurrentPathFigure != null)
			{
				interpreter.CurrentPathFigure.IsClosed = true;
				PdfReal x = new PdfReal(interpreter.CurrentPathFigure.StartPoint.X);
				PdfReal y = new PdfReal(interpreter.CurrentPathFigure.StartPoint.Y);
				interpreter.CurrentPathFigure = null;
				MoveTo.Execute(interpreter, x, y);
			}
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[0]);
		}
	}
}
