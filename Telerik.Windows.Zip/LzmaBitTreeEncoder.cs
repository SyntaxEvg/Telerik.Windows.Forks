using System;

namespace Telerik.Windows.Zip
{
	struct LzmaBitTreeEncoder
	{
		public LzmaBitTreeEncoder(int bitLevelsNumber)
		{
			this.bitLevels = bitLevelsNumber;
			this.models = new LzmaRangeBitEncoder[1 << this.bitLevels];
		}

		public static uint ReverseGetPrice(LzmaRangeBitEncoder[] models, uint startIndex, int bitLevelsNumber, uint symbol)
		{
			uint num = 0U;
			uint num2 = 1U;
			for (int i = bitLevelsNumber; i > 0; i--)
			{
				uint num3 = symbol & 1U;
				symbol >>= 1;
				num += models[(int)((UIntPtr)(startIndex + num2))].GetPrice(num3);
				num2 = (num2 << 1) | num3;
			}
			return num;
		}

		public static void ReverseEncode(LzmaRangeBitEncoder[] models, uint startIndex, LzmaRangeEncoder rangeEncoder, int bitLevelsNumber, uint symbol)
		{
			uint num = 1U;
			for (int i = 0; i < bitLevelsNumber; i++)
			{
				uint num2 = symbol & 1U;
				models[(int)((UIntPtr)(startIndex + num))].Encode(rangeEncoder, num2);
				num = (num << 1) | num2;
				symbol >>= 1;
			}
		}

		public void Init()
		{
			uint num = 1U;
			while ((ulong)num < (ulong)(1L << (this.bitLevels & 31)))
			{
				this.models[(int)((UIntPtr)num)].Init();
				num += 1U;
			}
		}

		public void Encode(LzmaRangeEncoder rangeEncoder, uint symbol)
		{
			uint num = 1U;
			int i = this.bitLevels;
			while (i > 0)
			{
				i--;
				uint num2 = (symbol >> i) & 1U;
				this.models[(int)((UIntPtr)num)].Encode(rangeEncoder, num2);
				num = (num << 1) | num2;
			}
		}

		public void ReverseEncode(LzmaRangeEncoder rangeEncoder, uint symbol)
		{
			uint num = 1U;
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)this.bitLevels))
			{
				uint num3 = symbol & 1U;
				this.models[(int)((UIntPtr)num)].Encode(rangeEncoder, num3);
				num = (num << 1) | num3;
				symbol >>= 1;
				num2 += 1U;
			}
		}

		public uint GetPrice(uint symbol)
		{
			uint num = 0U;
			uint num2 = 1U;
			int i = this.bitLevels;
			while (i > 0)
			{
				i--;
				uint num3 = (symbol >> i) & 1U;
				num += this.models[(int)((UIntPtr)num2)].GetPrice(num3);
				num2 = (num2 << 1) + num3;
			}
			return num;
		}

		public uint ReverseGetPrice(uint symbol)
		{
			uint num = 0U;
			uint num2 = 1U;
			for (int i = this.bitLevels; i > 0; i--)
			{
				uint num3 = symbol & 1U;
				symbol >>= 1;
				num += this.models[(int)((UIntPtr)num2)].GetPrice(num3);
				num2 = (num2 << 1) | num3;
			}
			return num;
		}

		LzmaRangeBitEncoder[] models;

		int bitLevels;
	}
}
