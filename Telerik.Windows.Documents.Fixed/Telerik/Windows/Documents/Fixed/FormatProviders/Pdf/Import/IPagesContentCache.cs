using System;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	interface IPagesContentCache
	{
		void CachePageContent(RadFixedPage page);

		bool TryLoadCachedPageContent(RadFixedPage page);

		void Clear();
	}
}
