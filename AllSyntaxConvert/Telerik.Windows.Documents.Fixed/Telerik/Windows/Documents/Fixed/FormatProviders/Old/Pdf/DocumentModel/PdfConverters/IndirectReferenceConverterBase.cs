using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	abstract class IndirectReferenceConverterBase : ConverterBase
	{
		public override bool HandlesIndirectReference
		{
			get
			{
				return true;
			}
		}

		public override object Convert(Type type, PdfContentManager contentManager, object value)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			IndirectReferenceOld indirectReferenceOld = value as IndirectReferenceOld;
			if (indirectReferenceOld != null)
			{
				return this.ConvertFromIndirectReference(type, contentManager, indirectReferenceOld);
			}
			return base.Convert(type, contentManager, value);
		}

		protected virtual object ConvertFromIndirectReference(Type type, PdfContentManager contentManager, IndirectReferenceOld indirectReference)
		{
			object obj;
			if (!contentManager.TryGetPdfObject(indirectReference, out obj))
			{
				IndirectObjectOld indirectObjectOld;
				if (contentManager.TryGetIndirectObject(indirectReference, out indirectObjectOld))
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
	}
}
