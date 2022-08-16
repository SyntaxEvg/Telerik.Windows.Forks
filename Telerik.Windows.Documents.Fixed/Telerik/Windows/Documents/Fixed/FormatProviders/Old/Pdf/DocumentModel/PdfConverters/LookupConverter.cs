using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class LookupConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfDataStream(Type type, PdfContentManager contentManager, PdfDataStream stream)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDataStream>(stream, "stream");
			LookupOld lookupOld = new LookupOld(contentManager);
			lookupOld.Load(stream);
			return lookupOld;
		}

		protected override object ConvertFromPdfString(Type type, PdfContentManager contentManager, PdfStringOld str)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfStringOld>(str, "str");
			LookupOld lookupOld = new LookupOld(contentManager);
			lookupOld.Load(str);
			return lookupOld;
		}
	}
}
