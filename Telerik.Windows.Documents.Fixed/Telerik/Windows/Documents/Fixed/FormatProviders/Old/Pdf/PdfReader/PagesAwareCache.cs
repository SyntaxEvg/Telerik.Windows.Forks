using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader
{
	class PagesAwareCache<Key, Value>
	{
		public PagesAwareCache()
		{
			this.cache = new Dictionary<Key, PagesAwareCache<Key, Value>.ValueUsedByPages>();
			this.pageToCachedValuesMapping = new Dictionary<RadFixedPage, HashSet<Key>>();
		}

		public bool TryGetFromCache(RadFixedPage forPage, Key key, out Value value)
		{
			PagesAwareCache<Key, Value>.ValueUsedByPages valueUsedByPages;
			if (this.cache.TryGetValue(key, out valueUsedByPages))
			{
				value = valueUsedByPages.Value;
				if (valueUsedByPages.AddPageForThisValue(forPage))
				{
					this.GetPageKeys(forPage).Add(key);
				}
				return true;
			}
			value = default(Value);
			return false;
		}

		public void AddToCache(RadFixedPage forPage, Key key, Value value)
		{
			this.cache.Add(key, new PagesAwareCache<Key, Value>.ValueUsedByPages(value, forPage));
			this.GetPageKeys(forPage).Add(key);
		}

		public void ClearCache(RadFixedPage forPage)
		{
			HashSet<Key> pageKeys = this.GetPageKeys(forPage);
			foreach (Key key in pageKeys)
			{
				PagesAwareCache<Key, Value>.ValueUsedByPages valueUsedByPages = this.cache[key];
				valueUsedByPages.RemovePageForThisValue(forPage);
				if (valueUsedByPages.PagesUsingThisValueCount == 0)
				{
					this.cache.Remove(key);
				}
			}
			pageKeys.Clear();
		}

		public void ClearCache()
		{
			this.cache.Clear();
			this.pageToCachedValuesMapping.Clear();
		}

		HashSet<Key> GetPageKeys(RadFixedPage page)
		{
			HashSet<Key> hashSet;
			if (!this.pageToCachedValuesMapping.TryGetValue(page, out hashSet))
			{
				hashSet = new HashSet<Key>();
				this.pageToCachedValuesMapping.Add(page, hashSet);
			}
			return hashSet;
		}

		readonly Dictionary<Key, PagesAwareCache<Key, Value>.ValueUsedByPages> cache;

		readonly Dictionary<RadFixedPage, HashSet<Key>> pageToCachedValuesMapping;

		class ValueUsedByPages
		{
			public ValueUsedByPages(Value value, RadFixedPage firstPage)
			{
				this.value = value;
				this.pagesUsingThisValue = new HashSet<RadFixedPage>();
				this.pagesUsingThisValue.Add(firstPage);
			}

			public Value Value
			{
				get
				{
					return this.value;
				}
			}

			public int PagesUsingThisValueCount
			{
				get
				{
					return this.pagesUsingThisValue.Count;
				}
			}

			public bool AddPageForThisValue(RadFixedPage page)
			{
				return this.pagesUsingThisValue.Add(page);
			}

			public void RemovePageForThisValue(RadFixedPage page)
			{
				this.pagesUsingThisValue.Remove(page);
			}

			readonly Value value;

			readonly HashSet<RadFixedPage> pagesUsingThisValue;
		}
	}
}
