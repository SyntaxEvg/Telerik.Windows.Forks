using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	class InstantLoadProperty<T> : PdfPropertyBase<T> where T : PdfObjectOld
	{
		public InstantLoadProperty(PdfContentManager contentManager, PdfPropertyDescriptor descriptor)
			: base(contentManager, descriptor)
		{
		}

		public InstantLoadProperty(PdfContentManager contentManager, PdfPropertyDescriptor descriptor, IConverter converter)
			: base(contentManager, descriptor, converter)
		{
		}

		public InstantLoadProperty(PdfContentManager contentManager, PdfPropertyDescriptor descriptor, T initialValue)
			: base(contentManager, descriptor)
		{
			this.value = initialValue;
		}

		public InstantLoadProperty(PdfContentManager contentManager, PdfPropertyDescriptor descriptor, T initialValue, IConverter converter)
			: base(contentManager, descriptor, converter)
		{
			this.value = initialValue;
		}

		public override T GetValue()
		{
			return this.value;
		}

		public override bool SetValue(object source)
		{
			if (this.value != source)
			{
				this.value = base.CreateValue(source);
				if (this.value != null)
				{
					base.ContentManager.LoadIndirectObject<T>(this.value);
				}
			}
			return true;
		}

		T value;
	}
}
