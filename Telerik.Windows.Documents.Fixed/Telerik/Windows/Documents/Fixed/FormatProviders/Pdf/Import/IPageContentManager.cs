using System;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	interface IPageContentManager
	{
		void LoadPageContent(RadFixedPage page);

		void UnloadPageContent(RadFixedPage page);

		bool IsPageContentEmpty(RadFixedPage page);
	}
}
