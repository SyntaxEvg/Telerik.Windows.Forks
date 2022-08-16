using System;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	interface IPagesCacheManager
	{
		void SavePage(RadFixedPage page);

		IDisposable BeginUsingPage(RadFixedPage page);

		void Clear();
	}
}
