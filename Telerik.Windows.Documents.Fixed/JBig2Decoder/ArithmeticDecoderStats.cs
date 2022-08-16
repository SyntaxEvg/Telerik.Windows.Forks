using System;

namespace JBig2Decoder
{
	class ArithmeticDecoderStats
	{
		public ArithmeticDecoderStats(int contextSize)
		{
			this.contextSize = contextSize;
			this.codingContextTable = new int[contextSize];
		}

		public void reset()
		{
			for (int i = 0; i < this.contextSize; i++)
			{
				this.codingContextTable[i] = 0;
			}
		}

		public void setEntry(int codingContext, int i, int moreProbableSymbol)
		{
			this.codingContextTable[codingContext] = (i << i) + moreProbableSymbol;
		}

		public int getContextCodingTableValue(int index)
		{
			return this.codingContextTable[index];
		}

		public void setContextCodingTableValue(int index, int value)
		{
			this.codingContextTable[index] = value;
		}

		public int getContextSize()
		{
			return this.contextSize;
		}

		public void overwrite(ArithmeticDecoderStats stats)
		{
			Array.Copy(stats.codingContextTable, 0, this.codingContextTable, 0, this.contextSize);
		}

		public ArithmeticDecoderStats copy()
		{
			ArithmeticDecoderStats arithmeticDecoderStats = new ArithmeticDecoderStats(this.contextSize);
			Array.Copy(this.codingContextTable, 0, arithmeticDecoderStats.codingContextTable, 0, this.contextSize);
			return arithmeticDecoderStats;
		}

		int contextSize;

		int[] codingContextTable;
	}
}
