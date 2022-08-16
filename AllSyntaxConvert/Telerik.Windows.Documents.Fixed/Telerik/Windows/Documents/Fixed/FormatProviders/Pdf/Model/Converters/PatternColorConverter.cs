using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class PatternColorConverter : Converter
	{
		protected override PdfPrimitive ConvertFromStream(Type type, PostScriptReader reader, IPdfImportContext context, PdfStream stream)
		{
			PdfInt pdfInt;
			stream.Dictionary.TryGetElement<PdfInt>(reader, context, "PatternType", out pdfInt);
			if (pdfInt.Value == 1)
			{
				Tiling tiling = base.ConvertFromStream(typeof(Tiling), reader, context, stream) as Tiling;
				return new TilingPatternObject(tiling);
			}
			return base.ConvertFromDictionary(type, reader, context, stream.Dictionary);
		}

		protected override object CreateInstance(Type type, PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			PdfInt pdfInt;
			dictionary.TryGetElement<PdfInt>(reader, context, "PatternType", out pdfInt);
			if (pdfInt.Value != 1)
			{
				return PatternColorObject.CreateInstance(pdfInt);
			}
			return base.CreateInstance(type, reader, context, dictionary);
		}

		protected override PdfPrimitive ConvertFromDictionary(Type type, PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			return base.ConvertFromDictionary(type, reader, context, dictionary);
		}

		const int TilingPatternType = 1;
	}
}
