using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.PostScript.Data
{
	class PostScriptDictionary : PostScriptObject, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		public PostScriptDictionary()
		{
			this.store = new Dictionary<string, object>();
		}

		public PostScriptDictionary(int capacity)
		{
			this.store = new Dictionary<string, object>(capacity);
		}

		public ICollection<string> Keys
		{
			get
			{
				return this.store.Keys;
			}
		}

		public ICollection<object> Values
		{
			get
			{
				return this.store.Values;
			}
		}

		public object this[string key]
		{
			get
			{
				return this.store[key];
			}
			set
			{
				this.store[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public void Add(string key, object value)
		{
			this.store.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return this.store.ContainsKey(key);
		}

		public bool Remove(string key)
		{
			return this.store.Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return this.store.TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<string, object> item)
		{
			this.store[item.Key] = item.Value;
		}

		public void Clear()
		{
			this.store.Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return this.store.Remove(item.Key);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		public T GetElementAs<T>(string key)
		{
			return (T)((object)this.store[key]);
		}

		readonly Dictionary<string, object> store;
	}
}
