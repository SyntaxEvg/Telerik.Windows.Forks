using System;
using CsQuery.Engine;

namespace CsQuery
{
	class NoDomIndexProvider : IDomIndexProvider
	{
		public IDomIndex GetDomIndex()
		{
			return new DomIndexNone();
		}
	}
}
