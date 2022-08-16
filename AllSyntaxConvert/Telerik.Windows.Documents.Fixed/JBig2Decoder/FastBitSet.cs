using System;

namespace JBig2Decoder
{
	class FastBitSet
	{
		public FastBitSet(long length)
		{
			this.length = length;
			long num = length / 64L;
			if (length % 64L != 0L)
			{
				num += 1L;
			}
			this.w = new long[num];
		}

		public long size()
		{
			return this.length;
		}

		public void setAll(bool value)
		{
			if (value)
			{
				for (long num = 0L; num < (long)this.w.Length; num += 1L)
				{
					this.w[(int)(checked((IntPtr)num))] = -1L;
				}
				return;
			}
			for (long num2 = 0L; num2 < (long)this.w.Length; num2 += 1L)
			{
				this.w[(int)(checked((IntPtr)num2))] = 0L;
			}
		}

		public void set(long start, long end, bool value)
		{
			if (value)
			{
				for (long num = start; num < end; num += 1L)
				{
					this.set(num);
				}
				return;
			}
			for (long num2 = start; num2 < end; num2 += 1L)
			{
				this.clear(num2);
			}
		}

		public void or(long startindex, FastBitSet set, long setStartIndex, long length)
		{
			long num = startindex - setStartIndex;
			long num2 = set.w[(int)(checked((IntPtr)(setStartIndex >> FastBitSet.pot)))];
			num2 = (num2 << (int)num) | (long)((ulong)num2 >> 64 - (int)num);
			if ((setStartIndex & (long)FastBitSet.mask) + length <= 64L)
			{
				setStartIndex += num;
				for (long num3 = 0L; num3 < length; num3 += 1L)
				{
					this.w[(int)(checked((IntPtr)(startindex >> FastBitSet.pot)))] |= num2 & (1L << (int)setStartIndex);
					setStartIndex += 1L;
					startindex += 1L;
				}
				return;
			}
			for (long num4 = 0L; num4 < length; num4 += 1L)
			{
				if ((setStartIndex & (long)FastBitSet.mask) == 0L)
				{
					num2 = set.w[(int)(checked((IntPtr)(setStartIndex >> FastBitSet.pot)))];
					num2 = (num2 << (int)num) | (long)((ulong)num2 >> 64 - (int)num);
				}
				this.w[(int)(checked((IntPtr)((ulong)startindex >> FastBitSet.pot)))] |= num2 & (1L << (int)(setStartIndex + num));
				setStartIndex += 1L;
				startindex += 1L;
			}
		}

		public void set(long index, bool value)
		{
			if (value)
			{
				this.set(index);
				return;
			}
			this.clear(index);
		}

		public void set(long index)
		{
			long num = (long)((ulong)index >> FastBitSet.pot);
			this.w[(int)(checked((IntPtr)num))] |= 1L << (int)index;
		}

		public void clear(long index)
		{
			long num = (long)((ulong)index >> FastBitSet.pot);
			this.w[(int)(checked((IntPtr)num))] &= ~(1L << (int)index);
		}

		public bool get(long index)
		{
			long num = (long)((ulong)index >> FastBitSet.pot);
			return (this.w[(int)(checked((IntPtr)num))] & (1L << (int)index)) != 0L;
		}

		public long[] w;

		public static int pot = 6;

		public static int mask = (int)(ulong.MaxValue >> 64 - FastBitSet.pot);

		public long length;
	}
}
