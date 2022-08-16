using System;

namespace CsQuery.HtmlParser
{
	[Flags]
	enum TokenProperties : ushort
	{
		BlockElement = 1,
		BooleanProperty = 2,
		AutoOpenOrClose = 4,
		ChildrenNotAllowed = 8,
		HtmlChildrenNotAllowed = 16,
		ParagraphCloser = 32,
		MetaDataTags = 64,
		CaseInsensitiveValues = 128,
		HasValue = 256,
		FormInputControl = 512
	}
}
