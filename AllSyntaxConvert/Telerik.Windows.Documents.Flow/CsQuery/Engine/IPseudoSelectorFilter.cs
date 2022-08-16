using System;
using System.Collections.Generic;

namespace CsQuery.Engine
{
	interface IPseudoSelectorFilter : IPseudoSelector
	{
		IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection);
	}
}
