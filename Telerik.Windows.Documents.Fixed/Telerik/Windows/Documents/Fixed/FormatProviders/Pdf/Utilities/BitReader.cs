using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
	class BitReader
	{
		protected byte[] Source { get; set; }

		protected int SourcePointer { get; set; }

		protected int BitPointer { get; set; }

		public BitReader(byte[] source, int intSizeInBits)
		{
			if (intSizeInBits > 31)
			{
				throw new NotSupportedException("BitReader supports only integers with less than 31 bits.");
			}
			this.Source = source;
			this.bits = intSizeInBits;
		}

		public virtual int Read()
		{
			if (this.SourcePointer >= this.Source.Length)
			{
				return -1;
			}
			int num = this.bits;
			int num2;
			if (num != 1)
			{
				if (num != 8)
				{
					if (num != 16)
					{
						num2 = this.ReadInGeneralScenario();
					}
					else
					{
						num2 = (int)this.Source[this.SourcePointer++] << 8;
						if (this.SourcePointer < this.Source.Length)
						{
							num2 |= (int)this.Source[this.SourcePointer++];
						}
					}
				}
				else
				{
					num2 = (int)this.Source[this.SourcePointer++];
				}
			}
			else
			{
				num2 = (this.Source[this.SourcePointer] >> ((7 - this.BitPointer++) & 31)) & 1;
				if (this.BitPointer == 8)
				{
					this.SourcePointer++;
					this.BitPointer = 0;
				}
			}
			return num2;
		}

		int ReadInGeneralScenario()
		{
			int num = 0;
			int i = this.bits;
			while (i > 0)
			{
				byte b = this.Source[this.SourcePointer];
				int bitPointer = this.BitPointer;
				int num2 = System.Math.Min(i, 8 - bitPointer);
				int num3 = 8 - bitPointer - num2;
				int num4 = (1 << num2) - 1;
				int num5 = (b >> num3) & num4;
				num |= num5 << i - num2;
				i -= num2;
				this.BitPointer += num2;
				if (this.BitPointer == 8)
				{
					this.SourcePointer++;
					this.BitPointer = 0;
				}
				if (this.SourcePointer >= this.Source.Length)
				{
					break;
				}
			}
			return num;
		}

		readonly int bits;
	}
}
