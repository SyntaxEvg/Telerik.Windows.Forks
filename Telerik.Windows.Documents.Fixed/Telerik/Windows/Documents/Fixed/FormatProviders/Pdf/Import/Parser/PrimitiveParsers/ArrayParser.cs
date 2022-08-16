using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PrimitiveParsers
{
	class ArrayParser
	{
		public ArrayParser(PostScriptReader parser)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(parser, "parser");
			this.parser = parser;
		}

		public void Start()
		{
			this.parser.PushCollection();
		}

		public void End()
		{
			this.parser.PushToken(new PdfArray(this.parser.PopCollection().Reverse<PdfPrimitive>()));
		}

		readonly PostScriptReader parser;
	}
}
