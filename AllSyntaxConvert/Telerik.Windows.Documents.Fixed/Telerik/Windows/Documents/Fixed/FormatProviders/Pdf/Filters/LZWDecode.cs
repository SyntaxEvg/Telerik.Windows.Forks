using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	class LZWDecode : IPdfFilter
	{
		public string Name
		{
			get
			{
				return "LZWDecode";
			}
		}

		public byte[] Encode(PdfObject encodedObject, byte[] data)
		{
			throw new NotImplementedException("LZW encoding is not supported.");
		}

		public byte[] Decode(PdfObject decodedObject, byte[] inputData, DecodeParameters parms)
		{
			int earlyChange = LZWDecode.GetEarlyChange(parms);
			LzwDecoder lzwDecoder = new LzwDecoder(inputData, earlyChange);
			byte[] bytes = lzwDecoder.GetBytes();
			if (parms == null)
			{
				return bytes;
			}
			return Predictor.Decode(bytes, parms);
		}

		static int GetEarlyChange(DecodeParameters parms)
		{
			int result = 1;
			object obj;
			if (parms != null && parms.TryGetValue("EarlyChange", out obj))
			{
				result = (int)obj;
			}
			return result;
		}
	}
}
