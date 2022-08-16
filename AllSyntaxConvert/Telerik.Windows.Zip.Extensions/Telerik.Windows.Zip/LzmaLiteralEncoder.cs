using System;

namespace Telerik.Windows.Zip
{
	class LzmaLiteralEncoder
	{
		public LzmaLiteralEncoder(int posBits, int previousBits)
		{
			this.positionBits = posBits;
			this.positionMask = (1U << posBits) - 1U;
			this.previousBits = previousBits;
			uint num = 1U << this.previousBits + this.positionBits;
			this.encoders = new LzmaLiteralEncoder.Encoder[num];
			for (uint num2 = 0U; num2 < num; num2 += 1U)
			{
				this.encoders[(int)((UIntPtr)num2)].Create();
			}
			for (uint num3 = 0U; num3 < num; num3 += 1U)
			{
				this.encoders[(int)((UIntPtr)num3)].Init();
			}
		}

		public LzmaLiteralEncoder.Encoder GetSubCoder(uint position, byte previousByte)
		{
			uint num = ((position & this.positionMask) << this.previousBits) + (uint)(previousByte >> 8 - this.previousBits);
			return this.encoders[(int)((UIntPtr)num)];
		}

		LzmaLiteralEncoder.Encoder[] encoders;

		int previousBits;

		int positionBits;

		uint positionMask;

		public struct Encoder
		{
			internal LzmaRangeBitEncoder[] Encoders { get; set; }

			public void Create()
			{
				this.Encoders = new LzmaRangeBitEncoder[768];
			}

			public void Init()
			{
				for (int i = 0; i < 768; i++)
				{
					this.Encoders[i].Init();
				}
			}

			public void Encode(LzmaRangeEncoder rangeEncoder, byte symbol)
			{
				uint num = 1U;
				for (int i = 7; i >= 0; i--)
				{
					uint num2 = (uint)((symbol >> i) & 1);
					this.Encoders[(int)((UIntPtr)num)].Encode(rangeEncoder, num2);
					num = (num << 1) | num2;
				}
			}

			public void EncodeMatched(LzmaRangeEncoder rangeEncoder, byte matchByte, byte symbol)
			{
				uint num = 1U;
				bool flag = true;
				for (int i = 7; i >= 0; i--)
				{
					uint num2 = (uint)((symbol >> i) & 1);
					uint num3 = num;
					if (flag)
					{
						uint num4 = (uint)((matchByte >> i) & 1);
						num3 += 1U + num4 << 8;
						flag = num4 == num2;
					}
					this.Encoders[(int)((UIntPtr)num3)].Encode(rangeEncoder, num2);
					num = (num << 1) | num2;
				}
			}

			public uint GetPrice(bool matchMode, byte matchByte, byte symbol)
			{
				uint num = 0U;
				uint num2 = 1U;
				int i = 7;
				if (matchMode)
				{
					while (i >= 0)
					{
						uint num3 = (uint)((matchByte >> i) & 1);
						uint num4 = (uint)((symbol >> i) & 1);
						num += this.Encoders[(int)((UIntPtr)((1U + num3 << 8) + num2))].GetPrice(num4);
						num2 = (num2 << 1) | num4;
						if (num3 != num4)
						{
							i--;
							break;
						}
						i--;
					}
				}
				while (i >= 0)
				{
					uint num5 = (uint)((symbol >> i) & 1);
					num += this.Encoders[(int)((UIntPtr)num2)].GetPrice(num5);
					num2 = (num2 << 1) | num5;
					i--;
				}
				return num;
			}
		}
	}
}
