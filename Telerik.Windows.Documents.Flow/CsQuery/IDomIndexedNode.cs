using System;
using System.Collections.Generic;

namespace CsQuery
{
	interface IDomIndexedNode : IDomNode, ICloneable
	{
		IEnumerable<ushort[]> IndexKeys();

		IDomObject IndexReference { get; }
	}
}
