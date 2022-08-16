using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	abstract class Keyword
	{
		public abstract string Name { get; }

		public abstract void Complete(PostScriptReader parser, IPdfImportContext context);
	}
}
