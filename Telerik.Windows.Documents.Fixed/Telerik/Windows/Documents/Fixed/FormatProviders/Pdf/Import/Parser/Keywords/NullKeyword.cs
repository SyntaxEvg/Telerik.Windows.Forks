using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	class NullKeyword : Keyword
	{
		public override string Name
		{
			get
			{
				return "null";
			}
		}

		public override void Complete(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			reader.PushToken(null);
		}
	}
}
