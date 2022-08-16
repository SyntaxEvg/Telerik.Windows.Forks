using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State
{
	class SetMiterLimit : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "M";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			PdfReal last = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			interpreter.GraphicState.MiterLimit = new double?(last.Value);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, double miterLimit)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[] { miterLimit.ToPdfReal() });
		}
	}
}
