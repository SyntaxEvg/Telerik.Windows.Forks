using System;

namespace Telerik.Windows.Zip
{
	class OutputWindow
	{
		public int AvailableBytes
		{
			get
			{
				return this.availableBytes;
			}
		}

		public int FreeBytes
		{
			get
			{
				return 32768 - this.availableBytes;
			}
		}

		public void AddByte(byte value)
		{
			this.window[this.end++] = value;
			this.end &= 32767;
			this.availableBytes++;
		}

		public void Copy(int length, int distance)
		{
			this.availableBytes += length;
			int num = (this.end - distance) & 32767;
			int num2 = 32768 - length;
			if (num <= num2)
			{
				if (this.end < num2)
				{
					if (length > distance)
					{
						while (length-- > 0)
						{
							this.window[this.end++] = this.window[num++];
						}
						return;
					}
					Array.Copy(this.window, num, this.window, this.end, length);
					this.end += length;
					return;
				}
			}
			while (length-- > 0)
			{
				this.window[this.end++] = this.window[num++];
				this.end &= 32767;
				num &= 32767;
			}
		}

		public int Read(byte[] output, int offset, int length)
		{
			int num;
			if (length <= this.availableBytes)
			{
				num = (this.end - this.availableBytes + length) & 32767;
			}
			else
			{
				num = this.end;
				length = this.availableBytes;
			}
			int num2 = length;
			int num3 = length - num;
			if (num3 > 0)
			{
				Array.Copy(this.window, 32768 - num3, output, offset, num3);
				offset += num3;
				length = num;
			}
			Array.Copy(this.window, num - length, output, offset, length);
			this.availableBytes -= num2;
			return num2;
		}

		public int ReadInput(InputBitsBuffer input, int length)
		{
			int num = 32768 - this.end;
			length = System.Math.Min(Math.Min(length, this.FreeBytes), input.AvailableBytes);
			int num2;
			if (length <= num)
			{
				num2 = input.Read(this.window, this.end, length);
			}
			else
			{
				num2 = input.Read(this.window, this.end, num);
				if (num2 == num)
				{
					num2 += input.Read(this.window, 0, length - num);
				}
			}
			this.end = (this.end + num2) & 32767;
			this.availableBytes += num2;
			return num2;
		}

		const int WindowMask = 32767;

		const int WindowSize = 32768;

		int end;

		int availableBytes;

		byte[] window = new byte[32768];
	}
}
