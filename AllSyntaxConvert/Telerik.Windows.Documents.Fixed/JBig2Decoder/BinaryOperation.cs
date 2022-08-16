using System;

namespace JBig2Decoder
{
	class BinaryOperation
	{
		public static int getInt32(short[] number)
		{
			return ((int)number[0] << 24) | ((int)number[1] << 16) | ((int)number[2] << 8) | (int)number[3];
		}

		public static int getInt16(short[] number)
		{
			return ((int)number[0] << 8) | (int)number[1];
		}

		public static long bit32ShiftL(long number, int shift)
		{
			return number << shift;
		}

		public static long bit32ShiftR(long number, int shift)
		{
			return number >> shift;
		}

		public static int bit8Shift(int number, int shift, int direction)
		{
			if (direction == 0)
			{
				number <<= shift;
			}
			else
			{
				number >>= shift;
			}
			return number & 255;
		}

		public static int getInt32(byte[] number)
		{
			return ((int)number[0] << 24) | ((int)number[1] << 16) | ((int)number[2] << 8) | (int)number[3];
		}

		public const int LEFT_SHIFT = 0;

		public const int RIGHT_SHIFT = 1;

		public const long LONGMASK = 4294967295L;

		public const int INTMASK = 255;
	}
}
