using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class SignatureByteRangeConverter : Converter
	{
		protected override PdfPrimitive ConvertFromArray(Type type, PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			return new SignatureByteRange(array);
		}
	}
}
