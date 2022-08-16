using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Filters
{
	class FiltersManagerOld
	{
		static FiltersManagerOld()
		{
			FiltersManagerOld.InitializeFilters();
		}

		public static void RegisterFilter(IPdfFilter filter)
		{
			FiltersManagerOld.filters[filter.Name] = filter;
		}

		internal static byte[] Decode(PdfDictionaryOld dictionary, PdfNameOld[] filters, byte[] data, DecodeParameters[] decodeParams)
		{
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			Guard.ThrowExceptionIfNull<PdfNameOld[]>(filters, "filters");
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			PdfObject decodedObject = new PdfObject(dictionary);
			DecodeParameters decodeParameters = ((decodeParams == null) ? null : decodeParams[0]);
			if (filters.Length == 0)
			{
				return data;
			}
			IPdfFilter pdfFilter = FiltersManagerOld.CreateFilter(filters[0].Value);
			int num = 1;
			for (;;)
			{
				data = pdfFilter.Decode(decodedObject, data, decodeParameters);
				if (num == filters.Length)
				{
					break;
				}
				decodeParameters = ((decodeParams == null) ? null : decodeParams[num]);
				pdfFilter = FiltersManagerOld.CreateFilter(filters[num].Value);
				num++;
			}
			return data;
		}

		internal static PdfNameOld[] GetFilters(PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			object value;
			if (!dictionary.TryGetValue("Filter", out value))
			{
				return new PdfNameOld[0];
			}
			PdfNameOld[] array = (PdfNameOld[])Converters.FiltersConverter.Convert(typeof(PdfNameOld[]), contentManager, value);
			if (array == null)
			{
				return new PdfNameOld[0];
			}
			return array;
		}

		internal static string[] GetFilterValues(PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			PdfNameOld[] array = FiltersManagerOld.GetFilters(contentManager, dictionary);
			string[] array2 = null;
			if (array != null)
			{
				array2 = new string[array.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					PdfNameOld pdfNameOld = array[i];
					string nameFromAbbreviation = PdfNames.GetNameFromAbbreviation(pdfNameOld.Value);
					array2[i] = nameFromAbbreviation;
				}
			}
			return array2;
		}

		internal static DecodeParameters[] GetDecodeParameters(PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			object value;
			if (!dictionary.TryGetValue("DecodeParms", out value))
			{
				return null;
			}
			return (DecodeParameters[])Converters.DecodeParametersConverter.Convert(typeof(DecodeParameters[]), contentManager, value);
		}

		static void InitializeFilters()
		{
			FlateDecode flateDecode = new FlateDecode();
			DctDecode dctDecode = new DctDecode();
			ASCII85Decode ascii85Decode = new ASCII85Decode();
			ASCIIHexDecode asciihexDecode = new ASCIIHexDecode();
			LZWDecode lzwdecode = new LZWDecode();
			CCITTFaxDecode ccittfaxDecode = new CCITTFaxDecode();
			RunLengthDecode runLengthDecode = new RunLengthDecode();
			JBIG2Decode jbig2Decode = new JBIG2Decode();
			FiltersManagerOld.filters[flateDecode.Name] = flateDecode;
			FiltersManagerOld.filters[dctDecode.Name] = dctDecode;
			FiltersManagerOld.filters[ascii85Decode.Name] = ascii85Decode;
			FiltersManagerOld.filters[asciihexDecode.Name] = asciihexDecode;
			FiltersManagerOld.filters[lzwdecode.Name] = lzwdecode;
			FiltersManagerOld.filters[ccittfaxDecode.Name] = ccittfaxDecode;
			FiltersManagerOld.filters[runLengthDecode.Name] = runLengthDecode;
			FiltersManagerOld.filters[jbig2Decode.Name] = jbig2Decode;
		}

		static IPdfFilter CreateFilter(string filterName)
		{
			filterName = PdfNames.GetNameFromAbbreviation(filterName);
			IPdfFilter result;
			if (FiltersManagerOld.filters.TryGetValue(filterName, out result))
			{
				return result;
			}
			throw new NotSupportedFilterException(filterName);
		}

		static readonly Dictionary<string, IPdfFilter> filters = new Dictionary<string, IPdfFilter>();
	}
}
