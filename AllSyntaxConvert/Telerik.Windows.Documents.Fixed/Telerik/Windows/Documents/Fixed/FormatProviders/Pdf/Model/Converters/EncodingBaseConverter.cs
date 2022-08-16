using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class EncodingBaseConverter : Converter
	{
		protected override PdfPrimitive ConvertFromName(Type type, PostScriptReader reader, IPdfImportContext context, PdfName name)
		{
			Guard.ThrowExceptionIfNull<PdfName>(name, "name");
			string value;
			if ((value = name.Value) != null && value == "Identity-H")
			{
				return EncodingBaseObject.IdentityH;
			}
			throw new NotSupportedException("Encoding type is not supported");
		}

		protected override PdfPrimitive ConvertFromStream(Type type, PostScriptReader reader, IPdfImportContext context, PdfStream stream)
		{
			Guard.ThrowExceptionIfNull<PdfStream>(stream, "name");
			EncodingObject encodingObject = new EncodingObject();
			encodingObject.Load(reader, context, stream);
			return encodingObject;
		}
	}
}
