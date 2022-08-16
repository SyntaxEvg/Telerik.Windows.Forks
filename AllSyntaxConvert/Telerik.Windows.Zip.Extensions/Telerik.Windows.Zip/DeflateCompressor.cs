using System;

namespace Telerik.Windows.Zip
{
	class DeflateCompressor : DeflateTransformBase
	{
		public DeflateCompressor(DeflateSettings settings)
			: base(settings)
		{
			int num = 573;
			this.dynamicLiteralsTree = new short[num * 2];
			this.dynamicDistanceTree = new short[122];
			this.dynamicBitLengthTree = new short[78];
			this.BitLengthCount = new short[16];
			this.Heap = new int[num];
			this.Depth = new sbyte[num];
			this.Initialize(settings);
		}

		public short[] BitLengthCount { get; set; }

		public int[] Heap { get; set; }

		public int HeapLength { get; set; }

		public int HeapMax { get; set; }

		public sbyte[] Depth { get; set; }

		public int OptimalLength { get; set; }

		public int StaticLength { get; set; }

		public override void CreateHeader()
		{
			if (base.Settings.HeaderType == CompressedStreamHeader.ZLib)
			{
				byte[] array = new byte[2];
				int num = 8 + (this.windowBits - 8 << 4) << 8;
				int num2 = ((this.compressionLevel - 1) & 255) >> 1;
				if (num2 > 3)
				{
					num2 = 3;
				}
				num |= num2 << 6;
				num += 31 - num % 31;
				array[0] = (byte)(num >> 8);
				array[1] = (byte)num;
				base.Header.Buffer = array;
			}
		}

		internal void DownHeap(short[] tree, int nodeIndex)
		{
			int i = nodeIndex << 1;
			int num = this.Heap[nodeIndex];
			while (i <= this.HeapLength)
			{
				if (i < this.HeapLength && DeflateCompressor.IsSmaller(tree, this.Heap[i + 1], this.Heap[i], this.Depth))
				{
					i++;
				}
				if (DeflateCompressor.IsSmaller(tree, num, this.Heap[i], this.Depth))
				{
					break;
				}
				this.Heap[nodeIndex] = this.Heap[i];
				nodeIndex = i;
				i <<= 1;
			}
			this.Heap[nodeIndex] = num;
		}

		protected override bool ProcessTransform(bool finalBlock)
		{
			this.Deflate(finalBlock);
			return this.pendingCount != 0;
		}

		static bool IsSmaller(short[] tree, int nodeIndex1, int nodeIndex2, sbyte[] depth)
		{
			short num = tree[nodeIndex1 * 2];
			short num2 = tree[nodeIndex2 * 2];
			return num < num2 || (num == num2 && depth[nodeIndex1] <= depth[nodeIndex2]);
		}

		void Deflate(bool flush)
		{
			if (base.AvailableBytesOut <= 1)
			{
				throw new InvalidOperationException("Output buffer is full");
			}
			if (this.pendingCount != 0)
			{
				this.FlushPending();
				if (base.AvailableBytesOut == 0)
				{
					return;
				}
			}
			else if (base.AvailableBytesIn == 0 && !flush)
			{
				return;
			}
			if (this.finishPending && base.AvailableBytesIn != 0)
			{
				throw new InvalidOperationException("Status is finish, but input bytes count is not zero");
			}
			if (base.AvailableBytesIn != 0 || this.lookAhead != 0 || (flush && !this.finishPending))
			{
				DeflateBlockState deflateBlockState = this.deflateFunction(flush);
				if (deflateBlockState == DeflateBlockState.FinishStarted || deflateBlockState == DeflateBlockState.FinishDone)
				{
					this.finishPending = true;
				}
				if (deflateBlockState == DeflateBlockState.NeedMore || deflateBlockState == DeflateBlockState.FinishStarted)
				{
					return;
				}
				if (deflateBlockState == DeflateBlockState.BlockDone)
				{
					this.SendStoredBlock(0, 0, false);
					this.FlushPending();
					int availableBytesOut = base.AvailableBytesOut;
				}
			}
		}

		void ScanTree(short[] tree, int maxCode)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			tree[(maxCode + 1) * 2 + 1] = short.MaxValue;
			for (int i = 0; i <= maxCode; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						this.dynamicBitLengthTree[num6 * 2] = (short)((int)this.dynamicBitLengthTree[num6 * 2] + num3);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							short[] array = this.dynamicBitLengthTree;
							int num7 = num6 * 2;
							array[num7] += 1;
						}
						short[] array2 = this.dynamicBitLengthTree;
						int num8 = 32;
						array2[num8] += 1;
					}
					else if (num3 <= 10)
					{
						short[] array3 = this.dynamicBitLengthTree;
						int num9 = 34;
						array3[num9] += 1;
					}
					else
					{
						short[] array4 = this.dynamicBitLengthTree;
						int num10 = 36;
						array4[num10] += 1;
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		int BuildBitLengthTree()
		{
			this.ScanTree(this.dynamicLiteralsTree, this.treeLiterals.MaxCode);
			this.ScanTree(this.dynamicDistanceTree, this.treeDistances.MaxCode);
			this.treeBitLengths.BuildTree(this);
			int num = 18;
			while (num >= 3 && this.dynamicBitLengthTree[(int)(Tree.BitLengthOrder[num] * 2 + 1)] == 0)
			{
				num--;
			}
			this.OptimalLength += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		void SendAllTrees(int literalCodes, int distanceCodes, int bitLengthCodes)
		{
			this.SendBits(literalCodes - 257, 5);
			this.SendBits(distanceCodes - 1, 5);
			this.SendBits(bitLengthCodes - 4, 4);
			for (int i = 0; i < bitLengthCodes; i++)
			{
				this.SendBits((int)this.dynamicBitLengthTree[(int)(Tree.BitLengthOrder[i] * 2 + 1)], 3);
			}
			this.SendTree(this.dynamicLiteralsTree, literalCodes - 1);
			this.SendTree(this.dynamicDistanceTree, distanceCodes - 1);
		}

		void SendTree(short[] tree, int maxCode)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int i = 0; i <= maxCode; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						do
						{
							this.SendCode(num6, this.dynamicBitLengthTree);
						}
						while (--num3 != 0);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							this.SendCode(num6, this.dynamicBitLengthTree);
							num3--;
						}
						this.SendCode(16, this.dynamicBitLengthTree);
						this.SendBits(num3 - 3, 2);
					}
					else if (num3 <= 10)
					{
						this.SendCode(17, this.dynamicBitLengthTree);
						this.SendBits(num3 - 3, 3);
					}
					else
					{
						this.SendCode(18, this.dynamicBitLengthTree);
						this.SendBits(num3 - 11, 7);
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		void PutBytes(byte[] buffer, int start, int length)
		{
			Array.Copy(buffer, start, this.pending, this.pendingCount, length);
			this.pendingCount += length;
		}

		void SendCode(int nodeIndex, short[] tree)
		{
			int num = nodeIndex * 2;
			this.SendBits((int)tree[num] & 65535, (int)tree[num + 1] & 65535);
		}

		void SendBits(int value, int length)
		{
			int num = (int)(this.bitsBuffer | (short)((value << this.bitsValid) & 65535));
			this.bitsBuffer = (short)num;
			if (this.bitsValid > 16 - length)
			{
				this.pending[this.pendingCount++] = (byte)this.bitsBuffer;
				this.pending[this.pendingCount++] = (byte)(this.bitsBuffer >> 8);
				this.bitsBuffer = (short)((uint)value >> 16 - this.bitsValid);
				this.bitsValid += length - 16;
				return;
			}
			this.bitsValid += length;
		}

		bool TreeTally(int distance, int lengthOrChar)
		{
			this.pending[this.distanceOffset + this.lastLiteral * 2] = (byte)((uint)distance >> 8);
			this.pending[this.distanceOffset + this.lastLiteral * 2 + 1] = (byte)distance;
			this.pending[this.lengthOffset + this.lastLiteral] = (byte)lengthOrChar;
			this.lastLiteral++;
			if (distance == 0)
			{
				short[] array = this.dynamicLiteralsTree;
				int num = lengthOrChar * 2;
				array[num] += 1;
			}
			else
			{
				this.matches++;
				distance--;
				short[] array2 = this.dynamicLiteralsTree;
				int num2 = ((int)Tree.LengthCode[lengthOrChar] + 256 + 1) * 2;
				array2[num2] += 1;
				short[] array3 = this.dynamicDistanceTree;
				int num3 = Tree.GetDistanceCode(distance) * 2;
				array3[num3] += 1;
			}
			if ((this.lastLiteral & 8191) == 0 && this.compressionLevel > 2)
			{
				int num4 = this.lastLiteral << 3;
				int num5 = this.startOfString - this.blockStart;
				for (int i = 0; i < 30; i++)
				{
					num4 = (int)((long)num4 + (long)this.dynamicDistanceTree[i * 2] * (5L + (long)Tree.ExtraDistanceBits[i]));
				}
				num4 >>= 3;
				if (this.matches < this.lastLiteral / 2 && num4 < num5 / 2)
				{
					return true;
				}
			}
			return this.lastLiteral == this.literalsBufsize - 1 || this.lastLiteral == this.literalsBufsize;
		}

		void SendCompressedBlock(short[] literalTree, short[] distanceTree)
		{
			int num = 0;
			if (this.lastLiteral != 0)
			{
				do
				{
					int num2 = this.distanceOffset + num * 2;
					int num3 = (((int)this.pending[num2] << 8) & 65280) | (int)(this.pending[num2 + 1] & byte.MaxValue);
					int num4 = (int)(this.pending[this.lengthOffset + num] & byte.MaxValue);
					num++;
					if (num3 == 0)
					{
						this.SendCode(num4, literalTree);
					}
					else
					{
						int num5 = (int)Tree.LengthCode[num4];
						this.SendCode(num5 + 256 + 1, literalTree);
						int num6 = Tree.ExtraLengthBits[num5];
						if (num6 != 0)
						{
							num4 -= Tree.LengthBase[num5];
							this.SendBits(num4, num6);
						}
						num3--;
						num5 = Tree.GetDistanceCode(num3);
						this.SendCode(num5, distanceTree);
						num6 = Tree.ExtraDistanceBits[num5];
						if (num6 != 0)
						{
							num3 -= Tree.DistanceBase[num5];
							this.SendBits(num3, num6);
						}
					}
				}
				while (num < this.lastLiteral);
			}
			this.SendCode(256, literalTree);
		}

		void AlginOnByteBoundary()
		{
			if (this.bitsValid > 8)
			{
				this.pending[this.pendingCount++] = (byte)this.bitsBuffer;
				this.pending[this.pendingCount++] = (byte)(this.bitsBuffer >> 8);
			}
			else if (this.bitsValid > 0)
			{
				this.pending[this.pendingCount++] = (byte)this.bitsBuffer;
			}
			this.bitsBuffer = 0;
			this.bitsValid = 0;
		}

		void CopyBlock(int buffer, int length, bool header)
		{
			this.AlginOnByteBoundary();
			if (header)
			{
				this.pending[this.pendingCount++] = (byte)length;
				this.pending[this.pendingCount++] = (byte)(length >> 8);
				this.pending[this.pendingCount++] = (byte)(~(byte)length);
				this.pending[this.pendingCount++] = (byte)(~length >> 8);
			}
			this.PutBytes(this.window, buffer, length);
		}

		void FlushBlockOnly(bool lastBlock)
		{
			this.TreeFlushBlock((this.blockStart >= 0) ? this.blockStart : (-1), this.startOfString - this.blockStart, lastBlock);
			this.blockStart = this.startOfString;
			this.FlushPending();
		}

		void SendStoredBlock(int offset, int length, bool lastBlock)
		{
			int value = (lastBlock ? 1 : 0);
			this.SendBits(value, 3);
			this.CopyBlock(offset, length, true);
		}

		void TreeFlushBlock(int offset, int length, bool lastBlock)
		{
			int num = 0;
			int num2;
			int num3;
			if (this.compressionLevel > 0)
			{
				this.treeLiterals.BuildTree(this);
				this.treeDistances.BuildTree(this);
				num = this.BuildBitLengthTree();
				num2 = this.OptimalLength + 3 + 7 >> 3;
				num3 = this.StaticLength + 3 + 7 >> 3;
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num3 = (num2 = length + 5);
			}
			if (length + 4 <= num2 && offset != -1)
			{
				this.SendStoredBlock(offset, length, lastBlock);
			}
			else
			{
				int num4 = (lastBlock ? 1 : 0);
				if (num3 == num2)
				{
					this.SendBits(2 + num4, 3);
					this.SendCompressedBlock(StaticTree.LengthAndLiteralsTreeCodes, StaticTree.DistTreeCodes);
				}
				else
				{
					this.SendBits(4 + num4, 3);
					this.SendAllTrees(this.treeLiterals.MaxCode + 1, this.treeDistances.MaxCode + 1, num + 1);
					this.SendCompressedBlock(this.dynamicLiteralsTree, this.dynamicDistanceTree);
				}
			}
			this.InitializeBlocks();
			if (lastBlock)
			{
				this.AlginOnByteBoundary();
			}
		}

		void FillWindow()
		{
			for (;;)
			{
				int num = this.actualWindowSize - this.lookAhead - this.startOfString;
				int num2;
				if (num == 0 && this.startOfString == 0 && this.lookAhead == 0)
				{
					num = this.windowSize;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (this.startOfString >= this.windowSize + this.windowSize - 262)
				{
					Array.Copy(this.window, this.windowSize, this.window, 0, this.windowSize);
					this.matchStart -= this.windowSize;
					this.startOfString -= this.windowSize;
					this.blockStart -= this.windowSize;
					num2 = this.hashSize;
					int num3 = num2;
					do
					{
						int num4 = (int)this.head[--num3] & 65535;
						this.head[num3] = (short)((num4 >= this.windowSize) ? (num4 - this.windowSize) : 0);
					}
					while (--num2 != 0);
					num2 = this.windowSize;
					num3 = num2;
					do
					{
						int num5 = (int)this.previous[--num3] & 65535;
						this.previous[num3] = (short)((num5 >= this.windowSize) ? (num5 - this.windowSize) : 0);
					}
					while (--num2 != 0);
					num += this.windowSize;
				}
				if (base.AvailableBytesIn == 0)
				{
					break;
				}
				num2 = this.ReadBuffer(this.window, this.startOfString + this.lookAhead, num);
				this.lookAhead += num2;
				if (this.lookAhead >= 3)
				{
					this.hashIndexOfString = (int)(this.window[this.startOfString] & byte.MaxValue);
					this.hashIndexOfString = ((this.hashIndexOfString << this.hashShift) ^ (int)(this.window[this.startOfString + 1] & byte.MaxValue)) & this.hashMask;
				}
				if (this.lookAhead >= 262 || base.AvailableBytesIn == 0)
				{
					return;
				}
			}
		}

		DeflateBlockState DeflateFast(bool flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookAhead < 262)
				{
					this.FillWindow();
					if (this.lookAhead < 262 && !flush)
					{
						break;
					}
					if (this.lookAhead == 0)
					{
						goto IL_1C5;
					}
				}
				if (this.lookAhead >= 3)
				{
					num = this.UpdateHead();
				}
				if ((long)num != 0L && ((this.startOfString - num) & 65535) <= this.windowSize - 262)
				{
					this.matchLength = this.LongestMatch(num);
				}
				bool flag;
				if (this.matchLength >= 3)
				{
					flag = this.TreeTally(this.startOfString - this.matchStart, this.matchLength - 3);
					this.lookAhead -= this.matchLength;
					if (this.matchLength <= this.maxLazy && this.lookAhead >= 3)
					{
						this.matchLength--;
						do
						{
							this.startOfString++;
							num = this.UpdateHead();
						}
						while (--this.matchLength != 0);
						this.startOfString++;
					}
					else
					{
						this.startOfString += this.matchLength;
						this.matchLength = 0;
						this.hashIndexOfString = (int)(this.window[this.startOfString] & byte.MaxValue);
						this.hashIndexOfString = ((this.hashIndexOfString << this.hashShift) ^ (int)(this.window[this.startOfString + 1] & byte.MaxValue)) & this.hashMask;
					}
				}
				else
				{
					flag = this.TreeTally(0, (int)(this.window[this.startOfString] & byte.MaxValue));
					this.lookAhead--;
					this.startOfString++;
				}
				if (flag)
				{
					this.FlushBlockOnly(false);
					if (base.AvailableBytesOut == 0)
					{
						return DeflateBlockState.NeedMore;
					}
				}
			}
			return DeflateBlockState.NeedMore;
			IL_1C5:
			return this.CompleteFlushBlock(flush);
		}

		int UpdateHead()
		{
			this.hashIndexOfString = ((this.hashIndexOfString << this.hashShift) ^ (int)(this.window[this.startOfString + 3 - 1] & byte.MaxValue)) & this.hashMask;
			int result = (int)this.head[this.hashIndexOfString] & 65535;
			this.previous[this.startOfString & this.windowMask] = this.head[this.hashIndexOfString];
			this.head[this.hashIndexOfString] = (short)this.startOfString;
			return result;
		}

		DeflateBlockState CompleteFlushBlock(bool flush)
		{
			this.FlushBlockOnly(flush);
			if (base.AvailableBytesOut == 0)
			{
				if (!flush)
				{
					return DeflateBlockState.NeedMore;
				}
				return DeflateBlockState.FinishStarted;
			}
			else
			{
				if (!flush)
				{
					return DeflateBlockState.BlockDone;
				}
				return DeflateBlockState.FinishDone;
			}
		}

		DeflateBlockState DeflateNone(bool flush)
		{
			int num = 65535;
			if (num > this.pending.Length - 5)
			{
				num = this.pending.Length - 5;
			}
			for (;;)
			{
				if (this.lookAhead <= 1)
				{
					this.FillWindow();
					if (this.lookAhead == 0 && !flush)
					{
						break;
					}
					if (this.lookAhead == 0)
					{
						goto IL_D1;
					}
				}
				this.startOfString += this.lookAhead;
				this.lookAhead = 0;
				int num2 = this.blockStart + num;
				if (this.startOfString == 0 || this.startOfString >= num2)
				{
					this.lookAhead = this.startOfString - num2;
					this.startOfString = num2;
					this.FlushBlockOnly(false);
					if (base.AvailableBytesOut == 0)
					{
						return DeflateBlockState.NeedMore;
					}
				}
				if (this.startOfString - this.blockStart >= this.windowSize - 262)
				{
					this.FlushBlockOnly(false);
					if (base.AvailableBytesOut == 0)
					{
						return DeflateBlockState.NeedMore;
					}
				}
			}
			return DeflateBlockState.NeedMore;
			IL_D1:
			return this.CompleteFlushBlock(flush);
		}

		DeflateBlockState DeflateSlow(bool flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookAhead < 262)
				{
					this.FillWindow();
					if (this.lookAhead < 262 && !flush)
					{
						break;
					}
					if (this.lookAhead == 0)
					{
						goto IL_21C;
					}
				}
				if (this.lookAhead >= 3)
				{
					num = this.UpdateHead();
				}
				this.previousLength = this.matchLength;
				this.previousMatch = this.matchStart;
				this.matchLength = 2;
				if (num != 0 && this.previousLength < this.maxLazy && ((this.startOfString - num) & 65535) <= this.windowSize - 262)
				{
					this.matchLength = this.LongestMatch(num);
					if (this.matchLength <= 5 && this.matchLength == 3 && this.startOfString - this.matchStart > 4096)
					{
						this.matchLength = 2;
					}
				}
				if (this.previousLength >= 3 && this.matchLength <= this.previousLength)
				{
					int num2 = this.startOfString + this.lookAhead - 3;
					bool flag = this.TreeTally(this.startOfString - 1 - this.previousMatch, this.previousLength - 3);
					this.lookAhead -= this.previousLength - 1;
					this.previousLength -= 2;
					do
					{
						if (++this.startOfString <= num2)
						{
							num = this.UpdateHead();
						}
					}
					while (--this.previousLength != 0);
					this.matchAvailable = 0;
					this.matchLength = 2;
					this.startOfString++;
					if (flag)
					{
						this.FlushBlockOnly(false);
						if (base.AvailableBytesOut == 0)
						{
							return DeflateBlockState.NeedMore;
						}
					}
				}
				else if (this.matchAvailable != 0)
				{
					bool flag = this.TreeTally(0, (int)(this.window[this.startOfString - 1] & byte.MaxValue));
					if (flag)
					{
						this.FlushBlockOnly(false);
					}
					this.startOfString++;
					this.lookAhead--;
					if (base.AvailableBytesOut == 0)
					{
						return DeflateBlockState.NeedMore;
					}
				}
				else
				{
					this.matchAvailable = 1;
					this.startOfString++;
					this.lookAhead--;
				}
			}
			return DeflateBlockState.NeedMore;
			IL_21C:
			if (this.matchAvailable != 0)
			{
				bool flag = this.TreeTally(0, (int)(this.window[this.startOfString - 1] & byte.MaxValue));
				this.matchAvailable = 0;
			}
			return this.CompleteFlushBlock(flush);
		}

		int LongestMatch(int currentMatch)
		{
			int num = this.maxChainLength;
			int num2 = this.startOfString;
			int num3 = this.previousLength;
			int num4 = ((this.startOfString > this.windowSize - 262) ? (this.startOfString - (this.windowSize - 262)) : 0);
			int num5 = this.niceLength;
			int num6 = this.windowMask;
			int num7 = this.startOfString + 258;
			int num8 = num2 + num3;
			byte b = this.window[num8 - 1];
			byte b2 = this.window[num8];
			if (this.previousLength >= this.goodLength)
			{
				num >>= 2;
			}
			if (num5 > this.lookAhead)
			{
				num5 = this.lookAhead;
			}
			do
			{
				int num9 = currentMatch;
				if (this.window[num9 + num3] == b2 && this.window[num9 + num3 - 1] == b && this.window[num9] == this.window[num2] && this.window[++num9] == this.window[num2 + 1])
				{
					num2 += 2;
					num9++;
					while (this.window[++num2] == this.window[++num9] && this.window[++num2] == this.window[++num9] && this.window[++num2] == this.window[++num9] && this.window[++num2] == this.window[++num9] && this.window[++num2] == this.window[++num9] && this.window[++num2] == this.window[++num9] && this.window[++num2] == this.window[++num9] && this.window[++num2] == this.window[++num9] && num2 < num7)
					{
					}
					int num10 = 258 - (num7 - num2);
					num2 = num7 - 258;
					if (num10 > num3)
					{
						this.matchStart = currentMatch;
						num3 = num10;
						if (num10 >= num5)
						{
							break;
						}
						b = this.window[num2 + num3 - 1];
						b2 = this.window[num2 + num3];
					}
				}
			}
			while ((currentMatch = (int)this.previous[currentMatch & num6] & 65535) > num4 && --num != 0);
			if (num3 <= this.lookAhead)
			{
				return num3;
			}
			return this.lookAhead;
		}

		void Initialize(DeflateSettings deflateSettings)
		{
			this.compressionLevel = (int)deflateSettings.CompressionLevel;
			this.windowBits = 15;
			int num = 8;
			this.windowSize = 1 << this.windowBits;
			this.windowMask = this.windowSize - 1;
			this.hashBits = num + 7;
			this.hashSize = 1 << this.hashBits;
			this.hashMask = this.hashSize - 1;
			this.hashShift = (this.hashBits + 3 - 1) / 3;
			this.window = new byte[this.windowSize * 2];
			this.previous = new short[this.windowSize];
			this.head = new short[this.hashSize];
			this.literalsBufsize = 1 << num + 6;
			this.pending = new byte[this.literalsBufsize * 4];
			this.distanceOffset = this.literalsBufsize;
			this.lengthOffset = 3 * this.literalsBufsize;
			base.TotalBytesIn = (base.TotalBytesOut = 0);
			this.pendingCount = 0;
			this.nextPending = 0;
			this.InitializeTreeData();
			this.InitializeLazyMatch();
		}

		void InitializeLazyMatch()
		{
			this.actualWindowSize = 2 * this.windowSize;
			Array.Clear(this.head, 0, this.hashSize);
			this.SetConfiguration(this.compressionLevel);
			this.startOfString = 0;
			this.blockStart = 0;
			this.lookAhead = 0;
			this.matchLength = (this.previousLength = 2);
			this.matchAvailable = 0;
			this.hashIndexOfString = 0;
		}

		void InitializeTreeData()
		{
			this.treeLiterals.DynamicTree = this.dynamicLiteralsTree;
			this.treeLiterals.StaticTree = StaticTree.Literals;
			this.treeDistances.DynamicTree = this.dynamicDistanceTree;
			this.treeDistances.StaticTree = StaticTree.Distances;
			this.treeBitLengths.DynamicTree = this.dynamicBitLengthTree;
			this.treeBitLengths.StaticTree = StaticTree.BitLengths;
			this.bitsBuffer = 0;
			this.bitsValid = 0;
			this.InitializeBlocks();
		}

		void InitializeBlocks()
		{
			for (int i = 0; i < 286; i++)
			{
				this.dynamicLiteralsTree[i * 2] = 0;
			}
			for (int j = 0; j < 30; j++)
			{
				this.dynamicDistanceTree[j * 2] = 0;
			}
			for (int k = 0; k < 19; k++)
			{
				this.dynamicBitLengthTree[k * 2] = 0;
			}
			this.dynamicLiteralsTree[512] = 1;
			this.OptimalLength = (this.StaticLength = 0);
			this.lastLiteral = (this.matches = 0);
		}

		void SetConfiguration(int level)
		{
			DeflateConfiguration deflateConfiguration = DeflateConfiguration.Lookup(level);
			this.goodLength = deflateConfiguration.GoodLength;
			this.maxChainLength = deflateConfiguration.MaxChainLength;
			this.maxLazy = deflateConfiguration.MaxLazy;
			this.niceLength = deflateConfiguration.NiceLength;
			if (level <= 0)
			{
				this.deflateFunction = new DeflateCompressor.CompressionMethod(this.DeflateNone);
				return;
			}
			if (level > 3)
			{
				this.deflateFunction = new DeflateCompressor.CompressionMethod(this.DeflateSlow);
				return;
			}
			this.deflateFunction = new DeflateCompressor.CompressionMethod(this.DeflateFast);
		}

		void FlushPending()
		{
			int availableBytesOut = this.pendingCount;
			if (availableBytesOut > base.AvailableBytesOut)
			{
				availableBytesOut = base.AvailableBytesOut;
			}
			if (availableBytesOut == 0)
			{
				return;
			}
			if (this.pending.Length <= this.nextPending || base.OutputBuffer.Length <= base.NextOut || this.pending.Length < this.nextPending + availableBytesOut || base.OutputBuffer.Length < base.NextOut + availableBytesOut)
			{
				throw new InvalidOperationException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.pending.Length, this.pendingCount));
			}
			Array.Copy(this.pending, this.nextPending, base.OutputBuffer, base.NextOut, availableBytesOut);
			base.NextOut += availableBytesOut;
			this.nextPending += availableBytesOut;
			base.TotalBytesOut += availableBytesOut;
			base.AvailableBytesOut -= availableBytesOut;
			this.pendingCount -= availableBytesOut;
			if (this.pendingCount == 0)
			{
				this.nextPending = 0;
			}
		}

		int ReadBuffer(byte[] buffer, int start, int size)
		{
			int num = base.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			base.AvailableBytesIn -= num;
			Array.Copy(base.InputBuffer, base.NextIn, buffer, start, num);
			base.NextIn += num;
			base.TotalBytesIn += num;
			return num;
		}

		const int WindowBitsDefault = 15;

		const int DefaultMemoryLevel = 8;

		const int StaticTreeBlock = 1;

		const int DynamicTreeBlock = 2;

		const int BufferSize = 16;

		const int MinMatch = 3;

		const int MaxMatch = 258;

		const int MinLookAhead = 262;

		const int EndBlock = 256;

		int goodLength;

		int maxChainLength;

		int maxLazy;

		int niceLength;

		DeflateCompressor.CompressionMethod deflateFunction;

		bool finishPending;

		int windowSize;

		int windowBits;

		int windowMask;

		byte[] window;

		int actualWindowSize;

		short[] head;

		short[] previous;

		int hashIndexOfString;

		int hashSize;

		int hashBits;

		int hashMask;

		int hashShift;

		int blockStart;

		int matchLength;

		int previousMatch;

		int matchAvailable;

		int startOfString;

		int matchStart;

		int lookAhead;

		int previousLength;

		int compressionLevel;

		short[] dynamicLiteralsTree;

		short[] dynamicDistanceTree;

		short[] dynamicBitLengthTree;

		Tree treeLiterals = new Tree();

		Tree treeDistances = new Tree();

		Tree treeBitLengths = new Tree();

		int lengthOffset;

		int literalsBufsize;

		int lastLiteral;

		int distanceOffset;

		int matches;

		short bitsBuffer;

		int bitsValid;

		byte[] pending;

		int nextPending;

		int pendingCount;

		delegate DeflateBlockState CompressionMethod(bool flush);
	}
}
