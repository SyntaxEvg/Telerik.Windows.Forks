using System;

namespace JBig2Decoder
{
	class HuffmanDecoder
	{
		public HuffmanDecoder(Big2StreamReader reader)
		{
			this.reader = reader;
		}

		public DecodeIntResult decodeInt(long[,] table)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while (table[num3, 2] != HuffmanDecoder.jbig2HuffmanEOT)
			{
				while ((long)num < table[num3, 1])
				{
					int num4 = this.reader.readBit();
					num2 = (num2 << 1) | num4;
					num++;
				}
				if ((long)num2 == table[num3, 3])
				{
					if (table[num3, 2] == HuffmanDecoder.jbig2HuffmanOOB)
					{
						return new DecodeIntResult(-1L, false);
					}
					long intResult;
					if (table[num3, 2] == HuffmanDecoder.jbig2HuffmanLOW)
					{
						int num5 = this.reader.readBits(32L);
						intResult = table[num3, 0] - (long)num5;
					}
					else if (table[num3, 2] > 0L)
					{
						int num6 = this.reader.readBits(table[num3, 2]);
						intResult = table[num3, 0] + (long)num6;
					}
					else
					{
						intResult = table[num3, 0];
					}
					return new DecodeIntResult(intResult, true);
				}
				else
				{
					num3++;
				}
			}
			return new DecodeIntResult(-1L, false);
		}

		public DecodeIntResult decodeInt(long[][] table)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while (table[num3][2] != HuffmanDecoder.jbig2HuffmanEOT)
			{
				while ((long)num < table[num3][1])
				{
					int num4 = this.reader.readBit();
					num2 = (num2 << 1) | num4;
					num++;
				}
				if ((long)num2 == table[num3][3])
				{
					if (table[num3][2] == HuffmanDecoder.jbig2HuffmanOOB)
					{
						return new DecodeIntResult(-1L, false);
					}
					long intResult;
					if (table[num3][2] == HuffmanDecoder.jbig2HuffmanLOW)
					{
						int num5 = this.reader.readBits(32L);
						intResult = table[num3][0] - (long)num5;
					}
					else if (table[num3][2] > 0L)
					{
						int num6 = this.reader.readBits(table[num3][2]);
						intResult = table[num3][0] + (long)num6;
					}
					else
					{
						intResult = table[num3][0];
					}
					return new DecodeIntResult(intResult, true);
				}
				else
				{
					num3++;
				}
			}
			return new DecodeIntResult(-1L, false);
		}

		public static long[][] buildTable(long[][] table, int length)
		{
			long num;
			for (num = 0L; num < (long)length; num += 1L)
			{
				long num2 = num;
				while (num2 < (long)length && table[(int)(checked((IntPtr)num2))][1] == 0L)
				{
					num2 += 1L;
				}
				if (num2 == (long)length)
				{
					break;
				}
				for (long num3 = num2 + 1L; num3 < (long)length; num3 += 1L)
				{
					if (checked(table[(int)((IntPtr)num3)][1] > 0L && table[(int)((IntPtr)num3)][1] < table[(int)((IntPtr)num2)][1]))
					{
						num2 = num3;
					}
				}
				if (num2 != num)
				{
					long[] array = table[(int)(checked((IntPtr)num2))];
					for (long num3 = num2; num3 > num; num3 -= 1L)
					{
						checked
						{
							table[(int)((IntPtr)num3)] = table[(int)((IntPtr)(unchecked(num3 - 1L)))];
						}
					}
					table[(int)(checked((IntPtr)num))] = array;
				}
			}
			table[(int)(checked((IntPtr)num))] = table[length];
			num = 0L;
			long num4 = 0L;
			long num5 = num;
			num = num5 + 1L;
			long[] array2 = table[(int)(checked((IntPtr)num5))];
			int num6 = 3;
			long num7 = num4;
			num4 = num7 + 1L;
			array2[num6] = num7;
			while (table[(int)(checked((IntPtr)num))][2] != HuffmanDecoder.jbig2HuffmanEOT)
			{
				num4 <<= (int)(table[(int)(checked((IntPtr)num))][1] - table[(int)(checked((IntPtr)(unchecked(num - 1L))))][1]);
				long[] array3 = table[(int)(checked((IntPtr)num))];
				int num8 = 3;
				long num9 = num4;
				num4 = num9 + 1L;
				array3[num8] = num9;
				num += 1L;
			}
			return table;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static HuffmanDecoder()
		{
			long[,] array = new long[,]
			{
				{ 0L, 1L, 4L, 0L },
				{ 16L, 2L, 8L, 2L },
				{ 272L, 3L, 16L, 6L },
				{ 65808L, 3L, 32L, 7L },
				{ 0L, 0L, 0L, 0L }
			};
			array[4, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableA = array;
			long[,] array2 = new long[,]
			{
				{ 0L, 1L, 0L, 0L },
				{ 1L, 2L, 0L, 2L },
				{ 2L, 3L, 0L, 6L },
				{ 3L, 4L, 3L, 14L },
				{ 11L, 5L, 6L, 30L },
				{ 75L, 6L, 32L, 62L },
				{ 0L, 6L, 0L, 63L },
				{ 0L, 0L, 0L, 0L }
			};
			array2[6, 2] = HuffmanDecoder.jbig2HuffmanOOB;
			array2[7, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableB = array2;
			long[,] array3 = new long[,]
			{
				{ 0L, 1L, 0L, 0L },
				{ 1L, 2L, 0L, 2L },
				{ 2L, 3L, 0L, 6L },
				{ 3L, 4L, 3L, 14L },
				{ 11L, 5L, 6L, 30L },
				{ 0L, 6L, 0L, 62L },
				{ 75L, 7L, 32L, 254L },
				{ -256L, 8L, 8L, 254L },
				{ -257L, 8L, 0L, 255L },
				{ 0L, 0L, 0L, 0L }
			};
			array3[5, 2] = HuffmanDecoder.jbig2HuffmanOOB;
			array3[8, 2] = HuffmanDecoder.jbig2HuffmanLOW;
			array3[9, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableC = array3;
			long[,] array4 = new long[,]
			{
				{ 1L, 1L, 0L, 0L },
				{ 2L, 2L, 0L, 2L },
				{ 3L, 3L, 0L, 6L },
				{ 4L, 4L, 3L, 14L },
				{ 12L, 5L, 6L, 30L },
				{ 76L, 5L, 32L, 31L },
				{ 0L, 0L, 0L, 0L }
			};
			array4[6, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableD = array4;
			long[,] array5 = new long[,]
			{
				{ 1L, 1L, 0L, 0L },
				{ 2L, 2L, 0L, 2L },
				{ 3L, 3L, 0L, 6L },
				{ 4L, 4L, 3L, 14L },
				{ 12L, 5L, 6L, 30L },
				{ 76L, 6L, 32L, 62L },
				{ -255L, 7L, 8L, 126L },
				{ -256L, 7L, 0L, 127L },
				{ 0L, 0L, 0L, 0L }
			};
			array5[7, 2] = HuffmanDecoder.jbig2HuffmanLOW;
			array5[8, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableE = array5;
			long[,] array6 = new long[,]
			{
				{ 0L, 2L, 7L, 0L },
				{ 128L, 3L, 7L, 2L },
				{ 256L, 3L, 8L, 3L },
				{ -1024L, 4L, 9L, 8L },
				{ -512L, 4L, 8L, 9L },
				{ -256L, 4L, 7L, 10L },
				{ -32L, 4L, 5L, 11L },
				{ 512L, 4L, 9L, 12L },
				{ 1024L, 4L, 10L, 13L },
				{ -2048L, 5L, 10L, 28L },
				{ -128L, 5L, 6L, 29L },
				{ -64L, 5L, 5L, 30L },
				{ -2049L, 6L, 0L, 62L },
				{ 2048L, 6L, 32L, 63L },
				{ 0L, 0L, 0L, 0L }
			};
			array6[12, 2] = HuffmanDecoder.jbig2HuffmanLOW;
			array6[14, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableF = array6;
			long[,] array7 = new long[,]
			{
				{ -512L, 3L, 8L, 0L },
				{ 256L, 3L, 8L, 1L },
				{ 512L, 3L, 9L, 2L },
				{ 1024L, 3L, 10L, 3L },
				{ -1024L, 4L, 9L, 8L },
				{ -256L, 4L, 7L, 9L },
				{ -32L, 4L, 5L, 10L },
				{ 0L, 4L, 5L, 11L },
				{ 128L, 4L, 7L, 12L },
				{ -128L, 5L, 6L, 26L },
				{ -64L, 5L, 5L, 27L },
				{ 32L, 5L, 5L, 28L },
				{ 64L, 5L, 6L, 29L },
				{ -1025L, 5L, 0L, 30L },
				{ 2048L, 5L, 32L, 31L },
				{ 0L, 0L, 0L, 0L }
			};
			array7[13, 2] = HuffmanDecoder.jbig2HuffmanLOW;
			array7[15, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableG = array7;
			long[,] array8 = new long[,]
			{
				{ 0L, 2L, 1L, 0L },
				{ 0L, 2L, 0L, 1L },
				{ 4L, 3L, 4L, 4L },
				{ -1L, 4L, 0L, 10L },
				{ 22L, 4L, 4L, 11L },
				{ 38L, 4L, 5L, 12L },
				{ 2L, 5L, 0L, 26L },
				{ 70L, 5L, 6L, 27L },
				{ 134L, 5L, 7L, 28L },
				{ 3L, 6L, 0L, 58L },
				{ 20L, 6L, 1L, 59L },
				{ 262L, 6L, 7L, 60L },
				{ 646L, 6L, 10L, 61L },
				{ -2L, 7L, 0L, 124L },
				{ 390L, 7L, 8L, 125L },
				{ -15L, 8L, 3L, 252L },
				{ -5L, 8L, 1L, 253L },
				{ -7L, 9L, 1L, 508L },
				{ -3L, 9L, 0L, 509L },
				{ -16L, 9L, 0L, 510L },
				{ 1670L, 9L, 32L, 511L },
				{ 0L, 0L, 0L, 0L }
			};
			array8[1, 2] = HuffmanDecoder.jbig2HuffmanOOB;
			array8[19, 2] = HuffmanDecoder.jbig2HuffmanLOW;
			array8[21, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableH = array8;
			long[,] array9 = new long[,]
			{
				{ 0L, 2L, 0L, 0L },
				{ -1L, 3L, 1L, 2L },
				{ 1L, 3L, 1L, 3L },
				{ 7L, 3L, 5L, 4L },
				{ -3L, 4L, 1L, 10L },
				{ 43L, 4L, 5L, 11L },
				{ 75L, 4L, 6L, 12L },
				{ 3L, 5L, 1L, 26L },
				{ 139L, 5L, 7L, 27L },
				{ 267L, 5L, 8L, 28L },
				{ 5L, 6L, 1L, 58L },
				{ 39L, 6L, 2L, 59L },
				{ 523L, 6L, 8L, 60L },
				{ 1291L, 6L, 11L, 61L },
				{ -5L, 7L, 1L, 124L },
				{ 779L, 7L, 9L, 125L },
				{ -31L, 8L, 4L, 252L },
				{ -11L, 8L, 2L, 253L },
				{ -15L, 9L, 2L, 508L },
				{ -7L, 9L, 1L, 509L },
				{ -32L, 9L, 0L, 510L },
				{ 3339L, 9L, 32L, 511L },
				{ 0L, 0L, 0L, 0L }
			};
			array9[0, 2] = HuffmanDecoder.jbig2HuffmanOOB;
			array9[20, 2] = HuffmanDecoder.jbig2HuffmanLOW;
			array9[22, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableI = array9;
			long[,] array10 = new long[,]
			{
				{ -2L, 2L, 2L, 0L },
				{ 6L, 2L, 6L, 1L },
				{ 0L, 2L, 0L, 2L },
				{ -3L, 5L, 0L, 24L },
				{ 2L, 5L, 0L, 25L },
				{ 70L, 5L, 5L, 26L },
				{ 3L, 6L, 0L, 54L },
				{ 102L, 6L, 5L, 55L },
				{ 134L, 6L, 6L, 56L },
				{ 198L, 6L, 7L, 57L },
				{ 326L, 6L, 8L, 58L },
				{ 582L, 6L, 9L, 59L },
				{ 1094L, 6L, 10L, 60L },
				{ -21L, 7L, 4L, 122L },
				{ -4L, 7L, 0L, 123L },
				{ 4L, 7L, 0L, 124L },
				{ 2118L, 7L, 11L, 125L },
				{ -5L, 8L, 0L, 252L },
				{ 5L, 8L, 0L, 253L },
				{ -22L, 8L, 0L, 254L },
				{ 4166L, 8L, 32L, 255L },
				{ 0L, 0L, 0L, 0L }
			};
			array10[2, 2] = HuffmanDecoder.jbig2HuffmanOOB;
			array10[19, 2] = HuffmanDecoder.jbig2HuffmanLOW;
			array10[21, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableJ = array10;
			long[,] array11 = new long[,]
			{
				{ 1L, 1L, 0L, 0L },
				{ 2L, 2L, 1L, 2L },
				{ 4L, 4L, 0L, 12L },
				{ 5L, 4L, 1L, 13L },
				{ 7L, 5L, 1L, 28L },
				{ 9L, 5L, 2L, 29L },
				{ 13L, 6L, 2L, 60L },
				{ 17L, 7L, 2L, 122L },
				{ 21L, 7L, 3L, 123L },
				{ 29L, 7L, 4L, 124L },
				{ 45L, 7L, 5L, 125L },
				{ 77L, 7L, 6L, 126L },
				{ 141L, 7L, 32L, 127L },
				{ 0L, 0L, 0L, 0L }
			};
			array11[13, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableK = array11;
			long[,] array12 = new long[,]
			{
				{ 1L, 1L, 0L, 0L },
				{ 2L, 2L, 0L, 2L },
				{ 3L, 3L, 1L, 6L },
				{ 5L, 5L, 0L, 28L },
				{ 6L, 5L, 1L, 29L },
				{ 8L, 6L, 1L, 60L },
				{ 10L, 7L, 0L, 122L },
				{ 11L, 7L, 1L, 123L },
				{ 13L, 7L, 2L, 124L },
				{ 17L, 7L, 3L, 125L },
				{ 25L, 7L, 4L, 126L },
				{ 41L, 8L, 5L, 254L },
				{ 73L, 8L, 32L, 255L },
				{ 0L, 0L, 0L, 0L }
			};
			array12[13, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableL = array12;
			long[,] array13 = new long[,]
			{
				{ 1L, 1L, 0L, 0L },
				{ 2L, 3L, 0L, 4L },
				{ 7L, 3L, 3L, 5L },
				{ 3L, 4L, 0L, 12L },
				{ 5L, 4L, 1L, 13L },
				{ 4L, 5L, 0L, 28L },
				{ 15L, 6L, 1L, 58L },
				{ 17L, 6L, 2L, 59L },
				{ 21L, 6L, 3L, 60L },
				{ 29L, 6L, 4L, 61L },
				{ 45L, 6L, 5L, 62L },
				{ 77L, 7L, 6L, 126L },
				{ 141L, 7L, 32L, 127L },
				{ 0L, 0L, 0L, 0L }
			};
			array13[13, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableM = array13;
			long[,] array14 = new long[,]
			{
				{ 0L, 1L, 0L, 0L },
				{ -2L, 3L, 0L, 4L },
				{ -1L, 3L, 0L, 5L },
				{ 1L, 3L, 0L, 6L },
				{ 2L, 3L, 0L, 7L },
				{ 0L, 0L, 0L, 0L }
			};
			array14[5, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableN = array14;
			long[,] array15 = new long[,]
			{
				{ 0L, 1L, 0L, 0L },
				{ -1L, 3L, 0L, 4L },
				{ 1L, 3L, 0L, 5L },
				{ -2L, 4L, 0L, 12L },
				{ 2L, 4L, 0L, 13L },
				{ -4L, 5L, 1L, 28L },
				{ 3L, 5L, 1L, 29L },
				{ -8L, 6L, 2L, 60L },
				{ 5L, 6L, 2L, 61L },
				{ -24L, 7L, 4L, 124L },
				{ 9L, 7L, 4L, 125L },
				{ -25L, 7L, 0L, 126L },
				{ 25L, 7L, 32L, 127L },
				{ 0L, 0L, 0L, 0L }
			};
			array15[11, 2] = HuffmanDecoder.jbig2HuffmanLOW;
			array15[13, 2] = HuffmanDecoder.jbig2HuffmanEOT;
			HuffmanDecoder.huffmanTableO = array15;
		}

		public static long jbig2HuffmanLOW = (long)(-3);

		public static long jbig2HuffmanOOB = (long)(-2);

		public static long jbig2HuffmanEOT = (long)(-1);

		Big2StreamReader reader;

		public static long[,] huffmanTableA;

		public static long[,] huffmanTableB;

		public static long[,] huffmanTableC;

		public static long[,] huffmanTableD;

		public static long[,] huffmanTableE;

		public static long[,] huffmanTableF;

		public static long[,] huffmanTableG;

		public static long[,] huffmanTableH;

		public static long[,] huffmanTableI;

		public static long[,] huffmanTableJ;

		public static long[,] huffmanTableK;

		public static long[,] huffmanTableL;

		public static long[,] huffmanTableM;

		public static long[,] huffmanTableN;

		public static long[,] huffmanTableO;
	}
}
