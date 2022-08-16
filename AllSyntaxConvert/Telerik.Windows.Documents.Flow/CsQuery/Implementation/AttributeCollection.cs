using System;
using System.Collections;
using System.Collections.Generic;
using CsQuery.ExtensionMethods.Internal;
using CsQuery.HtmlParser;

namespace CsQuery.Implementation
{
	class AttributeCollection : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable
	{
		internal string this[ushort nodeId]
		{
			get
			{
				return this.Get(nodeId);
			}
			set
			{
				this.Set(nodeId, value);
			}
		}

		public bool HasAttributes
		{
			get
			{
				return this.Attributes.Count > 0;
			}
		}

		public int Count
		{
			get
			{
				return this.Attributes.Count;
			}
		}

		public void Clear()
		{
			this.Attributes.Clear();
		}

		public AttributeCollection Clone()
		{
			AttributeCollection attributeCollection = new AttributeCollection();
			if (this.HasAttributes)
			{
				foreach (KeyValuePair<ushort, string> keyValuePair in this.Attributes)
				{
					attributeCollection.Attributes.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return attributeCollection;
		}

		public void Add(string name, string value)
		{
			this.Set(name, value);
		}

		public bool Remove(string name)
		{
			return this.Unset(name);
		}

		public bool Remove(ushort tokenId)
		{
			return this.Unset(tokenId);
		}

		public string this[string name]
		{
			get
			{
				return this.Get(name);
			}
			set
			{
				this.Set(name, value);
			}
		}

		public bool ContainsKey(string key)
		{
			return this.Attributes.ContainsKey(HtmlData.Tokenize(key));
		}

		public bool ContainsKey(ushort tokenId)
		{
			return this.Attributes.ContainsKey(tokenId);
		}

		public ICollection<string> Keys
		{
			get
			{
				List<string> list = new List<string>();
				foreach (ushort tokenId in this.Attributes.Keys)
				{
					list.Add(HtmlData.TokenName(tokenId).ToLower());
				}
				return list;
			}
		}

		public ICollection<string> Values
		{
			get
			{
				return this.Attributes.Values;
			}
		}

		public bool TryGetValue(string name, out string value)
		{
			value = this.Get(name);
			return value != null || this.Attributes.ContainsKey(HtmlData.Tokenize(name));
		}

		public bool TryGetValue(ushort tokenId, out string value)
		{
			value = this.Get(tokenId);
			return value != null || this.Attributes.ContainsKey(tokenId);
		}

		public void SetBoolean(string name)
		{
			ushort boolean = HtmlData.Tokenize(name);
			this.SetBoolean(boolean);
		}

		public void SetBoolean(ushort tokenId)
		{
			this.Attributes[tokenId] = null;
		}

		public bool Unset(string name)
		{
			return this.Unset(HtmlData.Tokenize(name));
		}

		public bool Unset(ushort tokenId)
		{
			return this.Attributes.Remove(tokenId);
		}

		string Get(string name)
		{
			name = name.CleanUp();
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			return this.Get(HtmlData.Tokenize(name));
		}

		string Get(ushort tokenId)
		{
			string result;
			if (this.Attributes.TryGetValue(tokenId, out result))
			{
				return result;
			}
			return null;
		}

		void Set(string name, string value)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Cannot set an attribute with no name.");
			}
			name = name.CleanUp();
			this.Set(HtmlData.Tokenize(name), value);
		}

		void Set(ushort tokenId, string value)
		{
			this.SetRaw(tokenId, value);
		}

		internal void SetRaw(ushort tokenId, string value)
		{
			if (value == null)
			{
				this.Unset(tokenId);
				return;
			}
			this.Attributes[tokenId] = value;
		}

		protected IEnumerable<KeyValuePair<string, string>> GetAttributes()
		{
			foreach (KeyValuePair<ushort, string> kvp in this.Attributes)
			{
				KeyValuePair<ushort, string> keyValuePair = kvp;
				string key = HtmlData.TokenName(keyValuePair.Key).ToLower();
				KeyValuePair<ushort, string> keyValuePair2 = kvp;
				yield return new KeyValuePair<string, string>(key, keyValuePair2.Value);
			}
			yield break;
		}

		internal IEnumerable<ushort> GetAttributeIds()
		{
			return this.Attributes.Keys;
		}

		bool ICollection<KeyValuePair<string, string>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
		{
			this.Add(item.Key, item.Value);
		}

		bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
		{
			return this.ContainsKey(item.Key) && this.Attributes[HtmlData.Tokenize(item.Key)] == item.Value;
		}

		void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			array = new KeyValuePair<string, string>[this.Attributes.Count];
			int num = 0;
			foreach (KeyValuePair<ushort, string> keyValuePair in this.Attributes)
			{
				array[num++] = new KeyValuePair<string, string>(HtmlData.TokenName(keyValuePair.Key), keyValuePair.Value);
			}
		}

		bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
		{
			return this.ContainsKey(item.Key) && this.Attributes[HtmlData.Tokenize(item.Key)] == item.Value && this.Remove(item.Key);
		}

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return this.GetAttributes().GetEnumerator();
		}

		IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IDictionary<ushort, string> Attributes = new Dictionary<ushort, string>();
	}
}
