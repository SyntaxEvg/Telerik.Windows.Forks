using System;
using System.Collections;
using System.Collections.Generic;

namespace CsQuery.Implementation
{
	interface IReadOnlyList<T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
	{
		T this[int index] { get; }
	}
}
