using System;

namespace Telerik.Windows.Zip
{
	class LzmaLiteralDecoder
	{
		public LzmaLiteralDecoder(int positionBits, int previousBits)
		{
			if (this.coders == null || this.numerPreviousBits != previousBits || this.numberPositionBits != positionBits)
			{
				this.numberPositionBits = positionBits;
				this.positionMask = (1U << positionBits) - 1U;
				this.numerPreviousBits = previousBits;
				uint num = 1U << this.numerPreviousBits + this.numberPositionBits;
				this.coders = new LzmaLiteralDecoder.InternalDecoder[num];
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					this.coders[(int)((UIntPtr)num2)].Create();
				}
			}
		}

		public void Init()
		{
			uint num = 1U << this.numerPreviousBits + this.numberPositionBits;
			for (uint num2 = 0U; num2 < num; num2 += 1U)
			{
				this.coders[(int)((UIntPtr)num2)].Init();
			}
		}

		public byte DecodeNormal(LzmaRangeDecoder rangeDecoder, uint position, byte previousByte)
		{
			return this.coders[(int)((UIntPtr)this.GetState(position, previousByte))].DecodeNormal(rangeDecoder);
		}

		public byte DecodeWithMatchByte(LzmaRangeDecoder rangeDecoder, uint pos, byte previousByte, byte matchByte)
		{
			return this.coders[(int)((UIntPtr)this.GetState(pos, previousByte))].DecodeWithMatchByte(rangeDecoder, matchByte);
		}

		uint GetState(uint position, byte previousByte)
		{
			return ((position & this.positionMask) << this.numerPreviousBits) + (uint)(previousByte >> 8 - this.numerPreviousBits);
		}

		LzmaLiteralDecoder.InternalDecoder[] coders;

		int numerPreviousBits;

		int numberPositionBits;

		uint positionMask;

		struct InternalDecoder
		{
			public void Create()
			{
				this.decoders = new LzmaRangeBitDecoder[768];
			}

			public void Init()
			{
				for (int i = 0; i < 768; i++)
				{
					this.decoders[i].Init();
				}
			}

			public byte DecodeNormal(LzmaRangeDecoder rangeDecoder)
			{
				uint num = 1U;
				do
				{
					num = (num << 1) | this.decoders[(int)((UIntPtr)num)].Decode(rangeDecoder);
				}
				while (num < 256U);
				return (byte)num;
			}

			public byte DecodeWithMatchByte(LzmaRangeDecoder rangeDecoder, byte matchByte)
			{
				uint num = 1U;
				for (;;)
				{
					uint num2 = (uint)((matchByte >> 7) & 1);
					matchByte = (byte)(matchByte << 1);
					uint num3 = this.decoders[(int)((UIntPtr)((1U + num2 << 8) + num))].Decode(rangeDecoder);
					num = (num << 1) | num3;
					if (num2 != num3)
					{
						break;
					}
					if (num >= 256U)
					{
						goto IL_5E;
					}
				}
				while (num < 256U)
				{
					num = (num << 1) | this.decoders[(int)((UIntPtr)num)].Decode(rangeDecoder);
				}
				IL_5E:
				return (byte)num;
			}

			LzmaRangeBitDecoder[] decoders;
		}
	}
}
