using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class LoadOnDemandPagesCacheManager : IPagesCacheManager
	{
		public LoadOnDemandPagesCacheManager(IPagesContentCache pagesContentCache, IPageContentManager contentManager)
		{
			this.pagesContentCache = pagesContentCache;
			this.pageContentManager = contentManager;
			this.lockObject = new object();
			this.pageToLoadingRequestsCount = new Dictionary<RadFixedPage, int>();
		}

		public void SavePage(RadFixedPage page)
		{
			this.pagesContentCache.CachePageContent(page);
		}

		public IDisposable BeginUsingPage(RadFixedPage page)
		{
			lock (this.lockObject)
			{
				int num;
				if (!this.pageToLoadingRequestsCount.TryGetValue(page, out num))
				{
					num = 0;
				}
				bool flag2 = this.pageContentManager.IsPageContentEmpty(page) && num == 0;
				if (flag2)
				{
					this.LoadPage(page);
				}
				this.pageToLoadingRequestsCount[page] = num + 1;
			}
			return new DisposableObject(delegate()
			{
				this.EndUsingPage(page);
			});
		}

		public void Clear()
		{
			this.pagesContentCache.Clear();
		}

		void EndUsingPage(RadFixedPage page)
		{
			lock (this.lockObject)
			{
				Dictionary<RadFixedPage, int> dictionary;
				(dictionary = this.pageToLoadingRequestsCount)[page] = dictionary[page] - 1;
				if (this.pageToLoadingRequestsCount[page] == 0)
				{
					this.pageContentManager.UnloadPageContent(page);
				}
			}
		}

		void LoadPage(RadFixedPage page)
		{
			if (!this.pagesContentCache.TryLoadCachedPageContent(page))
			{
				this.pageContentManager.LoadPageContent(page);
			}
		}

		readonly IPagesContentCache pagesContentCache;

		readonly IPageContentManager pageContentManager;

		readonly object lockObject;

		readonly Dictionary<RadFixedPage, int> pageToLoadingRequestsCount;
	}
}
