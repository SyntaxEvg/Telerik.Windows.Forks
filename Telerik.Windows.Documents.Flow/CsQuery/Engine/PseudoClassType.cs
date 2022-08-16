using System;

namespace CsQuery.Engine
{
	enum PseudoClassType
	{
		All = 1,
		Even,
		Odd,
		First,
		Last,
		IndexEquals,
		IndexLessThan,
		IndexGreaterThan,
		Parent,
		Visible,
		Hidden,
		Header,
		Has,
		Not,
		FirstChild = 20,
		LastChild,
		NthChild,
		FirstOfType,
		LastOfType,
		NthOfType,
		NthLastChild,
		NthLastOfType,
		OnlyChild,
		OnlyOfType,
		Empty
	}
}
