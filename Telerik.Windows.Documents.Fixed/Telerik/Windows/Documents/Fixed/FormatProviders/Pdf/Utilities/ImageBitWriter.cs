using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
	class ImageBitWriter : BitWriter
	{
		public ImageBitWriter(byte[] target, int intSizeInBits, int width)
			: base(target, intSizeInBits)
		{
			this.width = width;
		}

		public override void Write(int bits)
		{
			base.Write(bits);
			this.pixelIndexInRow++;
			if (this.pixelIndexInRow == this.width)
			{
				this.MoveToNextRow();
			}
		}

		public void WriteBitsToIndex(int bits, int index)
		{
			int num;
			int num2;
			this.GetBitPositionFromIndex(index, out num, out num2);
			for (int i = 0; i < base.IntSizeInBits; i++)
			{
				Guard.ThrowExceptionIfTrue(num == base.ResultBits.Length, "Cannot write after the last available byte!");
				base.WriteBit(bits, i, num, num2);
				num2++;
				if (num2 == 8)
				{
					num++;
					num2 = 0;
				}
			}
		}

		void GetBitPositionFromIndex(int index, out int byteIndexInArray, out int bitIndexInByte)
		{
			int num = index / this.width;
			int num2 = BitWriter.CalculateNumberOfBytes(this.width, base.IntSizeInBits);
			int num3 = index % this.width;
			int num4 = num3 * base.IntSizeInBits;
			int num5 = num4 / 8;
			bitIndexInByte = num4 % 8;
			byteIndexInArray = num * num2 + num5;
		}

		void MoveToNextRow()
		{
			this.pixelIndexInRow = 0;
			if (base.BitPointer > 0)
			{
				base.BitPointer = 0;
				base.BytePointer++;
			}
		}

		readonly int width;

		int pixelIndexInRow;
	}
}
