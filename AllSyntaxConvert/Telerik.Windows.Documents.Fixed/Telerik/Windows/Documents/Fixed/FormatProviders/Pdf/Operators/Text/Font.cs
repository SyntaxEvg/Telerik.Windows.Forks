using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class Font : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "Tf";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			interpreter.TextState.FontSize = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			interpreter.TextState.Font = context.GetFont(interpreter.Reader, interpreter.Operands.GetLast<PdfName>(interpreter.Reader, context.Owner));
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, string fontName, double fontSize)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				fontName.ToPdfName(),
				fontSize.ToPdfReal()
			});
		}
	}
}
