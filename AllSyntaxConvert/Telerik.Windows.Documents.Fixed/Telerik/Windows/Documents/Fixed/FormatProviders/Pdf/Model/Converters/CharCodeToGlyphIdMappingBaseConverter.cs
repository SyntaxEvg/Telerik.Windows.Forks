using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class CharCodeToGlyphIdMappingBaseConverter : Converter
	{
		protected override PdfPrimitive ConvertFromName(Type type, PostScriptReader reader, IPdfImportContext context, PdfName name)
		{
			Guard.ThrowExceptionIfNull<PdfName>(name, "name");
			string value;
			if ((value = name.Value) != null && value == "Identity")
			{
				return CharIdToGlyphIdMappingBase.Identity;
			}
			throw new NotSupportedException("Encoding type is not supported");
		}

		protected override PdfPrimitive ConvertFromStream(Type type, PostScriptReader reader, IPdfImportContext context, PdfStream stream)
		{
			Guard.ThrowExceptionIfNull<PdfStream>(stream, "name");
			CharIdToGlyphIdMapping charIdToGlyphIdMapping = new CharIdToGlyphIdMapping();
			charIdToGlyphIdMapping.Load(reader, context, stream);
			return charIdToGlyphIdMapping;
		}
	}
}
