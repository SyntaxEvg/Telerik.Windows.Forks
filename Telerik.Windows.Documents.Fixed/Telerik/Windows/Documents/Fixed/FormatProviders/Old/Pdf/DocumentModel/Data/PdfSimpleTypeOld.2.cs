using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfSimpleTypeOld<T> : PdfSimpleTypeOld
	{
		public PdfSimpleTypeOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public T Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				base.IsLoaded = true;
			}
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			PdfSimpleTypeOld<T> pdfSimpleTypeOld = indirectObject as PdfSimpleTypeOld<T>;
			if (pdfSimpleTypeOld != null)
			{
				this.Value = pdfSimpleTypeOld.Value;
			}
			base.Load(indirectObject);
		}

		public override object GetValue()
		{
			return this.Value;
		}

		public override string ToString()
		{
			T t = this.Value;
			return t.ToString();
		}

		T value;
	}
}
