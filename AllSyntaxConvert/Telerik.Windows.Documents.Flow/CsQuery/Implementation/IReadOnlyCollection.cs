using System;
using System.Collections;
using System.Collections.Generic;

namespace CsQuery.Implementation
{
	interface IReadOnlyCollection<T> : IEnumerable<T>, IEnumerable
	{
		int Count { get; }
	}
}
