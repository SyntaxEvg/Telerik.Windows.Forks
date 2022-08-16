using System;

namespace Telerik.Windows.Documents.Spreadsheet.Maths.Number
{
	class TwoComplementNumber
	{
		public int NegativeBitIndex
		{
			get
			{
				return this.bitsCount - 1;
			}
		}

		long this[int bitIndex]
		{
			get
			{
				return (this.bitsArray >> bitIndex) & 1L;
			}
		}

		TwoComplementNumber(long bitsArray, int bitsCount)
		{
			if (!TwoComplementNumber.HasValidBitsCount(bitsCount))
			{
				throw new ArgumentException(TwoComplementNumber.bitsCountOutOfRangeMessage, "negativeBitIndex");
			}
			this.bitsCount = bitsCount;
			this.bitsArray = bitsArray;
			this.CalculateMaxBitIndex();
			if (this.maxBitIndex > this.NegativeBitIndex)
			{
				throw new ArgumentException(string.Format("Invalid bits array - the maxIndex of a non-zero bit must be equal to (bitsCount-1)!", new object[0]), "bitsArray");
			}
		}

		int CalculateMaxBitIndex()
		{
			this.maxBitIndex = -1;
			for (int i = 63; i >= 0; i--)
			{
				if (this[i] == 1L)
				{
					this.maxBitIndex = i;
					return this.maxBitIndex;
				}
			}
			return this.maxBitIndex;
		}

		static bool HasValidBitsCount(int bitsCount)
		{
			return 2 <= bitsCount && bitsCount <= 64;
		}

		public static TwoComplementNumber FromDec(long decNumber, int bitsCount)
		{
			long num;
			long num2;
			if (!TwoComplementNumber.TryGetDecRange(bitsCount, out num, out num2))
			{
				throw new ArgumentException(TwoComplementNumber.bitsCountOutOfRangeMessage, "negativeBitIndex");
			}
			if (decNumber > num2 || decNumber < num)
			{
				throw new ArgumentOutOfRangeException("decNumber must be in range corresponding to the given bitsCount", "decNumber");
			}
			if (bitsCount == 64 || decNumber >= 0L)
			{
				return new TwoComplementNumber(decNumber, bitsCount);
			}
			long num3 = decNumber - num;
			long num4 = 1L << bitsCount - 1;
			long num5 = num4 | num3;
			return new TwoComplementNumber(num5, bitsCount);
		}

		static bool TryGetDecRange(int bitsCount, out long minNumber, out long maxNumber)
		{
			if (bitsCount == 64)
			{
				minNumber = long.MinValue;
				maxNumber = long.MaxValue;
			}
			else
			{
				minNumber = -(1L << bitsCount - 1);
				maxNumber = -minNumber - 1L;
			}
			return TwoComplementNumber.HasValidBitsCount(bitsCount);
		}

		public static TwoComplementNumber FromBin(string binNumber, int bitsCount)
		{
			long num = Convert.ToInt64(binNumber, 2);
			return new TwoComplementNumber(num, bitsCount);
		}

		public static TwoComplementNumber FromOct(string hexNumber, int bitsCount)
		{
			long num = Convert.ToInt64(hexNumber, 8);
			return new TwoComplementNumber(num, bitsCount);
		}

		public static TwoComplementNumber FromHex(string hexNumber, int bitsCount)
		{
			long num = Convert.ToInt64(hexNumber, 16);
			return new TwoComplementNumber(num, bitsCount);
		}

		public long ToDec()
		{
			if (this.bitsCount == 64)
			{
				return this.bitsArray;
			}
			long num = this.bitsArray & (1L << this.NegativeBitIndex);
			long num2 = this.bitsArray & ~(1L << this.NegativeBitIndex);
			return num2 - num;
		}

		public string ToBin()
		{
			return Convert.ToString(this.bitsArray, 2);
		}

		public string ToOct()
		{
			return Convert.ToString(this.bitsArray, 8);
		}

		public string ToHex()
		{
			return Convert.ToString(this.bitsArray, 16).ToUpper();
		}

		public override string ToString()
		{
			return string.Format("TwoComplement: dec -> {0}, bin -> {1}, oct -> {2}, hex -> {3}", new object[]
			{
				this.ToDec(),
				this.ToBin(),
				this.ToOct(),
				this.ToHex()
			});
		}

		public const int MaxSupportedBits = 64;

		public const int MinSupportedBits = 2;

		static readonly string bitsCountOutOfRangeMessage = string.Format("bitsCount must be in range from {0} to {1}!", 2, 64);

		readonly long bitsArray;

		readonly int bitsCount;

		int maxBitIndex;
	}
}
