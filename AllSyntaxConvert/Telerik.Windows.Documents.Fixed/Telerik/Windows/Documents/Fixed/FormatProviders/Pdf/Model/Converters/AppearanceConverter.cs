using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class AppearanceConverter : IConverter
	{
		public PdfPrimitive Convert(Type type, PostScriptReader reader, IPdfImportContext context, PdfPrimitive value)
		{
			if (value == null || value.Type == PdfElementType.Null)
			{
				return null;
			}
			PdfElementType type2 = value.Type;
			switch (type2)
			{
			case PdfElementType.Stream:
				return this.ConvertFromStream(reader, context, (PdfStream)value);
			case PdfElementType.PdfName:
			case PdfElementType.Array:
				break;
			case PdfElementType.Dictionary:
				return this.ConvertFromDictionary(reader, context, (PdfDictionary)value);
			case PdfElementType.IndirectObject:
				return this.Convert(type, reader, context, ((IndirectObject)value).Content);
			default:
				if (type2 == PdfElementType.IndirectReference)
				{
					return this.ConvertFromIndirectReference(type, reader, context, (IndirectReference)value);
				}
				break;
			}
			throw new NotSupportedException(string.Format("Not supported value type: {0}", value.Type));
		}

		Appearance ConvertFromIndirectReference(Type type, PostScriptReader reader, IPdfImportContext context, IndirectReference indirectReference)
		{
			PdfPrimitive pdfPrimitive;
			Appearance appearance;
			if (context.TryGetIndirectObject(indirectReference, out pdfPrimitive))
			{
				FormXObject formXObject = pdfPrimitive as FormXObject;
				appearance = ((formXObject == null) ? ((Appearance)pdfPrimitive) : new Appearance(formXObject));
			}
			else
			{
				PdfPrimitive content = context.ReadIndirectObject(indirectReference).Content;
				appearance = (Appearance)this.Convert(type, reader, context, content);
				if (appearance.SingleStateAppearance != null)
				{
					context.RegisterIndirectObject(indirectReference, appearance.SingleStateAppearance);
				}
				else
				{
					context.RegisterIndirectObject(indirectReference, appearance);
				}
			}
			return appearance;
		}

		Appearance ConvertFromStream(PostScriptReader reader, IPdfImportContext context, PdfStream pdfStream)
		{
			IConverter converter = PdfObjectDescriptors.GetPdfObjectDescriptor<FormXObject>().Converter;
			FormXObject singleStateAppearance = (FormXObject)converter.Convert(typeof(FormXObject), reader, context, pdfStream);
			return new Appearance(singleStateAppearance);
		}

		Appearance ConvertFromDictionary(PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Dictionary<string, FormXObject> dictionary2 = new Dictionary<string, FormXObject>();
			IConverter converter = PdfObjectDescriptors.GetPdfObjectDescriptor<FormXObject>().Converter;
			foreach (KeyValuePair<string, PdfPrimitive> keyValuePair in dictionary)
			{
				FormXObject value = (FormXObject)converter.Convert(typeof(FormXObject), reader, context, keyValuePair.Value);
				dictionary2[keyValuePair.Key] = value;
			}
			return new Appearance(dictionary2);
		}
	}
}
