using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
	static class BytesHelper
	{
		public static byte GetDecimalFromHex(char hex)
		{
			if (hex >= '0' && hex <= '9')
			{
				return (byte)(hex - '0');
			}
			if (hex >= 'a' && hex <= 'f')
			{
				return (byte)(hex - 'a' + '\n');
			}
			return (byte)(hex - 'A' + '\n');
		}

		public static string ToHexString(byte[] data)
		{
			StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
			foreach (byte b in data)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}

		public static byte[] ToByteArray(string hex)
		{
			if (hex.Length % 2 == 1)
			{
				throw new Exception("The binary key cannot have an odd number of digits");
			}
			int num = hex.Length >> 1;
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (byte)((BytesHelper.GetHexVal(hex[i << 1]) << 4) + BytesHelper.GetHexVal(hex[(i << 1) + 1]));
			}
			return array;
		}

		public static int GetHexVal(char hex)
		{
			return (int)(hex - ((hex < ':') ? '0' : ((hex < 'a') ? '7' : 'W')));
		}

		public static int GetInt(byte[] bytes)
		{
			byte[] bytes2 = BytesHelper.GetBytes(bytes);
			if (bytes2 == null)
			{
				return -1;
			}
			int num = 0;
			int num2 = bytes2.Length;
			for (int i = 0; i < num2; i++)
			{
				num |= (int)((num2 <= i) ? 0 : (bytes2[i] & byte.MaxValue));
				if (i < num2 - 1)
				{
					num <<= 8;
				}
			}
			return num;
		}

		public static void GetBytesReverse(int value, byte[] result)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			for (int i = 0; i < bytes.Length; i++)
			{
				int num = result.Length - i - 1;
				if (num < 0)
				{
					break;
				}
				if (num >= result.Length)
				{
					return;
				}
				result[num] = bytes[i];
			}
		}

		public static byte[] MergeBytes(byte[] first, byte[] second)
		{
			LinkedList<byte> linkedList = new LinkedList<byte>();
			foreach (byte value in first)
			{
				linkedList.AddFirst(value);
			}
			foreach (byte value2 in second)
			{
				linkedList.AddLast(value2);
			}
			return linkedList.ToArray<byte>();
		}

		public static byte[] GetBytes(Stream stream)
		{
			if (stream.Position > 0L)
			{
				stream.Seek(0L, SeekOrigin.Begin);
			}
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return array;
		}

		public static void GetBytes(int value, byte[] result)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = bytes[i];
			}
		}

		static byte[] GetBytes(byte[] bytes)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			Guard.ThrowExceptionIfLessThanOrEqual<int>(0, bytes.Length, "bytes");
			Guard.ThrowExceptionIfGreaterThan<int>(4, bytes.Length, "bytes");
			int num = bytes.Length;
			int num2 = (num / 2 + num % 2) * 2;
			byte[] array = new byte[num2];
			if (num == 1)
			{
				array[0] = 0;
				array[1] = bytes[0];
			}
			else if (num2 == 2)
			{
				array[0] = bytes[0];
				array[1] = bytes[1];
			}
			else if (num == 3)
			{
				array[0] = 0;
				array[1] = bytes[0];
				array[2] = bytes[1];
				array[3] = bytes[2];
			}
			else
			{
				array[0] = bytes[0];
				array[1] = bytes[1];
				array[2] = bytes[2];
				array[3] = bytes[3];
			}
			return array;
		}
	}
}
