using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CsQuery.ExtensionMethods;
using CsQuery.HtmlParser;

namespace CsQuery.Implementation
{
	class RangeSortedDictionary<TKey, TValue> : IRangeSortedDictionary<TKey, TValue>, IDictionary<TKey[], TValue>, ICollection<KeyValuePair<TKey[], TValue>>, IEnumerable<KeyValuePair<TKey[], TValue>>, IEnumerable where TKey : IConvertible, IComparable
	{
		public RangeSortedDictionary(IComparer<TKey[]> setComparer, IEqualityComparer<TKey[]> equalityComparer, TKey indexSeparator)
		{
			this.Keys = new SortedSet<TKey[]>(setComparer);
			this.Index = new Dictionary<TKey[], TValue>(equalityComparer);
			this.IndexSeparator = indexSeparator;
		}

		public IEnumerable<string> KeysAudit
		{
			get
			{
				foreach (TKey[] item in this.Keys)
				{
					yield return RangeSortedDictionary<TKey, TValue>.HumanReadableKey(item, this.IndexSeparator);
				}
				yield break;
			}
		}

		public static string HumanReadableKey(object indexKeyArray, object indexSeparator)
		{
			string text = "";
			int num = 1;
			ushort[] array = (ushort[])indexKeyArray;
			if (!array[0].Equals(indexSeparator))
			{
				ushort tokenId = (ushort)Convert.ChangeType(array[1], typeof(ushort));
				text = Convert.ChangeType(array[0], typeof(char)) + HtmlData.TokenName(tokenId) + '/';
				num = 3;
			}
			for (int i = num; i < array.Length; i++)
			{
				text += ((ushort)Convert.ChangeType(array[i], typeof(ushort))).ToString().PadLeft(3, '0');
				if (i < array.Length - 1)
				{
					text += '/';
				}
			}
			return text;
		}

		public static string HumanReadableKey(object indexKey)
		{
			return RangeSortedDictionary<TKey, TValue>.HumanReadableKey(indexKey, 0);
		}

		public IEnumerable<TKey[]> GetRangeKeys(TKey[] subkey)
		{
			if (subkey != null && subkey.Length != 0)
			{
				TKey[] lastKey = subkey.Concat(this.IndexSeparator).ToArray<TKey>();
				foreach (TKey[] key in this.Keys.GetViewBetween(subkey, lastKey))
				{
					if (key != lastKey)
					{
						yield return key;
					}
				}
			}
			yield break;
		}

		public IEnumerable<TValue> GetRange(TKey[] subKey, int depth, bool descendants)
		{
			if (depth == 0 && !descendants)
			{
				if (this.Index.ContainsKey(subKey))
				{
					yield return this.Index[subKey];
				}
			}
			else
			{
				int len = subKey.Length;
				int curDepth = 0;
				foreach (TKey[] key in this.GetRangeKeys(subKey))
				{
					if (key.Length > len)
					{
						curDepth = key.Length - len;
					}
					if (curDepth == depth || (descendants && curDepth >= depth))
					{
						yield return this.Index[key];
					}
				}
			}
			yield break;
		}

		public IEnumerable<TValue> GetRange(TKey[] subKey)
		{
			foreach (TKey[] key in this.GetRangeKeys(subKey))
			{
				yield return this.Index[key];
			}
			yield break;
		}

		public void Add(TKey[] key, TValue value)
		{
			this.Index.Add(key, value);
			this.Keys.Add(key);
		}

		public bool ContainsKey(TKey[] key)
		{
			return this.Keys.Contains(key);
		}

		ICollection<TKey[]> IDictionary<!0[], !1>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		public bool Remove(TKey[] key)
		{
			if (this.Keys.Remove(key))
			{
				this.Index.Remove(key);
				return true;
			}
			return false;
		}

		public bool TryGetValue(TKey[] key, out TValue value)
		{
			return this.Index.TryGetValue(key, out value);
		}

		public ICollection<TValue> Values
		{
			get
			{
				return this.Values;
			}
		}

		public TValue this[TKey[] key]
		{
			get
			{
				return this.Index[key];
			}
			set
			{
				if (this.ContainsKey(key))
				{
					this.Index[key] = value;
					return;
				}
				this.Add(key, value);
			}
		}

		public void Add(KeyValuePair<TKey[], TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			this.Keys.Clear();
			this.Index.Clear();
		}

		public bool Contains(KeyValuePair<TKey[], TValue> item)
		{
			return this.Index.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey[], TValue>[] array, int arrayIndex)
		{
			foreach (KeyValuePair<TKey[], TValue> keyValuePair in this)
			{
				array[arrayIndex++] = keyValuePair;
			}
		}

		public int Count
		{
			get
			{
				return this.Index.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool Remove(KeyValuePair<TKey[], TValue> item)
		{
			return this.Remove(item.Key);
		}

		public IEnumerator<KeyValuePair<TKey[], TValue>> GetEnumerator()
		{
			foreach (TKey[] key in this.Keys)
			{
				yield return new KeyValuePair<TKey[], TValue>(key, this.Index[key]);
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		TKey IndexSeparator;

		protected SortedSet<TKey[]> Keys;

		protected IDictionary<TKey[], TValue> Index;
	}
}
