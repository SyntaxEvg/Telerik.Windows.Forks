using System;

namespace CsQuery.Engine
{
	enum AttributeSelectorType
	{
		Exists = 1,
		Equals,
		StartsWith,
		Contains,
		NotExists,
		ContainsWord,
		EndsWith,
		NotEquals,
		StartsWithOrHyphen
	}
}
