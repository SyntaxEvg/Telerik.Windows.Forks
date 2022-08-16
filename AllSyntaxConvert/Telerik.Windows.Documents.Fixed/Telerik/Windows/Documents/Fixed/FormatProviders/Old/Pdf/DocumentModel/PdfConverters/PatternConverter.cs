using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class PatternConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfDictionary(Type type, PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			return PatternOld.CreatePattern(contentManager, dictionary);
		}

		protected override object ConvertFromPdfDataStream(Type type, PdfContentManager contentManager, PdfDataStream stream)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDataStream>(stream, "stream");
			return PatternOld.CreatePattern(contentManager, stream);
		}
	}
}
