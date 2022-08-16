using System;

namespace JBig2Decoder
{
	class Big2StreamReader
	{
		public Big2StreamReader(byte[] data)
		{
			this.data = data;
		}

		public short readbyte()
		{
			return (short)(this.data[this.bytePointer++] & byte.MaxValue);
		}

		public void readbyte(short[] buf)
		{
			for (int i = 0; i < buf.Length; i++)
			{
				buf[i] = (short)(this.data[this.bytePointer++] & byte.MaxValue);
			}
		}

		public int readBit()
		{
			short num = this.readbyte();
			short num2 = (short)(1 << this.bitPointer);
			int result = (num & num2) >> this.bitPointer;
			this.bitPointer--;
			if (this.bitPointer == -1)
			{
				this.bitPointer = 7;
			}
			else
			{
				this.movePointer(-1);
			}
			return result;
		}

		public int readBits(long num)
		{
			int num2 = 0;
			int num3 = 0;
			while ((long)num3 < num)
			{
				num2 = (num2 << 1) | this.readBit();
				num3++;
			}
			return num2;
		}

		public void movePointer(int ammount)
		{
			this.bytePointer += ammount;
		}

		public void consumeRemainingBits()
		{
			if (this.bitPointer != 7)
			{
				this.readBits((long)(this.bitPointer + 1));
			}
		}

		public bool isFinished()
		{
			return this.bytePointer == this.data.Length;
		}

		byte[] data;

		int bitPointer = 7;

		int bytePointer;
	}
}
