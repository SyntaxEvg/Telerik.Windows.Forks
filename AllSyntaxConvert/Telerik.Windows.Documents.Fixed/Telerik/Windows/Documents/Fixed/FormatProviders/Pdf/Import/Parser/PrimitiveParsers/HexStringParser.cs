using System;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PrimitiveParsers
{
	class HexStringParser
	{
		public HexStringParser(PostScriptReader parser)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(parser, "parser");
			this.parser = parser;
			this.token = new StringBuilder();
		}

		public virtual void Append(PostScriptReaderArgs args)
		{
			if (Characters.IsHexChar(args.Byte))
			{
				this.token.Append((char)args.Byte);
			}
		}

		public void Complete(PostScriptReaderArgs args)
		{
			Guard.ThrowExceptionIfNull<PostScriptReaderArgs>(args, "args");
			if (this.token.Length % 2 == 1)
			{
				this.token.Append("0");
			}
			this.parser.PushToken(new PdfHexString(args.Context.DecryptString(BytesHelper.ToByteArray(this.token.ToString()))));
			this.token.Clear();
		}

		readonly PostScriptReader parser;

		readonly StringBuilder token;
	}
}
