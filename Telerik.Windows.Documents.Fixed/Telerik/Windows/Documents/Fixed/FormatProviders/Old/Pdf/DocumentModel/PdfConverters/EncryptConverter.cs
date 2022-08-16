using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class EncryptConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromIndirectReference(Type type, PdfContentManager contentManager, IndirectReferenceOld indirectReference)
		{
			object obj;
			if (!contentManager.TryGetPdfObject(indirectReference, out obj))
			{
				IndirectObjectOld indirectObjectOld;
				if (contentManager.TryGetIndirectObject(indirectReference, false, out indirectObjectOld))
				{
					obj = this.Convert(type, contentManager, indirectObjectOld.Value);
				}
				PdfObjectOld pdfObjectOld = obj as PdfObjectOld;
				if (pdfObjectOld != null)
				{
					pdfObjectOld.Reference = indirectReference;
				}
				contentManager.RegisterIndirectReference(indirectReference, obj);
			}
			return obj;
		}

		protected override object ConvertFromPdfDictionary(Type type, PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			return EncryptOld.CreateEncrypt(contentManager, dictionary);
		}
	}
}
