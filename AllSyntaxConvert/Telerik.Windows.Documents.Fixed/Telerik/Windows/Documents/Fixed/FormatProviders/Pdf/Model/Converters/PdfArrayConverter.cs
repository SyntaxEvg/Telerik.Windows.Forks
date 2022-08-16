using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class PdfArrayConverter : Converter
	{
		protected override PdfPrimitive ConvertFromName(Type type, PostScriptReader reader, IPdfImportContext context, PdfName name)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfName>(name, "name");
			return this.ConvertFromPrimitive(name);
		}

		protected override PdfPrimitive ConvertFromInt(Type type, PostScriptReader reader, IPdfImportContext context, PdfInt i)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfInt>(i, "i");
			return this.ConvertFromPrimitive(i);
		}

		protected override PdfPrimitive ConvertFromReal(Type type, PostScriptReader reader, IPdfImportContext context, PdfReal r)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfReal>(r, "r");
			return this.ConvertFromPrimitive(r);
		}

		protected override PdfPrimitive ConvertFromString(Type type, PostScriptReader reader, IPdfImportContext context, PdfString str)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfString>(str, "str");
			return this.ConvertFromPrimitive(str);
		}

		protected override PdfPrimitive ConvertFromDictionary(Type type, PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfDictionary>(dictionary, "dictionary");
			return this.ConvertFromPrimitive(dictionary);
		}

		PdfArray ConvertFromPrimitive(PdfPrimitive primitive)
		{
			Guard.ThrowExceptionIfNull<PdfPrimitive>(primitive, "primitive");
			return new PdfArray(new PdfPrimitive[] { primitive });
		}
	}
}
