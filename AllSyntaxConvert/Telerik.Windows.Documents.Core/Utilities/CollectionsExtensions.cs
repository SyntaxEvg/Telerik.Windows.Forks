using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Utilities
{
	static class CollectionsExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T obj in enumeration)
			{
				action(obj);
			}
		}

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<TKey> source, Func<TKey, TValue> converter)
		{
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
			foreach (TKey tkey in source)
			{
				dictionary[tkey] = converter(tkey);
			}
			return dictionary;
		}

		public static T1 FindElement<T1, T2>(IList<T1> collection, T2 index, CompareDelegate<T1, T2> comparer) where T1 : class
		{
			if (collection.Count == 0)
			{
				return default(T1);
			}
			return CollectionsExtensions.FindElement<T1, T2>(collection, 0, collection.Count - 1, index, comparer);
		}

		static T1 FindElement<T1, T2>(IList<T1> collection, int lo, int hi, T2 element, CompareDelegate<T1, T2> comparer) where T1 : class
		{
			if (hi < lo)
			{
				int index = Math.Max(0, Math.Min(hi, collection.Count));
				return collection[index];
			}
			int num = lo + (hi - lo) / 2;
			T1 t = collection[num];
			int num2 = comparer(t, element);
			if (num2 == 0)
			{
				return t;
			}
			if (num2 > 0)
			{
				return CollectionsExtensions.FindElement<T1, T2>(collection, num + 1, hi, element, comparer);
			}
			return CollectionsExtensions.FindElement<T1, T2>(collection, lo, num - 1, element, comparer);
		}
	}
}
