using System;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class PdfStringConverter : ConverterBase
	{
		protected override object ConvertFromPdfName(Type type, PdfContentManager contentManager, PdfNameOld name)
		{
			return new PdfStringOld(contentManager, Encoding.UTF8.GetBytes(name.Value));
		}
	}
}
