using System;
using CsQuery.Engine;

namespace CsQuery
{
	class RangedDomIndexProvider : IDomIndexProvider
	{
		public IDomIndex GetDomIndex()
		{
			return new DomIndexRanged();
		}
	}
}
