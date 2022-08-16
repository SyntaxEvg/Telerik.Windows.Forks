using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace CsQuery
{
	class JsObject : DynamicObject, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		public JsObject()
		{
			this.Initialize(null, null);
		}

		public JsObject(StringComparer comparer = null, object missingPropertyValue = null)
		{
			this.Initialize(comparer, missingPropertyValue);
		}

		protected void Initialize(StringComparer comparer, object missingPropertyValue)
		{
			this.AllowMissingProperties = true;
			this.MissingPropertyValue = missingPropertyValue;
			this.InnerProperties = new Dictionary<string, object>(comparer ?? StringComparer.OrdinalIgnoreCase);
		}

		public IEnumerable<T> Enumerate<T>()
		{
			return Objects.EnumerateProperties<T>(this);
		}

		protected bool AllowMissingProperties { get; set; }

		protected object MissingPropertyValue { get; set; }

		public bool IgnoreCase { get; set; }

		protected IDictionary<string, object> InnerProperties { get; set; }

		public object this[string name]
		{
			get
			{
				object result;
				this.TryGetMember(name, typeof(object), out result);
				return result;
			}
			set
			{
				this.TrySetMember(name, value);
			}
		}

		public T Get<T>(string name)
		{
			object obj;
			this.TryGetMember(name, typeof(T), out obj);
			return (T)((object)obj);
		}

		public IEnumerable<T> GetList<T>(string name)
		{
			IEnumerable list = this.Get(name) as IEnumerable;
			if (list != null)
			{
				foreach (object item in list)
				{
					yield return (T)((object)item);
				}
				yield break;
			}
			throw new ArgumentException("The property '" + name + "' is not an array.");
		}

		public object Get(string name)
		{
			object result;
			this.TryGetMember(name, typeof(object), out result);
			return result;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return this.TryGetMember(binder.Name, binder.ReturnType, out result);
		}

		protected bool TryGetMember(string name, Type type, out object result)
		{
			object obj = null;
			bool flag = !string.IsNullOrEmpty(name) && this.InnerProperties.TryGetValue(name, out obj);
			if (!flag)
			{
				if (!this.AllowMissingProperties)
				{
					throw new KeyNotFoundException("There is no property named \"" + name + "\".");
				}
				if (type == typeof(object))
				{
					result = this.MissingPropertyValue;
				}
				else
				{
					result = Objects.DefaultValue(type);
				}
				flag = true;
			}
			else if (type == typeof(object))
			{
				result = obj;
			}
			else
			{
				result = Objects.Convert(obj, type);
			}
			return flag;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			return this.TrySetMember(binder.Name, value);
		}

		protected bool TrySetMember(string name, object value)
		{
			try
			{
				if (string.IsNullOrEmpty(name))
				{
					return false;
				}
				if (value is IDictionary<string, object> && !(value is JsObject))
				{
					this.InnerProperties[name] = this.ToJsObject((IDictionary<string, object>)value);
				}
				else
				{
					this.InnerProperties[name] = value;
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		public bool HasProperty(string name)
		{
			return this.InnerProperties.ContainsKey(name);
		}

		public bool Delete(string name)
		{
			return this.InnerProperties.Remove(name);
		}

		protected JsObject ToJsObject(IDictionary<string, object> value)
		{
			JsObject jsObject = new JsObject();
			foreach (KeyValuePair<string, object> keyValuePair in value)
			{
				jsObject[keyValuePair.Key] = keyValuePair.Value;
			}
			return jsObject;
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this.InnerProperties.Keys;
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this.InnerProperties.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		void IDictionary<string, object>.Add(string key, object value)
		{
			this.TrySetMember(key, value);
		}

		bool IDictionary<string, object>.ContainsKey(string key)
		{
			return this.InnerProperties.ContainsKey(key);
		}

		ICollection<string> IDictionary<string, object>.Keys
		{
			get
			{
				return this.InnerProperties.Keys;
			}
		}

		bool IDictionary<string, object>.Remove(string key)
		{
			return this.InnerProperties.Remove(key);
		}

		bool IDictionary<string, object>.TryGetValue(string key, out object value)
		{
			if (this.HasProperty(key))
			{
				return this.TryGetMember(key, typeof(object), out value);
			}
			value = null;
			return false;
		}

		ICollection<object> IDictionary<string, object>.Values
		{
			get
			{
				return this.InnerProperties.Values;
			}
		}

		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			this.TrySetMember(item.Key, item.Value);
		}

		void ICollection<KeyValuePair<string, object>>.Clear()
		{
			this.InnerProperties.Clear();
		}

		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
		{
			return this.InnerProperties.Contains(item);
		}

		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			this.InnerProperties.CopyTo(array, arrayIndex);
		}

		int ICollection<KeyValuePair<string, object>>.Count
		{
			get
			{
				return this.InnerProperties.Count;
			}
		}

		bool ICollection<KeyValuePair<string, object>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			return this.InnerProperties.Remove(item);
		}

		IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
