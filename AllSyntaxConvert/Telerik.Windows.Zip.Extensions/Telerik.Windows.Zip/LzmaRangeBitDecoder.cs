using System;

namespace Telerik.Windows.Zip
{
	struct LzmaRangeBitDecoder
	{
		public void Init()
		{
			this.bitMask = 1024U;
		}

		public uint DecodeState(LzmaRangeDecoder rangeDecoder)
		{
			rangeDecoder.SaveState();
			uint result = this.Decode(rangeDecoder);
			if (rangeDecoder.InputRequired)
			{
				rangeDecoder.RestoreState();
			}
			return result;
		}

		public uint Decode(LzmaRangeDecoder rangeDecoder)
		{
			uint num = (rangeDecoder.Range >> 11) * this.bitMask;
			this.SaveState();
			uint result;
			if (rangeDecoder.Code < num)
			{
				this.bitMask += 2048U - this.bitMask >> 5;
				rangeDecoder.UpdateRange(num);
				result = 0U;
			}
			else
			{
				this.bitMask -= this.bitMask >> 5;
				rangeDecoder.MoveRange(num);
				result = 1U;
			}
			if (rangeDecoder.InputRequired)
			{
				this.RestoreState();
			}
			return result;
		}

		public void RestoreState()
		{
			this.bitMask = this.savedBitMask;
		}

		void SaveState()
		{
			this.savedBitMask = this.bitMask;
		}

		const int ModelTotalBits = 11;

		const uint ModelTotal = 2048U;

		const int MoveBits = 5;

		uint bitMask;

		uint savedBitMask;
	}
}
