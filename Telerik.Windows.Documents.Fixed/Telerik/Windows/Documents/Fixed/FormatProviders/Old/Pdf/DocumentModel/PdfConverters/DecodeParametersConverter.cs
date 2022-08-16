using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class DecodeParametersConverter : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfDictionary(Type type, PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			return new DecodeParameters[] { DecodeParametersConverter.CreateDecodeParameters(contentManager, dictionary) };
		}

		protected override object ConvertFromPdfArray(Type type, PdfContentManager contentManager, PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			DecodeParameters[] array2 = new DecodeParameters[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				PdfDictionaryOld element = array.GetElement<PdfDictionaryOld>(i);
				if (element != null)
				{
					array2[i] = DecodeParametersConverter.CreateDecodeParameters(contentManager, element);
				}
			}
			return array2;
		}

		static DecodeParameters CreateDecodeParameters(PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			DecodeParameters decodeParameters = new DecodeParameters();
			foreach (string key in dictionary.Keys)
			{
				decodeParameters[key] = DecodeParametersConverter.StripPdfObject(contentManager, dictionary[key]);
			}
			return decodeParameters;
		}

		static object StripPdfObject(PdfContentManager contentManager, object obj)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			IndirectReferenceOld indirectReferenceOld = obj as IndirectReferenceOld;
			IndirectObjectOld indirectObjectOld;
			if (indirectReferenceOld != null && contentManager.TryGetIndirectObject(indirectReferenceOld, out indirectObjectOld))
			{
				return DecodeParametersConverter.StripPdfObject(contentManager, indirectObjectOld.Value);
			}
			PdfObjectOld pdfObjectOld = obj as PdfObjectOld;
			if (pdfObjectOld != null)
			{
				pdfObjectOld.Load();
				PdfSimpleTypeOld pdfSimpleTypeOld = pdfObjectOld as PdfSimpleTypeOld;
				if (pdfSimpleTypeOld != null)
				{
					return pdfSimpleTypeOld.GetValue();
				}
				PdfArrayOld pdfArrayOld = pdfObjectOld as PdfArrayOld;
				if (pdfArrayOld != null)
				{
					return DecodeParametersConverter.StripPdfArray(contentManager, pdfArrayOld);
				}
				PdfDictionaryOld pdfDictionaryOld = pdfObjectOld as PdfDictionaryOld;
				if (pdfDictionaryOld != null)
				{
					return DecodeParametersConverter.StripPdfDictionary(contentManager, pdfDictionaryOld);
				}
			}
			PdfDataStream pdfDataStream = obj as PdfDataStream;
			if (pdfDataStream != null)
			{
				return DecodeParametersConverter.StripPdfDataStream(contentManager, pdfDataStream);
			}
			return obj;
		}

		static object[] StripPdfArray(PdfContentManager contentManager, PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			object[] array2 = new object[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				array2[i] = DecodeParametersConverter.StripPdfObject(contentManager, array[i]);
			}
			return array2;
		}

		static Dictionary<string, object> StripPdfDictionary(PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			foreach (string key in dictionary.Keys)
			{
				dictionary2[key] = DecodeParametersConverter.StripPdfObject(contentManager, dictionary[key]);
			}
			return dictionary2;
		}

		static byte[] StripPdfDataStream(PdfContentManager contentManager, PdfDataStream stream)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDataStream>(stream, "stream");
			return stream.ReadData(contentManager);
		}
	}
}
