using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	class RunLengthDecode : IPdfFilter
	{
		public byte[] Encode(PdfObject encodedObject, byte[] inputData)
		{
			throw new NotImplementedException();
		}

		public byte[] Decode(PdfObject decodedObject, byte[] inputData, DecodeParameters decodeParameters)
		{
			LinkedList<byte> linkedList = new LinkedList<byte>();
			int num = 0;
			byte b;
			while (num < inputData.Length && (b = inputData[num++]) != 128)
			{
				if (b >= 0 && b <= 127)
				{
					for (int i = 0; i <= (int)b; i++)
					{
						linkedList.AddLast(inputData[num++]);
					}
				}
				else
				{
					for (int j = 0; j < 257 - (int)b; j++)
					{
						linkedList.AddLast(inputData[num]);
					}
					num++;
				}
			}
			return linkedList.ToArray<byte>();
		}

		public string Name
		{
			get
			{
				return "RunLengthDecode";
			}
		}

		const byte Eod = 128;
	}
}
