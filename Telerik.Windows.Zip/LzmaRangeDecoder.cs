using System;
using System.Collections.ObjectModel;

namespace Telerik.Windows.Zip
{
	class LzmaRangeDecoder
	{
		public bool FinalBlock { get; set; }

		public uint Range
		{
			get
			{
				return this.range;
			}
		}

		public uint Code
		{
			get
			{
				return this.code;
			}
		}

		public bool InputRequired
		{
			get
			{
				return this.inputRequired;
			}
		}

		public void Init(byte[] inputBuffer, int startIndex)
		{
			this.code = 0U;
			this.range = uint.MaxValue;
			for (int i = 0; i < 5; i++)
			{
				this.code = (this.code << 8) | (uint)inputBuffer[startIndex + i];
			}
		}

		public bool CheckInputRequired(int length, bool ignoreFinalBlock = false)
		{
			if (this.buffers.Count < 2 && (!this.FinalBlock || ignoreFinalBlock))
			{
				this.inputRequired = this.lastBufferSize - this.index < length;
			}
			return !this.inputRequired;
		}

		public void SetBuffer(byte[] inputBuffer, int length)
		{
			if (length > 0)
			{
				this.lastBufferSize = length;
				this.buffers.Add(inputBuffer);
				if (this.buffers.Count == 1)
				{
					this.buffer = this.buffers[0];
					this.index = 0;
				}
				this.inputRequired = false;
			}
		}

		public uint DecodeDirectBits(int numTotalBits)
		{
			uint num = 0U;
			for (int i = numTotalBits; i > 0; i--)
			{
				this.range >>= 1;
				uint num2 = this.code - this.range >> 31;
				this.code -= this.range & (num2 - 1U);
				num = (num << 1) | (1U - num2);
				if (this.range < 16777216U)
				{
					this.code = (this.code << 8) | (uint)this.GetNextByte();
					this.range <<= 8;
				}
			}
			return num;
		}

		public void MoveRange(uint newBound)
		{
			this.range -= newBound;
			this.code -= newBound;
			this.CheckRange();
		}

		public void UpdateRange(uint newBound)
		{
			this.range = newBound;
			this.CheckRange();
		}

		public void SaveState()
		{
			this.savedIndex = this.index;
			this.savedRange = this.range;
			this.savedCode = this.code;
		}

		public void RestoreState()
		{
			this.index = this.savedIndex;
			this.range = this.savedRange;
			this.code = this.savedCode;
		}

		void CheckRange()
		{
			if (this.range < 16777216U)
			{
				this.code = (this.code << 8) | (uint)this.GetNextByte();
				this.range <<= 8;
			}
		}

		byte GetNextByte()
		{
			if ((this.buffers.Count == 1 && this.index >= this.lastBufferSize) || this.index >= this.buffer.Length)
			{
				if (this.buffers.Count < 2)
				{
					if (this.inputRequired)
					{
						throw new InvalidOperationException("Decoder must check for request next block.");
					}
					this.inputRequired = true;
					return 0;
				}
				else
				{
					this.buffers.RemoveAt(0);
					this.buffer = this.buffers[0];
					this.index = 0;
				}
			}
			return this.buffer[this.index++];
		}

		public const uint TopValue = 16777216U;

		uint range;

		uint code;

		Collection<byte[]> buffers = new Collection<byte[]>();

		byte[] buffer;

		int lastBufferSize;

		int index;

		bool inputRequired = true;

		uint savedRange;

		uint savedCode;

		int savedIndex;
	}
}
