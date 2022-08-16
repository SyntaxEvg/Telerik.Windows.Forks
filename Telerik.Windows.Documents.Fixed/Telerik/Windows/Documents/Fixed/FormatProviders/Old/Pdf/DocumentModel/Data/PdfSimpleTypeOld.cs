using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	abstract class PdfSimpleTypeOld : PdfObjectOld
	{
		public PdfSimpleTypeOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public abstract object GetValue();
	}
}
