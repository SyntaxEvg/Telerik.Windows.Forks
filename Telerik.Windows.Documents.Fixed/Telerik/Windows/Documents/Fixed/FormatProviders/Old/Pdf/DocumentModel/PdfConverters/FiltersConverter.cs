using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class FiltersConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfName(Type type, PdfContentManager contentManager, PdfNameOld name)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfNameOld>(name, "name");
			return new PdfNameOld[] { name };
		}

		protected override object ConvertFromPdfArray(Type type, PdfContentManager contentManager, PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			PdfNameOld[] array2 = new PdfNameOld[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				array2[i] = array.GetElement<PdfNameOld>(i, Converters.PdfNameConverter);
			}
			return array2;
		}
	}
}
