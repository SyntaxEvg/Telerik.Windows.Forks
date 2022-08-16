using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class ShadingConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfDictionary(Type type, PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			return ShadingOld.CreateShading(contentManager, dictionary);
		}

		protected override object ConvertFromPdfDataStream(Type type, PdfContentManager contentManager, PdfDataStream stream)
		{
			return ShadingOld.CreateShading(contentManager, stream);
		}
	}
}
