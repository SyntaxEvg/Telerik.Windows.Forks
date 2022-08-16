using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	static class Predictor
	{
		internal static byte[] Decode(byte[] data, DecodeParameters decodeParameters)
		{
			int num = 1;
			object obj;
			if (decodeParameters.TryGetValue("Predictor", out obj))
			{
				num = (int)obj;
			}
			int colors = 1;
			if (decodeParameters.TryGetValue("Colors", out obj))
			{
				colors = (int)obj;
			}
			int bpc = 8;
			if (decodeParameters.TryGetValue("BitsPerComponent", out obj))
			{
				bpc = (int)obj;
			}
			int columns = 1;
			if (decodeParameters.TryGetValue("Columns", out obj))
			{
				columns = (int)obj;
			}
			switch (num)
			{
			case 1:
				return data;
			case 2:
				return Predictor.TiffPredictor(data, colors, columns);
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
				return Predictor.PngPretictor(data, columns, colors, bpc);
			}
			return null;
		}

		static byte[] TiffPredictor(byte[] data, int colors, int columns)
		{
			int num = data.Length / (columns * colors);
			for (int i = 0; i < num; i++)
			{
				int num2 = colors * (i * columns + 1);
				for (int j = colors; j < columns * colors; j++)
				{
					data[num2] += data[num2 - colors];
					num2++;
				}
			}
			return data;
		}

		static byte[] PngPretictor(byte[] data, int columns, int colors, int bpc)
		{
			List<byte[]> list = new List<byte[]>();
			int num = columns * colors * bpc;
			num = (int)Math.Ceiling((double)num / 8.0);
			int sub = (int)Math.Ceiling((double)(bpc * colors) / 8.0);
			byte[] prevLine = null;
			ByteReader byteReader = new ByteReader(data);
			while (byteReader.Remaining >= num + 1)
			{
				int num2 = (int)(byteReader.ReadByte() & byte.MaxValue);
				byte[] array = new byte[num];
				byteReader.Read(array);
				switch (num2)
				{
				case 1:
					Predictor.Sub(array, sub);
					break;
				case 2:
					Predictor.Up(array, prevLine);
					break;
				case 3:
					Predictor.Average(array, prevLine, sub);
					break;
				case 4:
					Predictor.Paeth(array, prevLine, sub);
					break;
				}
				list.Add(array);
				prevLine = array;
			}
			List<byte> list2 = new List<byte>();
			foreach (byte[] collection in list)
			{
				list2.AddRange(collection);
			}
			return list2.ToArray();
		}

		static void Sub(byte[] curLine, int sub)
		{
			for (int i = 0; i < curLine.Length; i++)
			{
				int num = i - sub;
				if (num >= 0)
				{
					int num2 = i;
					curLine[num2] += curLine[num];
				}
			}
		}

		static void Up(byte[] curLine, byte[] prevLine)
		{
			if (prevLine == null)
			{
				return;
			}
			for (int i = 0; i < curLine.Length; i++)
			{
				int num = i;
				curLine[num] += prevLine[i];
			}
		}

		static void Average(byte[] curLine, byte[] prevLine, int sub)
		{
			for (int i = 0; i < curLine.Length; i++)
			{
				int num = 0;
				int num2 = 0;
				int num3 = i - sub;
				if (num3 >= 0)
				{
					num = (int)(curLine[num3] & byte.MaxValue);
				}
				if (prevLine != null)
				{
					num2 = (int)(prevLine[i] & byte.MaxValue);
				}
				int num4 = i;
				curLine[num4] += (byte)Math.Floor((double)(num + num2) / 2.0);
			}
		}

		static void Paeth(byte[] curLine, byte[] prevLine, int sub)
		{
			for (int i = 0; i < curLine.Length; i++)
			{
				int left = 0;
				int up = 0;
				int upLeft = 0;
				int num = i - sub;
				if (num >= 0)
				{
					left = (int)(curLine[num] & byte.MaxValue);
				}
				if (prevLine != null)
				{
					up = (int)(prevLine[i] & byte.MaxValue);
				}
				if (num > 0 && prevLine != null)
				{
					upLeft = (int)(prevLine[num] & byte.MaxValue);
				}
				int num2 = i;
				curLine[num2] += (byte)Predictor.Paeth(left, up, upLeft);
			}
		}

		static int Paeth(int left, int up, int upLeft)
		{
			int num = left + up - upLeft;
			int num2 = Math.Abs(num - left);
			int num3 = Math.Abs(num - up);
			int num4 = Math.Abs(num - upLeft);
			if (num2 <= num3 && num2 <= num4)
			{
				return left;
			}
			if (num3 <= num4)
			{
				return up;
			}
			return upLeft;
		}
	}
}
