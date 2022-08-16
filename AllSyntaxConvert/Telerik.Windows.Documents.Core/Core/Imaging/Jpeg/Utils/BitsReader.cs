using System;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Utils
{
	class BitsReader
	{
		public bool IsEmpty
		{
			get
			{
				return this.bitsLeft <= 0;
			}
		}

		public byte Data
		{
			get
			{
				return this.data;
			}
			set
			{
				if (!this.IsEmpty)
				{
					throw new InvalidOperationException("There are still bits remaining.");
				}
				this.data = value;
				this.bitsLeft = 8;
			}
		}

		public int ReadBits(int n)
		{
			if (n == 0)
			{
				return 0;
			}
			if (this.bitsLeft >= n)
			{
				this.bitsLeft -= n;
				int result = this.data >> 8 - n;
				this.data = (byte)(this.data << n);
				return result;
			}
			throw new NotSupportedException("Not enought bits.");
		}

		byte data;

		int bitsLeft;
	}
}
