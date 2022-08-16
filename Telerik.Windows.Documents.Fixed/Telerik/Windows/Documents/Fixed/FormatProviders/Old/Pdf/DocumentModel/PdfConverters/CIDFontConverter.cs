using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class CIDFontConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfDictionary(Type type, PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			return CIDFontOld.CreateCIDFont(contentManager, dictionary);
		}
	}
}
