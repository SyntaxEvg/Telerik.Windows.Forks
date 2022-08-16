using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.MarkedContent
{
	class BeginMarkedContent : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "BMC";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			PdfName last = interpreter.Operands.GetLast<PdfName>(interpreter.Reader, context.Owner);
			context.CurrentMarker = new Marker(last.Value);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, string name)
		{
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				new PdfName(name)
			});
		}
	}
}
