using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
	class InstanceIdCache<K, V> where K : IInstanceIdOwner
	{
		public InstanceIdCache()
		{
			this.instanceIdToCachedValue = new Dictionary<int, V>();
		}

		public V this[K key]
		{
			get
			{
				return this.instanceIdToCachedValue[key.InstanceId];
			}
			set
			{
				this.instanceIdToCachedValue[key.InstanceId] = value;
			}
		}

		public bool TryGetValue(K key, out V value)
		{
			return this.instanceIdToCachedValue.TryGetValue(key.InstanceId, out value);
		}

		public void Add(K key, V value)
		{
			this.instanceIdToCachedValue.Add(key.InstanceId, value);
		}

		readonly Dictionary<int, V> instanceIdToCachedValue;
	}
}
