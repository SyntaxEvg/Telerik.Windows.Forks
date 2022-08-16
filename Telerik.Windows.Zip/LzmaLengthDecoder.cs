using System;

namespace Telerik.Windows.Zip
{
	class LzmaLengthDecoder
	{
		public LzmaLengthDecoder(uint positionStates)
		{
			for (uint num = this.numberPositionStates; num < positionStates; num += 1U)
			{
				this.lowCoder[(int)((UIntPtr)num)] = new LzmaBitTreeDecoder(3);
				this.middleCoder[(int)((UIntPtr)num)] = new LzmaBitTreeDecoder(3);
			}
			this.numberPositionStates = positionStates;
		}

		public void Init()
		{
			this.choice.Init();
			for (uint num = 0U; num < this.numberPositionStates; num += 1U)
			{
				this.lowCoder[(int)((UIntPtr)num)].Init();
				this.middleCoder[(int)((UIntPtr)num)].Init();
			}
			this.choice2.Init();
			this.highCoder.Init();
		}

		public uint Decode(LzmaRangeDecoder rangeDecoder, uint positionState)
		{
			rangeDecoder.SaveState();
			uint num = this.choice.Decode(rangeDecoder);
			if (rangeDecoder.InputRequired)
			{
				return 0U;
			}
			uint num2;
			if (num == 0U)
			{
				num2 = this.lowCoder[(int)((UIntPtr)positionState)].Decode(rangeDecoder);
			}
			else
			{
				num2 = 8U;
				num = this.choice2.Decode(rangeDecoder);
				if (!rangeDecoder.InputRequired)
				{
					if (num == 0U)
					{
						num2 += this.middleCoder[(int)((UIntPtr)positionState)].Decode(rangeDecoder);
					}
					else
					{
						num2 += 8U;
						num2 += this.highCoder.Decode(rangeDecoder);
					}
				}
				if (rangeDecoder.InputRequired)
				{
					this.choice2.RestoreState();
				}
			}
			if (rangeDecoder.InputRequired)
			{
				this.choice.RestoreState();
				rangeDecoder.RestoreState();
			}
			return num2;
		}

		LzmaRangeBitDecoder choice = default(LzmaRangeBitDecoder);

		LzmaRangeBitDecoder choice2 = default(LzmaRangeBitDecoder);

		LzmaBitTreeDecoder[] lowCoder = new LzmaBitTreeDecoder[16];

		LzmaBitTreeDecoder[] middleCoder = new LzmaBitTreeDecoder[16];

		LzmaBitTreeDecoder highCoder = new LzmaBitTreeDecoder(8);

		uint numberPositionStates;
	}
}
