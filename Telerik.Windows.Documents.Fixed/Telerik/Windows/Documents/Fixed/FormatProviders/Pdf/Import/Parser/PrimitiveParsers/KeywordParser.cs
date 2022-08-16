using System;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PrimitiveParsers
{
	class KeywordParser
	{
		public KeywordParser(PostScriptReader parser, KeywordCollection keywords)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(parser, "parser");
			this.parser = parser;
			this.token = new StringBuilder();
			this.keywords = keywords;
		}

		public void Append(PostScriptReaderArgs args)
		{
			Guard.ThrowExceptionIfNull<PostScriptReaderArgs>(args, "args");
			this.token.Append((char)args.Byte);
		}

		public void Complete(PostScriptReaderArgs args)
		{
			Guard.ThrowExceptionIfNull<PostScriptReaderArgs>(args, "args");
			Keyword keyword;
			if (this.keywords.TryGetKeyword(this.token.ToString(), out keyword))
			{
				keyword.Complete(this.parser, args.Context);
			}
			this.token.Clear();
		}

		readonly PostScriptReader parser;

		readonly StringBuilder token;

		readonly KeywordCollection keywords;
	}
}
