using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Flow.TextSearch.Core
{
	class IntervalMap<TKey, V> : IEnumerable where TKey : IComparable<TKey>
	{
		public IntervalMap()
		{
			this.intervalToValueDictionary = new Dictionary<Interval<TKey>, V>();
		}

		internal Interval<TKey> LastIntervalCached { get; set; }

		public void Add(TKey intervalStart, TKey intervalEnd, V mappedValue)
		{
			Interval<TKey> key = new Interval<TKey>(intervalStart, intervalEnd);
			this.intervalToValueDictionary.Add(key, mappedValue);
		}

		public V GetValueFromIntervalPoint(TKey intervalPoint)
		{
			int i = 0;
			int num = this.intervalToValueDictionary.Keys.Count - 1;
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				Interval<TKey> key = this.intervalToValueDictionary.ElementAt(num2).Key;
				if (key.Contains(intervalPoint))
				{
					this.LastIntervalCached = key;
					return this.intervalToValueDictionary[key];
				}
				if (key.CompareTo(intervalPoint) == 1)
				{
					num = num2 - 1;
				}
				else
				{
					i = num2 + 1;
				}
			}
			return default(V);
		}

		public IEnumerator GetEnumerator()
		{
			return this.intervalToValueDictionary.GetEnumerator();
		}

		public void Clear()
		{
			this.intervalToValueDictionary.Clear();
		}

		readonly Dictionary<Interval<TKey>, V> intervalToValueDictionary;
	}
}
