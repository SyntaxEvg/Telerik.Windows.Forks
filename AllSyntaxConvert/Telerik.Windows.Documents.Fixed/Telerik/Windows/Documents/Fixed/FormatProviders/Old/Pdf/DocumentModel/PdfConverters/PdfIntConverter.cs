using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class PdfIntConverter : ConverterBase
	{
		public override object Convert(Type type, PdfContentManager contentManager, object value)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<object>(value, "value");
			PdfIntOld pdfIntOld = value as PdfIntOld;
			if (pdfIntOld != null)
			{
				return pdfIntOld;
			}
			int value2;
			Helper.UnboxInt(value, out value2);
			return new PdfIntOld(contentManager, value2);
		}
	}
}
