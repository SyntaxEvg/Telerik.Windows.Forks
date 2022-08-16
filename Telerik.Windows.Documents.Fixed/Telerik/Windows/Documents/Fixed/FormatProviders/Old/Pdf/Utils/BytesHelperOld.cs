using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	static class BytesHelperOld
	{
		public static char GetUnicodeChar(int i)
		{
			return (char)i;
		}

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

		public static int GetInt(byte[] bytes, int offset, int count)
		{
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bytes[offset + i];
			}
			return BytesHelperOld.GetInt(array);
		}

		public static int GetInt(byte[] bytes)
		{
			byte[] bytes2 = BytesHelperOld.GetBytes(bytes);
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

		static byte[] GetBytes(byte[] bytes)
		{
			if (bytes == null || bytes.Length > 4)
			{
				return null;
			}
			if (bytes.Length == 0)
			{
				return bytes;
			}
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

		public static int ToInt16(byte[] bytes, int offset, int count)
		{
			if (bytes.Length <= offset)
			{
				return -1;
			}
			List<byte> list = new List<byte>();
			for (int i = 0; i < count; i++)
			{
				if (offset + i < bytes.Length)
				{
					list.Add(bytes[offset + i]);
				}
				else
				{
					list.Insert(0, 0);
				}
			}
			return BytesHelperOld.GetInt(list.ToArray());
		}

		public static int ToInt32(byte[] bytes, int offset, int count)
		{
			if (bytes.Length - 3 <= offset)
			{
				return -1;
			}
			List<byte> list = new List<byte>();
			for (int i = 0; i < count; i++)
			{
				if (offset + i < bytes.Length)
				{
					list.Add(bytes[offset + i]);
				}
				else
				{
					list.Insert(0, 0);
				}
			}
			return BytesHelperOld.GetInt(list.ToArray());
		}

		public static byte[] RemoveWhiteSpace(byte[] data)
		{
			int num = data.Length;
			int num2 = 0;
			int i = 0;
			while (i < num)
			{
				byte b = data[i];
				if (b == 0)
				{
					goto IL_38;
				}
				switch (b)
				{
				case 9:
				case 10:
				case 12:
				case 13:
					goto IL_38;
				case 11:
					break;
				default:
					if (b == 32)
					{
						goto IL_38;
					}
					break;
				}
				if (i != num2)
				{
					data[num2] = data[i];
				}
				IL_48:
				if (num2 < num)
				{
					byte[] array = data;
					data = new byte[num2];
					for (int j = 0; j < num2; j++)
					{
						data[j] = array[j];
					}
				}
				i++;
				num2++;
				continue;
				IL_38:
				num2--;
				goto IL_48;
			}
			return data;
		}

		public static void GetBytes(int b, byte[] output)
		{
			byte[] array = new byte[]
			{
				(byte)(b & 255),
				(byte)((b >> 8) & 255),
				(byte)((b >> 16) & 255),
				(byte)(b >> 24)
			};
			for (int i = 0; i < output.Length; i++)
			{
				output[i] = array[i];
			}
		}

		public const int CID_BYTES_COUNT = 4;
	}
}
