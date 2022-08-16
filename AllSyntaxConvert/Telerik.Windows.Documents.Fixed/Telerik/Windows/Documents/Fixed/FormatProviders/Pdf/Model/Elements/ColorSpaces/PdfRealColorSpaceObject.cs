using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	abstract class PdfRealColorSpaceObject : ColorSpaceObject
	{
		public sealed override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfArray components)
		{
			IConverter converter = PdfObjectDescriptors.GetPdfObjectDescriptor<PdfReal>().Converter;
			PdfReal[] array = new PdfReal[components.Count];
			for (int i = 0; i < components.Count; i++)
			{
				array[i] = (PdfReal)converter.Convert(typeof(PdfReal), reader, context.Owner, components[i]);
			}
			return this.GetColor(context, reader, array);
		}

		protected abstract ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfReal[] components);
	}
}
