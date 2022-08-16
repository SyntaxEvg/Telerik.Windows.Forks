using System;

namespace Telerik.Windows.Zip
{
	class LzmaLengthEncoder
	{
		public LzmaLengthEncoder()
		{
			for (uint num = 0U; num < 16U; num += 1U)
			{
				this.lowCoder[(int)((UIntPtr)num)] = new LzmaBitTreeEncoder(3);
				this.middleCoder[(int)((UIntPtr)num)] = new LzmaBitTreeEncoder(3);
			}
		}

		public void Init(uint posStates)
		{
			this.choice.Init();
			this.choice2.Init();
			for (uint num = 0U; num < posStates; num += 1U)
			{
				this.lowCoder[(int)((UIntPtr)num)].Init();
				this.middleCoder[(int)((UIntPtr)num)].Init();
			}
			this.highCoder.Init();
		}

		public void Encode(LzmaRangeEncoder rangeEncoder, uint symbol, uint posState)
		{
			if (symbol < 8U)
			{
				this.choice.Encode(rangeEncoder, 0U);
				this.lowCoder[(int)((UIntPtr)posState)].Encode(rangeEncoder, symbol);
				return;
			}
			symbol -= 8U;
			this.choice.Encode(rangeEncoder, 1U);
			if (symbol < 8U)
			{
				this.choice2.Encode(rangeEncoder, 0U);
				this.middleCoder[(int)((UIntPtr)posState)].Encode(rangeEncoder, symbol);
				return;
			}
			this.choice2.Encode(rangeEncoder, 1U);
			this.highCoder.Encode(rangeEncoder, symbol - 8U);
		}

		public void SetPrices(uint posState, uint symbolsCounter, uint[] prices, uint st)
		{
			uint price = this.choice.GetPrice0();
			uint price2 = this.choice.GetPrice1();
			uint num = price2 + this.choice2.GetPrice0();
			uint num2 = price2 + this.choice2.GetPrice1();
			uint num3;
			for (num3 = 0U; num3 < 8U; num3 += 1U)
			{
				if (num3 >= symbolsCounter)
				{
					return;
				}
				prices[(int)((UIntPtr)(st + num3))] = price + this.lowCoder[(int)((UIntPtr)posState)].GetPrice(num3);
			}
			while (num3 < 16U)
			{
				if (num3 >= symbolsCounter)
				{
					return;
				}
				prices[(int)((UIntPtr)(st + num3))] = num + this.middleCoder[(int)((UIntPtr)posState)].GetPrice(num3 - 8U);
				num3 += 1U;
			}
			while (num3 < symbolsCounter)
			{
				uint price3 = this.highCoder.GetPrice(num3 - 8U - 8U);
				prices[(int)((UIntPtr)(st + num3))] = num2 + price3;
				num3 += 1U;
			}
		}

		LzmaRangeBitEncoder choice = default(LzmaRangeBitEncoder);

		LzmaRangeBitEncoder choice2 = default(LzmaRangeBitEncoder);

		LzmaBitTreeEncoder[] lowCoder = new LzmaBitTreeEncoder[16];

		LzmaBitTreeEncoder[] middleCoder = new LzmaBitTreeEncoder[16];

		LzmaBitTreeEncoder highCoder = new LzmaBitTreeEncoder(8);
	}
}
