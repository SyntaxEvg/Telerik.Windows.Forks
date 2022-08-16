using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	class OperatorKeyword : Keyword
	{
		public OperatorKeyword(Operator op)
		{
			this.op = op;
		}

		public override string Name
		{
			get
			{
				return this.op.Name;
			}
		}

		public override void Complete(PostScriptReader parser, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(parser, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			parser.PushToken(this.op);
		}

		readonly Operator op;
	}
}
