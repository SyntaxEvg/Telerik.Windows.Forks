using System;

namespace JBig2Decoder
{
	class SymbolDictionarySegment : Segment
	{
		public SymbolDictionarySegment(JBIG2StreamDecoder streamDecoder)
			: base(streamDecoder)
		{
		}

		public override void readSegment()
		{
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("==== Read Segment Symbol Dictionary ====");
			}
			this.readSymbolDictionaryFlags();
			int num = 0;
			int referredToSegmentCount = this.segmentHeader.getReferredToSegmentCount();
			int[] referredToSegments = this.segmentHeader.getReferredToSegments();
			long num2;
			for (num2 = 0L; num2 < (long)referredToSegmentCount; num2 += 1L)
			{
				Segment segment = this.decoder.findSegment(referredToSegments[(int)(checked((IntPtr)num2))]);
				if (segment.getSegmentHeader().getSegmentType() == 0)
				{
					num += ((SymbolDictionarySegment)segment).noOfExportedSymbols;
				}
			}
			int num3 = 0;
			for (num2 = 1L; num2 < (long)(num + this.noOfNewSymbols); num2 <<= 1)
			{
				num3++;
			}
			JBIG2Bitmap[] array = new JBIG2Bitmap[num + this.noOfNewSymbols];
			long num4 = 0L;
			SymbolDictionarySegment symbolDictionarySegment = null;
			long num5;
			for (num2 = 0L; num2 < (long)referredToSegmentCount; num2 += 1L)
			{
				Segment segment2 = this.decoder.findSegment(referredToSegments[(int)(checked((IntPtr)num2))]);
				if (segment2.getSegmentHeader().getSegmentType() == 0)
				{
					symbolDictionarySegment = (SymbolDictionarySegment)segment2;
					for (num5 = 0L; num5 < (long)symbolDictionarySegment.noOfExportedSymbols; num5 += 1L)
					{
						JBIG2Bitmap[] array2 = array;
						long num6 = num4;
						num4 = num6 + 1L;
						checked
						{
							array2[(int)((IntPtr)num6)] = symbolDictionarySegment.bitmaps[(int)((IntPtr)num5)];
						}
					}
				}
			}
			long[,] table = null;
			long[,] table2 = null;
			long[,] table3 = null;
			long[,] table4 = null;
			bool flag = this.symbolDictionaryFlags.getFlagValue("SD_HUFF") != 0;
			int flagValue = this.symbolDictionaryFlags.getFlagValue("SD_HUFF_DH");
			int flagValue2 = this.symbolDictionaryFlags.getFlagValue("SD_HUFF_DW");
			int flagValue3 = this.symbolDictionaryFlags.getFlagValue("SD_HUFF_BM_SIZE");
			int flagValue4 = this.symbolDictionaryFlags.getFlagValue("SD_HUFF_AGG_INST");
			if (flag)
			{
				if (flagValue == 0)
				{
					table = HuffmanDecoder.huffmanTableD;
				}
				else if (flagValue == 1)
				{
					table = HuffmanDecoder.huffmanTableE;
				}
				if (flagValue2 == 0)
				{
					table2 = HuffmanDecoder.huffmanTableB;
				}
				else if (flagValue2 == 1)
				{
					table2 = HuffmanDecoder.huffmanTableC;
				}
				if (flagValue3 == 0)
				{
					table3 = HuffmanDecoder.huffmanTableA;
				}
				if (flagValue4 == 0)
				{
					table4 = HuffmanDecoder.huffmanTableA;
				}
			}
			int flagValue5 = this.symbolDictionaryFlags.getFlagValue("BITMAP_CC_USED");
			int flagValue6 = this.symbolDictionaryFlags.getFlagValue("SD_TEMPLATE");
			if (!flag)
			{
				if (flagValue5 != 0 && symbolDictionarySegment != null)
				{
					this.arithmeticDecoder.resetGenericStats(flagValue6, symbolDictionarySegment.genericRegionStats);
				}
				else
				{
					this.arithmeticDecoder.resetGenericStats(flagValue6, null);
				}
				this.arithmeticDecoder.resetIntStats(num3);
				this.arithmeticDecoder.start();
			}
			int flagValue7 = this.symbolDictionaryFlags.getFlagValue("SD_REF_AGG");
			int flagValue8 = this.symbolDictionaryFlags.getFlagValue("SD_R_TEMPLATE");
			if (flagValue7 != 0)
			{
				if (flagValue5 != 0 && symbolDictionarySegment != null)
				{
					this.arithmeticDecoder.resetRefinementStats(flagValue8, symbolDictionarySegment.refinementRegionStats);
				}
				else
				{
					this.arithmeticDecoder.resetRefinementStats(flagValue8, null);
				}
			}
			long[] array3 = new long[this.noOfNewSymbols];
			long num7 = 0L;
			num2 = 0L;
			while (num2 < (long)this.noOfNewSymbols)
			{
				long num8;
				if (flag)
				{
					num8 = this.huffmanDecoder.decodeInt(table).intResult();
				}
				else
				{
					num8 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iadhStats).intResult();
				}
				if (num8 < 0L && -num8 >= num7 && JBIG2StreamDecoder.debug)
				{
					Console.WriteLine("Bad delta-height value in JBIG2 symbol dictionary");
				}
				num7 += num8;
				long num9 = 0L;
				long num10 = 0L;
				num5 = num2;
				for (;;)
				{
					DecodeIntResult decodeIntResult;
					if (flag)
					{
						decodeIntResult = this.huffmanDecoder.decodeInt(table2);
					}
					else
					{
						decodeIntResult = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iadwStats);
					}
					if (!decodeIntResult.booleanResult())
					{
						break;
					}
					long num11 = decodeIntResult.intResult();
					if (num11 < 0L && -num11 >= num9 && JBIG2StreamDecoder.debug)
					{
						Console.WriteLine("Bad delta-width value in JBIG2 symbol dictionary");
					}
					num9 += num11;
					if (flag && flagValue7 == 0)
					{
						array3[(int)(checked((IntPtr)num2))] = num9;
						num10 += num9;
					}
					else if (flagValue7 == 1)
					{
						long num12;
						if (flag)
						{
							num12 = this.huffmanDecoder.decodeInt(table4).intResult();
						}
						else
						{
							num12 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iaaiStats).intResult();
						}
						if (num12 == 1L)
						{
							long num13;
							long referenceDX;
							long referenceDY;
							if (flag)
							{
								num13 = (long)this.decoder.readBits((long)num3);
								referenceDX = this.huffmanDecoder.decodeInt(HuffmanDecoder.huffmanTableO).intResult();
								referenceDY = this.huffmanDecoder.decodeInt(HuffmanDecoder.huffmanTableO).intResult();
								this.decoder.consumeRemainingBits();
								this.arithmeticDecoder.start();
							}
							else
							{
								num13 = (long)((int)this.arithmeticDecoder.decodeIAID((long)num3, this.arithmeticDecoder.iaidStats));
								referenceDX = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardxStats).intResult();
								referenceDY = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardyStats).intResult();
							}
							JBIG2Bitmap referredToBitmap = array[(int)(checked((IntPtr)num13))];
							JBIG2Bitmap jbig2Bitmap = new JBIG2Bitmap(num9, num7, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
							jbig2Bitmap.readGenericRefinementRegion((long)flagValue8, false, referredToBitmap, referenceDX, referenceDY, this.symbolDictionaryRAdaptiveTemplateX, this.symbolDictionaryRAdaptiveTemplateY);
							array[(int)(checked((IntPtr)(unchecked((long)num + num2))))] = jbig2Bitmap;
						}
						else
						{
							JBIG2Bitmap jbig2Bitmap2 = new JBIG2Bitmap(num9, num7, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
							jbig2Bitmap2.readTextRegion(flag, true, num12, 0L, (long)num + num2, null, (long)num3, array, 0, 0, false, 1, 0, HuffmanDecoder.huffmanTableF, HuffmanDecoder.huffmanTableH, HuffmanDecoder.huffmanTableK, HuffmanDecoder.huffmanTableO, HuffmanDecoder.huffmanTableO, HuffmanDecoder.huffmanTableO, HuffmanDecoder.huffmanTableO, HuffmanDecoder.huffmanTableA, flagValue8, this.symbolDictionaryRAdaptiveTemplateX, this.symbolDictionaryRAdaptiveTemplateY, this.decoder);
							array[(int)(checked((IntPtr)(unchecked((long)num + num2))))] = jbig2Bitmap2;
						}
					}
					else
					{
						JBIG2Bitmap jbig2Bitmap3 = new JBIG2Bitmap(num9, num7, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
						jbig2Bitmap3.readBitmap(false, flagValue6, false, false, null, this.symbolDictionaryAdaptiveTemplateX, this.symbolDictionaryAdaptiveTemplateY, 0L);
						array[(int)(checked((IntPtr)(unchecked((long)num + num2))))] = jbig2Bitmap3;
					}
					num2 += 1L;
				}
				if (flag && flagValue7 == 0)
				{
					long num14 = this.huffmanDecoder.decodeInt(table3).intResult();
					this.decoder.consumeRemainingBits();
					JBIG2Bitmap jbig2Bitmap4 = new JBIG2Bitmap(num10, num7, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
					if (num14 == 0L)
					{
						long num15 = num10 % 8L;
						long num16 = (long)((int)Math.Ceiling((double)num10 / 8.0));
						long num17 = num7 * (num10 + 7L >> 3);
						short[] array4 = new short[num17];
						this.decoder.readbyte(array4);
						short[][] array5 = new short[num7][];
						int num18 = 0;
						int num19 = 0;
						while ((long)num19 < num7)
						{
							int num20 = 0;
							while ((long)num20 < num16)
							{
								array5[num19][num20] = array4[num18];
								num18++;
								num20++;
							}
							num19++;
						}
						int num21 = 0;
						int num22 = 0;
						int num23 = 0;
						while ((long)num23 < num7)
						{
							int num24 = 0;
							while ((long)num24 < num16)
							{
								if ((long)num24 == num16 - 1L)
								{
									short num25 = array5[num23][num24];
									int num26 = 7;
									while ((long)num26 >= num15)
									{
										short num27 = (short)(1 << num26);
										int num28 = (num25 & num27) >> num26;
										jbig2Bitmap4.setPixel((long)num22, (long)num21, (long)num28);
										num22++;
										num26--;
									}
									num21++;
									num22 = 0;
								}
								else
								{
									short num29 = array5[num23][num24];
									for (int i = 7; i >= 0; i--)
									{
										short num30 = (short)(1 << i);
										int num31 = (num29 & num30) >> i;
										jbig2Bitmap4.setPixel((long)num22, (long)num21, (long)num31);
										num22++;
									}
								}
								num24++;
							}
							num23++;
						}
					}
					else
					{
						jbig2Bitmap4.readBitmap(true, 0, false, false, null, null, null, num14);
					}
					long num32 = 0L;
					while (num5 < num2)
					{
						checked
						{
							array[(int)((IntPtr)(unchecked((long)num + num5)))] = jbig2Bitmap4.getSlice(num32, 0L, array3[(int)((IntPtr)num5)], num7);
						}
						num32 += array3[(int)(checked((IntPtr)num5))];
						num5 += 1L;
					}
				}
			}
			this.bitmaps = new JBIG2Bitmap[this.noOfExportedSymbols];
			num2 = (num5 = 0L);
			bool flag2 = false;
			while (num2 < (long)(num + this.noOfNewSymbols))
			{
				long num33;
				if (flag)
				{
					num33 = this.huffmanDecoder.decodeInt(HuffmanDecoder.huffmanTableA).intResult();
				}
				else
				{
					num33 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iaexStats).intResult();
				}
				if (flag2)
				{
					int num34 = 0;
					while ((long)num34 < num33)
					{
						JBIG2Bitmap[] array6 = this.bitmaps;
						long num35 = num5;
						num5 = num35 + 1L;
						int num36 = (int)(checked((IntPtr)num35));
						JBIG2Bitmap[] array7 = array;
						long num37 = num2;
						num2 = num37 + 1L;
						array6[num36] = array7[(int)(checked((IntPtr)num37))];
						num34++;
					}
				}
				else
				{
					num2 += num33;
				}
				flag2 = !flag2;
			}
			int flagValue9 = this.symbolDictionaryFlags.getFlagValue("BITMAP_CC_RETAINED");
			if (!flag && flagValue9 == 1)
			{
				this.genericRegionStats = this.genericRegionStats.copy();
				if (flagValue7 == 1)
				{
					this.refinementRegionStats = this.refinementRegionStats.copy();
				}
			}
			this.decoder.consumeRemainingBits();
		}

		void readSymbolDictionaryFlags()
		{
			short[] array = new short[2];
			this.decoder.readbyte(array);
			int @int = BinaryOperation.getInt16(array);
			this.symbolDictionaryFlags.setFlags(@int);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("symbolDictionaryFlags = " + @int);
			}
			int flagValue = this.symbolDictionaryFlags.getFlagValue("SD_HUFF");
			int flagValue2 = this.symbolDictionaryFlags.getFlagValue("SD_TEMPLATE");
			if (flagValue == 0)
			{
				if (flagValue2 == 0)
				{
					this.symbolDictionaryAdaptiveTemplateX[0] = base.readATValue();
					this.symbolDictionaryAdaptiveTemplateY[0] = base.readATValue();
					this.symbolDictionaryAdaptiveTemplateX[1] = base.readATValue();
					this.symbolDictionaryAdaptiveTemplateY[1] = base.readATValue();
					this.symbolDictionaryAdaptiveTemplateX[2] = base.readATValue();
					this.symbolDictionaryAdaptiveTemplateY[2] = base.readATValue();
					this.symbolDictionaryAdaptiveTemplateX[3] = base.readATValue();
					this.symbolDictionaryAdaptiveTemplateY[3] = base.readATValue();
				}
				else
				{
					this.symbolDictionaryAdaptiveTemplateX[0] = base.readATValue();
					this.symbolDictionaryAdaptiveTemplateY[0] = base.readATValue();
				}
			}
			int flagValue3 = this.symbolDictionaryFlags.getFlagValue("SD_REF_AGG");
			int flagValue4 = this.symbolDictionaryFlags.getFlagValue("SD_R_TEMPLATE");
			if (flagValue3 != 0 && flagValue4 == 0)
			{
				this.symbolDictionaryRAdaptiveTemplateX[0] = base.readATValue();
				this.symbolDictionaryRAdaptiveTemplateY[0] = base.readATValue();
				this.symbolDictionaryRAdaptiveTemplateX[1] = base.readATValue();
				this.symbolDictionaryRAdaptiveTemplateY[1] = base.readATValue();
			}
			short[] array2 = new short[4];
			this.decoder.readbyte(array2);
			int int2 = BinaryOperation.getInt32(array2);
			this.noOfExportedSymbols = int2;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("noOfExportedSymbols = " + int2);
			}
			short[] array3 = new short[4];
			this.decoder.readbyte(array3);
			int int3 = BinaryOperation.getInt32(array3);
			this.noOfNewSymbols = int3;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("noOfNewSymbols = " + int3);
			}
		}

		public int getNoOfExportedSymbols()
		{
			return this.noOfExportedSymbols;
		}

		public void setNoOfExportedSymbols(int noOfExportedSymbols)
		{
			this.noOfExportedSymbols = noOfExportedSymbols;
		}

		public int getNoOfNewSymbols()
		{
			return this.noOfNewSymbols;
		}

		public void setNoOfNewSymbols(int noOfNewSymbols)
		{
			this.noOfNewSymbols = noOfNewSymbols;
		}

		public JBIG2Bitmap[] getBitmaps()
		{
			return this.bitmaps;
		}

		public SymbolDictionaryFlags getSymbolDictionaryFlags()
		{
			return this.symbolDictionaryFlags;
		}

		public void setSymbolDictionaryFlags(SymbolDictionaryFlags symbolDictionaryFlags)
		{
			this.symbolDictionaryFlags = symbolDictionaryFlags;
		}

		ArithmeticDecoderStats getGenericRegionStats()
		{
			return this.genericRegionStats;
		}

		void setGenericRegionStats(ArithmeticDecoderStats genericRegionStats)
		{
			this.genericRegionStats = genericRegionStats;
		}

		void setRefinementRegionStats(ArithmeticDecoderStats refinementRegionStats)
		{
			this.refinementRegionStats = refinementRegionStats;
		}

		ArithmeticDecoderStats getRefinementRegionStats()
		{
			return this.refinementRegionStats;
		}

		int noOfExportedSymbols;

		int noOfNewSymbols;

		short[] symbolDictionaryAdaptiveTemplateX = new short[4];

		short[] symbolDictionaryAdaptiveTemplateY = new short[4];

		short[] symbolDictionaryRAdaptiveTemplateX = new short[2];

		short[] symbolDictionaryRAdaptiveTemplateY = new short[2];

		JBIG2Bitmap[] bitmaps;

		SymbolDictionaryFlags symbolDictionaryFlags = new SymbolDictionaryFlags();

		ArithmeticDecoderStats genericRegionStats;

		ArithmeticDecoderStats refinementRegionStats;
	}
}
