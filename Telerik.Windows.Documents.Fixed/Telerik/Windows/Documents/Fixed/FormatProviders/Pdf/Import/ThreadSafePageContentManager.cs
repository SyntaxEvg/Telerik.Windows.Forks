using System;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class ThreadSafePageContentManager : IPageContentManager
	{
		public ThreadSafePageContentManager(IPageContentManager pageContentLoader)
		{
			this.pageContentManager = pageContentLoader;
		}

		public void LoadPageContent(RadFixedPage page)
		{
			if (this.pageContentManager.IsPageContentEmpty(page))
			{
				lock (this.pageContentManager)
				{
					if (this.pageContentManager.IsPageContentEmpty(page))
					{
						this.pageContentManager.LoadPageContent(page);
					}
				}
			}
		}

		public void UnloadPageContent(RadFixedPage page)
		{
			if (!this.pageContentManager.IsPageContentEmpty(page))
			{
				lock (this.pageContentManager)
				{
					if (!this.pageContentManager.IsPageContentEmpty(page))
					{
						this.pageContentManager.UnloadPageContent(page);
					}
				}
			}
		}

		public bool IsPageContentEmpty(RadFixedPage page)
		{
			return this.pageContentManager.IsPageContentEmpty(page);
		}

		readonly IPageContentManager pageContentManager;
	}
}
