using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class ArrayCache<T>
	{
		public ArrayCache()
		{
			this.cache = new Dictionary<HashArray, T>();
		}

		public void AddToCache(object[] data, T result)
		{
			HashArray key = new HashArray(data);
			this.cache[key] = result;
		}

		public bool TryGetValue(object[] data, out T result)
		{
			result = default(T);
			HashArray key = new HashArray(data);
			return this.cache.TryGetValue(key, out result);
		}

		public T GetValue(object[] data)
		{
			HashArray key = new HashArray(data);
			return this.cache[key];
		}

		public void Clear()
		{
			this.cache.Clear();
		}

		readonly Dictionary<HashArray, T> cache;
	}
}
