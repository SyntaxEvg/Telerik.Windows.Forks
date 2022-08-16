using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	class ASCIIHexDecode : IPdfFilter
	{
		public byte[] Encode(PdfObject encodedObject, byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			int num = data.Length;
			byte[] array = new byte[2 * num];
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				byte b = data[i];
				array[num2++] = (byte)((b >> 4) + ((b >> 4 < 10) ? 48 : 55));
				array[num2++] = (b & 15) + (((b & 15) < 10) ? 48 : 55);
				i++;
			}
			return array;
		}

		public byte[] Decode(PdfObject decodedObject, Stream stream, DecodeParameters parms)
		{
			throw new NotSupportedException();
		}

		public string Name
		{
			get
			{
				return "ASCIIHexDecode";
			}
		}

		public byte[] Decode(PdfObject decodedObject, byte[] inputData, DecodeParameters parms)
		{
			inputData = ASCIIHexDecode.RemoveWhiteSpace(inputData);
			List<byte> list = new List<byte>(inputData.Length);
			int num = 0;
			byte hex;
			while (num < inputData.Length && (hex = inputData[num++]) != 62)
			{
				byte b = inputData[num++];
				if (b == 62)
				{
					b = 48;
				}
				byte decimalFromHex = BytesHelper.GetDecimalFromHex((char)hex);
				byte decimalFromHex2 = BytesHelper.GetDecimalFromHex((char)b);
				byte item = (byte)(((int)decimalFromHex << 4) | (int)decimalFromHex2);
				list.Add(item);
			}
			return list.ToArray();
		}

		static byte[] RemoveWhiteSpace(byte[] data)
		{
			List<byte> list = new List<byte>(data.Length);
			for (int i = 0; i < data.Length; i++)
			{
				if (!char.IsWhiteSpace((char)data[i]))
				{
					list.Add(data[i]);
				}
			}
			return list.ToArray();
		}

		const byte Eod = 62;
	}
}
