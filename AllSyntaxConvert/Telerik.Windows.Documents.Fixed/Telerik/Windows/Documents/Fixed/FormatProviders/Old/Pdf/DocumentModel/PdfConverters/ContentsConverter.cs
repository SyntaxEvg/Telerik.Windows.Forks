using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class ContentsConverter : ConverterBase
	{
		protected override object ConvertFromPdfDataStream(Type type, PdfContentManager contentManager, PdfDataStream stream)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDataStream>(stream, "stream");
			ContentStreamOld contentStreamOld = new ContentStreamOld(contentManager);
			contentStreamOld.AppendData(stream);
			return contentStreamOld;
		}

		protected override object ConvertFromPdfArray(Type type, PdfContentManager contentManager, PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			ContentStreamOld contentStreamOld = new ContentStreamOld(contentManager);
			contentStreamOld.Load(array);
			return contentStreamOld;
		}
	}
}
