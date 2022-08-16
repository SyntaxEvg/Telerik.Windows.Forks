using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	class TrueKeyword : Keyword
	{
		public override string Name
		{
			get
			{
				return "true";
			}
		}

		public override void Complete(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			reader.PushToken(new PdfBool(true));
		}
	}
}
