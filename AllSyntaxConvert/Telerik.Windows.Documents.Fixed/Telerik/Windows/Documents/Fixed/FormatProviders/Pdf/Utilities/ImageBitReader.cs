using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
	class ImageBitReader : BitReader
	{
		public ImageBitReader(byte[] source, int bitsPerComponent, int width)
			: base(source, bitsPerComponent)
		{
			this.width = width;
			this.pixelIndexInRow = 0;
		}

		public override int Read()
		{
			int result = base.Read();
			this.pixelIndexInRow++;
			if (this.pixelIndexInRow == this.width)
			{
				this.MoveToNextRow();
			}
			return result;
		}

		public int GetBitAt(int index)
		{
			int row = index / this.width;
			int column = index % this.width;
			return this.GetBitAt(row, column);
		}

		public int GetBitAt(int row, int column)
		{
			int num = 1 + (this.width - 1) / 8;
			int num2 = column / 8;
			int num3 = row * num + num2;
			int num4 = column - num2 * 8;
			return (base.Source[num3] >> 7 - num4) & 1;
		}

		void MoveToNextRow()
		{
			if (base.BitPointer != 0)
			{
				base.SourcePointer++;
				base.BitPointer = 0;
			}
			this.pixelIndexInRow = 0;
		}

		readonly int width;

		int pixelIndexInRow;
	}
}
