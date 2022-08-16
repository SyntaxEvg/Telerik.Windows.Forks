using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
	class BitWriter
	{
		public BitWriter(int numberOfRecords, int intSizeInBits)
			: this(new byte[BitWriter.CalculateNumberOfBytes(numberOfRecords, intSizeInBits)], intSizeInBits)
		{
		}

		protected BitWriter(byte[] target, int intSizeInBits)
		{
			if (intSizeInBits > 31)
			{
				throw new NotSupportedException("BitReader supports only integers with less than 31 bits.");
			}
			this.target = target;
			this.intSizeInBits = intSizeInBits;
		}

		public byte[] ResultBits
		{
			get
			{
				return this.target;
			}
		}

		protected int IntSizeInBits
		{
			get
			{
				return this.intSizeInBits;
			}
		}

		protected int BytePointer { get; set; }

		protected int BitPointer { get; set; }

		public static int CalculateNumberOfBytes(int numberOfRecords, int bitsPerRecord)
		{
			int num = numberOfRecords * bitsPerRecord;
			return (num + 7) / 8;
		}

		public virtual void Write(int bits)
		{
			for (int i = 0; i < this.IntSizeInBits; i++)
			{
				Guard.ThrowExceptionIfTrue(this.BytePointer == this.ResultBits.Length, "Cannot write after the last available byte!");
				this.WriteBit(bits, i, this.BytePointer, this.BitPointer);
				this.BitPointer++;
				if (this.BitPointer == 8)
				{
					this.BytePointer++;
					this.BitPointer = 0;
				}
			}
		}

		protected void WriteBit(int bitSource, int bitIndex, int bytePointer, int bitPointer)
		{
			int num = (bitSource >> this.IntSizeInBits - bitIndex - 1) & 1;
			int num2 = 1 << 7 - bitPointer;
			if (num == 1)
			{
				this.ResultBits[bytePointer] = (byte)((int)this.ResultBits[bytePointer] | num2);
				return;
			}
			num2 = ~num2;
			this.ResultBits[bytePointer] = (byte)((int)this.ResultBits[bytePointer] & num2);
		}

		readonly byte[] target;

		readonly int intSizeInBits;
	}
}
