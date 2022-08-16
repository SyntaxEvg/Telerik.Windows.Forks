using System;

namespace Telerik.Windows.Zip
{
	class LzmaDecompressor : LzmaTransformBase
	{
		public LzmaDecompressor(LzmaSettings settings)
			: base(settings)
		{
			this.dictionarySize = uint.MaxValue;
			int num = 0;
			while ((long)num < 4L)
			{
				this.positionSlotDecoder[num] = new LzmaBitTreeDecoder(6);
				num++;
			}
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
			if (base.Settings.UseZipHeader)
			{
				base.Header.BytesToRead = 14;
				return;
			}
			base.Header.BytesToRead = 18;
		}

		public override void ProcessHeader()
		{
			base.ProcessHeader();
			this.SetProperties();
			base.Header.BytesToRead = 0;
		}

		protected override bool ProcessTransform(bool finalBlock)
		{
			this.rangeDecoder.FinalBlock = finalBlock;
			this.SetInputBuffer();
			this.outputWindow.SetOutputBuffer(base.OutputBuffer);
			this.outputWindow.Flush();
			while (!this.rangeDecoder.InputRequired && !this.outputWindow.Full && this.decompressState != LzmaDecompressor.LzmaDecompressorState.Done)
			{
				this.Decompress();
				this.outputWindow.Flush();
			}
			base.NextOut += this.outputWindow.Copied;
			return this.outputWindow.AvailableBytes > 0 || (this.decompressState != LzmaDecompressor.LzmaDecompressorState.Done && !this.rangeDecoder.InputRequired);
		}

		static void InvalidLzmaProperties()
		{
			throw new InvalidDataException("Invalid LZMA properties");
		}

		void SetProperties()
		{
			string text = string.Empty;
			string text2 = "Invalid header length";
			if (base.Settings.UseZipHeader)
			{
				if (base.Header.Buffer == null || base.Header.Buffer.Length != 14)
				{
					text = text2;
				}
				else
				{
					this.InitializeDecompressor(4);
					this.Init(base.Header.Buffer, 9);
				}
			}
			else if (base.Header.Buffer == null || base.Header.Buffer.Length != 18)
			{
				text = text2;
			}
			else
			{
				this.InitializeDecompressor(0);
				this.Init(base.Header.Buffer, 13);
				base.Settings.InternalStreamLength = BitConverter.ToInt64(base.Header.Buffer, 5);
			}
			if (text.Length != 0)
			{
				throw new InvalidDataException(text);
			}
		}

		void InitializeDecompressor(int propertiesIndex)
		{
			byte[] buffer = base.Header.Buffer;
			int lc = (int)(buffer[propertiesIndex] % 9);
			int num = (int)(buffer[propertiesIndex] / 9);
			int lp = num % 5;
			int num2 = num / 5;
			if (num2 > 4)
			{
				LzmaDecompressor.InvalidLzmaProperties();
			}
			uint num3 = 0U;
			propertiesIndex++;
			for (int i = 0; i < 4; i++)
			{
				num3 += (uint)((uint)buffer[propertiesIndex + i] << i * 8);
			}
			this.SetDictionarySize(num3);
			this.SetLiteralProperties(lp, lc);
			this.SetPosBitsProperties(num2);
		}

		void SetLiteralProperties(int lp, int lc)
		{
			if (lp > 8 || lc > 8)
			{
				LzmaDecompressor.InvalidLzmaProperties();
			}
			this.literalDecoder = new LzmaLiteralDecoder(lp, lc);
		}

		void SetPosBitsProperties(int pb)
		{
			if (pb > 4)
			{
				LzmaDecompressor.InvalidLzmaProperties();
			}
			uint num = 1U << pb;
			this.lengthDecoder = new LzmaLengthDecoder(num);
			this.repeaterLengthDecoder = new LzmaLengthDecoder(num);
			this.positionStateMask = num - 1U;
		}

		void SetDictionarySize(uint size)
		{
			this.dictionarySize = size;
			this.dictionarySizeCheck = Math.Max(this.dictionarySize, 1U);
			uint blockSize = Math.Max(this.dictionarySizeCheck, 4096U);
			this.outputWindow = new LzmaOutputWindow(blockSize);
		}

		void Init(byte[] inputBuffer, int startIndex)
		{
			this.lzmaState = default(LzmaState);
			this.rangeDecoder.Init(inputBuffer, startIndex);
			for (uint num = 0U; num < 12U; num += 1U)
			{
				for (uint num2 = 0U; num2 <= this.positionStateMask; num2 += 1U)
				{
					uint num3 = (num << 4) + num2;
					this.matchDecoders[(int)((UIntPtr)num3)].Init();
					this.repeaterLongDecoders[(int)((UIntPtr)num3)].Init();
				}
				this.repeaterDecoders[(int)((UIntPtr)num)].Init();
				this.repeaterG0Decoders[(int)((UIntPtr)num)].Init();
				this.repeaterG1Decoders[(int)((UIntPtr)num)].Init();
				this.repeaterG2Decoders[(int)((UIntPtr)num)].Init();
			}
			this.literalDecoder.Init();
			for (uint num = 0U; num < 4U; num += 1U)
			{
				this.positionSlotDecoder[(int)((UIntPtr)num)].Init();
			}
			for (uint num = 0U; num < 114U; num += 1U)
			{
				this.positionDecoders[(int)((UIntPtr)num)].Init();
			}
			this.lengthDecoder.Init();
			this.repeaterLengthDecoder.Init();
			this.positionAlignDecoder.Init();
		}

		void DecompressStart()
		{
			uint num = this.lzmaState.Index << 4;
			this.decodedState = this.matchDecoders[(int)((UIntPtr)num)].DecodeState(this.rangeDecoder);
			if (this.decodedState == 0U)
			{
				this.lzmaState.UpdateChar();
				byte value = this.literalDecoder.DecodeNormal(this.rangeDecoder, 0U, 0);
				this.outputWindow.PutByte(value);
				this.currentPosition += 1UL;
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.MatchDecoder;
				return;
			}
			if (this.currentPosition == 0UL && (base.AvailableBytesIn == 0 || base.AvailableBytesIn == 5))
			{
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.Done;
				return;
			}
			throw new InvalidDataException();
		}

		bool Decompress()
		{
			while (!this.rangeDecoder.InputRequired && !this.outputWindow.Full && this.decompressState != LzmaDecompressor.LzmaDecompressorState.Done)
			{
				switch (this.decompressState)
				{
				case LzmaDecompressor.LzmaDecompressorState.Start:
					this.DecompressStart();
					break;
				case LzmaDecompressor.LzmaDecompressorState.MatchDecoder:
					this.positionState = (uint)this.currentPosition & this.positionStateMask;
					this.DecompressMatchDecoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.LiteralDecoder:
					this.DecompressLiteralDecoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.RepeaterDecoder:
					this.DecompressRepeaterDecoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.RepeaterG0Decoder:
					this.DecompressRepeaterG0Decoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.Repeater0LongDecoder:
					this.DecompressRepeater0LongDecoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.RepeaterG1Decoder:
					this.DecompressRepeaterG1Decoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.RepeaterG2Decoder:
					this.DecompressRepeaterG2Decoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.RepeaterLengthDecoder:
					this.DecompressRepeaterLengthDecoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.LengthDecoder:
					this.DecompressLengthDecoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.PosSlotDecoder:
					this.DecompressPosSlotDecoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.PosSlotProcess:
					this.DecompressPosSlotProcess();
					break;
				case LzmaDecompressor.LzmaDecompressorState.ReverseDecoder:
					this.DecompressReverseDecoder();
					break;
				case LzmaDecompressor.LzmaDecompressorState.CopyBlock:
					this.DecompressCopyBlock();
					break;
				}
			}
			return false;
		}

		void DecompressMatchDecoder()
		{
			uint num = (this.lzmaState.Index << 4) + this.positionState;
			uint num2 = this.matchDecoders[(int)((UIntPtr)num)].DecodeState(this.rangeDecoder);
			bool inputRequired = this.rangeDecoder.InputRequired;
			bool flag = this.rangeDecoder.CheckInputRequired(1, true);
			if (!flag && !inputRequired && this.rangeDecoder.FinalBlock)
			{
				if (this.currentPosition < (ulong)base.Settings.InternalStreamLength)
				{
					flag = this.rangeDecoder.CheckInputRequired(0, true);
				}
				else
				{
					this.decompressState = LzmaDecompressor.LzmaDecompressorState.Done;
				}
			}
			if (flag)
			{
				this.decodedState = num2;
				if (this.decodedState == 0U)
				{
					this.decompressState = LzmaDecompressor.LzmaDecompressorState.LiteralDecoder;
					return;
				}
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.RepeaterDecoder;
			}
		}

		void DecompressLiteralDecoder()
		{
			byte @byte = this.outputWindow.GetByte(0U);
			if (this.rangeDecoder.CheckInputRequired(256, false))
			{
				byte value;
				if (this.lzmaState.IsCharState())
				{
					value = this.literalDecoder.DecodeNormal(this.rangeDecoder, (uint)this.currentPosition, @byte);
				}
				else
				{
					value = this.literalDecoder.DecodeWithMatchByte(this.rangeDecoder, (uint)this.currentPosition, @byte, this.outputWindow.GetByte(this.repeater));
				}
				this.outputWindow.PutByte(value);
				this.lzmaState.UpdateChar();
				this.currentPosition += 1UL;
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.MatchDecoder;
			}
		}

		void DecompressRepeaterDecoder()
		{
			uint num = this.repeaterDecoders[(int)((UIntPtr)this.lzmaState.Index)].DecodeState(this.rangeDecoder);
			if (!this.rangeDecoder.InputRequired)
			{
				this.decodedState = num;
				if (this.decodedState == 0U)
				{
					this.decompressState = LzmaDecompressor.LzmaDecompressorState.LengthDecoder;
					return;
				}
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.RepeaterG0Decoder;
			}
		}

		void DecompressRepeaterG0Decoder()
		{
			uint num = this.repeaterG0Decoders[(int)((UIntPtr)this.lzmaState.Index)].DecodeState(this.rangeDecoder);
			if (!this.rangeDecoder.InputRequired)
			{
				this.decodedState = num;
				if (this.decodedState == 0U)
				{
					this.decompressState = LzmaDecompressor.LzmaDecompressorState.Repeater0LongDecoder;
					return;
				}
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.RepeaterG1Decoder;
			}
		}

		void DecompressRepeater0LongDecoder()
		{
			uint num = (this.lzmaState.Index << 4) + this.positionState;
			uint num2 = this.repeaterLongDecoders[(int)((UIntPtr)num)].DecodeState(this.rangeDecoder);
			if (!this.rangeDecoder.InputRequired)
			{
				this.decodedState = num2;
				if (this.decodedState == 0U)
				{
					this.lzmaState.UpdateShortRep();
					this.outputWindow.PutByte(this.outputWindow.GetByte(this.repeater));
					this.currentPosition += 1UL;
					this.decompressState = LzmaDecompressor.LzmaDecompressorState.MatchDecoder;
					return;
				}
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.RepeaterLengthDecoder;
			}
		}

		void DecompressRepeaterLengthDecoder()
		{
			this.decodedLength = this.repeaterLengthDecoder.Decode(this.rangeDecoder, this.positionState) + 2U;
			if (!this.rangeDecoder.InputRequired)
			{
				this.lzmaState.UpdateRep();
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.CopyBlock;
			}
		}

		void DecompressRepeaterG1Decoder()
		{
			uint num = this.repeaterG1Decoders[(int)((UIntPtr)this.lzmaState.Index)].DecodeState(this.rangeDecoder);
			if (!this.rangeDecoder.InputRequired)
			{
				this.decodedState = num;
				if (this.decodedState == 0U)
				{
					uint num2 = this.repeaterPosition1;
					this.repeaterPosition1 = this.repeater;
					this.repeater = num2;
					this.decompressState = LzmaDecompressor.LzmaDecompressorState.RepeaterLengthDecoder;
					return;
				}
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.RepeaterG2Decoder;
			}
		}

		void DecompressRepeaterG2Decoder()
		{
			uint num = this.repeaterG2Decoders[(int)((UIntPtr)this.lzmaState.Index)].DecodeState(this.rangeDecoder);
			if (!this.rangeDecoder.InputRequired)
			{
				this.decodedState = num;
				uint num2;
				if (this.decodedState == 0U)
				{
					num2 = this.repeaterPosition2;
				}
				else
				{
					num2 = this.repeaterPosition3;
					this.repeaterPosition3 = this.repeaterPosition2;
				}
				this.repeaterPosition2 = this.repeaterPosition1;
				this.repeaterPosition1 = this.repeater;
				this.repeater = num2;
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.RepeaterLengthDecoder;
			}
		}

		void DecompressCopyBlock()
		{
			if ((ulong)this.repeater < this.currentPosition && this.repeater < this.dictionarySizeCheck)
			{
				this.outputWindow.CopyBlock(this.repeater, this.decodedLength);
				this.currentPosition += (ulong)this.decodedLength;
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.MatchDecoder;
				return;
			}
			if (this.repeater == 4294967295U)
			{
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.Done;
				return;
			}
			throw new InvalidDataException();
		}

		void DecompressReverseDecoder()
		{
			uint num = this.positionAlignDecoder.ReverseDecode(this.rangeDecoder);
			if (!this.rangeDecoder.InputRequired)
			{
				this.repeater += num;
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.CopyBlock;
			}
		}

		void DecompressPosSlotProcess()
		{
			if (this.positionSlot >= 4U)
			{
				int num = (int)((this.positionSlot >> 1) - 1U);
				this.repeater = (2U | (this.positionSlot & 1U)) << num;
				if (this.positionSlot < 14U)
				{
					uint num2 = LzmaBitTreeDecoder.ReverseDecode(this.positionDecoders, this.repeater - this.positionSlot - 1U, this.rangeDecoder, num);
					if (this.rangeDecoder.InputRequired)
					{
						return;
					}
					this.repeater += num2;
				}
				else
				{
					uint num3 = this.rangeDecoder.DecodeDirectBits(num - 4) << 4;
					if (this.rangeDecoder.InputRequired)
					{
						return;
					}
					this.repeater += num3;
					this.decompressState = LzmaDecompressor.LzmaDecompressorState.ReverseDecoder;
					return;
				}
			}
			else
			{
				this.repeater = this.positionSlot;
			}
			this.decompressState = LzmaDecompressor.LzmaDecompressorState.CopyBlock;
		}

		void DecompressPosSlotDecoder()
		{
			this.positionSlot = this.positionSlotDecoder[(int)((UIntPtr)LzmaState.GetLenToPosState(this.decodedLength))].Decode(this.rangeDecoder);
			if (!this.rangeDecoder.InputRequired)
			{
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.PosSlotProcess;
			}
		}

		void DecompressLengthDecoder()
		{
			this.decodedLength = 2U + this.lengthDecoder.Decode(this.rangeDecoder, this.positionState);
			if (!this.rangeDecoder.InputRequired)
			{
				this.repeaterPosition3 = this.repeaterPosition2;
				this.repeaterPosition2 = this.repeaterPosition1;
				this.repeaterPosition1 = this.repeater;
				this.lzmaState.UpdateMatch();
				this.decompressState = LzmaDecompressor.LzmaDecompressorState.PosSlotDecoder;
			}
		}

		void SetInput(byte[] inputBytes, int length)
		{
			this.rangeDecoder.SetBuffer(inputBytes, length);
		}

		void SetInputBuffer()
		{
			if (base.AvailableBytesIn > 0)
			{
				byte[] array = new byte[base.AvailableBytesIn];
				Array.Copy(base.InputBuffer, 0, array, base.NextIn, array.Length);
				this.SetInput(array, base.AvailableBytesIn);
			}
		}

		LzmaState lzmaState;

		uint dictionarySize;

		uint dictionarySizeCheck;

		LzmaLengthDecoder lengthDecoder;

		LzmaLengthDecoder repeaterLengthDecoder;

		LzmaLiteralDecoder literalDecoder;

		LzmaBitTreeDecoder[] positionSlotDecoder = new LzmaBitTreeDecoder[4];

		LzmaOutputWindow outputWindow;

		LzmaRangeDecoder rangeDecoder = new LzmaRangeDecoder();

		LzmaRangeBitDecoder[] matchDecoders = new LzmaRangeBitDecoder[192];

		LzmaRangeBitDecoder[] repeaterDecoders = new LzmaRangeBitDecoder[12];

		LzmaRangeBitDecoder[] repeaterG0Decoders = new LzmaRangeBitDecoder[12];

		LzmaRangeBitDecoder[] repeaterG1Decoders = new LzmaRangeBitDecoder[12];

		LzmaRangeBitDecoder[] repeaterG2Decoders = new LzmaRangeBitDecoder[12];

		LzmaRangeBitDecoder[] repeaterLongDecoders = new LzmaRangeBitDecoder[192];

		LzmaRangeBitDecoder[] positionDecoders = new LzmaRangeBitDecoder[114];

		LzmaBitTreeDecoder positionAlignDecoder = new LzmaBitTreeDecoder(4);

		LzmaDecompressor.LzmaDecompressorState decompressState;

		ulong currentPosition;

		uint repeater;

		uint repeaterPosition1;

		uint repeaterPosition2;

		uint repeaterPosition3;

		uint decodedLength;

		uint positionStateMask;

		uint decodedState;

		uint positionState;

		uint positionSlot;

		enum LzmaDecompressorState
		{
			Start,
			MatchDecoder,
			LiteralDecoder,
			RepeaterDecoder,
			RepeaterG0Decoder,
			Repeater0LongDecoder,
			RepeaterG1Decoder,
			RepeaterG2Decoder,
			RepeaterLengthDecoder,
			LengthDecoder,
			PosSlotDecoder,
			PosSlotProcess,
			ReverseDecoder,
			CopyBlock,
			Done
		}
	}
}
