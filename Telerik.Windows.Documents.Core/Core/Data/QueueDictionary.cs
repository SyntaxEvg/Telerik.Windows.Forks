using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.Data
{
	class QueueDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		public QueueDictionary()
		{
			this.dictionary = new Dictionary<TKey, TValue>();
			this.list = new List<TKey>();
		}

		public ICollection<TKey> Keys
		{
			get
			{
				return this.list.AsReadOnly();
			}
		}

		public ICollection<TValue> Values
		{
			get
			{
				return this.dictionary.Values;
			}
		}

		public int Count
		{
			get
			{
				return this.dictionary.Count;
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				return this.dictionary[key];
			}
			set
			{
				this.Remove(key);
				this.Add(key, value);
			}
		}

		public TValue GetAt(int index)
		{
			return this.dictionary[this.list[index]];
		}

		public void Add(TKey key, TValue value)
		{
			this.dictionary.Add(key, value);
			this.list.Add(key);
		}

		public bool ContainsKey(TKey key)
		{
			return this.dictionary.ContainsKey(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.dictionary.TryGetValue(key, out value);
		}

		public bool Remove(TKey key)
		{
			if (this.dictionary.Remove(key))
			{
				this.list.Remove(key);
				return true;
			}
			return false;
		}

		public void Clear()
		{
			this.dictionary.Clear();
			this.list.Clear();
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (TKey key in this.list)
			{
				yield return new KeyValuePair<TKey, TValue>(key, this.dictionary[key]);
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return this.Remove(item.Key);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).Contains(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).CopyTo(array, arrayIndex);
		}

		readonly Dictionary<TKey, TValue> dictionary;

		readonly List<TKey> list;
	}
}
