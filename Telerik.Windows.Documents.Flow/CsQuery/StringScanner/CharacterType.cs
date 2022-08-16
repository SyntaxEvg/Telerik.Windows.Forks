using System;

namespace CsQuery.StringScanner
{
	[Flags]
	enum CharacterType
	{
		Whitespace = 1,
		Alpha = 2,
		Number = 4,
		NumberPart = 8,
		Lower = 16,
		Upper = 32,
		Operator = 64,
		Enclosing = 128,
		Quote = 256,
		Escape = 512,
		Separator = 1024,
		AlphaISO10646 = 2048,
		HtmlTagSelectorStart = 4096,
		HtmlTagSelectorExceptStart = 8192,
		HtmlTagOpenerEnd = 16384,
		HtmlTagAny = 32768,
		HtmlTagNameStart = 65536,
		HtmlTagNameExceptStart = 131072,
		HtmlAttributeName = 262144,
		SelectorTerminator = 524288,
		HtmlSpace = 1048576,
		HtmlMustBeEncoded = 2097152,
		HtmlAttributeValueTerminator = 4194304,
		Hexadecimal = 8388608
	}
}
