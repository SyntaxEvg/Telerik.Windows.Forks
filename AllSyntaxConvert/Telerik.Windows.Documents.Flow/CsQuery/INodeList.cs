using System;
using System.Collections;
using System.Collections.Generic;
using CsQuery.Implementation;

namespace CsQuery
{
	interface INodeList<T> : CsQuery.Implementation.IReadOnlyList<T>, CsQuery.Implementation.IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable where T : IDomObject
	{
		int Length { get; }

		T Item(int index);

		IList<T> ToList();
	}
}
