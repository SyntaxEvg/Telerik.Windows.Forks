using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class SignatureContentsConverter : Converter
	{
		protected override PdfPrimitive ConvertFromString(Type type, PostScriptReader reader, IPdfImportContext context, PdfString str)
		{
			return new SignatureContents(str.Value);
		}
	}
}
