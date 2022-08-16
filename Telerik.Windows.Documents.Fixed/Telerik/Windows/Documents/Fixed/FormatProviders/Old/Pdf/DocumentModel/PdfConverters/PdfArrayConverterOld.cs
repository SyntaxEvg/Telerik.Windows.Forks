using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class PdfArrayConverterOld : IndirectReferenceConverterBase
	{
		public override object Convert(Type type, PdfContentManager contentManager, object value)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<object>(value, "value");
			object obj = base.Convert(type, contentManager, value);
			PdfArrayOld pdfArrayOld = obj as PdfArrayOld;
			if (pdfArrayOld == null)
			{
				return new PdfArrayOld(contentManager, Enumerable.Repeat<object>(obj, 1));
			}
			return pdfArrayOld;
		}
	}
}
