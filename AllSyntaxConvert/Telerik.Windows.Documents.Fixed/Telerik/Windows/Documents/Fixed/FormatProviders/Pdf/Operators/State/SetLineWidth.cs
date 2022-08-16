using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State
{
	class SetLineWidth : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "w";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			PdfReal last = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			interpreter.GraphicState.StrokeLineWidth = last.Value;
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, double width)
		{
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[] { width.ToPdfReal() });
		}
	}
}
