using System;
using System.IO;
using Telerik.Windows.Documents.Utilities;
using Telerik.Windows.Zip;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	public class FlateDecode : IPdfFilter
	{
		public string Name
		{
			get
			{
				return "FlateDecode";
			}
		}

		public byte[] Decode(PdfObject decodedObject, byte[] data, DecodeParameters parms)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			if (data.Length == 0)
			{
				return data;
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				byte[] array = null;
				using (CompressedStream compressedStream = new CompressedStream(memoryStream, StreamOperationMode.Read, new DeflateSettings()))
				{
					array = compressedStream.ReadAllBytes();
				}
				if (parms != null)
				{
					array = Predictor.Decode(array, parms);
				}
				result = array;
			}
			return result;
		}

		public byte[] Encode(PdfObject encodedObject, byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CompressedStream compressedStream = new CompressedStream(memoryStream, StreamOperationMode.Write, new DeflateSettings()))
				{
					compressedStream.Write(data, 0, data.Length);
				}
				result = memoryStream.ReadAllBytes();
			}
			return result;
		}
	}
}
