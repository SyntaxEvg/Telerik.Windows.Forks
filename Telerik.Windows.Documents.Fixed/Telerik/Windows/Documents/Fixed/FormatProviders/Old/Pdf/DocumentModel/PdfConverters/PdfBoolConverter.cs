using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class PdfBoolConverter : ConverterBase
	{
		public override object Convert(Type type, PdfContentManager contentManager, object value)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<object>(value, "value");
			PdfBoolOld pdfBoolOld = value as PdfBoolOld;
			if (pdfBoolOld != null)
			{
				return pdfBoolOld;
			}
			bool value2;
			Helper.UnboxBool(value, out value2);
			return new PdfBoolOld(contentManager, value2);
		}
	}
}
