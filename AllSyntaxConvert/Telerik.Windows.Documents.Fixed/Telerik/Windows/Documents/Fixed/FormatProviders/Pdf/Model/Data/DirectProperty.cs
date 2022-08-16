using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data
{
	class DirectProperty<T> : PdfPropertyBase<T> where T : PdfPrimitive
	{
		public DirectProperty(PdfPropertyDescriptor descriptor)
			: base(descriptor)
		{
		}

		public DirectProperty(PdfPropertyDescriptor descriptor, T defaultValue)
			: base(descriptor, defaultValue)
		{
			this.value = defaultValue;
		}

		public override T GetValue()
		{
			return this.value;
		}

		public override void SetValue(PdfPrimitive source)
		{
			if (this.value != source)
			{
				this.value = (T)((object)source);
			}
		}

		public override void SetValue(PostScriptReader reader, IPdfImportContext context, PdfPrimitive value)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			T t = base.PrepareValue(reader, context, value);
			this.SetValue(t);
		}

		T value;
	}
}
