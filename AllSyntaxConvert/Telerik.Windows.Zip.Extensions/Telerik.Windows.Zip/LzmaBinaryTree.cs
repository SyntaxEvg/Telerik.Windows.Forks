using System;

namespace Telerik.Windows.Zip
{
	class LzmaBinaryTree
	{
		public LzmaBinaryTree(int hashBytes, uint historySize, uint matchMaxLen, uint keepAddBufferAfter)
		{
			this.useHashArray = hashBytes > 2;
			if (this.useHashArray)
			{
				this.hashDirectBytes = 0U;
				this.minMatchCheck = 4U;
				this.fixHashSize = 66560U;
			}
			else
			{
				this.hashDirectBytes = 2U;
				this.minMatchCheck = 3U;
				this.fixHashSize = 0U;
			}
			uint num = 4096U;
			uint num2 = 256U + (historySize + num + matchMaxLen + keepAddBufferAfter) / 2U;
			this.keepSizeBefore = historySize + num;
			this.keepSizeAfter = matchMaxLen + keepAddBufferAfter;
			uint num3 = this.keepSizeBefore + this.keepSizeAfter + num2;
			this.blockSize = num3;
			this.buffer = new byte[this.blockSize];
			this.lastSafePosition = this.blockSize - this.keepSizeAfter;
			this.matchMaxLength = matchMaxLen;
			this.cutValue = 16U + (this.matchMaxLength >> 1);
			uint num4 = historySize + 1U;
			if (this.cyclicBufferSize != num4)
			{
				this.son = new uint[(this.cyclicBufferSize = num4) * 2U];
			}
			this.CreateHash(historySize);
			this.Init();
		}

		public uint AvailableBytes
		{
			get
			{
				return this.streamIndex - this.bufferIndex;
			}
		}

		public bool HasInput
		{
			get
			{
				return this.inputBufferIndex - this.startInputBufferIndex < this.inputBufferLength;
			}
		}

		public bool Ready
		{
			get
			{
				if (!this.inputComplete)
				{
					int num = (int)(0U - this.bufferOffset + this.blockSize - this.streamIndex);
					if (num != 0)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool ReadyToMove
		{
			get
			{
				if (!this.inputComplete)
				{
					uint num = this.bufferIndex + 1U;
					if (num > this.positionLimit)
					{
						uint num2 = this.bufferOffset + num;
						if (num2 > this.lastSafePosition)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public uint GetMatches(uint[] distances)
		{
			this.GetLengthLimit();
			if (this.currentLengthLimit == 0U)
			{
				return this.currentLengthLimit;
			}
			this.SetInitialMatchValues(distances);
			this.CalculateHashValues(true);
			this.CheckDirectBytes();
			this.ProcessMatches(true);
			return this.currentOffset;
		}

		public void Skip(uint counter)
		{
			do
			{
				this.GetLengthLimit();
				if (this.currentLengthLimit != 0U)
				{
					this.SetInitialMatchValues(null);
					this.CalculateHashValues(false);
					this.ProcessMatches(false);
				}
			}
			while ((counter -= 1U) != 0U);
		}

		public void MoveBlock()
		{
			uint num = this.bufferOffset + this.bufferIndex - this.keepSizeBefore;
			if (num > 0U)
			{
				num -= 1U;
			}
			int length = (int)(this.bufferOffset + this.streamIndex - num);
			Array.Copy(this.buffer, (int)num, this.buffer, 0, length);
			this.bufferOffset -= num;
		}

		public void SetInputBuffer(byte[] buffer, int offset, int length, bool finalBlock)
		{
			if (this.HasInput)
			{
				this.ReadCicleBlock();
			}
			bool hasInput = this.HasInput;
			this.inputBuffer = buffer;
			this.inputBufferLength = length;
			this.inputBufferIndex = offset;
			this.startInputBufferIndex = offset;
			this.finalBlock = finalBlock;
			this.ReadCicleBlock();
		}

		public byte GetIndexByte(int index)
		{
			return this.buffer[(int)(checked((IntPtr)(unchecked((ulong)(this.bufferOffset + this.bufferIndex) + (ulong)((long)index)))))];
		}

		public uint GetMatchLen(int index, uint distance, uint limit)
		{
			if (this.inputComplete && (ulong)this.bufferIndex + (ulong)((long)index) + (ulong)limit > (ulong)this.streamIndex)
			{
				limit = this.streamIndex - (uint)((ulong)this.bufferIndex + (ulong)((long)index));
			}
			distance += 1U;
			uint num = this.bufferOffset + this.bufferIndex + (uint)index;
			uint num2 = 0U;
			while (num2 < limit && this.buffer[(int)((UIntPtr)(num + num2))] == this.buffer[(int)((UIntPtr)(num + num2 - distance))])
			{
				num2 += 1U;
			}
			return num2;
		}

		static void NormalizeLinks(uint[] items, uint itemsCounter, uint subValue)
		{
			for (uint num = 0U; num < itemsCounter; num += 1U)
			{
				uint num2 = items[(int)((UIntPtr)num)];
				if (num2 <= subValue)
				{
					num2 = 0U;
				}
				else
				{
					num2 -= subValue;
				}
				items[(int)((UIntPtr)num)] = num2;
			}
		}

		void Init()
		{
			this.bufferOffset = 0U;
			this.bufferIndex = 0U;
			this.streamIndex = 0U;
			this.inputComplete = false;
			for (uint num = 0U; num < this.hashSizeSum; num += 1U)
			{
				this.hash[(int)((UIntPtr)num)] = 0U;
			}
			this.cyclicBufferPos = 0U;
			this.ReduceOffsets(-1);
		}

		void CreateHash(uint historySize)
		{
			uint num = 65536U;
			if (this.useHashArray)
			{
				num = historySize - 1U;
				num |= num >> 1;
				num |= num >> 2;
				num |= num >> 4;
				num |= num >> 8;
				num >>= 1;
				num |= 65535U;
				if (num > 16777216U)
				{
					num >>= 1;
				}
				this.hashMask = num;
				num += 1U;
				num += this.fixHashSize;
			}
			if (num != this.hashSizeSum)
			{
				this.hash = new uint[this.hashSizeSum = num];
			}
		}

		void Normalize()
		{
			uint subValue = this.bufferIndex - this.cyclicBufferSize;
			LzmaBinaryTree.NormalizeLinks(this.son, this.cyclicBufferSize * 2U, subValue);
			LzmaBinaryTree.NormalizeLinks(this.hash, this.hashSizeSum, subValue);
			this.ReduceOffsets((int)subValue);
		}

		void ReduceOffsets(int subValue)
		{
			this.bufferOffset += (uint)subValue;
			this.positionLimit -= (uint)subValue;
			this.bufferIndex -= (uint)subValue;
			this.streamIndex -= (uint)subValue;
		}

		void ReadBlock()
		{
			if (!this.inputComplete)
			{
				for (;;)
				{
					int num = (int)(0U - this.bufferOffset + this.blockSize - this.streamIndex);
					if (num == 0)
					{
						break;
					}
					num = System.Math.Min(num, this.inputBufferLength + this.startInputBufferIndex - this.inputBufferIndex);
					Array.Copy(this.inputBuffer, this.inputBufferIndex, this.buffer, (int)(this.bufferOffset + this.streamIndex), num);
					this.inputBufferIndex += num;
					this.streamIndex += (uint)num;
					if (this.streamIndex >= this.bufferIndex + this.keepSizeAfter)
					{
						this.positionLimit = this.streamIndex - this.keepSizeAfter;
					}
					if (!this.HasInput)
					{
						goto Block_3;
					}
				}
				this.SetRestInputBuffer();
				return;
				Block_3:
				if (this.finalBlock)
				{
					this.SetInputComplete();
				}
			}
		}

		void SetInputComplete()
		{
			this.positionLimit = this.streamIndex;
			uint num = this.bufferOffset + this.positionLimit;
			if (num > this.lastSafePosition)
			{
				this.positionLimit = this.lastSafePosition - this.bufferOffset;
			}
			this.inputComplete = true;
		}

		void SetRestInputBuffer()
		{
			int num = this.inputBufferLength + this.startInputBufferIndex - this.inputBufferIndex;
			byte[] destinationArray = new byte[num];
			Array.Copy(this.inputBuffer, this.inputBufferIndex, destinationArray, 0, num);
			this.startInputBufferIndex = (this.inputBufferIndex = 0);
			this.inputBufferLength = num;
			this.inputBuffer = destinationArray;
		}

		void ReadCicleBlock()
		{
			this.TryMoveBlock();
			this.ReadBlock();
		}

		bool TryMoveBlock()
		{
			if (this.bufferIndex > this.positionLimit)
			{
				uint num = this.bufferOffset + this.bufferIndex;
				if (num > this.lastSafePosition)
				{
					this.MoveBlock();
					return true;
				}
			}
			return false;
		}

		void GetLengthLimit()
		{
			if (this.bufferIndex + this.matchMaxLength <= this.streamIndex)
			{
				this.currentLengthLimit = this.matchMaxLength;
				return;
			}
			this.currentLengthLimit = this.streamIndex - this.bufferIndex;
			if (this.currentLengthLimit < this.minMatchCheck)
			{
				this.MovePos();
				this.currentLengthLimit = 0U;
			}
		}

		void MovePos()
		{
			if ((this.cyclicBufferPos += 1U) >= this.cyclicBufferSize)
			{
				this.cyclicBufferPos = 0U;
			}
			this.bufferIndex += 1U;
			if (this.TryMoveBlock())
			{
				this.ReadBlock();
			}
			if (this.bufferIndex == 2147483647U)
			{
				this.Normalize();
			}
		}

		void SetInitialMatchValues(uint[] distances = null)
		{
			this.currentDistances = distances;
			this.currentOffset = 0U;
			this.currentMatchMinPos = ((this.bufferIndex > this.cyclicBufferSize) ? (this.bufferIndex - this.cyclicBufferSize) : 0U);
			this.currentBufferIndex = this.bufferOffset + this.bufferIndex;
			this.currentMatchMaxLen = 1U;
		}

		void CalculateHashValues(bool matchProcess = true)
		{
			uint num = this.currentBufferIndex;
			uint num2 = 0U;
			uint num3 = 0U;
			uint num5;
			if (this.useHashArray)
			{
				uint num4 = Crc32.Table[(int)this.buffer[(int)((UIntPtr)num)]] ^ (uint)this.buffer[(int)((UIntPtr)(num + 1U))];
				num2 = num4 & 1023U;
				num4 ^= (uint)((uint)this.buffer[(int)((UIntPtr)(num + 2U))] << 8);
				num3 = num4 & 65535U;
				num5 = (num4 ^ (Crc32.Table[(int)this.buffer[(int)((UIntPtr)(num + 3U))]] << 5)) & this.hashMask;
			}
			else
			{
				num5 = (uint)((int)this.buffer[(int)((UIntPtr)num)] ^ ((int)this.buffer[(int)((UIntPtr)(num + 1U))] << 8));
				this.currentMatch = this.hash[(int)((UIntPtr)(this.fixHashSize + num5))];
			}
			this.currentMatch = this.hash[(int)((UIntPtr)(this.fixHashSize + num5))];
			if (this.useHashArray)
			{
				uint curMatch = this.hash[(int)((UIntPtr)num2)];
				this.hash[(int)((UIntPtr)num2)] = this.bufferIndex;
				uint curMatch2 = this.hash[(int)((UIntPtr)(1024U + num3))];
				this.hash[(int)((UIntPtr)(1024U + num3))] = this.bufferIndex;
				if (matchProcess)
				{
					this.UpdateMatches(curMatch, curMatch2);
				}
			}
			else
			{
				num5 = (uint)((int)this.buffer[(int)((UIntPtr)num)] ^ ((int)this.buffer[(int)((UIntPtr)(num + 1U))] << 8));
				this.currentMatch = this.hash[(int)((UIntPtr)(this.fixHashSize + num5))];
			}
			this.hash[(int)((UIntPtr)(this.fixHashSize + num5))] = this.bufferIndex;
		}

		void UpdateMatches(uint curMatch2, uint curMatch3)
		{
			if (this.useHashArray)
			{
				uint[] array = this.currentDistances;
				uint num = this.currentMatchMinPos;
				uint num2 = this.currentOffset;
				uint num3 = this.currentBufferIndex;
				if (curMatch2 > num && this.buffer[(int)((UIntPtr)(this.bufferOffset + curMatch2))] == this.buffer[(int)((UIntPtr)num3)])
				{
					array[(int)((UIntPtr)(num2++))] = (this.currentMatchMaxLen = 2U);
					array[(int)((UIntPtr)(num2++))] = this.bufferIndex - curMatch2 - 1U;
				}
				if (curMatch3 > num && this.buffer[(int)((UIntPtr)(this.bufferOffset + curMatch3))] == this.buffer[(int)((UIntPtr)num3)])
				{
					if (curMatch3 == curMatch2)
					{
						num2 -= 2U;
					}
					array[(int)((UIntPtr)(num2++))] = (this.currentMatchMaxLen = 3U);
					array[(int)((UIntPtr)(num2++))] = this.bufferIndex - curMatch3 - 1U;
					curMatch2 = curMatch3;
				}
				if (num2 != 0U && curMatch2 == this.currentMatch)
				{
					num2 -= 2U;
					this.currentMatchMaxLen = 1U;
				}
				this.currentOffset = num2;
			}
		}

		void CheckDirectBytes()
		{
			if (this.hashDirectBytes != 0U && this.currentMatch > this.currentMatchMinPos && this.buffer[(int)((UIntPtr)(this.bufferOffset + this.currentMatch + this.hashDirectBytes))] != this.buffer[(int)((UIntPtr)(this.currentBufferIndex + this.hashDirectBytes))])
			{
				this.currentDistances[(int)((UIntPtr)(this.currentOffset++))] = (this.currentMatchMaxLen = this.hashDirectBytes);
				this.currentDistances[(int)((UIntPtr)(this.currentOffset++))] = this.bufferIndex - this.currentMatch - 1U;
			}
		}

		void ProcessMatches(bool matchProcess = true)
		{
			uint num = this.cyclicBufferPos << 1;
			uint num2 = num + 1U;
			uint num3 = this.currentBufferIndex;
			uint num4 = this.currentMatchMinPos;
			uint num5 = this.cutValue;
			uint val2;
			uint val = (val2 = this.hashDirectBytes);
			for (;;)
			{
				uint num6 = this.currentMatch;
				if (num6 <= num4 || num5-- == 0U)
				{
					break;
				}
				uint num7 = this.bufferIndex - num6;
				uint num8 = ((num7 <= this.cyclicBufferPos) ? (this.cyclicBufferPos - num7) : (this.cyclicBufferPos - num7 + this.cyclicBufferSize)) << 1;
				uint num9 = this.bufferOffset + num6;
				uint num10 = System.Math.Min(val2, val);
				if (this.buffer[(int)((UIntPtr)(num9 + num10))] == this.buffer[(int)((UIntPtr)(num3 + num10))])
				{
					num10 = this.SkipToLimit(num3, num9, num10);
					if (this.CheckMatchLength(matchProcess, num8, num10, num2, num))
					{
						goto IL_14B;
					}
				}
				if (this.buffer[(int)((UIntPtr)(num9 + num10))] < this.buffer[(int)((UIntPtr)(num3 + num10))])
				{
					this.son[(int)((UIntPtr)num)] = num6;
					num = num8 + 1U;
					this.currentMatch = this.son[(int)((UIntPtr)num)];
					val = num10;
				}
				else
				{
					this.son[(int)((UIntPtr)num2)] = num6;
					num2 = num8;
					this.currentMatch = this.son[(int)((UIntPtr)num2)];
					val2 = num10;
				}
			}
			this.son[(int)((UIntPtr)num2)] = (this.son[(int)((UIntPtr)num)] = 0U);
			IL_14B:
			this.MovePos();
		}

		uint SkipToLimit(uint current, uint pointerByte1, uint length)
		{
			while ((length += 1U) != this.currentLengthLimit && this.buffer[(int)((UIntPtr)(pointerByte1 + length))] == this.buffer[(int)((UIntPtr)(current + length))])
			{
			}
			return length;
		}

		bool CheckMatchLength(bool matchProcess, uint cyclicPosition, uint length, uint pointer0, uint pointer1)
		{
			bool flag = this.currentMatchMaxLen < length;
			if (matchProcess && flag)
			{
				uint[] array = this.currentDistances;
				uint num = this.currentOffset;
				uint num2 = this.bufferIndex - this.currentMatch;
				uint[] array2 = array;
				int num3 = (int)((UIntPtr)(num++));
				this.currentMatchMaxLen = length;
				array2[num3] = length;
				array[(int)((UIntPtr)(num++))] = num2 - 1U;
				this.currentOffset = num;
			}
			if ((!matchProcess || flag) && length == this.currentLengthLimit)
			{
				this.son[(int)((UIntPtr)pointer1)] = this.son[(int)((UIntPtr)cyclicPosition)];
				this.son[(int)((UIntPtr)pointer0)] = this.son[(int)((UIntPtr)(cyclicPosition + 1U))];
				return true;
			}
			return false;
		}

		const uint Hash2Size = 1024U;

		const uint Hash3Size = 65536U;

		const uint BT2HashSize = 65536U;

		const uint StartMaxLen = 1U;

		const uint Hash3Offset = 1024U;

		const uint EmptyHashValue = 0U;

		const uint NormalizeMaxValue = 2147483647U;

		byte[] buffer;

		uint positionLimit;

		bool inputComplete;

		uint lastSafePosition;

		uint bufferOffset;

		uint[] currentDistances;

		uint currentBufferIndex;

		uint currentLengthLimit;

		uint currentMatch;

		uint currentMatchMaxLen;

		uint currentMatchMinPos;

		uint currentOffset;

		uint blockSize;

		uint keepSizeBefore;

		uint keepSizeAfter;

		uint bufferIndex;

		uint streamIndex;

		byte[] inputBuffer;

		int inputBufferLength;

		int inputBufferIndex;

		bool finalBlock;

		uint cyclicBufferPos;

		uint cyclicBufferSize;

		uint matchMaxLength;

		uint[] son;

		uint[] hash;

		uint cutValue = 255U;

		uint hashMask;

		uint hashSizeSum;

		bool useHashArray = true;

		uint hashDirectBytes;

		uint minMatchCheck = 4U;

		uint fixHashSize = 66560U;

		int startInputBufferIndex;
	}
}
