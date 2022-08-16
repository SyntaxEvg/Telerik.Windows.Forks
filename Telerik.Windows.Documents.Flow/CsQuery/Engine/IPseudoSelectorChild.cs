using System;
using System.Collections.Generic;

namespace CsQuery.Engine
{
	interface IPseudoSelectorChild : IPseudoSelector
	{
		bool Matches(IDomObject element);

		IEnumerable<IDomObject> ChildMatches(IDomContainer element);
	}
}
