using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	class LoadOnDemandProperty<T> : PdfPropertyBase<T> where T : PdfObjectOld
	{
		public LoadOnDemandProperty(PdfContentManager contentManager, PdfPropertyDescriptor descriptor)
			: base(contentManager, descriptor)
		{
		}

		public LoadOnDemandProperty(PdfContentManager contentManager, PdfPropertyDescriptor descriptor, T defaultValue)
			: this(contentManager, descriptor)
		{
			this.source = defaultValue;
		}

		public LoadOnDemandProperty(PdfContentManager contentManager, PdfPropertyDescriptor descriptor, IConverter converter)
			: base(contentManager, descriptor, converter)
		{
		}

		public LoadOnDemandProperty(PdfContentManager contentManager, PdfPropertyDescriptor descriptor, T initialValue, IConverter converter)
			: base(contentManager, descriptor, converter)
		{
			this.source = initialValue;
		}

		void LoadValue()
		{
			if (this.source == null)
			{
				return;
			}
			if (this.valueChanged || this.value == null)
			{
				this.value = base.CreateValue(this.source);
				if (this.value != null)
				{
					base.ContentManager.LoadIndirectObject<T>(this.value);
				}
				this.valueChanged = false;
			}
		}

		public override T GetValue()
		{
			this.LoadValue();
			return this.value;
		}

		public override bool SetValue(object value)
		{
			if (this.source != value)
			{
				this.source = value;
				this.valueChanged = true;
			}
			return true;
		}

		object source;

		T value;

		bool valueChanged;
	}
}
