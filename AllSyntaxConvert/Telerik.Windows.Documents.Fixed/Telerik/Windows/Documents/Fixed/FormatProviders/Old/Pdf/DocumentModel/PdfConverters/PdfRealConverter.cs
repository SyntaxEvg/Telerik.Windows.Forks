using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class PdfRealConverter : ConverterBase
	{
		public override object Convert(Type type, PdfContentManager contentManager, object value)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<object>(value, "value");
			PdfRealOld pdfRealOld = value as PdfRealOld;
			if (pdfRealOld != null)
			{
				return pdfRealOld;
			}
			double val;
			Helper.UnboxDouble(value, out val);
			return new PdfRealOld(contentManager, val);
		}
	}
}
