using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class LookupOld : PdfObjectOld
	{
		public LookupOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		public void Load(PdfDataStream stream)
		{
			Guard.ThrowExceptionIfNull<PdfDataStream>(stream, "stream");
			this.data = stream.ReadData(base.ContentManager);
		}

		public void Load(PdfStringOld str)
		{
			Guard.ThrowExceptionIfNull<PdfStringOld>(str, "str");
			this.data = str.Value;
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			PdfDataStream pdfDataStream = indirectObject.Value as PdfDataStream;
			if (pdfDataStream != null)
			{
				this.Load(pdfDataStream);
			}
			else
			{
				PdfStringOld pdfStringOld = indirectObject.Value as PdfStringOld;
				if (pdfStringOld != null)
				{
					this.Load(pdfStringOld);
				}
			}
			base.Load(indirectObject);
		}

		byte[] data;
	}
}
