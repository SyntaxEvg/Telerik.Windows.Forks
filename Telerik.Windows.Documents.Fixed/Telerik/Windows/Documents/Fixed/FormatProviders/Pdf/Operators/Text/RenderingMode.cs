using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class RenderingMode : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "Tr";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			RenderingMode value = (RenderingMode)interpreter.Operands.GetLast<PdfInt>(interpreter.Reader, context.Owner).Value;
			interpreter.TextState.RenderingMode = value;
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, RenderingMode renderingMode)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[] { ((int)renderingMode).ToPdfInt() });
		}
	}
}
