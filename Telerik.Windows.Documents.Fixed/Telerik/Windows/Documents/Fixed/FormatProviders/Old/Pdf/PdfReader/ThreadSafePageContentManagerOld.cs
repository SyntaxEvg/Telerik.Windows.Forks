using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader
{
	class ThreadSafePageContentManagerOld : IPageContentManager
	{
		public ThreadSafePageContentManagerOld(PdfFormatProvider provider)
		{
			Guard.ThrowExceptionIfNull<PdfFormatProvider>(provider, "provider");
			this.provider = provider;
		}

		public void LoadPageContent(RadFixedPage page)
		{
			Guard.ThrowExceptionIfNull<RadFixedPageInternal>(page.InternalRadFixedPage, "page.InternalRadFixedPage");
			RadFixedPageInternal internalRadFixedPage = page.InternalRadFixedPage;
			if (internalRadFixedPage.Content == null)
			{
				lock (this.provider)
				{
					if (internalRadFixedPage.Content == null)
					{
						internalRadFixedPage.Content = this.provider.ParsePageContent(internalRadFixedPage);
						internalRadFixedPage.Arrange();
					}
				}
			}
		}

		public void UnloadPageContent(RadFixedPage page)
		{
			Guard.ThrowExceptionIfNull<RadFixedPageInternal>(page.InternalRadFixedPage, "page.InternalRadFixedPage");
			RadFixedPageInternal internalRadFixedPage = page.InternalRadFixedPage;
			if (internalRadFixedPage.Content != null)
			{
				lock (this.provider)
				{
					if (internalRadFixedPage.Content != null)
					{
						internalRadFixedPage.Content = null;
					}
				}
			}
		}

		public bool IsPageContentEmpty(RadFixedPage page)
		{
			return page.Content.Count == 0;
		}

		readonly PdfFormatProvider provider;
	}
}
