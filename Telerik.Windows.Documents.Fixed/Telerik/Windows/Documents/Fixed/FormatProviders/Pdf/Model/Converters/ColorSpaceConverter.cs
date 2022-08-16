using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class ColorSpaceConverter : Converter
	{
		protected override PdfPrimitive ConvertFromName(Type type, PostScriptReader reader, IPdfImportContext context, PdfName name)
		{
			return ColorSpaceManager.CreateColorSpaceObject(name.Value);
		}

		protected override PdfPrimitive ConvertFromArray(Type type, PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			PdfName pdfName = (PdfName)array[0];
			ColorSpaceObject colorSpaceObject = ColorSpaceManager.CreateColorSpaceObject(pdfName.Value);
			if (colorSpaceObject != null)
			{
				colorSpaceObject.ImportFromArray(reader, context, array);
				return colorSpaceObject;
			}
			throw new NotSupportedException(string.Format("The provided color space: {0} is not supported.", pdfName));
		}
	}
}
