using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	abstract class PdfPropertyBase<T> : IPdfProperty where T : PdfObjectOld
	{
		public PdfPropertyBase(PdfContentManager contentManager, PdfPropertyDescriptor descriptor)
		{
			this.contentManager = contentManager;
			this.descriptor = descriptor;
		}

		public PdfPropertyBase(PdfContentManager contentManager, PdfPropertyDescriptor descriptor, IConverter converter)
			: this(contentManager, descriptor)
		{
			this.converter = converter;
		}

		protected PdfContentManager ContentManager
		{
			get
			{
				return this.contentManager;
			}
		}

		public PdfPropertyDescriptor Descriptor
		{
			get
			{
				return this.descriptor;
			}
		}

		protected T CreateValue(object source)
		{
			IndirectReferenceOld indirectReferenceOld = source as IndirectReferenceOld;
			if (indirectReferenceOld != null && (this.converter == null || !this.converter.HandlesIndirectReference))
			{
				return Converters.IndirectReferenceToPdfObjectConverter.Convert<T>(this.contentManager, indirectReferenceOld);
			}
			if (this.converter != null)
			{
				object obj = this.converter.Convert(typeof(T), this.contentManager, source);
				return (T)((object)obj);
			}
			T t = source as T;
			if (t != null)
			{
				return t;
			}
			PdfDictionaryOld pdfDictionaryOld = source as PdfDictionaryOld;
			if (pdfDictionaryOld != null)
			{
				return (T)((object)Converters.PdfDictionaryToPdfObjectConverter.Convert(typeof(T), this.contentManager, pdfDictionaryOld));
			}
			return default(T);
		}

		public abstract T GetValue();

		public abstract bool SetValue(object value);

		readonly PdfContentManager contentManager;

		readonly IConverter converter;

		readonly PdfPropertyDescriptor descriptor;
	}
}
