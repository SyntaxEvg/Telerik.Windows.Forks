using System;

namespace Telerik.Windows.Zip
{
	class InflateTree
	{
		static InflateTree()
		{
			InflateTree.StaticDistanceTree = new InflateTree(InflateTree.GetStaticDistanceTreeLength());
		}

		public InflateTree(byte[] codeLengths)
		{
			this.codeLengthArray = codeLengths;
			if (this.codeLengthArray.Length == 288)
			{
				this.tableBits = 9;
			}
			else
			{
				this.tableBits = 7;
			}
			this.tableBitMask = 1 << this.tableBits;
			this.tableMask = this.tableBitMask - 1;
			this.CreateTable();
		}

		public static InflateTree StaticDistanceTree { get; set; }

		public static InflateTree StaticLiteralLengthTree { get; set; } = new InflateTree(InflateTree.GetStaticLiteralTreeLength());

		public int GetNextSymbol(InputBitsBuffer input)
		{
			uint num = input.Get16Bits();
			if (input.AvailableBits == 0)
			{
				return -1;
			}
			int num2 = (int)this.table[(int)(checked((IntPtr)(unchecked((ulong)num & (ulong)((long)this.tableMask)))))];
			if (num2 < 0)
			{
				uint num3 = (uint)this.tableBitMask;
				do
				{
					num2 = -num2;
					num2 = (int)(((num & num3) != 0U) ? this.right[num2] : this.left[num2]);
					num3 <<= 1;
				}
				while (num2 < 0);
			}
			int num4 = (int)this.codeLengthArray[num2];
			if (num4 <= 0)
			{
				InflateTree.InvalidHuffmanData();
			}
			if (num4 > input.AvailableBits)
			{
				return -1;
			}
			input.SkipBits(num4);
			return num2;
		}

		static byte[] GetStaticDistanceTreeLength()
		{
			byte[] array = new byte[32];
			for (int i = 0; i < 32; i++)
			{
				array[i] = 5;
			}
			return array;
		}

		static byte[] GetStaticLiteralTreeLength()
		{
			byte[] array = new byte[288];
			for (int i = 0; i <= 143; i++)
			{
				array[i] = 8;
			}
			for (int j = 144; j <= 255; j++)
			{
				array[j] = 9;
			}
			for (int k = 256; k <= 279; k++)
			{
				array[k] = 7;
			}
			for (int l = 280; l <= 287; l++)
			{
				array[l] = 8;
			}
			return array;
		}

		static void InvalidHuffmanData()
		{
			throw new InvalidDataException("Invalid huffman data");
		}

		uint[] CalculateHuffmanCode()
		{
			uint[] array = new uint[17];
			foreach (int num in this.codeLengthArray)
			{
				array[num] += 1U;
			}
			array[0] = 0U;
			uint num2 = 0U;
			uint[] array3 = new uint[17];
			for (int j = 1; j <= 16; j++)
			{
				num2 = num2 + array[j - 1] << 1;
				array3[j] = num2;
			}
			uint[] array4 = new uint[288];
			for (int k = 0; k < this.codeLengthArray.Length; k++)
			{
				int num3 = (int)this.codeLengthArray[k];
				if (num3 > 0)
				{
					array4[k] = (uint)Tree.BitReverse((int)array3[num3], num3);
					array3[num3] += 1U;
				}
			}
			return array4;
		}

		void CreateTable()
		{
			short num = (short)this.codeLengthArray.Length;
			uint[] array = this.CalculateHuffmanCode();
			int num2 = this.codeLengthArray.Length * 2;
			this.table = new short[this.tableBitMask];
			this.left = new short[num2];
			this.right = new short[num2];
			for (int i = 0; i < this.codeLengthArray.Length; i++)
			{
				int num3 = (int)this.codeLengthArray[i];
				if (num3 > 0)
				{
					int start = (int)array[i];
					short code = (short)i;
					if (num3 > this.tableBits)
					{
						this.BuildTree(code, start, num3, ref num);
					}
					else
					{
						this.DuplicateCode(code, start, num3);
					}
				}
			}
		}

		void DuplicateCode(short code, int start, int length)
		{
			int num = 1 << length;
			if (start >= num)
			{
				InflateTree.InvalidHuffmanData();
			}
			int num2 = 1 << this.tableBits - length;
			for (int i = 0; i < num2; i++)
			{
				this.table[start] = code;
				start += num;
			}
		}

		void BuildTree(short code, int start, int length, ref short avail)
		{
			int num = length - this.tableBits;
			int num2 = this.tableBitMask;
			int num3 = start & this.tableMask;
			short[] array = this.table;
			do
			{
				short num4 = array[num3];
				if (num4 == 0)
				{
					num4 = (short)-avail;
					array[num3] = num4;
					avail += 1;
				}
				if (num4 > 0)
				{
					InflateTree.InvalidHuffmanData();
				}
				array = (((start & num2) == 0) ? this.left : this.right);
				num2 <<= 1;
				num3 = (int)(-(int)num4);
				num--;
			}
			while (num != 0);
			array[num3] = code;
		}

		byte[] codeLengthArray;

		short[] left;

		short[] right;

		short[] table;

		int tableBits;

		int tableMask;

		int tableBitMask;
	}
}
