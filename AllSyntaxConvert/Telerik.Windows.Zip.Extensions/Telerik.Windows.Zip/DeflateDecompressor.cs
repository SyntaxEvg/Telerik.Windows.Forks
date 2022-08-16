using System;
using System.Collections.Generic;

namespace Telerik.Windows.Zip
{
	class DeflateDecompressor : DeflateTransformBase
	{
		public DeflateDecompressor(DeflateSettings settings)
			: base(settings)
		{
			this.output = new OutputWindow();
			this.input = new InputBitsBuffer();
			this.codeList = new byte[320];
			this.codeLengthTreeCodeLength = new byte[19];
			this.Reset();
		}

		public override int OutputBlockSize
		{
			get
			{
				return 262144;
			}
		}

		public override void InitHeaderReading()
		{
			base.InitHeaderReading();
			if (base.Settings.HeaderType == CompressedStreamHeader.ZLib)
			{
				base.Header.BytesToRead = 2;
			}
		}

		public override void ProcessHeader()
		{
			base.ProcessHeader();
			if (base.Settings.HeaderType == CompressedStreamHeader.ZLib)
			{
				string text = string.Empty;
				if (base.Header.Buffer == null || base.Header.Buffer.Length != 2)
				{
					text = "Invalid header length";
				}
				else
				{
					int num = (int)base.Header.Buffer[0];
					int num2 = (num >> 4) + 8;
					int num3 = (int)base.Header.Buffer[1];
					if ((num & 15) != 8)
					{
						text = string.Format("Unknown compression method (0x{0:X2})", num);
					}
					else if (num2 > 15)
					{
						text = string.Format("Invalid window size ({0})", num2);
					}
					else if (((num << 8) + num3) % 31 != 0)
					{
						text = "Invalid header";
					}
				}
				if (text.Length != 0)
				{
					throw new InvalidDataException(text);
				}
			}
			base.Header.BytesToRead = 0;
		}

		protected override bool ProcessTransform(bool finalBlock)
		{
			this.SetInputBuffer();
			int num = this.Inflate(base.OutputBuffer, base.NextOut, base.AvailableBytesOut);
			base.NextOut += num;
			return this.output.AvailableBytes > 0 || (this.state != DeflateDecompressor.DecompressorState.Done && (!this.input.InputRequired || this.pendingInput.Count > 0));
		}

		static void ThrowInvalidData()
		{
			throw new InvalidDataException("Invalid data");
		}

		static void ThrowInvalidDataGeneric()
		{
			throw new InvalidDataException();
		}

		static void ThrowUnknownBlockType()
		{
			throw new InvalidDataException("Unknown block type");
		}

		static void ThrowUnknownState()
		{
			throw new InvalidDataException("Unknown state");
		}

		bool Decode()
		{
			if (this.state == DeflateDecompressor.DecompressorState.ReadingBFinal)
			{
				if (!this.input.CheckAvailable(1))
				{
					return false;
				}
				this.bfinal = this.input.GetBits(1);
				this.state = DeflateDecompressor.DecompressorState.ReadingBType;
			}
			if (this.state == DeflateDecompressor.DecompressorState.ReadingBType)
			{
				if (!this.input.CheckAvailable(2))
				{
					return false;
				}
				this.ProcessBlockType();
			}
			return this.CheckDecodeState();
		}

		void ProcessBlockType()
		{
			this.blockType = (DeflateDecompressor.BlockType)this.input.GetBits(2);
			switch (this.blockType)
			{
			case DeflateDecompressor.BlockType.Stored:
				this.state = DeflateDecompressor.DecompressorState.UncompressedAligning;
				return;
			case DeflateDecompressor.BlockType.Static:
				this.literalLengthTree = InflateTree.StaticLiteralLengthTree;
				this.distanceTree = InflateTree.StaticDistanceTree;
				this.state = DeflateDecompressor.DecompressorState.DecodeTop;
				return;
			case DeflateDecompressor.BlockType.Dynamic:
				this.state = DeflateDecompressor.DecompressorState.ReadingNumLitCodes;
				return;
			default:
				DeflateDecompressor.ThrowUnknownBlockType();
				return;
			}
		}

		bool CheckDecodeState()
		{
			bool flag = false;
			bool result;
			if (this.blockType == DeflateDecompressor.BlockType.Dynamic)
			{
				if (this.state >= DeflateDecompressor.DecompressorState.DecodeTop)
				{
					result = this.DecodeBlock(out flag);
				}
				else
				{
					result = this.DecodeDynamicBlockHeader();
				}
			}
			else if (this.blockType != DeflateDecompressor.BlockType.Static)
			{
				if (this.blockType != DeflateDecompressor.BlockType.Stored)
				{
					DeflateDecompressor.ThrowUnknownBlockType();
				}
				result = this.DecodeUncompressedBlock(out flag);
			}
			else
			{
				result = this.DecodeBlock(out flag);
			}
			if (flag && this.bfinal != 0)
			{
				this.state = DeflateDecompressor.DecompressorState.Done;
			}
			return result;
		}

		bool DecodeBlock(out bool endOfBlock)
		{
			int i = this.output.FreeBytes;
			endOfBlock = false;
			while (i > 258)
			{
				switch (this.state)
				{
				case DeflateDecompressor.DecompressorState.DecodeTop:
				{
					bool? flag = this.DecodeTop(ref endOfBlock, ref i);
					if (flag != null)
					{
						return flag.Value;
					}
					break;
				}
				case DeflateDecompressor.DecompressorState.HaveInitialLength:
				{
					bool? flag2 = this.DecodeInitialLength();
					if (flag2 != null)
					{
						return flag2.Value;
					}
					break;
				}
				case DeflateDecompressor.DecompressorState.HaveFullLength:
				{
					bool? flag3 = this.DecodeFullLength();
					if (flag3 != null)
					{
						return flag3.Value;
					}
					break;
				}
				case DeflateDecompressor.DecompressorState.HaveDistCode:
				{
					bool? flag4 = this.DecodeDistanceCode(ref i);
					if (flag4 != null)
					{
						return flag4.Value;
					}
					break;
				}
				default:
					DeflateDecompressor.ThrowUnknownState();
					break;
				}
			}
			return true;
		}

		bool? DecodeTop(ref bool endOfBlock, ref int freeBytes)
		{
			int nextSymbol = this.literalLengthTree.GetNextSymbol(this.input);
			if (nextSymbol < 0)
			{
				return new bool?(false);
			}
			if (nextSymbol >= 256)
			{
				if (nextSymbol == 256)
				{
					endOfBlock = true;
					this.state = DeflateDecompressor.DecompressorState.ReadingBFinal;
					return new bool?(true);
				}
				this.DecodeDistance(ref nextSymbol);
				this.state = DeflateDecompressor.DecompressorState.HaveInitialLength;
			}
			else
			{
				this.output.AddByte((byte)nextSymbol);
				freeBytes--;
			}
			return null;
		}

		void DecodeDistance(ref int nextSymbol)
		{
			nextSymbol -= 257;
			if (nextSymbol < 8)
			{
				nextSymbol += 3;
				this.extraBits = 0;
			}
			else if (nextSymbol != 28)
			{
				if (nextSymbol < 0 || nextSymbol >= Tree.ExtraLengthBits.Length)
				{
					DeflateDecompressor.ThrowInvalidData();
				}
				this.extraBits = Tree.ExtraLengthBits[nextSymbol];
			}
			else
			{
				nextSymbol = 258;
				this.extraBits = 0;
			}
			this.blockLength = nextSymbol;
		}

		bool? DecodeInitialLength()
		{
			if (this.extraBits > 0)
			{
				int bits = this.input.GetBits(this.extraBits);
				if (bits < 0)
				{
					return new bool?(false);
				}
				if (this.blockLength < 0 || this.blockLength >= DeflateDecompressor.lengthBase.Length)
				{
					DeflateDecompressor.ThrowInvalidData();
				}
				this.blockLength = DeflateDecompressor.lengthBase[this.blockLength] + bits;
			}
			this.state = DeflateDecompressor.DecompressorState.HaveFullLength;
			return null;
		}

		bool? DecodeFullLength()
		{
			if (this.blockType != DeflateDecompressor.BlockType.Dynamic)
			{
				this.distanceCode = this.input.GetBits(5);
				if (this.distanceCode >= 0)
				{
					this.distanceCode = (int)DeflateDecompressor.staticDistanceTreeTable[this.distanceCode];
				}
			}
			else
			{
				this.distanceCode = this.distanceTree.GetNextSymbol(this.input);
			}
			if (this.distanceCode < 0)
			{
				return new bool?(false);
			}
			this.state = DeflateDecompressor.DecompressorState.HaveDistCode;
			return null;
		}

		bool? DecodeDistanceCode(ref int freeBytes)
		{
			int distance;
			if (this.distanceCode <= 3)
			{
				distance = this.distanceCode + 1;
			}
			else
			{
				this.extraBits = this.distanceCode - 2 >> 1;
				int bits = this.input.GetBits(this.extraBits);
				if (bits < 0)
				{
					return new bool?(false);
				}
				distance = DeflateDecompressor.distanceBasePosition[this.distanceCode] + bits;
			}
			this.output.Copy(this.blockLength, distance);
			freeBytes -= this.blockLength;
			this.state = DeflateDecompressor.DecompressorState.DecodeTop;
			return null;
		}

		bool DecodeDynamicBlockHeader()
		{
			switch (this.state)
			{
			case DeflateDecompressor.DecompressorState.ReadingNumLitCodes:
				this.literalLengthCodeCount = this.input.GetBits(5);
				if (this.literalLengthCodeCount < 0)
				{
					return false;
				}
				this.literalLengthCodeCount += 257;
				this.state = DeflateDecompressor.DecompressorState.ReadingNumDistCodes;
				break;
			case DeflateDecompressor.DecompressorState.ReadingNumDistCodes:
				break;
			case DeflateDecompressor.DecompressorState.ReadingNumCodeLengthCodes:
				goto IL_94;
			case DeflateDecompressor.DecompressorState.ReadingCodeLengthCodes:
				goto IL_CD;
			case DeflateDecompressor.DecompressorState.ReadingTreeCodesBefore:
			case DeflateDecompressor.DecompressorState.ReadingTreeCodesAfter:
				goto IL_DE;
			default:
				DeflateDecompressor.ThrowUnknownState();
				return true;
			}
			this.distanceCodeCount = this.input.GetBits(5);
			if (this.distanceCodeCount < 0)
			{
				return false;
			}
			this.distanceCodeCount++;
			this.state = DeflateDecompressor.DecompressorState.ReadingNumCodeLengthCodes;
			IL_94:
			this.codeLengthCodeCount = this.input.GetBits(4);
			if (this.codeLengthCodeCount < 0)
			{
				return false;
			}
			this.codeLengthCodeCount += 4;
			this.loopCounter = 0;
			this.state = DeflateDecompressor.DecompressorState.ReadingCodeLengthCodes;
			IL_CD:
			if (!this.DecodeDynamicCodes())
			{
				return false;
			}
			this.state = DeflateDecompressor.DecompressorState.ReadingTreeCodesBefore;
			IL_DE:
			return this.ReadTreeCodes();
		}

		bool DecodeDynamicCodes()
		{
			while (this.loopCounter < this.codeLengthCodeCount)
			{
				int bits = this.input.GetBits(3);
				if (bits < 0)
				{
					return false;
				}
				this.codeLengthTreeCodeLength[(int)Tree.BitLengthOrder[this.loopCounter]] = (byte)bits;
				this.loopCounter++;
			}
			for (int i = this.codeLengthCodeCount; i < Tree.BitLengthOrder.Length; i++)
			{
				this.codeLengthTreeCodeLength[(int)Tree.BitLengthOrder[i]] = 0;
			}
			this.codeLengthTree = new InflateTree(this.codeLengthTreeCodeLength);
			this.codeArraySize = this.literalLengthCodeCount + this.distanceCodeCount;
			this.loopCounter = 0;
			return true;
		}

		bool ReadTreeCodes()
		{
			while (this.loopCounter < this.codeArraySize)
			{
				if (this.state == DeflateDecompressor.DecompressorState.ReadingTreeCodesBefore)
				{
					this.lengthCode = this.codeLengthTree.GetNextSymbol(this.input);
					if (this.lengthCode < 0)
					{
						return false;
					}
				}
				if (this.lengthCode > 15)
				{
					if (!this.input.CheckAvailable(7))
					{
						this.state = DeflateDecompressor.DecompressorState.ReadingTreeCodesAfter;
						return false;
					}
					this.SetPreviousCode();
				}
				else
				{
					this.codeList[this.loopCounter++] = (byte)this.lengthCode;
				}
				this.state = DeflateDecompressor.DecompressorState.ReadingTreeCodesBefore;
			}
			this.SetTreeCodes();
			this.state = DeflateDecompressor.DecompressorState.DecodeTop;
			return true;
		}

		void SetPreviousCode()
		{
			byte b = 0;
			int num;
			if (this.lengthCode == 16)
			{
				if (this.loopCounter == 0)
				{
					DeflateDecompressor.ThrowInvalidDataGeneric();
				}
				b = this.codeList[this.loopCounter - 1];
				num = this.input.GetBits(2) + 3;
			}
			else if (this.lengthCode != 17)
			{
				num = this.input.GetBits(7) + 11;
			}
			else
			{
				num = this.input.GetBits(3) + 3;
			}
			if (this.loopCounter + num > this.codeArraySize)
			{
				DeflateDecompressor.ThrowInvalidDataGeneric();
			}
			for (int i = 0; i < num; i++)
			{
				this.codeList[this.loopCounter++] = b;
			}
		}

		void SetTreeCodes()
		{
			byte[] array = new byte[288];
			byte[] array2 = new byte[32];
			Array.Copy(this.codeList, array, this.literalLengthCodeCount);
			Array.Copy(this.codeList, this.literalLengthCodeCount, array2, 0, this.distanceCodeCount);
			if (array[256] == 0)
			{
				DeflateDecompressor.ThrowInvalidDataGeneric();
			}
			this.literalLengthTree = new InflateTree(array);
			this.distanceTree = new InflateTree(array2);
		}

		bool DecodeUncompressedBlock(out bool endOfBlock)
		{
			endOfBlock = false;
			while (this.state != DeflateDecompressor.DecompressorState.DecodingUncompressed)
			{
				switch (this.state)
				{
				case DeflateDecompressor.DecompressorState.UncompressedAligning:
					this.input.SkipToByteBoundary();
					this.state = DeflateDecompressor.DecompressorState.UncompressedByte1;
					break;
				case DeflateDecompressor.DecompressorState.UncompressedByte1:
				case DeflateDecompressor.DecompressorState.UncompressedByte2:
				case DeflateDecompressor.DecompressorState.UncompressedByte3:
				case DeflateDecompressor.DecompressorState.UncompressedByte4:
					if (!this.DecodeBits())
					{
						return false;
					}
					break;
				default:
					DeflateDecompressor.ThrowUnknownState();
					break;
				}
			}
			int num = this.output.ReadInput(this.input, this.blockLength);
			this.blockLength -= num;
			if (this.blockLength == 0)
			{
				this.state = DeflateDecompressor.DecompressorState.ReadingBFinal;
				endOfBlock = true;
				return true;
			}
			return this.output.FreeBytes == 0;
		}

		bool DecodeBits()
		{
			int bits = this.input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			this.blockLengthBuffer[this.state - DeflateDecompressor.DecompressorState.UncompressedByte1] = (byte)bits;
			if (this.state == DeflateDecompressor.DecompressorState.UncompressedByte4)
			{
				this.blockLength = (int)this.blockLengthBuffer[0] + (int)this.blockLengthBuffer[1] * 256;
				int num = (int)this.blockLengthBuffer[2] + (int)this.blockLengthBuffer[3] * 256;
				if ((ushort)this.blockLength != (ushort)(~(ushort)num))
				{
					throw new InvalidDataException("Invalid block length");
				}
			}
			this.state++;
			return true;
		}

		int Inflate(byte[] bytes, int offset, int length)
		{
			int num = 0;
			while (length > 0 && this.state != DeflateDecompressor.DecompressorState.Done)
			{
				int num2 = this.output.Read(bytes, offset, length);
				if (num2 > 0)
				{
					offset += num2;
					num += num2;
					length -= num2;
				}
				if (!this.Decode() && (this.state == DeflateDecompressor.DecompressorState.Done || !this.TrySetInputBuffer()))
				{
					break;
				}
			}
			if (this.output.AvailableBytes > 0 && length > 0)
			{
				int num3 = this.output.Read(bytes, offset, length);
				num += num3;
			}
			return num;
		}

		void Reset()
		{
			this.state = DeflateDecompressor.DecompressorState.ReadingBFinal;
		}

		void SetInput(byte[] inputBytes, int offset, int length)
		{
			this.input.SetBuffer(inputBytes, offset, length);
		}

		void EnqueueInputBuffer()
		{
			if (base.AvailableBytesIn > 0)
			{
				byte[] array = new byte[base.AvailableBytesIn];
				Array.Copy(base.InputBuffer, 0, array, 0, array.Length);
				this.pendingInput.Enqueue(array);
			}
		}

		void SetInputBuffer()
		{
			if (this.input.InputRequired)
			{
				byte[] array;
				if (this.pendingInput.Count > 0)
				{
					this.EnqueueInputBuffer();
					array = this.pendingInput.Dequeue();
					base.AvailableBytesIn = array.Length;
				}
				else
				{
					array = new byte[base.AvailableBytesIn];
					Array.Copy(base.InputBuffer, 0, array, 0, array.Length);
				}
				this.SetInput(array, 0, base.AvailableBytesIn);
				return;
			}
			this.EnqueueInputBuffer();
		}

		bool TrySetInputBuffer()
		{
			if (this.input.InputRequired && this.pendingInput.Count > 0)
			{
				byte[] array = this.pendingInput.Dequeue();
				base.AvailableBytesIn = array.Length;
				this.SetInput(array, 0, base.AvailableBytesIn);
				return true;
			}
			return false;
		}

		static readonly int[] lengthBase = new int[]
		{
			3, 4, 5, 6, 7, 8, 9, 10, 11, 13,
			15, 17, 19, 23, 27, 31, 35, 43, 51, 59,
			67, 83, 99, 115, 131, 163, 195, 227, 258
		};

		static readonly int[] distanceBasePosition = new int[]
		{
			1, 2, 3, 4, 5, 7, 9, 13, 17, 25,
			33, 49, 65, 97, 129, 193, 257, 385, 513, 769,
			1025, 1537, 2049, 3073, 4097, 6145, 8193, 12289, 16385, 24577,
			0, 0
		};

		static readonly byte[] staticDistanceTreeTable = new byte[]
		{
			0, 16, 8, 24, 4, 20, 12, 28, 2, 18,
			10, 26, 6, 22, 14, 30, 1, 17, 9, 25,
			5, 21, 13, 29, 3, 19, 11, 27, 7, 23,
			15, 31
		};

		DeflateDecompressor.DecompressorState state;

		InputBitsBuffer input;

		OutputWindow output;

		int bfinal;

		DeflateDecompressor.BlockType blockType;

		byte[] blockLengthBuffer = new byte[4];

		int blockLength;

		int distanceCode;

		int extraBits;

		int loopCounter;

		int literalLengthCodeCount;

		int distanceCodeCount;

		int codeLengthCodeCount;

		int codeArraySize;

		int lengthCode;

		byte[] codeList;

		byte[] codeLengthTreeCodeLength;

		InflateTree literalLengthTree;

		InflateTree distanceTree;

		InflateTree codeLengthTree;

		Queue<byte[]> pendingInput = new Queue<byte[]>();

		enum DecompressorState
		{
			ReadingBFinal = 2,
			ReadingBType,
			ReadingNumLitCodes,
			ReadingNumDistCodes,
			ReadingNumCodeLengthCodes,
			ReadingCodeLengthCodes,
			ReadingTreeCodesBefore,
			ReadingTreeCodesAfter,
			DecodeTop,
			HaveInitialLength,
			HaveFullLength,
			HaveDistCode,
			UncompressedAligning = 15,
			UncompressedByte1,
			UncompressedByte2,
			UncompressedByte3,
			UncompressedByte4,
			DecodingUncompressed,
			Done = 24
		}

		enum BlockType
		{
			Stored,
			Static,
			Dynamic
		}
	}
}
