using System;

namespace Telerik.Windows.Zip
{
	class InputBitsBuffer
	{
		public int AvailableBits
		{
			get
			{
				return this.availableBits;
			}
		}

		public int AvailableBytes
		{
			get
			{
				return this.end - this.start + this.availableBits / 8;
			}
		}

		public bool InputRequired
		{
			get
			{
				return this.start == this.end;
			}
		}

		public bool CheckAvailable(int count)
		{
			if (this.availableBits < count)
			{
				if (this.InputRequired)
				{
					return false;
				}
				this.bitBuffer |= (uint)((uint)this.buffer[this.start++] << (this.availableBits & 31 & 31));
				this.availableBits += 8;
				if (this.availableBits < count)
				{
					if (this.InputRequired)
					{
						return false;
					}
					this.bitBuffer |= (uint)((uint)this.buffer[this.start++] << (this.availableBits & 31 & 31));
					this.availableBits += 8;
				}
			}
			return true;
		}

		public int GetBits(int count)
		{
			if (!this.CheckAvailable(count))
			{
				return -1;
			}
			int result = (int)(this.bitBuffer & InputBitsBuffer.GetBitMask(count));
			this.bitBuffer >>= count;
			this.availableBits -= count;
			return result;
		}

		public int Read(byte[] output, int offset, int length)
		{
			int num = 0;
			while (this.availableBits > 0 && length > 0)
			{
				output[offset++] = (byte)this.bitBuffer;
				this.bitBuffer >>= 8;
				this.availableBits -= 8;
				length--;
				num++;
			}
			if (length == 0)
			{
				return num;
			}
			int num2 = this.end - this.start;
			if (length > num2)
			{
				length = num2;
			}
			Array.Copy(this.buffer, this.start, output, offset, length);
			this.start += length;
			return num + length;
		}

		public void SetBuffer(byte[] buffer, int offset, int length)
		{
			this.buffer = buffer;
			this.start = offset;
			this.end = offset + length;
		}

		public void SkipBits(int count)
		{
			this.bitBuffer >>= count;
			this.availableBits -= count;
		}

		public void SkipToByteBoundary()
		{
			this.bitBuffer >>= this.availableBits % 8;
			this.availableBits -= this.availableBits % 8;
		}

		public uint Get16Bits()
		{
			if (this.availableBits < 8)
			{
				this.Get8Bits();
				this.Get8Bits();
			}
			else if (this.availableBits < 16)
			{
				this.Get8Bits();
			}
			return this.bitBuffer;
		}

		static uint GetBitMask(int count)
		{
			return (1U << count) - 1U;
		}

		void Get8Bits()
		{
			if (this.start < this.end)
			{
				this.bitBuffer |= (uint)((uint)this.buffer[this.start++] << (this.availableBits & 31 & 31));
				this.availableBits += 8;
			}
		}

		uint bitBuffer;

		int availableBits;

		byte[] buffer;

		int end;

		int start;
	}
}
