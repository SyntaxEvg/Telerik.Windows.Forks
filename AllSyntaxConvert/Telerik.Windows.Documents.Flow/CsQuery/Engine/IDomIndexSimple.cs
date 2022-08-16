using System;
using System.Collections.Generic;

namespace CsQuery.Engine
{
	interface IDomIndexSimple
	{
		IEnumerable<IDomObject> QueryIndex(ushort[] subKey);
	}
}
