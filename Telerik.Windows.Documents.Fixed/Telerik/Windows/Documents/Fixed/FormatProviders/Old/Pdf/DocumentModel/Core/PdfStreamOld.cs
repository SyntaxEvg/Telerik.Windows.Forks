using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	class PdfStreamOld : PdfObjectOld
	{
		public PdfStreamOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public virtual void Load(PdfDataStream stream)
		{
			this.Load(stream.Dictionary);
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			PdfDataStream pdfDataStream = indirectObject.Value as PdfDataStream;
			if (pdfDataStream != null)
			{
				this.Load(pdfDataStream);
			}
			base.Load(indirectObject);
		}
	}
}
