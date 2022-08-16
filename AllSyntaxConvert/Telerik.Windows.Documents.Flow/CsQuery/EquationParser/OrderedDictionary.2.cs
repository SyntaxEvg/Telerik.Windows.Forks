using System;
using System.Collections;
using System.Collections.Generic;

namespace CsQuery.EquationParser
{
	class OrderedDictionary<TKey, TValue> : OrderedDictionary<Dictionary<TKey, TValue>, TKey, TValue>, IOrderedDictionary<TKey, TValue>, IDictionary<TKey, TValue>, IList<KeyValuePair<TKey, TValue>>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
	}
}
