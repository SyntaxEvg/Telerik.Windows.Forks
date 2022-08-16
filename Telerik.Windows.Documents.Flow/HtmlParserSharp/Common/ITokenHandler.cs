using System;
using HtmlParserSharp.Core;

namespace HtmlParserSharp.Common
{
	interface ITokenHandler
	{
		void StartTokenization(Tokenizer self);

		bool WantsComments { get; }

		void Doctype(string name, string publicIdentifier, string systemIdentifier, bool forceQuirks);

		void StartTag(ElementName eltName, HtmlAttributes attributes, bool selfClosing);

		void EndTag(ElementName eltName);

		void Comment(char[] buf, int start, int length);

		void Characters(char[] buf, int start, int length);

		void ZeroOriginatingReplacementCharacter();

		void Eof();

		void EndTokenization();

		bool IsCDataSectionAllowed { get; }

		bool AllowSelfClosingTags { get; }
	}
}
