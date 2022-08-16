using System;

namespace Telerik.Windows.Zip
{
	class LzmaOutputWindow
	{
		public LzmaOutputWindow(uint blockSize)
		{
			this.windowSize = blockSize;
			this.window = new byte[blockSize];
		}

		public int AvailableBytes
		{
			get
			{
				int num = (int)(this.end - this.start);
				if (num < 0)
				{
					num = (int)(this.end + this.windowSize - this.start);
				}
				return num;
			}
		}

		public int Copied
		{
			get
			{
				return this.outputOffset;
			}
		}

		public bool Full
		{
			get
			{
				return this.outputBuffer.Length - this.outputOffset < 1;
			}
		}

		public void SetOutputBuffer(byte[] buffer)
		{
			this.outputBuffer = buffer;
			this.outputOffset = 0;
		}

		public void Flush()
		{
			int num = this.outputBuffer.Length - this.outputOffset;
			if (num > 0)
			{
				int availableBytes = this.AvailableBytes;
				if (availableBytes > 0)
				{
					if (num > availableBytes)
					{
						num = availableBytes;
					}
					this.Flush(num);
				}
			}
		}

		public void CopyBlock(uint distance, uint length)
		{
			uint num = this.end - distance - 1U;
			if (num >= this.windowSize)
			{
				num += this.windowSize;
			}
			while (length > 0U)
			{
				if (num >= this.windowSize)
				{
					num = 0U;
				}
				this.window[(int)((UIntPtr)(this.end++))] = this.window[(int)((UIntPtr)(num++))];
				if (this.end >= this.windowSize)
				{
					this.Flush();
					this.end = 0U;
				}
				length -= 1U;
			}
		}

		public void PutByte(byte value)
		{
			this.window[(int)((UIntPtr)(this.end++))] = value;
			if (this.end >= this.windowSize)
			{
				this.Flush();
				this.end = 0U;
			}
		}

		public byte GetByte(uint distance)
		{
			uint num = this.end - distance - 1U;
			if (num >= this.windowSize)
			{
				num += this.windowSize;
			}
			return this.window[(int)((UIntPtr)num)];
		}

		void Flush(int length)
		{
			if ((long)(this.start + (uint)length) > (long)((ulong)this.windowSize))
			{
				int num = (int)(this.windowSize - this.start);
				Array.Copy(this.window, (int)this.start, this.outputBuffer, this.outputOffset, num);
				this.outputOffset += num;
				length -= num;
				this.UpdatePosition(num);
			}
			Array.Copy(this.window, (int)this.start, this.outputBuffer, this.outputOffset, length);
			this.outputOffset += length;
			this.UpdatePosition(length);
		}

		void UpdatePosition(int length)
		{
			uint num = this.end - this.start;
			if (num != 0U)
			{
				this.start += (uint)length;
				if (this.start >= this.windowSize)
				{
					this.start -= this.windowSize;
				}
			}
		}

		byte[] window;

		uint end;

		uint windowSize;

		uint start;

		byte[] outputBuffer;

		int outputOffset;
	}
}
