using System;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PrimitiveParsers
{
	class NameParser
	{
		public NameParser(PostScriptReader parser)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(parser, "parser");
			this.parser = parser;
			this.token = new StringBuilder();
		}

		public void Append(PostScriptReaderArgs args)
		{
			this.token.Append((char)args.Byte);
		}

		public void Complete()
		{
			string initialValue = PdfNames.StripEscape(this.token);
			PdfName primitive = new PdfName(initialValue);
			this.parser.PushToken(primitive);
			this.token.Clear();
		}

		readonly PostScriptReader parser;

		readonly StringBuilder token;
	}
}
