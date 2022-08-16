using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State
{
	class SetLineDashPattern : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "d";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			PdfReal last = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			PdfArray last2 = interpreter.Operands.GetLast<PdfArray>(interpreter.Reader, context.Owner);
			interpreter.GraphicState.StrokeDashOffset = last.Value;
			List<double> list = new List<double>();
			for (int i = 0; i < last2.Count; i++)
			{
				PdfReal pdfReal;
				last2.TryGetElement<PdfReal>(interpreter.Reader, context.Owner, i, out pdfReal);
				list.Add(pdfReal.Value);
			}
			interpreter.GraphicState.StrokeDashArray = list;
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, IEnumerable<double> dashArray, double dashOffset)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IEnumerable<double>>(dashArray, "dashArray");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				dashArray.ToPdfArray(),
				dashOffset.ToPdfReal()
			});
		}
	}
}
