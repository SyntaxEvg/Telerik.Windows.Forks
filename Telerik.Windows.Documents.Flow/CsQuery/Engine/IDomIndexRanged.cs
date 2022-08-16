using System;
using System.Collections.Generic;

namespace CsQuery.Engine
{
	interface IDomIndexRanged
	{
		IEnumerable<IDomObject> QueryIndex(ushort[] subKey, int depth, bool includeDescendants);
	}
}
