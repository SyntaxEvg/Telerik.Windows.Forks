using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class StitchingFunctionConverter : Converter
	{
		protected override PdfPrimitive ConvertFromArray(Type type, PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (PdfPrimitive pdfPrimitive in array)
			{
				IndirectReference indirectReference = pdfPrimitive as IndirectReference;
				if (indirectReference != null)
				{
					PdfPrimitive item;
					context.TryGetIndirectObject(indirectReference, out item);
					pdfArray.Add(item);
				}
				else
				{
					pdfArray.Add(pdfPrimitive);
				}
			}
			return pdfArray;
		}
	}
}
