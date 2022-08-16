using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class IndirectReferenceToPdfObjectConverter
	{
		public T Convert<T>(PdfContentManager contentManager, IndirectReferenceOld reference) where T : PdfObjectOld
		{
			object obj;
			if (!contentManager.TryGetPdfObject(reference, out obj))
			{
				T t = (T)((object)Activator.CreateInstance(typeof(T), new object[] { contentManager }));
				t.Reference = reference;
				contentManager.RegisterIndirectReference(reference, t);
				return t;
			}
			if (!(obj is T))
			{
				T result = (T)((object)Activator.CreateInstance(typeof(T), new object[] { contentManager }));
				result.Reference = reference;
				return result;
			}
			return (T)((object)obj);
		}
	}
}
