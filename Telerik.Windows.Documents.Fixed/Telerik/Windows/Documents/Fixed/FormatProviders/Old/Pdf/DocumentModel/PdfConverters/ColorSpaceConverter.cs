using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class ColorSpaceConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfName(Type type, PdfContentManager contentManager, PdfNameOld name)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfNameOld>(name, "name");
			return ColorSpaceOld.CreateColorSpace(contentManager, name.Value);
		}

		protected override object ConvertFromPdfArray(Type type, PdfContentManager contentManager, PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			return ColorSpaceOld.CreateColorSpace(contentManager, array);
		}
	}
}
