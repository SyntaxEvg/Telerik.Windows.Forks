using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms
{
	[PdfClass]
	class XFAStreamOld : PdfObjectOld
	{
		public XFAStreamOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}
	}
}
