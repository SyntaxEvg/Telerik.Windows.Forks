using System;

namespace CsQuery
{
	[Flags]
	enum HtmlParsingOptions : byte
	{
		None = 0,
		Default = 1,
		AllowSelfClosingTags = 2,
		IgnoreComments = 4
	}
}
