using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	class ContentStreamOld : PdfObjectOld
	{
		public ContentStreamOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.Data = null;
		}

		public byte[] Data { get; set; }

		public PdfArrayOld Array { get; set; }

		public void AppendData(PdfDataStream stream)
		{
			Guard.ThrowExceptionIfNull<PdfDataStream>(stream, "stream");
			byte[] dataToAppend = stream.ReadData(base.ContentManager);
			bool concatWithWhiteSpace = true;
			this.Data = PdfConcatenatableDataStream.Concat(this.Data, dataToAppend, concatWithWhiteSpace);
			base.IsLoaded = true;
		}

		public void Load(PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			foreach (object obj in array)
			{
				IndirectReferenceOld indirectReferenceOld = obj as IndirectReferenceOld;
				IndirectObjectOld indirectObject;
				if (indirectReferenceOld != null && base.ContentManager.TryGetIndirectObject(indirectReferenceOld, out indirectObject))
				{
					this.Load(indirectObject);
				}
			}
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			PdfDataStream pdfDataStream = indirectObject.Value as PdfDataStream;
			if (pdfDataStream != null)
			{
				this.AppendData(pdfDataStream);
			}
			else
			{
				PdfArrayOld pdfArrayOld = indirectObject.Value as PdfArrayOld;
				if (pdfArrayOld != null)
				{
					this.Load(pdfArrayOld);
				}
			}
			base.Load(indirectObject);
		}
	}
}
