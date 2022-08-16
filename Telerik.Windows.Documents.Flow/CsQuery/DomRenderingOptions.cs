using System;

namespace CsQuery
{
	[Flags]
	enum DomRenderingOptions
	{
		None = 0,
		Default = 32,
		[Obsolete]
		RemoveMismatchedCloseTags = 1,
		RemoveComments = 2,
		QuoteAllAttributes = 4
	}
}
