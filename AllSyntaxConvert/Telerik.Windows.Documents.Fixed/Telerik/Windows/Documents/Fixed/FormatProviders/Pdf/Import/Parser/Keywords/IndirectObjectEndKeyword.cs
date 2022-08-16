using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	class IndirectObjectEndKeyword : Keyword
	{
		public override string Name
		{
			get
			{
				return "endobj";
			}
		}

		public override void Complete(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			PdfPrimitive content = reader.PopToken();
			int value = ((PdfInt)reader.PopToken()).Value;
			int value2 = ((PdfInt)reader.PopToken()).Value;
			reader.PushToken(new IndirectObject(new IndirectReference(value2, value), content));
		}
	}
}
