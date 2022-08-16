using System;

namespace Org.BouncyCastle.Security
{
	class SecureRandom : Random
	{
		public static byte[] GetNextBytes(SecureRandom secureRandom, int length)
		{
			byte[] array = new byte[length];
			secureRandom.NextBytes(array);
			return array;
		}

		public override int Next()
		{
			return this.NextInt() & int.MaxValue;
		}

		public override int Next(int maxValue)
		{
			if (maxValue < 2)
			{
				if (maxValue < 0)
				{
					throw new ArgumentOutOfRangeException("maxValue", "cannot be negative");
				}
				return 0;
			}
			else
			{
				if ((maxValue & (maxValue - 1)) == 0)
				{
					return this.NextInt() & (maxValue - 1);
				}
				int num;
				int num2;
				do
				{
					num = this.NextInt() & int.MaxValue;
					num2 = num % maxValue;
				}
				while (num - num2 + (maxValue - 1) < 0);
				return num2;
			}
		}

		public override int Next(int minValue, int maxValue)
		{
			if (maxValue <= minValue)
			{
				if (maxValue == minValue)
				{
					return minValue;
				}
				throw new ArgumentException("maxValue cannot be less than minValue");
			}
			else
			{
				int num = maxValue - minValue;
				if (num > 0)
				{
					return minValue + this.Next(num);
				}
				int num2;
				do
				{
					num2 = this.NextInt();
				}
				while (num2 < minValue || num2 >= maxValue);
				return num2;
			}
		}

		public override double NextDouble()
		{
			return Convert.ToDouble((ulong)this.NextLong()) / SecureRandom.DoubleScale;
		}

		public virtual int NextInt()
		{
			byte[] array = new byte[4];
			this.NextBytes(array);
			uint num = (uint)array[0];
			num <<= 8;
			num |= (uint)array[1];
			num <<= 8;
			num |= (uint)array[2];
			num <<= 8;
			return (int)(num | (uint)array[3]);
		}

		public virtual long NextLong()
		{
			return (long)(((ulong)this.NextInt() << 32) | (ulong)this.NextInt());
		}

		private static readonly double DoubleScale = global::System.Math.Pow(2.0, 64.0);
	}
}
