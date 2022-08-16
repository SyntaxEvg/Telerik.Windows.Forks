using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class LzmaRangeEncoder : IDisposable
	{
		public LzmaRangeEncoder()
		{
			this.pendingData = new MemoryStream();
			this.Low = 0UL;
			this.Range = uint.MaxValue;
			this.cacheSize = 1U;
			this.cache = 0;
		}

		public ulong Low { get; set; }

		public uint Range { get; set; }

		public bool HasData
		{
			get
			{
				return this.pendingData.Length > 0L;
			}
		}

		public int NextOut
		{
			get
			{
				return this.nextOut;
			}
		}

		bool CanWrite
		{
			get
			{
				return this.NextOut < this.outputBuffer.Length;
			}
		}

		public void SetOutputBuffer(byte[] outputBuffer, int index)
		{
			this.outputBuffer = outputBuffer;
			this.nextOut = index;
			this.Flush();
		}

		public void FlushData()
		{
			for (int i = 0; i < 5; i++)
			{
				this.ShiftLow();
			}
		}

		public void Dispose()
		{
			if (this.pendingData != null)
			{
				this.pendingData.Dispose();
			}
		}

		public void ShiftLow()
		{
			if ((uint)this.Low < 4278190080U || (uint)(this.Low >> 32) == 1U)
			{
				byte maxValue = this.cache;
				do
				{
					byte b = (byte)((ulong)maxValue + (this.Low >> 32));
					if (this.CanWrite)
					{
						this.outputBuffer[this.nextOut++] = b;
					}
					else
					{
						this.pendingData.WriteByte(b);
					}
					maxValue = byte.MaxValue;
				}
				while ((this.cacheSize -= 1U) != 0U);
				this.cache = (byte)((uint)this.Low >> 24);
			}
			this.cacheSize += 1U;
			this.Low = (ulong)((ulong)((uint)this.Low) << 8);
		}

		public void EncodeDirectBits(uint value, int totalBits)
		{
			for (int i = totalBits - 1; i >= 0; i--)
			{
				this.Range >>= 1;
				if (((value >> i) & 1U) == 1U)
				{
					this.Low += (ulong)this.Range;
				}
				if (this.Range < 16777216U)
				{
					this.Range <<= 8;
					this.ShiftLow();
				}
			}
		}

		void Flush()
		{
			if (this.pendingData.Length > 0L)
			{
				if (this.pendingData.Position == this.pendingData.Length)
				{
					this.pendingData.Position = 0L;
				}
				int num = this.pendingData.Read(this.outputBuffer, this.NextOut, this.outputBuffer.Length - this.NextOut);
				this.nextOut += num;
				if (this.pendingData.Position == this.pendingData.Length)
				{
					this.pendingData.SetLength(0L);
				}
			}
		}

		public const uint TopValue = 16777216U;

		byte cache;

		uint cacheSize;

		byte[] outputBuffer;

		int nextOut;

		MemoryStream pendingData;
	}
}
