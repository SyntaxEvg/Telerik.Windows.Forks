using System;
using CsQuery.Engine;

namespace CsQuery
{
	class SimpleDomIndexProvider : IDomIndexProvider
	{
		public IDomIndex GetDomIndex()
		{
			return new DomIndexSimple();
		}
	}
}
