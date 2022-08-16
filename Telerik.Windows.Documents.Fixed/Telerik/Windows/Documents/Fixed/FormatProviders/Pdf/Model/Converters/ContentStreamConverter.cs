using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class ContentStreamConverter : Converter
	{
		protected override PdfPrimitive ConvertFromArray(Type type, PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfArray>(array, "array");
			PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor<ContentStream>();
			ContentStream contentStream = new ContentStream();
			for (int i = 0; i < array.Count; i++)
			{
				PdfPrimitive value = array[i];
				ContentStream stream = (ContentStream)pdfObjectDescriptor.Converter.Convert(typeof(ContentStream), reader, context, value);
				contentStream.Concat(stream);
			}
			return contentStream;
		}
	}
}
