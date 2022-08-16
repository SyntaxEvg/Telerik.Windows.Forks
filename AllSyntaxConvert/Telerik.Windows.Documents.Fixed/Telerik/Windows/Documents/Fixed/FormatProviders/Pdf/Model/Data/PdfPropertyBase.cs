using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data
{
	abstract class PdfPropertyBase<T> : IPdfProperty where T : PdfPrimitive
	{
		public PdfPropertyBase(PdfPropertyDescriptor descriptor)
		{
			Guard.ThrowExceptionIfNull<PdfPropertyDescriptor>(descriptor, "descriptor");
			this.descriptor = descriptor;
		}

		public PdfPropertyBase(PdfPropertyDescriptor descriptor, PdfPrimitive defaultValue)
			: this(descriptor)
		{
			this.defaultValue = defaultValue;
		}

		public PdfPropertyDescriptor Descriptor
		{
			get
			{
				return this.descriptor;
			}
		}

		public bool HasNonDefaultValue
		{
			get
			{
				if (this.defaultValue == null)
				{
					return this.GetValue() != null;
				}
				return !this.defaultValue.Equals(this.GetValue());
			}
		}

		public bool HasValue
		{
			get
			{
				return this.GetValue() != null;
			}
		}

		public abstract T GetValue();

		public abstract void SetValue(PdfPrimitive value);

		public abstract void SetValue(PostScriptReader reader, IPdfImportContext context, PdfPrimitive value);

		PdfPrimitive IPdfProperty.GetValue()
		{
			return this.GetValue();
		}

		protected T PrepareValue(PostScriptReader reader, IPdfImportContext context, PdfPrimitive value)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor<T>();
			PdfPrimitive pdfPrimitive = pdfObjectDescriptor.Converter.Convert(typeof(T), reader, context, value);
			return (T)((object)pdfPrimitive);
		}

		readonly PdfPropertyDescriptor descriptor;

		readonly PdfPrimitive defaultValue;
	}
}
