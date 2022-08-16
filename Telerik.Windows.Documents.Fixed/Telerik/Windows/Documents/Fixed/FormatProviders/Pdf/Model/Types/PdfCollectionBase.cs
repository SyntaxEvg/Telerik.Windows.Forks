using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	abstract class PdfCollectionBase<TIndex> : PdfPrimitive
	{
		public abstract int Count { get; }

		public abstract PdfPrimitive this[TIndex index] { get; set; }

		public T GetElement<T>(PostScriptReader reader, IPdfImportContext context, TIndex index) where T : PdfPrimitive
		{
			PdfPrimitive element = this[index];
			return PdfCollectionBase<TIndex>.ConvertElementToType<T>(reader, context, element);
		}

		public bool TryGetElement<T>(PostScriptReader reader, IPdfImportContext context, TIndex index, out T element) where T : PdfPrimitive
		{
			PdfPrimitive element2;
			if (!this.TryGetElementOverride(index, out element2))
			{
				element = default(T);
				return false;
			}
			element = PdfCollectionBase<TIndex>.ConvertElementToType<T>(reader, context, element2);
			return true;
		}

		public bool TryGetElement(TIndex index, out PdfPrimitive element)
		{
			return this.TryGetElementOverride(index, out element);
		}

		protected abstract bool TryGetElementOverride(TIndex index, out PdfPrimitive element);

		static T ConvertElementToType<T>(PostScriptReader reader, IPdfImportContext context, PdfPrimitive element) where T : PdfPrimitive
		{
			PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor<T>();
			PdfPrimitive pdfPrimitive = pdfObjectDescriptor.Converter.Convert(typeof(T), reader, context, element);
			return (T)((object)pdfPrimitive);
		}
	}
}
