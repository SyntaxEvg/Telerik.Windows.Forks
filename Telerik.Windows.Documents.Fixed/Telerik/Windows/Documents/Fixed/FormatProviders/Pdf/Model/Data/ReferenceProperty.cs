using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data
{
	class ReferenceProperty<T> : PdfPropertyBase<T> where T : PdfPrimitive
	{
		public ReferenceProperty(PdfPropertyDescriptor descriptor)
			: base(descriptor)
		{
		}

		public ReferenceProperty(PdfPropertyDescriptor descriptor, T defaultValue)
			: base(descriptor, defaultValue)
		{
			this.value = defaultValue;
		}

		internal IndirectReference Reference { get; set; }

		public override T GetValue()
		{
			this.EnsureValueSourceIsApplied();
			return this.value;
		}

		public override void SetValue(PdfPrimitive newValue)
		{
			this.EnsureValueSourceIsApplied();
			if (this.value != newValue)
			{
				this.value = (T)((object)newValue);
			}
		}

		public override void SetValue(PostScriptReader reader, IPdfImportContext context, PdfPrimitive value)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			this.source = null;
			if (value == null)
			{
				this.value = default(T);
				return;
			}
			if (value.Type == PdfElementType.IndirectReference)
			{
				this.Reference = (IndirectReference)value;
				this.source = new ReferencePropertyValueSource(reader, context, this.Reference);
				return;
			}
			this.SetValue(base.PrepareValue(reader, context, value));
		}

		void EnsureValueSourceIsApplied()
		{
			if (this.source != null)
			{
				this.value = base.PrepareValue(this.source.Reader, this.source.Context, this.source.Source);
				this.source = null;
			}
		}

		ReferencePropertyValueSource source;

		T value;
	}
}
