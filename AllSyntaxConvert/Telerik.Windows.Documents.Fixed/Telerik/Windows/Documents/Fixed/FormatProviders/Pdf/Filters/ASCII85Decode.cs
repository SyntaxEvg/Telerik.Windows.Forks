using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	class ASCII85Decode : IPdfFilter
	{
		public byte[] Encode(PdfObject encodedObject, byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			int num = data.Length;
			int num2 = num / 4;
			int num3 = num - num2 * 4;
			byte[] array = new byte[num2 * 5 + ((num3 == 0) ? 0 : (num3 + 1)) + 2];
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < num2; i++)
			{
				uint num6 = (uint)(((int)data[num4++] << 24) + ((int)data[num4++] << 16) + ((int)data[num4++] << 8) + (int)data[num4++]);
				if (num6 == 0U)
				{
					array[num5++] = 122;
				}
				else
				{
					byte b = (byte)(num6 % 85U + 33U);
					num6 /= 85U;
					byte b2 = (byte)(num6 % 85U + 33U);
					num6 /= 85U;
					byte b3 = (byte)(num6 % 85U + 33U);
					num6 /= 85U;
					byte b4 = (byte)(num6 % 85U + 33U);
					num6 /= 85U;
					byte b5 = (byte)(num6 + 33U);
					array[num5++] = b5;
					array[num5++] = b4;
					array[num5++] = b3;
					array[num5++] = b2;
					array[num5++] = b;
				}
			}
			if (num3 == 1)
			{
				uint num7 = (uint)((uint)data[num4] << 24);
				num7 /= 614125U;
				byte b6 = (byte)(num7 % 85U + 33U);
				num7 /= 85U;
				byte b7 = (byte)(num7 + 33U);
				array[num5++] = b7;
				array[num5++] = b6;
			}
			else if (num3 == 2)
			{
				uint num8 = (uint)(((int)data[num4++] << 24) + ((int)data[num4] << 16));
				num8 /= 7225U;
				byte b8 = (byte)(num8 % 85U + 33U);
				num8 /= 85U;
				byte b9 = (byte)(num8 % 85U + 33U);
				num8 /= 85U;
				byte b10 = (byte)(num8 + 33U);
				array[num5++] = b10;
				array[num5++] = b9;
				array[num5++] = b8;
			}
			else if (num3 == 3)
			{
				uint num9 = (uint)(((int)data[num4++] << 24) + ((int)data[num4++] << 16) + ((int)data[num4] << 8));
				num9 /= 85U;
				byte b11 = (byte)(num9 % 85U + 33U);
				num9 /= 85U;
				byte b12 = (byte)(num9 % 85U + 33U);
				num9 /= 85U;
				byte b13 = (byte)(num9 % 85U + 33U);
				num9 /= 85U;
				byte b14 = (byte)(num9 + 33U);
				array[num5++] = b14;
				array[num5++] = b13;
				array[num5++] = b12;
				array[num5++] = b11;
			}
			array[num5++] = 126;
			array[num5++] = 62;
			if (num5 < array.Length)
			{
				Array.Resize<byte>(ref array, num5);
			}
			return array;
		}

		public byte[] Decode(PdfObject decodedObject, byte[] inputData, DecodeParameters parms)
		{
			int num = inputData.Length;
			int num2 = 0;
			int num3 = 0;
			int i;
			for (i = 0; i < num; i++)
			{
				char c = (char)inputData[i];
				if (c >= '!' && c <= 'u')
				{
					inputData[num3++] = (byte)c;
				}
				else if (c == 'z')
				{
					inputData[num3++] = (byte)c;
					num2++;
				}
				else if (c == '~')
				{
					if (inputData[i + 1] != 62)
					{
						throw new ArgumentException("Illegal character.", "data");
					}
					break;
				}
			}
			if (i == num)
			{
				throw new ArgumentException("Illegal character.", "data");
			}
			num = num3;
			int num4 = num - num2;
			int num5 = 4 * (num2 + num4 / 5);
			int num6 = num4 % 5;
			if (num6 == 1)
			{
				throw new InvalidOperationException("Illegal character.");
			}
			if (num6 != 0)
			{
				num5 += num6 - 1;
			}
			byte[] array = new byte[num5];
			num3 = 0;
			i = 0;
			while (i + 4 < num)
			{
				char c2 = (char)inputData[i];
				if (c2 == 'z')
				{
					i++;
					num3 += 4;
				}
				else
				{
					uint num7 = (uint)(inputData[i++] - 33) * 52200625U + (uint)(inputData[i++] - 33) * 614125U + (uint)(inputData[i++] - 33) * 7225U + (uint)((inputData[i++] - 33) * 85) + (uint)(inputData[i++] - 33);
					array[num3++] = (byte)(num7 >> 24);
					array[num3++] = (byte)(num7 >> 16);
					array[num3++] = (byte)(num7 >> 8);
					array[num3++] = (byte)num7;
				}
			}
			if (num6 == 2)
			{
				uint num8 = (uint)(inputData[i++] - 33) * 52200625U + (uint)(inputData[i] - 33) * 614125U;
				if (num8 != 0U)
				{
					num8 += 16777216U;
				}
				array[num3] = (byte)(num8 >> 24);
			}
			else if (num6 == 3)
			{
				int num9 = i;
				uint num10 = (uint)(inputData[i++] - 33) * 52200625U + (uint)(inputData[i++] - 33) * 614125U + (uint)(inputData[i] - 33) * 7225U;
				if (num10 != 0U)
				{
					num10 &= 4294901760U;
					uint num11 = num10 / 7225U;
					byte b = (byte)(num11 % 85U + 33U);
					num11 /= 85U;
					byte b2 = (byte)(num11 % 85U + 33U);
					num11 /= 85U;
					byte b3 = (byte)(num11 + 33U);
					if (b3 != inputData[num9] || b2 != inputData[num9 + 1] || b != inputData[num9 + 2])
					{
						num10 += 65536U;
					}
				}
				array[num3++] = (byte)(num10 >> 24);
				array[num3] = (byte)(num10 >> 16);
			}
			else if (num6 == 4)
			{
				int num12 = i;
				uint num13 = (uint)(inputData[i++] - 33) * 52200625U + (uint)(inputData[i++] - 33) * 614125U + (uint)(inputData[i++] - 33) * 7225U + (uint)((inputData[i] - 33) * 85);
				if (num13 != 0U)
				{
					num13 &= 4294967040U;
					uint num14 = num13 / 85U;
					byte b4 = (byte)(num14 % 85U + 33U);
					num14 /= 85U;
					byte b5 = (byte)(num14 % 85U + 33U);
					num14 /= 85U;
					byte b6 = (byte)(num14 % 85U + 33U);
					num14 /= 85U;
					byte b7 = (byte)(num14 + 33U);
					if (b7 != inputData[num12] || b6 != inputData[num12 + 1] || b5 != inputData[num12 + 2] || b4 != inputData[num12 + 3])
					{
						num13 += 256U;
					}
				}
				array[num3++] = (byte)(num13 >> 24);
				array[num3++] = (byte)(num13 >> 16);
				array[num3] = (byte)(num13 >> 8);
			}
			return array;
		}

		public string Name
		{
			get
			{
				return "ASCII85Decode";
			}
		}
	}
}
