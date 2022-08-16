using System;

namespace Telerik.Windows.Zip
{
	class LzmaCompressor : LzmaTransformBase
	{
		static LzmaCompressor()
		{
			int num = 2;
			LzmaCompressor.fastPositions[0] = 0;
			LzmaCompressor.fastPositions[1] = 1;
			for (byte b = 2; b < 22; b += 1)
			{
				uint num2 = 1U << (b >> 1) - 1;
				uint num3 = 0U;
				while (num3 < num2)
				{
					LzmaCompressor.fastPositions[num] = b;
					num3 += 1U;
					num++;
				}
			}
		}

		public LzmaCompressor(LzmaSettings settings)
			: base(settings)
		{
			int num = 0;
			while ((long)num < 4096L)
			{
				this.optimizationData[num] = new LzmaOptimizationData();
				num++;
			}
			int num2 = 0;
			while ((long)num2 < 4L)
			{
				this.positionSlotEncoder[num2] = new LzmaBitTreeEncoder(6);
				num2++;
			}
			this.Initialize();
		}

		public override int OutputBlockSize
		{
			get
			{
				int num = (int)this.configDictionarySize;
				if (num < 8192)
				{
					return 8192;
				}
				if (num > 262144)
				{
					return 262144;
				}
				return num;
			}
		}

		public override void CreateHeader()
		{
			byte[] array;
			if (base.Settings.UseZipHeader)
			{
				array = new byte[] { 9, 20, 5, 0, 0, 0, 0, 0, 0 };
				this.CreatePropertiesHeader(array, 4);
				base.Header.CountHeaderInCompressedSize = true;
			}
			else
			{
				array = new byte[]
				{
					0, 0, 0, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
					byte.MaxValue, byte.MaxValue, byte.MaxValue
				};
				if (base.Settings.StreamLength > -1L)
				{
					byte[] bytes = BitConverter.GetBytes(base.Settings.StreamLength);
					Array.Copy(bytes, 0, array, 5, bytes.Length);
				}
				this.CreatePropertiesHeader(array, 0);
			}
			base.Header.Buffer = array;
		}

		protected override bool ProcessTransform(bool finalBlock)
		{
			this.rangeEncoder.SetOutputBuffer(base.OutputBuffer, base.NextOut);
			if (!this.finished)
			{
				this.matchFinder.SetInputBuffer(base.InputBuffer, base.NextIn, base.AvailableBytesIn, finalBlock);
				this.CodeOneBlock(finalBlock);
			}
			base.NextOut = this.rangeEncoder.NextOut;
			return this.rangeEncoder.HasData || this.matchFinder.HasInput;
		}

		protected override void Dispose(bool disposing)
		{
			this.rangeEncoder.Dispose();
			base.Dispose(disposing);
		}

		static uint GetPosSlot(uint index)
		{
			if (index < 2048U)
			{
				return (uint)LzmaCompressor.fastPositions[(int)((UIntPtr)index)];
			}
			if (index < 2097152U)
			{
				return (uint)(LzmaCompressor.fastPositions[(int)((UIntPtr)(index >> 10))] + 20);
			}
			return (uint)(LzmaCompressor.fastPositions[(int)((UIntPtr)(index >> 20))] + 40);
		}

		static uint GetPosSlot2(uint index)
		{
			if (index < 131072U)
			{
				return (uint)(LzmaCompressor.fastPositions[(int)((UIntPtr)(index >> 6))] + 12);
			}
			if (index < 134217728U)
			{
				return (uint)(LzmaCompressor.fastPositions[(int)((UIntPtr)(index >> 16))] + 32);
			}
			return (uint)(LzmaCompressor.fastPositions[(int)((UIntPtr)(index >> 26))] + 52);
		}

		static void CheckPrice(LzmaOptimizationData optimum, uint curAndLenPrice, uint current, uint backPrev)
		{
			if (curAndLenPrice < optimum.Price)
			{
				optimum.Price = curAndLenPrice;
				optimum.PosPrev = current;
				optimum.BackPrev = backPrev;
				optimum.Prev1IsChar = false;
			}
		}

		void InitRepeaterDistances()
		{
			this.currentPreviousByte = 0;
			for (uint num = 0U; num < 4U; num += 1U)
			{
				this.repeaterDistances[(int)((UIntPtr)num)] = 0U;
			}
		}

		void CreatePropertiesHeader(byte[] byteHeader, int propertiesIndex)
		{
			byteHeader[propertiesIndex] = (byte)((this.configPositionStateBits * 5 + this.configLiteralPositionStateBits) * 9 + this.configLiteralContextBits);
			propertiesIndex++;
			for (int i = 0; i < 4; i++)
			{
				byteHeader[propertiesIndex + i] = (byte)((this.configDictionarySize >> 8 * i) & 255U);
			}
		}

		void InitEncoders()
		{
			this.InitRepeaterDistances();
			for (uint num = 0U; num < 12U; num += 1U)
			{
				for (uint num2 = 0U; num2 <= this.configPositionStateMask; num2 += 1U)
				{
					uint num3 = (num << 4) + num2;
					this.matchRangeBitEncoders[(int)((UIntPtr)num3)].Init();
					this.repeaterLongRangeBitEncoders[(int)((UIntPtr)num3)].Init();
				}
				this.repeaterRangeBitEncoders[(int)((UIntPtr)num)].Init();
				this.repeaterG0RangeBitEncoders[(int)((UIntPtr)num)].Init();
				this.repeaterG1RangeBitEncoders[(int)((UIntPtr)num)].Init();
				this.repeaterG2RangeBitEncoders[(int)((UIntPtr)num)].Init();
			}
			for (uint num = 0U; num < 4U; num += 1U)
			{
				this.positionSlotEncoder[(int)((UIntPtr)num)].Init();
			}
			for (uint num = 0U; num < 114U; num += 1U)
			{
				this.positionEncoders[(int)((UIntPtr)num)].Init();
			}
			this.lengthEncoder.Init(1U << this.configPositionStateBits);
			this.repeaterMatchLengthEncoder.Init(1U << this.configPositionStateBits);
			this.positionAlignEncoder.Init();
			this.longestMatchWasFound = false;
			this.optimumEndIndex = 0U;
			this.optimumCurrentIndex = 0U;
			this.additionalOffset = 0U;
		}

		void ReadMatchDistances(out uint length)
		{
			length = 0U;
			this.distancePairsCounter = this.matchFinder.GetMatches(this.matchDistances);
			uint num = this.distancePairsCounter;
			if (num > 0U)
			{
				length = this.matchDistances[(int)((UIntPtr)(num - 2U))];
				if (length == this.configFastBytes)
				{
					length += this.matchFinder.GetMatchLen((int)(length - 1U), this.matchDistances[(int)((UIntPtr)(num - 1U))], 273U - length);
				}
			}
			this.additionalOffset += 1U;
		}

		void MovePosition(uint counter)
		{
			if (counter > 0U)
			{
				this.matchFinder.Skip(counter);
				this.additionalOffset += counter;
			}
		}

		uint GetRepeaterLen1Price(LzmaState state, uint positionState)
		{
			uint num = (state.Index << 4) + positionState;
			return this.repeaterG0RangeBitEncoders[(int)((UIntPtr)state.Index)].GetPrice0() + this.repeaterLongRangeBitEncoders[(int)((UIntPtr)num)].GetPrice0();
		}

		uint GetPureRepeaterPrice(uint index, LzmaState state, uint positionState)
		{
			uint num;
			if (index == 0U)
			{
				num = this.repeaterG0RangeBitEncoders[(int)((UIntPtr)state.Index)].GetPrice0();
				uint num2 = (state.Index << 4) + positionState;
				num += this.repeaterLongRangeBitEncoders[(int)((UIntPtr)num2)].GetPrice1();
			}
			else
			{
				num = this.repeaterG0RangeBitEncoders[(int)((UIntPtr)state.Index)].GetPrice1();
				if (index == 1U)
				{
					num += this.repeaterG1RangeBitEncoders[(int)((UIntPtr)state.Index)].GetPrice0();
				}
				else
				{
					num += this.repeaterG1RangeBitEncoders[(int)((UIntPtr)state.Index)].GetPrice1();
					num += this.repeaterG2RangeBitEncoders[(int)((UIntPtr)state.Index)].GetPrice(index - 2U);
				}
			}
			return num;
		}

		uint GetRepeaterPrice(uint index, uint length, LzmaState state, uint positionState)
		{
			uint price = this.repeaterMatchLengthEncoder.GetPrice(length - 2U, positionState);
			return price + this.GetPureRepeaterPrice(index, state, positionState);
		}

		uint GetPosLenPrice(uint position, uint length, uint positionState)
		{
			uint lenToPosState = LzmaState.GetLenToPosState(length);
			uint num;
			if (position < 128U)
			{
				num = this.distancesPrices[(int)((UIntPtr)(lenToPosState * 128U + position))];
			}
			else
			{
				uint num2 = (lenToPosState << 6) + LzmaCompressor.GetPosSlot2(position);
				num = this.positionSlotPrices[(int)((UIntPtr)num2)] + this.alignPrices[(int)((UIntPtr)(position & 15U))];
			}
			return num + this.lengthEncoder.GetPrice(length - 2U, positionState);
		}

		void Backward(uint current)
		{
			this.optimumEndIndex = current;
			uint posPrev = this.optimizationData[(int)((UIntPtr)current)].PosPrev;
			uint backPrev = this.optimizationData[(int)((UIntPtr)current)].BackPrev;
			do
			{
				if (this.optimizationData[(int)((UIntPtr)current)].Prev1IsChar)
				{
					this.optimizationData[(int)((UIntPtr)posPrev)].MakeAsChar();
					this.optimizationData[(int)((UIntPtr)posPrev)].PosPrev = posPrev - 1U;
					if (this.optimizationData[(int)((UIntPtr)current)].Prev2)
					{
						this.optimizationData[(int)((UIntPtr)(posPrev - 1U))].Prev1IsChar = false;
						this.optimizationData[(int)((UIntPtr)(posPrev - 1U))].PosPrev = this.optimizationData[(int)((UIntPtr)current)].PosPrev2;
						this.optimizationData[(int)((UIntPtr)(posPrev - 1U))].BackPrev = this.optimizationData[(int)((UIntPtr)current)].BackPrev2;
					}
				}
				uint num = posPrev;
				uint backPrev2 = backPrev;
				backPrev = this.optimizationData[(int)((UIntPtr)num)].BackPrev;
				posPrev = this.optimizationData[(int)((UIntPtr)num)].PosPrev;
				this.optimizationData[(int)((UIntPtr)num)].BackPrev = backPrev2;
				this.optimizationData[(int)((UIntPtr)num)].PosPrev = current;
				current = num;
			}
			while (current > 0U);
			this.optimumPosition = this.optimizationData[0].BackPrev;
			this.optimumLength = (this.optimumCurrentIndex = this.optimizationData[0].PosPrev);
		}

		void GetOptimum(uint position)
		{
			this.optimumPosition = position;
			this.optimumPositionState = position & this.configPositionStateMask;
			bool flag = this.CheckOptimumIndex();
			if (flag)
			{
				return;
			}
			this.optimumCurrent = 0U;
			for (;;)
			{
				this.optimumCurrent += 1U;
				this.optimumPosition += 1U;
				this.optimumPositionState = this.optimumPosition & this.configPositionStateMask;
				flag = this.CheckFastBytes();
				if (flag)
				{
					break;
				}
				this.GetCurrentState();
				this.ProcessOptimum();
			}
		}

		void ProcessOptimum()
		{
			uint num = this.optimumCurrent;
			uint position = this.optimumPosition;
			uint price = this.optimizationData[(int)((UIntPtr)num)].Price;
			byte indexByte = this.matchFinder.GetIndexByte(-1);
			byte indexByte2 = this.matchFinder.GetIndexByte((int)(-(long)this.repeaters[0] - 2L));
			uint num2 = this.optimumPositionState;
			uint num3 = (this.currentState.Index << 4) + num2;
			LzmaLiteralEncoder.Encoder subCoder = this.literalEncoder.GetSubCoder(position, this.matchFinder.GetIndexByte(-2));
			LzmaOptimizationData lzmaOptimizationData = this.optimizationData[(int)((UIntPtr)(num + 1U))];
			uint num4 = price + this.matchRangeBitEncoders[(int)((UIntPtr)num3)].GetPrice0() + subCoder.GetPrice(!this.currentState.IsCharState(), indexByte2, indexByte);
			bool flag = false;
			if (num4 < lzmaOptimizationData.Price)
			{
				lzmaOptimizationData.Price = num4;
				lzmaOptimizationData.PosPrev = num;
				lzmaOptimizationData.MakeAsChar();
				flag = true;
			}
			num3 = (this.currentState.Index << 4) + num2;
			uint num5 = price + this.matchRangeBitEncoders[(int)((UIntPtr)num3)].GetPrice1();
			uint num6 = num5 + this.repeaterRangeBitEncoders[(int)((UIntPtr)this.currentState.Index)].GetPrice1();
			if (indexByte2 == indexByte && (lzmaOptimizationData.PosPrev >= num || lzmaOptimizationData.BackPrev != 0U))
			{
				uint num7 = num6 + this.GetRepeaterLen1Price(this.currentState, num2);
				if (num7 <= lzmaOptimizationData.Price)
				{
					lzmaOptimizationData.Price = num7;
					lzmaOptimizationData.PosPrev = num;
					lzmaOptimizationData.MakeAsShortRep();
					flag = true;
				}
			}
			uint availableBytes = this.GetAvailableBytes(num);
			if (availableBytes < 2U)
			{
				return;
			}
			if (!flag && indexByte2 != indexByte)
			{
				this.TryLiteralRep0(num4);
			}
			this.ProcessOptimumDistances(num6, availableBytes, num5);
		}

		uint GetAvailableBytes(uint current)
		{
			this.optimumAvailableBytesFull = this.matchFinder.AvailableBytes + 1U;
			this.optimumAvailableBytesFull = System.Math.Min(4095U - current, this.optimumAvailableBytesFull);
			uint num = this.optimumAvailableBytesFull;
			if (num >= 2U && num > this.configFastBytes)
			{
				num = this.configFastBytes;
			}
			return num;
		}

		void UpdatePositionLengthPrice(uint startLen, uint matchPrice)
		{
			uint num = this.optimumCurrent;
			uint normalMatchPrice = matchPrice + this.repeaterRangeBitEncoders[(int)((UIntPtr)this.currentState.Index)].GetPrice0();
			while (this.optimumLengthEnd < num + this.optimumNewLength)
			{
				this.optimizationData[(int)((UIntPtr)(this.optimumLengthEnd += 1U))].Price = 268435455U;
			}
			this.UpdatePositionLengthPrice(startLen, num, normalMatchPrice);
		}

		void UpdatePositionLengthPrice(uint startLen, uint current, uint normalMatchPrice)
		{
			uint positionState = this.optimumPositionState;
			uint num = 0U;
			while (startLen > this.matchDistances[(int)((UIntPtr)num)])
			{
				num += 2U;
			}
			uint num2 = startLen;
			for (;;)
			{
				uint num3 = this.matchDistances[(int)((UIntPtr)(num + 1U))];
				uint num4 = normalMatchPrice + this.GetPosLenPrice(num3, num2, positionState);
				LzmaCompressor.CheckPrice(this.optimizationData[(int)((UIntPtr)(current + num2))], num4, current, num3 + 4U);
				if (num2 == this.matchDistances[(int)((UIntPtr)num)])
				{
					this.CheckAvailableLength(num2, num3, num4, num3 + 4U, false);
					num += 2U;
					if (num == this.distancePairsCounter)
					{
						break;
					}
				}
				num2 += 1U;
			}
		}

		void ProcessOptimumDistances(uint repMatchPrice, uint availableBytes, uint matchPrice)
		{
			uint num = 2U;
			uint num2 = this.optimumCurrent;
			uint positionState = this.optimumPositionState;
			for (uint num3 = 0U; num3 < 4U; num3 += 1U)
			{
				uint num4 = this.matchFinder.GetMatchLen(-1, this.repeaters[(int)((UIntPtr)num3)], availableBytes);
				if (num4 >= 2U)
				{
					uint num5 = num4;
					for (;;)
					{
						if (this.optimumLengthEnd >= num2 + num4)
						{
							uint curAndLenPrice = repMatchPrice + this.GetRepeaterPrice(num3, num4, this.currentState, positionState);
							LzmaCompressor.CheckPrice(this.optimizationData[(int)((UIntPtr)(num2 + num4))], curAndLenPrice, num2, num3);
							if ((num4 -= 1U) < 2U)
							{
								break;
							}
						}
						else
						{
							this.optimizationData[(int)((UIntPtr)(this.optimumLengthEnd += 1U))].Price = 268435455U;
						}
					}
					num4 = num5;
					if (num3 == 0U)
					{
						num = num4 + 1U;
					}
					this.CheckAvailableLength(num4, this.repeaters[(int)((UIntPtr)num3)], repMatchPrice, num3, true);
				}
			}
			if (this.optimumNewLength > availableBytes)
			{
				this.optimumNewLength = availableBytes;
				this.distancePairsCounter = 0U;
				while (this.optimumNewLength > this.matchDistances[(int)((UIntPtr)this.distancePairsCounter)])
				{
					this.distancePairsCounter += 2U;
				}
				this.matchDistances[(int)((UIntPtr)this.distancePairsCounter)] = this.optimumNewLength;
				this.distancePairsCounter += 2U;
			}
			if (this.optimumNewLength >= num)
			{
				this.UpdatePositionLengthPrice(num, matchPrice);
			}
		}

		void TryLiteralRep0(uint curAnd1Price)
		{
			uint num = this.optimumCurrent + 1U;
			uint limit = System.Math.Min(this.optimumAvailableBytesFull - 1U, this.configFastBytes);
			uint matchLen = this.matchFinder.GetMatchLen(0, this.repeaters[0], limit);
			if (matchLen >= 2U)
			{
				LzmaState state = this.currentState;
				state.UpdateChar();
				uint num2 = (this.optimumPosition + 1U) & this.configPositionStateMask;
				uint num3 = curAnd1Price + this.matchRangeBitEncoders[(int)((UIntPtr)((state.Index << 4) + num2))].GetPrice1() + this.repeaterRangeBitEncoders[(int)((UIntPtr)state.Index)].GetPrice1();
				uint num4 = num + matchLen;
				while (this.optimumLengthEnd < num4)
				{
					this.optimizationData[(int)((UIntPtr)(this.optimumLengthEnd += 1U))].Price = 268435455U;
				}
				uint num5 = num3 + this.GetRepeaterPrice(0U, matchLen, state, num2);
				LzmaOptimizationData lzmaOptimizationData = this.optimizationData[(int)((UIntPtr)num4)];
				if (num5 < lzmaOptimizationData.Price)
				{
					lzmaOptimizationData.Price = num5;
					lzmaOptimizationData.PosPrev = num;
					lzmaOptimizationData.BackPrev = 0U;
					lzmaOptimizationData.Prev1IsChar = true;
					lzmaOptimizationData.Prev2 = false;
				}
			}
		}

		void CheckAvailableLength(uint lenTest, uint curBack, uint startPrice, uint repIndex, bool repeater = true)
		{
			if (lenTest < this.optimumAvailableBytesFull)
			{
				uint num = this.optimumPosition;
				uint num2 = this.optimumCurrent;
				uint positionState = this.optimumPositionState;
				uint limit = System.Math.Min(this.optimumAvailableBytesFull - 1U - lenTest, this.configFastBytes);
				uint matchLen = this.matchFinder.GetMatchLen((int)lenTest, curBack, limit);
				if (matchLen >= 2U)
				{
					LzmaState state = this.currentState;
					uint num3 = (num + lenTest) & this.configPositionStateMask;
					uint num4;
					uint num5;
					if (repeater)
					{
						state.UpdateRep();
						num4 = (state.Index << 4) + num3;
						num5 = startPrice + (this.GetRepeaterPrice(repIndex, lenTest, this.currentState, positionState) + this.matchRangeBitEncoders[(int)((UIntPtr)num4)].GetPrice0() + this.literalEncoder.GetSubCoder(num + lenTest, this.matchFinder.GetIndexByte((int)(lenTest - 2U))).GetPrice(true, this.matchFinder.GetIndexByte((int)(lenTest - 1U - (curBack + 1U))), this.matchFinder.GetIndexByte((int)(lenTest - 1U))));
					}
					else
					{
						state.UpdateMatch();
						num4 = (state.Index << 4) + num3;
						num5 = startPrice + (this.matchRangeBitEncoders[(int)((UIntPtr)num4)].GetPrice0() + this.literalEncoder.GetSubCoder(num + lenTest, this.matchFinder.GetIndexByte((int)(lenTest - 2U))).GetPrice(true, this.matchFinder.GetIndexByte((int)(lenTest - (curBack + 1U) - 1U)), this.matchFinder.GetIndexByte((int)(lenTest - 1U))));
					}
					state.UpdateChar();
					num3 = (num + lenTest + 1U) & this.configPositionStateMask;
					num4 = (state.Index << 4) + num3;
					uint num6 = num5 + this.matchRangeBitEncoders[(int)((UIntPtr)num4)].GetPrice1();
					uint num7 = num6 + this.repeaterRangeBitEncoders[(int)((UIntPtr)state.Index)].GetPrice1();
					uint num8 = lenTest + 1U + matchLen;
					while (this.optimumLengthEnd < num2 + num8)
					{
						this.optimizationData[(int)((UIntPtr)(this.optimumLengthEnd += 1U))].Price = 268435455U;
					}
					uint num9 = num7 + this.GetRepeaterPrice(0U, matchLen, state, num3);
					LzmaOptimizationData lzmaOptimizationData = this.optimizationData[(int)((UIntPtr)(num2 + num8))];
					if (num9 < lzmaOptimizationData.Price)
					{
						lzmaOptimizationData.Price = num9;
						lzmaOptimizationData.PosPrev = num2 + lenTest + 1U;
						lzmaOptimizationData.BackPrev = 0U;
						lzmaOptimizationData.Prev1IsChar = true;
						lzmaOptimizationData.Prev2 = true;
						lzmaOptimizationData.PosPrev2 = num2;
						lzmaOptimizationData.BackPrev2 = repIndex;
					}
				}
			}
		}

		LzmaState GetCurrentState()
		{
			uint num = this.optimumCurrent;
			uint num2 = this.optimizationData[(int)((UIntPtr)num)].PosPrev;
			if (this.optimizationData[(int)((UIntPtr)num)].Prev1IsChar)
			{
				num2 -= 1U;
				if (this.optimizationData[(int)((UIntPtr)num)].Prev2)
				{
					this.currentState = this.optimizationData[(int)((UIntPtr)this.optimizationData[(int)((UIntPtr)num)].PosPrev2)].State;
					if (this.optimizationData[(int)((UIntPtr)num)].BackPrev2 < 4U)
					{
						this.currentState.UpdateRep();
					}
					else
					{
						this.currentState.UpdateMatch();
					}
				}
				else
				{
					this.currentState = this.optimizationData[(int)((UIntPtr)num2)].State;
				}
				this.currentState.UpdateChar();
			}
			else
			{
				this.currentState = this.optimizationData[(int)((UIntPtr)num2)].State;
			}
			this.UpdateCurrentState(num2);
			return this.currentState;
		}

		void UpdateCurrentState(uint previousPosition)
		{
			uint num = this.optimumCurrent;
			if (previousPosition == num - 1U)
			{
				if (this.optimizationData[(int)((UIntPtr)num)].IsShortRep())
				{
					this.currentState.UpdateShortRep();
				}
				else
				{
					this.currentState.UpdateChar();
				}
			}
			else
			{
				uint num2;
				if (this.optimizationData[(int)((UIntPtr)num)].Prev1IsChar && this.optimizationData[(int)((UIntPtr)num)].Prev2)
				{
					previousPosition = this.optimizationData[(int)((UIntPtr)num)].PosPrev2;
					num2 = this.optimizationData[(int)((UIntPtr)num)].BackPrev2;
					this.currentState.UpdateRep();
				}
				else
				{
					num2 = this.optimizationData[(int)((UIntPtr)num)].BackPrev;
					if (num2 < 4U)
					{
						this.currentState.UpdateRep();
					}
					else
					{
						this.currentState.UpdateMatch();
					}
				}
				this.UpdateRepeaters(num2, previousPosition);
			}
			this.optimizationData[(int)((UIntPtr)num)].State = this.currentState;
			this.optimizationData[(int)((UIntPtr)num)].Backs0 = this.repeaters[0];
			this.optimizationData[(int)((UIntPtr)num)].Backs1 = this.repeaters[1];
			this.optimizationData[(int)((UIntPtr)num)].Backs2 = this.repeaters[2];
			this.optimizationData[(int)((UIntPtr)num)].Backs3 = this.repeaters[3];
		}

		void UpdateRepeaters(uint position, uint previousPosition)
		{
			LzmaOptimizationData lzmaOptimizationData = this.optimizationData[(int)((UIntPtr)previousPosition)];
			if (position >= 4U)
			{
				this.repeaters[0] = position - 4U;
				this.repeaters[1] = lzmaOptimizationData.Backs0;
				this.repeaters[2] = lzmaOptimizationData.Backs1;
				this.repeaters[3] = lzmaOptimizationData.Backs2;
				return;
			}
			if (position == 0U)
			{
				this.repeaters[0] = lzmaOptimizationData.Backs0;
				this.repeaters[1] = lzmaOptimizationData.Backs1;
				this.repeaters[2] = lzmaOptimizationData.Backs2;
				this.repeaters[3] = lzmaOptimizationData.Backs3;
				return;
			}
			if (position == 1U)
			{
				this.repeaters[0] = lzmaOptimizationData.Backs1;
				this.repeaters[1] = lzmaOptimizationData.Backs0;
				this.repeaters[2] = lzmaOptimizationData.Backs2;
				this.repeaters[3] = lzmaOptimizationData.Backs3;
				return;
			}
			if (position == 2U)
			{
				this.repeaters[0] = lzmaOptimizationData.Backs2;
				this.repeaters[1] = lzmaOptimizationData.Backs0;
				this.repeaters[2] = lzmaOptimizationData.Backs1;
				this.repeaters[3] = lzmaOptimizationData.Backs3;
				return;
			}
			this.repeaters[0] = lzmaOptimizationData.Backs3;
			this.repeaters[1] = lzmaOptimizationData.Backs0;
			this.repeaters[2] = lzmaOptimizationData.Backs1;
			this.repeaters[3] = lzmaOptimizationData.Backs2;
		}

		bool CheckFastBytes()
		{
			uint num = this.optimumCurrent;
			if (num == this.optimumLengthEnd)
			{
				this.Backward(num);
				return true;
			}
			uint num2;
			this.ReadMatchDistances(out num2);
			this.optimumNewLength = num2;
			if (this.optimumNewLength >= this.configFastBytes)
			{
				this.distancePairsNumber = this.distancePairsCounter;
				this.longestMatchLength = this.optimumNewLength;
				this.longestMatchWasFound = true;
				this.Backward(num);
				return true;
			}
			return false;
		}

		bool SetOptimizationData(uint lengthMain)
		{
			byte indexByte = this.matchFinder.GetIndexByte(-1);
			byte indexByte2 = this.matchFinder.GetIndexByte((int)(0U - this.repeaterDistances[0] - 2U));
			if (lengthMain < 2U && indexByte != indexByte2 && this.repeaterLengths[(int)((UIntPtr)this.repeaterMaxIndex)] < 2U)
			{
				this.optimumPosition = uint.MaxValue;
				this.optimumLength = 1U;
				return true;
			}
			uint position = (uint)this.currentPosition;
			uint num = this.optimumPositionState;
			this.optimizationData[0].State = this.compressorState;
			uint num2 = (this.compressorState.Index << 4) + num;
			bool flag = this.compressorState.IsCharState();
			LzmaLiteralEncoder.Encoder subCoder = this.literalEncoder.GetSubCoder(position, this.currentPreviousByte);
			this.optimizationData[1].Price = this.matchRangeBitEncoders[(int)((UIntPtr)num2)].GetPrice0() + subCoder.GetPrice(!flag, indexByte2, indexByte);
			this.optimizationData[1].MakeAsChar();
			uint price = this.matchRangeBitEncoders[(int)((UIntPtr)((this.compressorState.Index << 4) + num))].GetPrice1();
			uint num3 = price + this.repeaterRangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].GetPrice1();
			if (indexByte2 == indexByte)
			{
				uint num4 = num3 + this.GetRepeaterLen1Price(this.compressorState, num);
				if (num4 < this.optimizationData[1].Price)
				{
					this.optimizationData[1].Price = num4;
					this.optimizationData[1].MakeAsShortRep();
				}
			}
			this.optimumLengthEnd = ((lengthMain >= this.repeaterLengths[(int)((UIntPtr)this.repeaterMaxIndex)]) ? lengthMain : this.repeaterLengths[(int)((UIntPtr)this.repeaterMaxIndex)]);
			if (this.optimumLengthEnd < 2U)
			{
				this.optimumPosition = this.optimizationData[1].BackPrev;
				this.optimumLength = 1U;
				return true;
			}
			this.SetOptimizationData(num3, num);
			this.UpdateDistancePairs(price, lengthMain);
			return false;
		}

		void UpdateDistancePairs(uint matchPrice, uint lengthMain)
		{
			uint num = matchPrice + this.repeaterRangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].GetPrice0();
			uint num2 = ((this.repeaterLengths[0] >= 2U) ? (this.repeaterLengths[0] + 1U) : 2U);
			if (num2 <= lengthMain)
			{
				uint positionState = this.optimumPositionState;
				uint num3 = 0U;
				while (num2 > this.matchDistances[(int)((UIntPtr)num3)])
				{
					num3 += 2U;
				}
				for (;;)
				{
					uint num4 = this.matchDistances[(int)((UIntPtr)(num3 + 1U))];
					uint curAndLenPrice = num + this.GetPosLenPrice(num4, num2, positionState);
					LzmaCompressor.CheckPrice(this.optimizationData[(int)((UIntPtr)num2)], curAndLenPrice, 0U, num4 + 4U);
					if (num2 == this.matchDistances[(int)((UIntPtr)num3)])
					{
						num3 += 2U;
						if (num3 == this.distancePairsCounter)
						{
							break;
						}
					}
					num2 += 1U;
				}
				return;
			}
		}

		void SetOptimizationData(uint repMatchPrice, uint posState)
		{
			this.optimizationData[1].PosPrev = 0U;
			this.optimizationData[0].Backs0 = this.repeaters[0];
			this.optimizationData[0].Backs1 = this.repeaters[1];
			this.optimizationData[0].Backs2 = this.repeaters[2];
			this.optimizationData[0].Backs3 = this.repeaters[3];
			uint num = this.optimumLengthEnd;
			do
			{
				this.optimizationData[(int)((UIntPtr)(num--))].Price = 268435455U;
			}
			while (num >= 2U);
			for (uint num2 = 0U; num2 < 4U; num2 += 1U)
			{
				uint num3 = this.repeaterLengths[(int)((UIntPtr)num2)];
				if (num3 >= 2U)
				{
					uint num4 = repMatchPrice + this.GetPureRepeaterPrice(num2, this.compressorState, posState);
					do
					{
						uint curAndLenPrice = num4 + this.repeaterMatchLengthEncoder.GetPrice(num3 - 2U, posState);
						LzmaCompressor.CheckPrice(this.optimizationData[(int)((UIntPtr)num3)], curAndLenPrice, 0U, num2);
					}
					while ((num3 -= 1U) >= 2U);
				}
			}
		}

		bool CalculateRepeaterMaxIndex(uint lengthMain)
		{
			this.repeaterMaxIndex = 0U;
			for (uint num = 0U; num < 4U; num += 1U)
			{
				this.repeaters[(int)((UIntPtr)num)] = this.repeaterDistances[(int)((UIntPtr)num)];
				this.repeaterLengths[(int)((UIntPtr)num)] = this.matchFinder.GetMatchLen(-1, this.repeaters[(int)((UIntPtr)num)], 273U);
				if (this.repeaterLengths[(int)((UIntPtr)num)] > this.repeaterLengths[(int)((UIntPtr)this.repeaterMaxIndex)])
				{
					this.repeaterMaxIndex = num;
				}
			}
			if (this.repeaterLengths[(int)((UIntPtr)this.repeaterMaxIndex)] >= this.configFastBytes)
			{
				this.optimumPosition = this.repeaterMaxIndex;
				this.optimumLength = this.repeaterLengths[(int)((UIntPtr)this.repeaterMaxIndex)];
				this.MovePosition(this.optimumLength - 1U);
				return true;
			}
			if (lengthMain >= this.configFastBytes)
			{
				this.optimumPosition = this.matchDistances[(int)((UIntPtr)(this.distancePairsCounter - 1U))] + 4U;
				this.MovePosition(lengthMain - 1U);
				this.optimumLength = lengthMain;
				return true;
			}
			return false;
		}

		bool CheckOptimumIndex()
		{
			if (this.optimumEndIndex != this.optimumCurrentIndex)
			{
				uint num = this.optimumCurrentIndex;
				this.optimumLength = this.optimizationData[(int)((UIntPtr)num)].PosPrev - num;
				this.optimumPosition = this.optimizationData[(int)((UIntPtr)num)].BackPrev;
				this.optimumCurrentIndex = this.optimizationData[(int)((UIntPtr)num)].PosPrev;
				return true;
			}
			this.optimumCurrentIndex = (this.optimumEndIndex = 0U);
			uint lengthMain;
			if (this.CalculateMatchDistances(out lengthMain))
			{
				this.optimumPosition = uint.MaxValue;
				this.optimumLength = 1U;
				return true;
			}
			return this.CalculateRepeaterMaxIndex(lengthMain) || this.SetOptimizationData(lengthMain);
		}

		bool CalculateMatchDistances(out uint lenMain)
		{
			if (!this.longestMatchWasFound)
			{
				this.ReadMatchDistances(out lenMain);
			}
			else
			{
				lenMain = this.longestMatchLength;
				this.distancePairsCounter = this.distancePairsNumber;
				this.longestMatchWasFound = false;
			}
			uint num = this.matchFinder.AvailableBytes + 1U;
			return num < 2U;
		}

		void WriteEndMarker(uint posState)
		{
			if (!base.Settings.UseZipHeader && base.Settings.StreamLength <= -1L)
			{
				uint num = (this.compressorState.Index << 4) + posState;
				this.matchRangeBitEncoders[(int)((UIntPtr)num)].Encode(this.rangeEncoder, 1U);
				this.repeaterRangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].Encode(this.rangeEncoder, 0U);
				this.compressorState.UpdateMatch();
				uint num2 = 2U;
				this.lengthEncoder.Encode(this.rangeEncoder, num2 - 2U, posState);
				uint symbol = 63U;
				uint lenToPosState = LzmaState.GetLenToPosState(num2);
				this.positionSlotEncoder[(int)((UIntPtr)lenToPosState)].Encode(this.rangeEncoder, symbol);
				int num3 = 30;
				uint num4 = (1U << num3) - 1U;
				this.rangeEncoder.EncodeDirectBits(num4 >> 4, num3 - 4);
				this.positionAlignEncoder.ReverseEncode(this.rangeEncoder, num4 & 15U);
			}
		}

		void Flush(bool finalBlock)
		{
			if (finalBlock)
			{
				this.finished = true;
				this.WriteEndMarker((uint)this.currentPosition & this.configPositionStateMask);
				this.rangeEncoder.FlushData();
			}
		}

		void CodeOneBlock(bool finalBlock)
		{
			if (this.CodeFirstPart(finalBlock))
			{
				if (this.matchFinder.AvailableBytes == 0U)
				{
					this.Flush(finalBlock);
					return;
				}
				bool readyToMove = this.matchFinder.ReadyToMove;
				while (!readyToMove && (this.matchFinder.Ready || finalBlock))
				{
					this.GetOptimum((uint)this.currentPosition);
					uint num = (uint)this.currentPosition & this.configPositionStateMask;
					uint complexState = (this.compressorState.Index << 4) + num;
					if (this.optimumLength == 1U && this.optimumPosition == 4294967295U)
					{
						this.Code0(complexState);
					}
					else
					{
						this.Code1(complexState, num);
					}
					if (this.UpdateCodePosition(finalBlock))
					{
						return;
					}
				}
				if (readyToMove)
				{
					this.matchFinder.MoveBlock();
				}
			}
		}

		bool UpdateCodePosition(bool finalBlock)
		{
			uint num = this.optimumLength;
			this.additionalOffset -= num;
			this.currentPosition += (long)((ulong)num);
			if (this.additionalOffset == 0U)
			{
				if (this.matchPriceCount >= 128U)
				{
					this.FillDistancesPrices();
				}
				if (this.alignPriceCount >= 16U)
				{
					this.FillAlignPrices();
				}
				if (this.matchFinder.AvailableBytes == 0U)
				{
					this.Flush(finalBlock);
					return true;
				}
			}
			return false;
		}

		void Code1(uint complexState, uint positionState)
		{
			uint num = this.optimumPosition;
			uint num2 = this.optimumLength;
			this.matchRangeBitEncoders[(int)((UIntPtr)complexState)].Encode(this.rangeEncoder, 1U);
			if (num < 4U)
			{
				this.Code1Encoding(complexState, positionState);
			}
			else
			{
				this.Code0Encoding(positionState);
			}
			this.currentPreviousByte = this.matchFinder.GetIndexByte((int)(num2 - 1U - this.additionalOffset));
		}

		void Code0Encoding(uint positionState)
		{
			uint num = this.optimumPosition;
			uint num2 = this.optimumLength;
			this.repeaterRangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].Encode(this.rangeEncoder, 0U);
			this.compressorState.UpdateMatch();
			this.lengthEncoder.Encode(this.rangeEncoder, num2 - 2U, positionState);
			num -= 4U;
			uint posSlot = LzmaCompressor.GetPosSlot(num);
			uint lenToPosState = LzmaState.GetLenToPosState(num2);
			this.positionSlotEncoder[(int)((UIntPtr)lenToPosState)].Encode(this.rangeEncoder, posSlot);
			if (posSlot >= 4U)
			{
				int num3 = (int)((posSlot >> 1) - 1U);
				uint num4 = (2U | (posSlot & 1U)) << num3;
				uint num5 = num - num4;
				if (posSlot < 14U)
				{
					uint startIndex = num4 - posSlot - 1U;
					LzmaBitTreeEncoder.ReverseEncode(this.positionEncoders, startIndex, this.rangeEncoder, num3, num5);
				}
				else
				{
					this.rangeEncoder.EncodeDirectBits(num5 >> 4, num3 - 4);
					this.positionAlignEncoder.ReverseEncode(this.rangeEncoder, num5 & 15U);
					this.alignPriceCount += 1U;
				}
			}
			uint num6 = num;
			for (uint num7 = 3U; num7 >= 1U; num7 -= 1U)
			{
				this.repeaterDistances[(int)((UIntPtr)num7)] = this.repeaterDistances[(int)((UIntPtr)(num7 - 1U))];
			}
			this.repeaterDistances[0] = num6;
			this.matchPriceCount += 1U;
			this.optimumPosition = num;
		}

		void Code1Encoding(uint complexState, uint positionState)
		{
			uint num = this.optimumPosition;
			uint num2 = this.optimumLength;
			this.repeaterRangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].Encode(this.rangeEncoder, 1U);
			if (num == 0U)
			{
				this.repeaterG0RangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].Encode(this.rangeEncoder, 0U);
				if (num2 == 1U)
				{
					this.repeaterLongRangeBitEncoders[(int)((UIntPtr)complexState)].Encode(this.rangeEncoder, 0U);
				}
				else
				{
					this.repeaterLongRangeBitEncoders[(int)((UIntPtr)complexState)].Encode(this.rangeEncoder, 1U);
				}
			}
			else
			{
				this.repeaterG0RangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].Encode(this.rangeEncoder, 1U);
				if (num == 1U)
				{
					this.repeaterG1RangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].Encode(this.rangeEncoder, 0U);
				}
				else
				{
					this.repeaterG1RangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].Encode(this.rangeEncoder, 1U);
					this.repeaterG2RangeBitEncoders[(int)((UIntPtr)this.compressorState.Index)].Encode(this.rangeEncoder, num - 2U);
				}
			}
			if (num2 == 1U)
			{
				this.compressorState.UpdateShortRep();
			}
			else
			{
				this.repeaterMatchLengthEncoder.Encode(this.rangeEncoder, num2 - 2U, positionState);
				this.compressorState.UpdateRep();
			}
			uint num3 = this.repeaterDistances[(int)((UIntPtr)num)];
			if (num != 0U)
			{
				for (uint num4 = num; num4 >= 1U; num4 -= 1U)
				{
					this.repeaterDistances[(int)((UIntPtr)num4)] = this.repeaterDistances[(int)((UIntPtr)(num4 - 1U))];
				}
				this.repeaterDistances[0] = num3;
			}
		}

		void Code0(uint complexState)
		{
			this.matchRangeBitEncoders[(int)((UIntPtr)complexState)].Encode(this.rangeEncoder, 0U);
			byte indexByte = this.matchFinder.GetIndexByte((int)(0U - this.additionalOffset));
			LzmaLiteralEncoder.Encoder subCoder = this.literalEncoder.GetSubCoder((uint)this.currentPosition, this.currentPreviousByte);
			if (!this.compressorState.IsCharState())
			{
				byte indexByte2 = this.matchFinder.GetIndexByte((int)(0U - this.repeaterDistances[0] - 1U - this.additionalOffset));
				subCoder.EncodeMatched(this.rangeEncoder, indexByte2, indexByte);
			}
			else
			{
				subCoder.Encode(this.rangeEncoder, indexByte);
			}
			this.currentPreviousByte = indexByte;
			this.compressorState.UpdateChar();
		}

		bool CodeFirstPart(bool finalBlock)
		{
			if (this.currentPosition == 0L)
			{
				if (!this.matchFinder.Ready)
				{
					return false;
				}
				if (this.matchFinder.AvailableBytes == 0U)
				{
					this.Flush(finalBlock);
					return false;
				}
				uint num;
				this.ReadMatchDistances(out num);
				uint num2 = (uint)this.currentPosition & this.configPositionStateMask;
				uint num3 = (this.compressorState.Index << 4) + num2;
				this.matchRangeBitEncoders[(int)((UIntPtr)num3)].Encode(this.rangeEncoder, 0U);
				this.compressorState.UpdateChar();
				byte indexByte = this.matchFinder.GetIndexByte((int)(0U - this.additionalOffset));
				this.literalEncoder.GetSubCoder((uint)this.currentPosition, this.currentPreviousByte).Encode(this.rangeEncoder, indexByte);
				this.currentPreviousByte = indexByte;
				this.additionalOffset -= 1U;
				this.currentPosition += 1L;
			}
			return true;
		}

		void Initialize()
		{
			this.ApplySettings();
			this.InitEncoders();
			this.FillDistancesPrices();
			this.FillAlignPrices();
			this.lengthEncoder.SetTableSize(this.configFastBytes + 1U - 2U);
			this.lengthEncoder.UpdateTables(1U << this.configPositionStateBits);
			this.repeaterMatchLengthEncoder.SetTableSize(this.configFastBytes + 1U - 2U);
			this.repeaterMatchLengthEncoder.UpdateTables(1U << this.configPositionStateBits);
			this.currentPosition = 0L;
		}

		void ApplySettings()
		{
			this.ApplyDictionarySize();
			this.ApplyPositionStateBits();
			this.ApplyLiteralContextBits();
			this.ApplyLiteralPositionBits();
			this.ApplyFastBytes();
			this.ApplyMatchFinderType();
			this.literalEncoder = new LzmaLiteralEncoder(this.configLiteralPositionStateBits, this.configLiteralContextBits);
		}

		void ApplyDictionarySize()
		{
			int num = base.Settings.DictionarySize;
			if (num < 0)
			{
				num = 0;
			}
			else if (num > 27)
			{
				num = 27;
			}
			int num2 = 1 << num;
			this.configDictionarySize = (uint)num2;
			int num3 = 0;
			while ((long)num3 < 27L && (long)num2 > (long)(1UL << (num3 & 31)))
			{
				num3++;
			}
			this.configDistanceTableSize = (uint)(num3 * 2);
		}

		void ApplyPositionStateBits()
		{
			int num = base.Settings.PositionStateBits;
			if (num < 0)
			{
				num = 0;
			}
			else if (num > 4)
			{
				num = 4;
			}
			this.configPositionStateBits = num;
			this.configPositionStateMask = (1U << this.configPositionStateBits) - 1U;
		}

		void ApplyLiteralContextBits()
		{
			int num = base.Settings.LiteralContextBits;
			if (num < 0)
			{
				num = 0;
			}
			else if (num > 8)
			{
				num = 8;
			}
			this.configLiteralContextBits = num;
		}

		void ApplyLiteralPositionBits()
		{
			int num = base.Settings.LiteralPositionBits;
			if (num < 0)
			{
				num = 0;
			}
			else if ((long)num > 4L)
			{
				num = 4;
			}
			this.configLiteralPositionStateBits = num;
		}

		void ApplyFastBytes()
		{
			uint num = (uint)base.Settings.FastBytes;
			if (num < 5U)
			{
				num = 5U;
			}
			else if (num > 273U)
			{
				num = 273U;
			}
			this.configFastBytes = num;
		}

		void ApplyMatchFinderType()
		{
			this.matchFinderType = base.Settings.MatchFinderType;
			int hashBytes = 4;
			if (this.matchFinderType == LzmaMatchFinderType.BT2)
			{
				hashBytes = 2;
			}
			this.matchFinder = new LzmaBinaryTree(hashBytes, this.configDictionarySize, this.configFastBytes, 274U);
		}

		void FillDistancesPrices()
		{
			for (uint num = 4U; num < 128U; num += 1U)
			{
				uint posSlot = LzmaCompressor.GetPosSlot(num);
				int num2 = (int)((posSlot >> 1) - 1U);
				uint num3 = (2U | (posSlot & 1U)) << num2;
				this.tempPrices[(int)((UIntPtr)num)] = LzmaBitTreeEncoder.ReverseGetPrice(this.positionEncoders, num3 - posSlot - 1U, num2, num - num3);
			}
			for (uint num4 = 0U; num4 < 4U; num4 += 1U)
			{
				LzmaBitTreeEncoder lzmaBitTreeEncoder = this.positionSlotEncoder[(int)((UIntPtr)num4)];
				uint num5 = num4 << 6;
				for (uint num6 = 0U; num6 < this.configDistanceTableSize; num6 += 1U)
				{
					this.positionSlotPrices[(int)((UIntPtr)(num5 + num6))] = lzmaBitTreeEncoder.GetPrice(num6);
				}
				for (uint num6 = 14U; num6 < this.configDistanceTableSize; num6 += 1U)
				{
					uint num7 = (num6 >> 1) - 1U - 4U << 6;
					this.positionSlotPrices[(int)((UIntPtr)(num5 + num6))] += num7;
				}
				uint num8 = num4 * 128U;
				uint num9;
				for (num9 = 0U; num9 < 4U; num9 += 1U)
				{
					this.distancesPrices[(int)((UIntPtr)(num8 + num9))] = this.positionSlotPrices[(int)((UIntPtr)(num5 + num9))];
				}
				while (num9 < 128U)
				{
					this.distancesPrices[(int)((UIntPtr)(num8 + num9))] = this.positionSlotPrices[(int)((UIntPtr)(num5 + LzmaCompressor.GetPosSlot(num9)))] + this.tempPrices[(int)((UIntPtr)num9)];
					num9 += 1U;
				}
			}
			this.matchPriceCount = 0U;
		}

		void FillAlignPrices()
		{
			for (uint num = 0U; num < 16U; num += 1U)
			{
				this.alignPrices[(int)((UIntPtr)num)] = this.positionAlignEncoder.ReverseGetPrice(num);
			}
			this.alignPriceCount = 0U;
		}

		public const uint OptimumsNumber = 4096U;

		const int MinBlockSize = 8192;

		const int MaxBlockSize = 262144;

		const int DefaultDictionaryLogSize = 22;

		const int DictionaryMaxCompressValue = 27;

		const uint FastBytesDefaultNumber = 32U;

		const uint LengthSpecialSymbolsNumber = 16U;

		const uint InfinityPrice = 268435455U;

		const byte FastSlotsNumber = 22;

		static byte[] fastPositions = new byte[2048];

		LzmaState compressorState = default(LzmaState);

		bool finished;

		byte currentPreviousByte;

		uint[] repeaterDistances = new uint[4];

		LzmaOptimizationData[] optimizationData = new LzmaOptimizationData[4096];

		LzmaBinaryTree matchFinder;

		LzmaRangeEncoder rangeEncoder = new LzmaRangeEncoder();

		LzmaRangeBitEncoder[] matchRangeBitEncoders = new LzmaRangeBitEncoder[192];

		LzmaRangeBitEncoder[] repeaterRangeBitEncoders = new LzmaRangeBitEncoder[12];

		LzmaRangeBitEncoder[] repeaterG0RangeBitEncoders = new LzmaRangeBitEncoder[12];

		LzmaRangeBitEncoder[] repeaterG1RangeBitEncoders = new LzmaRangeBitEncoder[12];

		LzmaRangeBitEncoder[] repeaterG2RangeBitEncoders = new LzmaRangeBitEncoder[12];

		LzmaRangeBitEncoder[] repeaterLongRangeBitEncoders = new LzmaRangeBitEncoder[192];

		LzmaBitTreeEncoder[] positionSlotEncoder = new LzmaBitTreeEncoder[4];

		LzmaRangeBitEncoder[] positionEncoders = new LzmaRangeBitEncoder[114];

		LzmaBitTreeEncoder positionAlignEncoder = new LzmaBitTreeEncoder(4);

		LzmaLengthTableEncoder lengthEncoder = new LzmaLengthTableEncoder();

		LzmaLengthTableEncoder repeaterMatchLengthEncoder = new LzmaLengthTableEncoder();

		LzmaLiteralEncoder literalEncoder;

		uint[] matchDistances = new uint[548];

		uint configFastBytes = 32U;

		uint longestMatchLength;

		uint distancePairsNumber;

		uint additionalOffset;

		uint optimumEndIndex;

		uint optimumCurrentIndex;

		bool longestMatchWasFound;

		uint[] positionSlotPrices = new uint[256];

		uint[] distancesPrices = new uint[512];

		uint[] alignPrices = new uint[16];

		uint alignPriceCount;

		uint configDistanceTableSize;

		int configPositionStateBits;

		uint configPositionStateMask;

		int configLiteralPositionStateBits;

		int configLiteralContextBits;

		uint configDictionarySize;

		long currentPosition;

		LzmaMatchFinderType matchFinderType = LzmaMatchFinderType.BT4;

		uint[] repeaters = new uint[4];

		uint[] repeaterLengths = new uint[4];

		uint[] tempPrices = new uint[128];

		LzmaState currentState;

		uint matchPriceCount;

		uint optimumPosition;

		uint optimumLength;

		uint optimumLengthEnd;

		uint optimumNewLength;

		uint repeaterMaxIndex;

		uint distancePairsCounter;

		uint optimumCurrent;

		uint optimumAvailableBytesFull;

		uint optimumPositionState;
	}
}
