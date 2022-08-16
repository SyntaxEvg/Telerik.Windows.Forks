using System;

namespace JBig2Decoder
{
	class ArithmeticDecoder
	{
		ArithmeticDecoder()
		{
		}

		public ArithmeticDecoder(Big2StreamReader reader)
		{
			this.reader = reader;
			this.genericRegionStats = new ArithmeticDecoderStats(2);
			this.refinementRegionStats = new ArithmeticDecoderStats(2);
			this.iadhStats = new ArithmeticDecoderStats(512);
			this.iadwStats = new ArithmeticDecoderStats(512);
			this.iaexStats = new ArithmeticDecoderStats(512);
			this.iaaiStats = new ArithmeticDecoderStats(512);
			this.iadtStats = new ArithmeticDecoderStats(512);
			this.iaitStats = new ArithmeticDecoderStats(512);
			this.iafsStats = new ArithmeticDecoderStats(512);
			this.iadsStats = new ArithmeticDecoderStats(512);
			this.iardxStats = new ArithmeticDecoderStats(512);
			this.iardyStats = new ArithmeticDecoderStats(512);
			this.iardwStats = new ArithmeticDecoderStats(512);
			this.iardhStats = new ArithmeticDecoderStats(512);
			this.iariStats = new ArithmeticDecoderStats(512);
			this.iaidStats = new ArithmeticDecoderStats(2);
		}

		public void resetIntStats(int symbolCodeLength)
		{
			this.iadhStats.reset();
			this.iadwStats.reset();
			this.iaexStats.reset();
			this.iaaiStats.reset();
			this.iadtStats.reset();
			this.iaitStats.reset();
			this.iafsStats.reset();
			this.iadsStats.reset();
			this.iardxStats.reset();
			this.iardyStats.reset();
			this.iardwStats.reset();
			this.iardhStats.reset();
			this.iariStats.reset();
			if (this.iaidStats.getContextSize() == 1 << symbolCodeLength + 1)
			{
				this.iaidStats.reset();
				return;
			}
			this.iaidStats = new ArithmeticDecoderStats(1 << symbolCodeLength + 1);
		}

		public void resetGenericStats(int template, ArithmeticDecoderStats previousStats)
		{
			int num = this.contextSize[template];
			if (previousStats != null && previousStats.getContextSize() == num)
			{
				if (this.genericRegionStats.getContextSize() == num)
				{
					this.genericRegionStats.overwrite(previousStats);
					return;
				}
				this.genericRegionStats = previousStats.copy();
				return;
			}
			else
			{
				if (this.genericRegionStats.getContextSize() == num)
				{
					this.genericRegionStats.reset();
					return;
				}
				this.genericRegionStats = new ArithmeticDecoderStats(1 << num);
				return;
			}
		}

		public void resetRefinementStats(int template, ArithmeticDecoderStats previousStats)
		{
			int num = this.referredToContextSize[template];
			if (previousStats != null && previousStats.getContextSize() == num)
			{
				if (this.refinementRegionStats.getContextSize() == num)
				{
					this.refinementRegionStats.overwrite(previousStats);
					return;
				}
				this.refinementRegionStats = previousStats.copy();
				return;
			}
			else
			{
				if (this.refinementRegionStats.getContextSize() == num)
				{
					this.refinementRegionStats.reset();
					return;
				}
				this.refinementRegionStats = new ArithmeticDecoderStats(1 << num);
				return;
			}
		}

		public void start()
		{
			this.buffer0 = (long)this.reader.readbyte();
			this.buffer1 = (long)this.reader.readbyte();
			this.c = BinaryOperation.bit32ShiftL(this.buffer0 ^ 255L, 16);
			this.readbyte();
			this.c = BinaryOperation.bit32ShiftL(this.c, 7);
			this.counter -= 7;
			this.a = (long)int.MinValue;
		}

		public DecodeIntResult decodeInt(ArithmeticDecoderStats stats)
		{
			this.previous = 1L;
			int num = this.decodeIntBit(stats);
			long num2;
			if (this.decodeIntBit(stats) != 0)
			{
				if (this.decodeIntBit(stats) != 0)
				{
					if (this.decodeIntBit(stats) != 0)
					{
						if (this.decodeIntBit(stats) != 0)
						{
							if (this.decodeIntBit(stats) != 0)
							{
								num2 = 0L;
								for (int i = 0; i < 32; i++)
								{
									num2 = BinaryOperation.bit32ShiftL(num2, 1) | (long)this.decodeIntBit(stats);
								}
								num2 += 4436L;
							}
							else
							{
								num2 = 0L;
								for (int j = 0; j < 12; j++)
								{
									num2 = BinaryOperation.bit32ShiftL(num2, 1) | (long)this.decodeIntBit(stats);
								}
								num2 += 340L;
							}
						}
						else
						{
							num2 = 0L;
							for (int k = 0; k < 8; k++)
							{
								num2 = BinaryOperation.bit32ShiftL(num2, 1) | (long)this.decodeIntBit(stats);
							}
							num2 += 84L;
						}
					}
					else
					{
						num2 = 0L;
						for (int l = 0; l < 6; l++)
						{
							num2 = BinaryOperation.bit32ShiftL(num2, 1) | (long)this.decodeIntBit(stats);
						}
						num2 += 20L;
					}
				}
				else
				{
					num2 = (long)this.decodeIntBit(stats);
					num2 = BinaryOperation.bit32ShiftL(num2, 1) | (long)this.decodeIntBit(stats);
					num2 = BinaryOperation.bit32ShiftL(num2, 1) | (long)this.decodeIntBit(stats);
					num2 = BinaryOperation.bit32ShiftL(num2, 1) | (long)this.decodeIntBit(stats);
					num2 += 4L;
				}
			}
			else
			{
				num2 = (long)this.decodeIntBit(stats);
				num2 = BinaryOperation.bit32ShiftL(num2, 1) | (long)this.decodeIntBit(stats);
			}
			int num3;
			if (num != 0)
			{
				if (num2 == 0L)
				{
					return new DecodeIntResult((long)((int)num2), false);
				}
				num3 = (int)(-(int)num2);
			}
			else
			{
				num3 = (int)num2;
			}
			return new DecodeIntResult((long)num3, true);
		}

		public long decodeIAID(long codeLen, ArithmeticDecoderStats stats)
		{
			this.previous = 1L;
			for (long num = 0L; num < codeLen; num += 1L)
			{
				int num2 = this.decodeBit(this.previous, stats);
				this.previous = BinaryOperation.bit32ShiftL(this.previous, 1) | (long)num2;
			}
			return this.previous - (1L << ((int)codeLen & 31));
		}

		public int decodeBit(long context, ArithmeticDecoderStats stats)
		{
			int num = BinaryOperation.bit8Shift(stats.getContextCodingTableValue((int)context), 1, 1);
			int num2 = stats.getContextCodingTableValue((int)context) & 1;
			int num3 = this.qeTable[num];
			this.a -= (long)num3;
			int result;
			if (this.c < this.a)
			{
				if ((this.a & -2147483648L )!= 0L)
				{
					result = num2;
				}
				else
				{
					if (this.a < (long)num3)
					{
						result = 1 - num2;
						if (this.switchTable[num] != 0)
						{
							stats.setContextCodingTableValue((int)context, (this.nlpsTable[num] << 1) | (1 - num2));
						}
						else
						{
							stats.setContextCodingTableValue((int)context, (this.nlpsTable[num] << 1) | num2);
						}
					}
					else
					{
						result = num2;
						stats.setContextCodingTableValue((int)context, (this.nmpsTable[num] << 1) | num2);
					}
					do
					{
						if (this.counter == 0)
						{
							this.readbyte();
						}
						this.a = BinaryOperation.bit32ShiftL(this.a, 1);
						this.c = BinaryOperation.bit32ShiftL(this.c, 1);
						this.counter--;
					}
					while ((this.a & (long)-2147483648) == 0L);
				}
			}
			else
			{
				this.c -= this.a;
				if (this.a < (long)num3)
				{
					result = num2;
					stats.setContextCodingTableValue((int)context, (this.nmpsTable[num] << 1) | num2);
				}
				else
				{
					result = 1 - num2;
					if (this.switchTable[num] != 0)
					{
						stats.setContextCodingTableValue((int)context, (this.nlpsTable[num] << 1) | (1 - num2));
					}
					else
					{
						stats.setContextCodingTableValue((int)context, (this.nlpsTable[num] << 1) | num2);
					}
				}
				this.a = (long)num3;
				do
				{
					if (this.counter == 0)
					{
						this.readbyte();
					}
					this.a = BinaryOperation.bit32ShiftL(this.a, 1);
					this.c = BinaryOperation.bit32ShiftL(this.c, 1);
					this.counter--;
				}
				while ((this.a & (long)-2147483648) == 0L);
			}
			return result;
		}

		void readbyte()
		{
			if (this.buffer0 != 255L)
			{
				this.buffer0 = this.buffer1;
				this.buffer1 = (long)this.reader.readbyte();
				this.c = this.c + 65280L - BinaryOperation.bit32ShiftL(this.buffer0, 8);
				this.counter = 8;
				return;
			}
			if (this.buffer1 > 143L)
			{
				this.counter = 8;
				return;
			}
			this.buffer0 = this.buffer1;
			this.buffer1 = (long)this.reader.readbyte();
			this.c = this.c + 65024L - BinaryOperation.bit32ShiftL(this.buffer0, 9);
			this.counter = 7;
		}

		int decodeIntBit(ArithmeticDecoderStats stats)
		{
			int num = this.decodeBit(this.previous, stats);
			if (this.previous < 256L)
			{
				this.previous = BinaryOperation.bit32ShiftL(this.previous, 1) | (long)num;
			}
			else
			{
				this.previous = ((BinaryOperation.bit32ShiftL(this.previous, 1) | (long)num) & 511L) | 256L;
			}
			return num;
		}

		Big2StreamReader reader;

		public ArithmeticDecoderStats genericRegionStats;

		public ArithmeticDecoderStats refinementRegionStats;

		public ArithmeticDecoderStats iadhStats;

		public ArithmeticDecoderStats iadwStats;

		public ArithmeticDecoderStats iaexStats;

		public ArithmeticDecoderStats iaaiStats;

		public ArithmeticDecoderStats iadtStats;

		public ArithmeticDecoderStats iaitStats;

		public ArithmeticDecoderStats iafsStats;

		public ArithmeticDecoderStats iadsStats;

		public ArithmeticDecoderStats iardxStats;

		public ArithmeticDecoderStats iardyStats;

		public ArithmeticDecoderStats iardwStats;

		public ArithmeticDecoderStats iardhStats;

		public ArithmeticDecoderStats iariStats;

		public ArithmeticDecoderStats iaidStats;

		int[] contextSize = new int[] { 16, 13, 10, 10 };

		int[] referredToContextSize = new int[] { 13, 10 };

		long buffer0;

		long buffer1;

		long c;

		long a;

		long previous;

		int counter;

		int[] qeTable = new int[]
		{
			1442906112, 872480768, 402718720, 180420608, 86048768, 35717120, 1442906112, 1409351680, 1208025088, 939589632,
			805371904, 604045312, 469827584, 369164288, 1442906112, 1409351680, 1359020032, 1208025088, 939589632, 872480768,
			805371904, 671154176, 604045312, 570490880, 469827584, 402718720, 369164288, 335609856, 302055424, 285278208,
			180420608, 163643392, 144769024, 86048768, 71368704, 44105728, 35717120, 21037056, 17891328, 8716288,
			4784128, 2424832, 1376256, 589824, 327680, 65536, 1442906112
		};

		int[] nmpsTable = new int[]
		{
			1, 2, 3, 4, 5, 38, 7, 8, 9, 10,
			11, 12, 13, 29, 15, 16, 17, 18, 19, 20,
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
			41, 42, 43, 44, 45, 45, 46
		};

		int[] nlpsTable = new int[]
		{
			1, 6, 9, 12, 29, 33, 6, 14, 14, 14,
			17, 18, 20, 21, 14, 14, 15, 16, 17, 18,
			19, 19, 20, 21, 22, 23, 24, 25, 26, 27,
			28, 29, 30, 31, 32, 33, 34, 35, 36, 37,
			38, 39, 40, 41, 42, 43, 46
		};

		int[] switchTable = new int[]
		{
			1, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0
		};
	}
}
