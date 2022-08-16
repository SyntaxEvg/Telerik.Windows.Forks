using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class MaskConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfArray(Type type, PdfContentManager contentManager, PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			return new ColorKeyMaskOld(contentManager, array);
		}

		protected override object ConvertFromPdfDataStream(Type type, PdfContentManager contentManager, PdfDataStream stream)
		{
			return new ImageMaskOld(contentManager, stream.ReadData(contentManager));
		}
	}
}
