using System;
using System.Collections;
using System.Collections.Generic;

namespace CsQuery.EquationParser
{
	interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IList<KeyValuePair<TKey, TValue>>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		int IndexOf(TKey key);
	}
}
