using System;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Utils
{
	class BitsWriter
	{
		public BitsWriter()
		{
			this.Clear();
		}

		public int BitsLeft { get; set; }

		public bool IsEmpty
		{
			get
			{
				return this.BitsLeft == 8;
			}
		}

		public bool IsFull
		{
			get
			{
				return this.BitsLeft <= 0;
			}
		}

		public byte Data
		{
			get
			{
				return this.data;
			}
		}

		public void Clear()
		{
			this.data = 0;
			this.BitsLeft = 8;
		}

		public void WriteBits(byte value, int n)
		{
			if (n == 0)
			{
				return;
			}
			if (this.BitsLeft >= n)
			{
				int num = 0;
				for (int i = 0; i < n; i++)
				{
					num <<= 1;
					num |= 1;
				}
				int num2 = (int)value & num;
				this.data = (byte)(this.data << n);
				this.data = (byte)((int)this.data | num2);
				this.BitsLeft -= n;
				return;
			}
			throw new NotSupportedException("Not enought bits.");
		}

		byte data;
	}
}
