using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;
using Telerik.Windows.Documents.Fixed.PostScript.Parser.Operators;

namespace Telerik.Windows.Documents.Fixed.PostScript.Parser.Keywords
{
	class PostScriptKeywordCollection : KeywordCollection
	{
		public PostScriptKeywordCollection()
		{
			base.RegisterKeyword(new OperatorKeyword(PostScriptOperators.BeginBFChar));
			base.RegisterKeyword(new OperatorKeyword(PostScriptOperators.EndBFChar));
		}
	}
}
