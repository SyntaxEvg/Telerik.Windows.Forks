using System;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class AllAtOncePagesCacheManager : IPagesCacheManager
	{
		public IDisposable BeginUsingPage(RadFixedPage page)
		{
			return new DisposableObject(delegate()
			{
			});
		}

		public void Clear()
		{
		}

		public void SavePage(RadFixedPage page)
		{
		}
	}
}
