using System;

namespace Telerik.Windows.Zip
{
	class LzmaBitTreeDecoder
	{
		public LzmaBitTreeDecoder(int levels)
		{
			this.bitLevels = levels;
			this.models = new LzmaRangeBitDecoder[1 << this.bitLevels];
		}

		public static uint ReverseDecode(LzmaRangeBitDecoder[] models, uint startIndex, LzmaRangeDecoder rangeDecoder, int numBitLevels)
		{
			uint num = 1U;
			uint num2 = 0U;
			if (rangeDecoder.CheckInputRequired(numBitLevels, false))
			{
				for (int i = 0; i < numBitLevels; i++)
				{
					uint num3 = models[(int)((UIntPtr)(startIndex + num))].Decode(rangeDecoder);
					if (rangeDecoder.InputRequired)
					{
						break;
					}
					num <<= 1;
					num += num3;
					num2 |= num3 << i;
				}
			}
			return num2;
		}

		public void Init()
		{
			int num = 1 << this.bitLevels;
			uint num2 = 1U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				this.models[(int)((UIntPtr)num2)].Init();
				num2 += 1U;
			}
		}

		public uint Decode(LzmaRangeDecoder rangeDecoder)
		{
			uint num = 1U;
			if (rangeDecoder.CheckInputRequired(this.bitLevels, false))
			{
				for (int i = this.bitLevels; i > 0; i--)
				{
					num = (num << 1) + this.models[(int)((UIntPtr)num)].Decode(rangeDecoder);
				}
			}
			return num - (1U << this.bitLevels);
		}

		public uint ReverseDecode(LzmaRangeDecoder rangeDecoder)
		{
			return LzmaBitTreeDecoder.ReverseDecode(this.models, 0U, rangeDecoder, this.bitLevels);
		}

		LzmaRangeBitDecoder[] models;

		int bitLevels;
	}
}
