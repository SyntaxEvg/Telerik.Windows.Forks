using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfNameOld : PdfSimpleTypeOld<string>
	{
		public PdfNameOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public PdfNameOld(PdfContentManager contentManager, string value)
			: base(contentManager)
		{
			base.Value = value;
		}
	}
}
