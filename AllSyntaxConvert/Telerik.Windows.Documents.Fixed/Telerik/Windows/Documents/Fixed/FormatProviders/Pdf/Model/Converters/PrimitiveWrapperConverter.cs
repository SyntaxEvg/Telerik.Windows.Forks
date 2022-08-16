using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class PrimitiveWrapperConverter : IConverter
	{
		public PdfPrimitive Convert(Type type, PostScriptReader reader, IPdfImportContext context, PdfPrimitive value)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			if (value == null)
			{
				return null;
			}
			if (value.Type == PdfElementType.IndirectReference)
			{
				IndirectReference reference = (IndirectReference)value;
				IndirectObject indirectObject = context.ReadIndirectObject(reference);
				return new PrimitiveWrapper(indirectObject.Content);
			}
			return new PrimitiveWrapper(value);
		}
	}
}
