using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class IndirectObjectOld : IndirectReferenceOld
	{
		public object Value { get; set; }

		public PdfNameOld GetObjectType()
		{
			if (this.Value is PdfDictionaryOld)
			{
				return (this.Value as PdfDictionaryOld).GetElement<PdfNameOld>("Type");
			}
			return null;
		}
	}
}
