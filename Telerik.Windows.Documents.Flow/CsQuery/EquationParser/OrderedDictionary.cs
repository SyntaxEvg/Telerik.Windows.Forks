using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.EquationParser
{
	public class OrderedDictionary<T, TKey, TValue> : IDictionary<TKey, TValue>, IList<KeyValuePair<TKey, TValue>>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where T : IDictionary<TKey, TValue>, new()
	{
		protected IDictionary<TKey, TValue> InnerDictionary
		{
			get
			{
				if (this._InnerDictionary == null)
				{
					this._InnerDictionary = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
					this.InnerList = new List<KeyValuePair<TKey, TValue>>();
				}
				return this._InnerDictionary;
			}
		}

		public IList<TKey> Keys
		{
			get
			{
				return (from item in this.InnerList
					select item.Key).ToList<TKey>().AsReadOnly();
			}
		}

		public IList<TValue> Values
		{
			get
			{
				return this.GetValuesOrdered().ToList<TValue>().AsReadOnly();
			}
		}

		public int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		public TValue this[int index]
		{
			get
			{
				return this.InnerDictionary[this.InnerList[index].Key];
			}
			set
			{
				KeyValuePair<TKey, TValue> value2 = this.InnerList[index];
				this.InnerDictionary[value2.Key] = value;
				this.InnerList[index] = value2;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public void Add(TKey key, TValue value)
		{
			if (!this.InnerDictionary.ContainsKey(key))
			{
				this.InnerList.Add(new KeyValuePair<TKey, TValue>(key, value));
			}
			this.InnerDictionary[key] = value;
		}

		public bool ContainsKey(TKey key)
		{
			return this.InnerDictionary.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			if (key.Equals(default(TKey)))
			{
				return false;
			}
			TValue tvalue;
			if (this.InnerDictionary.TryGetValue(key, out tvalue))
			{
				this.InnerDictionary.Remove(key);
				KeyValuePair<TKey, TValue> item2 = this.InnerList.FirstOrDefault(delegate(KeyValuePair<TKey, TValue> item)
				{
					TKey key2 = item.Key;
					return key2.Equals(key);
				});
				this.InnerList.Remove(item2);
				return true;
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.InnerDictionary.TryGetValue(key, out value);
		}

		public TValue this[TKey key]
		{
			get
			{
				return this.InnerDictionary[key];
			}
			set
			{
				KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
				TValue tvalue;
				if (this.InnerDictionary.TryGetValue(key, out tvalue))
				{
					this.InnerList[this.IndexOf(key)] = keyValuePair;
				}
				else
				{
					this.InnerList.Add(keyValuePair);
				}
				this.InnerDictionary[key] = value;
			}
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.InnerDictionary.Add(item);
			this.InnerList.Add(item);
		}

		public void Clear()
		{
			this.InnerDictionary.Clear();
			this.InnerList.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.InnerDictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.InnerDictionary.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (this.InnerDictionary.Remove(item))
			{
				this.InnerList.Remove(item);
				return true;
			}
			return false;
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.InnerDictionary.GetEnumerator();
		}

		public int IndexOf(TKey key)
		{
			for (int i = 0; i < this.InnerList.Count; i++)
			{
				TKey key2 = this.InnerList[i].Key;
				if (key2.Equals(key))
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOf(KeyValuePair<TKey, TValue> item)
		{
			return this.InnerList.IndexOf(item);
		}

		protected TKey GetKey(TValue item)
		{
			KeyValuePair<TKey, TValue> keyValuePair = this.InnerDictionary.FirstOrDefault(delegate(KeyValuePair<TKey, TValue> val)
			{
				TValue value = val.Value;
				return value.Equals(item);
			});
			if (keyValuePair.Equals(default(KeyValuePair<TKey, TValue>)))
			{
				return default(TKey);
			}
			return keyValuePair.Key;
		}

		public void Insert(int index, TValue item)
		{
			if (typeof(TKey) != typeof(string) && typeof(TKey) != typeof(int))
			{
				throw new InvalidOperationException("I can only autogenerate keys for string & int key types.");
			}
			TKey key = (TKey)((object)Convert.ChangeType(this.AutoKeys++.ToString(), typeof(TKey)));
			this.Insert(index, new KeyValuePair<TKey, TValue>(key, item));
		}

		public void Insert(int index, KeyValuePair<TKey, TValue> item)
		{
			if (!this.InnerDictionary.ContainsKey(item.Key))
			{
				this.InnerList.Insert(index, item);
				this.InnerDictionary.Add(item.Key, item.Value);
			}
		}

		public void RemoveAt(int index)
		{
			KeyValuePair<TKey, TValue> item = this.InnerList[index];
			this.InnerDictionary.Remove(item);
			this.InnerList.RemoveAt(index);
		}

		public void Add(TValue value)
		{
			if (typeof(TKey) != typeof(string) && typeof(TKey) != typeof(int))
			{
				throw new InvalidOperationException("I can only autogenerate keys for string & int key types.");
			}
			int count = this.InnerList.Count;
			TKey key = (TKey)((object)Convert.ChangeType(count, typeof(TKey)));
			if (!this.InnerDictionary.ContainsKey(key))
			{
				this.InnerList.Insert(count, new KeyValuePair<TKey, TValue>(key, value));
				this.InnerDictionary.Add(key, value);
			}
		}

		public bool Contains(TValue item)
		{
			TKey key = this.GetKey(item);
			return !key.Equals(default(TKey));
		}

		public void CopyTo(TValue[] array, int arrayIndex)
		{
			this.Values.CopyTo(array, arrayIndex);
		}

		public bool Remove(TValue item)
		{
			return this.Remove(this.GetKey(item));
		}

		public override string ToString()
		{
			return this.InnerDictionary.ToString();
		}

		protected IEnumerable<TValue> GetValuesOrdered()
		{
			for (int i = 0; i < this.InnerList.Count; i++)
			{
				yield return this.InnerDictionary[this.InnerList[i].Key];
			}
			yield break;
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.Values;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.InnerDictionary.GetEnumerator();
		}

		KeyValuePair<TKey, TValue> IList<KeyValuePair<TKey, TValue>>.this[int index]
		{
			get
			{
				return this.InnerList[index];
			}
			set
			{
				KeyValuePair<TKey, TValue> value2 = this.InnerList[index];
				this.InnerDictionary[value2.Key] = value.Value;
				this.InnerList[index] = value2;
			}
		}

		IDictionary<TKey, TValue> _InnerDictionary;

		List<KeyValuePair<TKey, TValue>> InnerList;

		int AutoKeys;
	}
}
