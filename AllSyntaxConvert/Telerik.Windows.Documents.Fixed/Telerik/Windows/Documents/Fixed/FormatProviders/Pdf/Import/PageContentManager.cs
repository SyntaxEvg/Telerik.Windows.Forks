using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class PageContentManager : IPageContentManager
	{
		public PageContentManager(IRadFixedDocumentImportContext importContext)
		{
			this.importContext = importContext;
		}

		public void LoadPageContent(RadFixedPage fixedPage)
		{
			Page page = this.importContext.GetPage(fixedPage);
			PdfDictionary value;
			if (this.importContext.TryGetPdfDictionary(page, out value))
			{
				PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor<PdfDictionary>();
				page = pdfObjectDescriptor.Converter.Convert(typeof(Page), this.importContext.Reader, this.importContext, value) as Page;
				this.importContext.UnmapPdfDictionary(page);
				this.importContext.MapPages(page, fixedPage);
			}
			page.CopyPageContentTo(this.importContext, fixedPage);
			page.CopyPageAnnotationsTo(this.importContext, fixedPage);
		}

		public void UnloadPageContent(RadFixedPage page)
		{
			if (this.importContext.ImportSettings.ReadingMode == ReadingMode.OnDemand)
			{
				page.Content.Clear();
				page.Annotations.Clear();
			}
		}

		public bool IsPageContentEmpty(RadFixedPage page)
		{
			return page.Content.Count == 0 && page.Annotations.Count == 0;
		}

		readonly IRadFixedDocumentImportContext importContext;
	}
}
