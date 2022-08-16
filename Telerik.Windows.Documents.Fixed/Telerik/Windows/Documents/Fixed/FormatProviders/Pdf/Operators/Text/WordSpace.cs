using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class WordSpace : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "Tw";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			double value = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			WordSpace.Execute(interpreter.TextState, value);
		}

		public static void Execute(TextState textState, double wordSpacing)
		{
			textState.WordSpacing = new double?(wordSpacing);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, double wordSpace)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[] { wordSpace.ToPdfReal() });
		}
	}
}
