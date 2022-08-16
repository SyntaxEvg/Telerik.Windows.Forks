using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	class KeywordCollection
	{
		public KeywordCollection()
		{
			this.keywords = new Dictionary<string, Keyword>();
			this.RegisterKeyword(new IndirectReferenceKeyword());
			this.RegisterKeyword(new IndirectObjectEndKeyword());
			this.RegisterKeyword(new TrueKeyword());
			this.RegisterKeyword(new FalseKeyword());
			this.RegisterKeyword(new NullKeyword());
			this.RegisterKeyword(new StreamStartKeyword());
		}

		public bool TryGetKeyword(string keywordName, out Keyword keyword)
		{
			return this.keywords.TryGetValue(keywordName, out keyword);
		}

		protected void RegisterKeyword(Keyword keyword)
		{
			this.keywords.Add(keyword.Name, keyword);
		}

		readonly Dictionary<string, Keyword> keywords;
	}
}
