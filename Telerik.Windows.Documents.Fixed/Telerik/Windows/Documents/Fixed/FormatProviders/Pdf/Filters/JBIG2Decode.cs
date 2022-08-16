using System;
using JBig2Decoder;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	class JBIG2Decode : IPdfFilter
	{
		public string Name
		{
			get
			{
				return "JBIG2Decode";
			}
		}

		public byte[] Encode(PdfObject encodedObject, byte[] inputData)
		{
			throw new NotImplementedException();
		}

		public byte[] Decode(PdfObject decodedObject, byte[] inputData, DecodeParameters parms)
		{
			JBIG2StreamDecoder jbig2StreamDecoder = new JBIG2StreamDecoder();
			object obj;
			byte[] array2;
			if (parms != null && parms.TryGetValue("JBIG2Globals", out obj))
			{
				byte[] array = (byte[])obj;
				array2 = new byte[array.Length + inputData.Length];
				array.CopyTo(array2, 0);
				inputData.CopyTo(array2, array.Length);
			}
			else
			{
				array2 = inputData;
			}
			return jbig2StreamDecoder.decodeJBIG2(array2);
		}

		const string JBIG2GlobalsName = "JBIG2Globals";
	}
}
