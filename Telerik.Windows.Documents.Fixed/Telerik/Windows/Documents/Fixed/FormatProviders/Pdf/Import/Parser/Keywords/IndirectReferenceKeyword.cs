using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	class IndirectReferenceKeyword : Keyword
	{
		public override string Name
		{
			get
			{
				return "R";
			}
		}

		public override void Complete(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			PdfPrimitive pdfPrimitive = reader.PopToken();
			int value = ((PdfInt)pdfPrimitive).Value;
			PdfPrimitive pdfPrimitive2 = reader.PopToken();
			int value2 = ((PdfInt)pdfPrimitive2).Value;
			IndirectReference primitive = new IndirectReference(value2, value);
			reader.PushToken(primitive);
		}
	}
}
