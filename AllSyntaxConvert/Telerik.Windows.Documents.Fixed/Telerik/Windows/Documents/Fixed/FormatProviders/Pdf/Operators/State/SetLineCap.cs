using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State
{
	class SetLineCap : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "J";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			PdfReal last = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			interpreter.GraphicState.StrokeLineCap = (LineCap)last.Value;
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, LineCap lineCap)
		{
			PdfInt pdfInt = new PdfInt((int)lineCap);
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[] { pdfInt });
		}
	}
}
