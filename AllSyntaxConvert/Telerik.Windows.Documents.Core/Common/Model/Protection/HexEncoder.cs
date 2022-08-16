using System;
using System.IO;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	class HexEncoder : IEncoder
	{
		static HexEncoder()
		{
			for (int i = 0; i < HexEncoder.encodingTable.Length; i++)
			{
				HexEncoder.DecodingTable[(int)HexEncoder.encodingTable[i]] = (byte)i;
			}
			HexEncoder.DecodingTable[65] = HexEncoder.DecodingTable[97];
			HexEncoder.DecodingTable[66] = HexEncoder.DecodingTable[98];
			HexEncoder.DecodingTable[67] = HexEncoder.DecodingTable[99];
			HexEncoder.DecodingTable[68] = HexEncoder.DecodingTable[100];
			HexEncoder.DecodingTable[69] = HexEncoder.DecodingTable[101];
			HexEncoder.DecodingTable[70] = HexEncoder.DecodingTable[102];
		}

		public int Encode(byte[] data, int off, int length, Stream outStream)
		{
			for (int i = off; i < off + length; i++)
			{
				int num = (int)data[i];
				outStream.WriteByte(HexEncoder.encodingTable[num >> 4]);
				outStream.WriteByte(HexEncoder.encodingTable[num & 15]);
			}
			return length * 2;
		}

		static bool Ignore(char c)
		{
			return c == '\n' || c == '\r' || c == '\t' || c == ' ';
		}

		public int Decode(byte[] data, int off, int length, Stream outStream)
		{
			int num = 0;
			int num2 = off + length;
			while (num2 > off && HexEncoder.Ignore((char)data[num2 - 1]))
			{
				num2--;
			}
			int i = off;
			while (i < num2)
			{
				while (i < num2 && HexEncoder.Ignore((char)data[i]))
				{
					i++;
				}
				byte b = HexEncoder.DecodingTable[(int)data[i++]];
				while (i < num2 && HexEncoder.Ignore((char)data[i]))
				{
					i++;
				}
				byte b2 = HexEncoder.DecodingTable[(int)data[i++]];
				outStream.WriteByte((byte)(((int)b << 4) | (int)b2));
				num++;
			}
			return num;
		}

		public int DecodeString(string data, Stream outStream)
		{
			int num = 0;
			int num2 = data.Length;
			while (num2 > 0 && HexEncoder.Ignore(data[num2 - 1]))
			{
				num2--;
			}
			int i = 0;
			while (i < num2)
			{
				while (i < num2 && HexEncoder.Ignore(data[i]))
				{
					i++;
				}
				byte b = HexEncoder.DecodingTable[(int)data[i++]];
				while (i < num2 && HexEncoder.Ignore(data[i]))
				{
					i++;
				}
				byte b2 = HexEncoder.DecodingTable[(int)data[i++]];
				outStream.WriteByte((byte)(((int)b << 4) | (int)b2));
				num++;
			}
			return num;
		}

		static readonly byte[] encodingTable = new byte[]
		{
			48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
			97, 98, 99, 100, 101, 102
		};

		internal static readonly byte[] DecodingTable = new byte[128];
	}
}
