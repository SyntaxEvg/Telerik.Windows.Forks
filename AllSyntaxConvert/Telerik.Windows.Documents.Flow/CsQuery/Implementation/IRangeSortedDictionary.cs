using System;
using System.Collections;
using System.Collections.Generic;

namespace CsQuery.Implementation
{
	interface IRangeSortedDictionary<TKey, TValue> : IDictionary<TKey[], TValue>, ICollection<KeyValuePair<TKey[], TValue>>, IEnumerable<KeyValuePair<TKey[], TValue>>, IEnumerable
	{
		IEnumerable<TKey[]> GetRangeKeys(TKey[] subKey);

		IEnumerable<TValue> GetRange(TKey[] subKey);
	}
}
