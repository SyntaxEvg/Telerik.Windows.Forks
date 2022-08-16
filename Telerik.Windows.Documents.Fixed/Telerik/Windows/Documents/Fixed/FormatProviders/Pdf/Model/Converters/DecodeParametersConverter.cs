using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	static class DecodeParametersConverter
	{
		public static DecodeParameters[] Convert(PostScriptReader reader, IPdfImportContext context, PdfPrimitive primitive)
		{
			object obj = DecodeParametersConverter.Strip(reader, context, primitive);
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				return DecodeParametersConverter.CreateDecodeParameters(new Dictionary<string, object>[] { dictionary });
			}
			object[] array = obj as object[];
			if (array != null)
			{
				return DecodeParametersConverter.CreateDecodeParameters(array);
			}
			return null;
		}

		public static PrimitiveWrapper CreateDecodeParms(Dictionary<string, object>[] decodeParameters)
		{
			if (decodeParameters == null)
			{
				return null;
			}
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (Dictionary<string, object> dictionary in decodeParameters)
			{
				PdfDictionary pdfDictionary = new PdfDictionary();
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					pdfDictionary[keyValuePair.Key] = DecodeParametersConverter.CreatePrimitive(keyValuePair.Value);
				}
				pdfArray.Add(pdfDictionary);
			}
			return new PrimitiveWrapper(pdfArray);
		}

		static DecodeParameters[] CreateDecodeParameters(object[] decodeParametersArray)
		{
			Guard.ThrowExceptionIfNull<object[]>(decodeParametersArray, "decodeParametersArray");
			DecodeParameters[] array = new DecodeParameters[decodeParametersArray.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)decodeParametersArray[i];
				array[i] = new DecodeParameters(dictionary);
			}
			return array;
		}

		static DecodeParameters CreateDecodeParameters(PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfDictionary>(dictionary, "dictionary");
			DecodeParameters decodeParameters = new DecodeParameters();
			foreach (string text in dictionary.Keys)
			{
				decodeParameters[text] = DecodeParametersConverter.Strip(reader, context, dictionary[text]);
			}
			return decodeParameters;
		}

		static object Strip(PostScriptReader reader, IPdfImportContext context, PdfPrimitive primitive)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfPrimitive>(primitive, "primitive");
			IPdfSimpleType pdfSimpleType = primitive as IPdfSimpleType;
			if (pdfSimpleType != null)
			{
				return pdfSimpleType.Value;
			}
			PdfElementType type = primitive.Type;
			switch (type)
			{
			case PdfElementType.Dictionary:
				return DecodeParametersConverter.StripDictionary(reader, context, (PdfDictionary)primitive);
			case PdfElementType.Array:
				return DecodeParametersConverter.StripArray(reader, context, (PdfArray)primitive);
			default:
				if (type == PdfElementType.IndirectReference)
				{
					return DecodeParametersConverter.StripIndirectReference(reader, context, (IndirectReference)primitive);
				}
				throw new NotSupportedException("Type is not supported.");
			}
		}

		static object StripIndirectReference(PostScriptReader reader, IPdfImportContext context, IndirectReference reference)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IndirectReference>(reference, "reference");
			PdfPrimitive primitive;
			if (!context.TryGetIndirectObject(reference, out primitive))
			{
				primitive = context.ReadIndirectObject(reference);
				context.RegisterIndirectObject(reference, primitive);
			}
			return DecodeParametersConverter.Strip(reader, context, primitive);
		}

		static object[] StripArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfArray>(array, "array");
			object[] array2 = new object[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				array2[i] = DecodeParametersConverter.Strip(reader, context, array[i]);
			}
			return array2;
		}

		static Dictionary<string, object> StripDictionary(PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfDictionary>(dictionary, "dictionary");
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			foreach (string text in dictionary.Keys)
			{
				dictionary2[text] = DecodeParametersConverter.Strip(reader, context, dictionary[text]);
			}
			return dictionary2;
		}

		static PdfPrimitive CreatePrimitive(object obj)
		{
			if (obj == null)
			{
				return PdfNull.Instance;
			}
			string text = obj as string;
			if (text != null)
			{
				return new PdfName(text);
			}
			if (obj is int)
			{
				return new PdfInt((int)obj);
			}
			if (obj is double)
			{
				return new PdfReal((double)obj);
			}
			if (obj is bool)
			{
				return new PdfBool((bool)obj);
			}
			Array array = obj as Array;
			if (array != null)
			{
				PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
				foreach (object obj2 in array)
				{
					PdfPrimitive item = DecodeParametersConverter.CreatePrimitive(obj2);
					pdfArray.Add(item);
				}
				return pdfArray;
			}
			throw new NotSupportedException(string.Format("Cannot create PdfPrimitive from type: {0}", obj.GetType()));
		}
	}
}
