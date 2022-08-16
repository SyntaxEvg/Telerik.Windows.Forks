using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects
{
	class XObject : PdfStreamOld
	{
		public XObject(PdfContentManager contentManager)
			: base(contentManager)
		{
		}
	}
}
