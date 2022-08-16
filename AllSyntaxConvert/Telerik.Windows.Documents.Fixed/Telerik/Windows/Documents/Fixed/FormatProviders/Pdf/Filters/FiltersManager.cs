using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	public static class FiltersManager
	{
		static FiltersManager()
		{
			FiltersManager.InitializeDefaultFilters();
		}

		public static void RegisterFilter(IPdfFilter filter)
		{
			FiltersManager.filters[filter.Name] = filter;
			FiltersManagerOld.RegisterFilter(filter);
		}

		internal static byte[] Encode(PdfObject encodeObject, byte[] data, PdfArray filters)
		{
			Guard.ThrowExceptionIfNull<PdfObject>(encodeObject, "encodeObject");
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			Guard.ThrowExceptionIfNull<PdfArray>(filters, "filters");
			foreach (PdfPrimitive pdfPrimitive in filters)
			{
				PdfName pdfName = (PdfName)pdfPrimitive;
				IPdfFilter filter = FiltersManager.GetFilter(pdfName.Value);
				data = filter.Encode(encodeObject, data);
			}
			return data;
		}

		internal static byte[] Decode(PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary, PdfStreamBase stream, PdfArray filters)
		{
			Guard.ThrowExceptionIfNull<PdfStreamBase>(stream, "stream");
			byte[] array = stream.ReadRawPdfData();
			if (filters != null)
			{
				DecodeParameters[] decodeParameters = stream.DecodeParameters;
				PdfObject obj = new PdfObject(reader, context, dictionary);
				string[] array2 = new string[filters.Count];
				for (int i = 0; i < array2.Length; i++)
				{
					PdfName pdfName;
					if (filters.TryGetElement<PdfName>(reader, context, i, out pdfName))
					{
						array2[i] = pdfName.Value;
					}
				}
				array = FiltersManager.Decode(obj, array, array2, decodeParameters);
			}
			return array;
		}

		internal static byte[] Decode(PdfObject obj, byte[] rawData, string[] filters, DecodeParameters[] decodeParameters)
		{
			byte[] array = rawData;
			if (filters != null)
			{
				for (int i = 0; i < filters.Length; i++)
				{
					string text = filters[i];
					if (text != null)
					{
						IPdfFilter filter = FiltersManager.GetFilter(text);
						array = filter.Decode(obj, array, (decodeParameters == null) ? null : decodeParameters[i]);
					}
				}
			}
			return array;
		}

		static IPdfFilter GetFilter(string name)
		{
			string nameFromAbbreviation = PdfNames.GetNameFromAbbreviation(name);
			IPdfFilter result;
			if (!FiltersManager.filters.TryGetValue(nameFromAbbreviation, out result))
			{
				throw new NotSupportedFilterException(nameFromAbbreviation);
			}
			return result;
		}

		static void InitializeDefaultFilters()
		{
			FiltersManager.RegisterFilter(new FlateDecode());
			FiltersManager.RegisterFilter(new DctDecode());
			FiltersManager.RegisterFilter(new ASCII85Decode());
			FiltersManager.RegisterFilter(new ASCIIHexDecode());
			FiltersManager.RegisterFilter(new LZWDecode());
			FiltersManager.RegisterFilter(new CCITTFaxDecode());
			FiltersManager.RegisterFilter(new RunLengthDecode());
			FiltersManager.RegisterFilter(new JBIG2Decode());
		}

		static readonly Dictionary<string, IPdfFilter> filters = new Dictionary<string, IPdfFilter>();
	}
}
