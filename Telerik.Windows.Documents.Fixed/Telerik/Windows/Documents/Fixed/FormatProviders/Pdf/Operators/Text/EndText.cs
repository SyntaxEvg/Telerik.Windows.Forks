using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class EndText : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "ET";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[0]);
		}
	}
}
