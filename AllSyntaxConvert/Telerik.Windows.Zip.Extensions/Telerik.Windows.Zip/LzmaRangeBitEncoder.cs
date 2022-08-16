using System;

namespace Telerik.Windows.Zip
{
	struct LzmaRangeBitEncoder
	{
		static LzmaRangeBitEncoder()
		{
			for (int i = 8; i >= 0; i--)
			{
				uint num = 1U << 9 - i - 1;
				uint num2 = 1U << 9 - i;
				for (uint num3 = num; num3 < num2; num3 += 1U)
				{
					LzmaRangeBitEncoder.prices[(int)((UIntPtr)num3)] = (uint)((i << 6) + (int)(num2 - num3 << 6 >> 9 - i - 1));
				}
			}
		}

		public void Init()
		{
			this.probability = 1024U;
		}

		public void Encode(LzmaRangeEncoder encoder, uint symbol)
		{
			uint num = (encoder.Range >> 11) * this.probability;
			if (symbol == 0U)
			{
				encoder.Range = num;
				this.probability += 2048U - this.probability >> 5;
			}
			else
			{
				encoder.Low += (ulong)num;
				encoder.Range -= num;
				this.probability -= this.probability >> 5;
			}
			if (encoder.Range < 16777216U)
			{
				encoder.Range <<= 8;
				encoder.ShiftLow();
			}
		}

		public uint GetPrice(uint symbol)
		{
			return LzmaRangeBitEncoder.prices[(int)(checked((IntPtr)((unchecked((ulong)(this.probability - symbol) ^ (ulong)((long)(-(long)symbol))) & 2047UL) >> 2)))];
		}

		public uint GetPrice0()
		{
			return LzmaRangeBitEncoder.prices[(int)((UIntPtr)(this.probability >> 2))];
		}

		public uint GetPrice1()
		{
			return LzmaRangeBitEncoder.prices[(int)((UIntPtr)(2048U - this.probability >> 2))];
		}

		public const int BitPriceShiftBits = 6;

		const int BitModelTotalBits = 11;

		const uint BitModelTotal = 2048U;

		const int MoveBitsNumber = 5;

		const int MoveReducingBitsNumber = 2;

		const int BitsNumber = 9;

		static uint[] prices = new uint[512];

		uint probability;
	}
}
