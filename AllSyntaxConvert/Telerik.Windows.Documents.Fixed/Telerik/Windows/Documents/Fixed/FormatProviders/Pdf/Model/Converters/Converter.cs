using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class Converter : IConverter
	{
		public PdfPrimitive Convert(Type type, PostScriptReader reader, IPdfImportContext context, PdfPrimitive value)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			if (value == null)
			{
				return null;
			}
			switch (value.Type)
			{
			case PdfElementType.PdfReal:
				return this.ConvertFromReal(type, reader, context, (PdfReal)value);
			case PdfElementType.PdfInt:
				return this.ConvertFromInt(type, reader, context, (PdfInt)value);
			case PdfElementType.String:
				return this.ConvertFromString(type, reader, context, (PdfString)value);
			case PdfElementType.Stream:
				return this.ConvertFromStream(type, reader, context, (PdfStream)value);
			case PdfElementType.PdfName:
				return this.ConvertFromName(type, reader, context, (PdfName)value);
			case PdfElementType.Dictionary:
				return this.ConvertFromDictionary(type, reader, context, (PdfDictionary)value);
			case PdfElementType.Array:
				return this.ConvertFromArray(type, reader, context, (PdfArray)value);
			case PdfElementType.IndirectObject:
				return this.Convert(type, reader, context, ((IndirectObject)value).Content);
			case PdfElementType.IndirectReference:
			{
				IndirectReference reference = (IndirectReference)value;
				PdfPrimitive pdfPrimitive;
				if (!context.TryGetIndirectObject(reference, out pdfPrimitive))
				{
					pdfPrimitive = this.ConvertFromIndirectReference(type, reader, context, (IndirectReference)value);
					context.RegisterIndirectObject(reference, pdfPrimitive);
				}
				if (pdfPrimitive.Type != PdfElementType.Null)
				{
					return pdfPrimitive;
				}
				return null;
			}
			}
			return value;
		}

		protected virtual PdfPrimitive ConvertFromIndirectReference(Type type, PostScriptReader reader, IPdfImportContext context, IndirectReference reference)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IndirectReference>(reference, "reference");
			IndirectObject indirectObject = context.ReadIndirectObject(reference);
			PdfPrimitive content = indirectObject.Content;
			return this.Convert(type, reader, context, content);
		}

		protected virtual PdfPrimitive ConvertFromDictionary(Type type, PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfDictionary>(dictionary, "dictionary");
			PdfObject pdfObject = this.CreateInstance(type, reader, context, dictionary) as PdfObject;
			if (pdfObject != null)
			{
				pdfObject.Load(reader, context, dictionary);
				return pdfObject;
			}
			return dictionary;
		}

		protected virtual PdfPrimitive ConvertFromStream(Type type, PostScriptReader reader, IPdfImportContext context, PdfStream stream)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfStream>(stream, "stream");
			PdfStreamObjectBase pdfStreamObjectBase = this.CreateInstance(type, reader, context, stream.Dictionary) as PdfStreamObjectBase;
			if (pdfStreamObjectBase != null)
			{
				pdfStreamObjectBase.Load(reader, context, stream);
				return pdfStreamObjectBase;
			}
			return stream;
		}

		protected virtual PdfPrimitive ConvertFromName(Type type, PostScriptReader reader, IPdfImportContext context, PdfName name)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfName>(name, "name");
			return name;
		}

		protected virtual PdfPrimitive ConvertFromString(Type type, PostScriptReader reader, IPdfImportContext context, PdfString str)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfString>(str, "str");
			return str;
		}

		protected virtual PdfPrimitive ConvertFromArray(Type type, PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfArray>(array, "array");
			return array;
		}

		protected virtual PdfPrimitive ConvertFromInt(Type type, PostScriptReader reader, IPdfImportContext context, PdfInt i)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfInt>(i, "i");
			return i;
		}

		protected virtual PdfPrimitive ConvertFromReal(Type type, PostScriptReader reader, IPdfImportContext context, PdfReal r)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfReal>(r, "r");
			return r;
		}

		protected virtual object CreateInstance(Type type, PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfDictionary>(dictionary, "dictionary");
			return Activator.CreateInstance(type);
		}
	}
}
