using System;

namespace CsQuery.Engine
{
	[Flags]
	enum SelectorType
	{
		All = 1,
		Tag = 2,
		ID = 4,
		Class = 8,
		AttributeValue = 32,
		PseudoClass = 128,
		Elements = 256,
		HTML = 512,
		None = 1024
	}
}
