using System;
using System.Collections.Generic;

namespace JBig2Decoder
{
	class TextRegionSegment : RegionSegment
	{
		public TextRegionSegment(JBIG2StreamDecoder streamDecoder, bool inlineImage)
			: base(streamDecoder)
		{
			this.inlineImage = inlineImage;
		}

		public override void readSegment()
		{
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("==== Reading Text Region ====");
			}
			base.readSegment();
			this.readTextRegionFlags();
			short[] array = new short[4];
			this.decoder.readbyte(array);
			long num = (long)BinaryOperation.getInt32(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("noOfSymbolInstances = " + num);
			}
			int referredToSegmentCount = this.segmentHeader.getReferredToSegmentCount();
			int[] referredToSegments = this.segmentHeader.getReferredToSegments();
			List<Segment> list = new List<Segment>();
			long num2 = 0L;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("noOfReferredToSegments = " + referredToSegmentCount);
			}
			for (int i = 0; i < referredToSegmentCount; i++)
			{
				Segment segment = this.decoder.findSegment(referredToSegments[i]);
				if (segment.getSegmentHeader().getSegmentType() == 0)
				{
					list.Add(segment);
					num2 += (long)((SymbolDictionarySegment)segment).getNoOfExportedSymbols();
				}
			}
			long num3 = 0L;
			int num4 = 1;
			while ((long)num4 < num2)
			{
				num3 += 1L;
				num4 <<= 1;
			}
			int num5 = 0;
			JBIG2Bitmap[] array2 = new JBIG2Bitmap[num2];
			foreach (Segment segment2 in list)
			{
				if (segment2.getSegmentHeader().getSegmentType() == 0)
				{
					JBIG2Bitmap[] bitmaps = ((SymbolDictionarySegment)segment2).getBitmaps();
					for (int j = 0; j < bitmaps.Length; j++)
					{
						array2[num5] = bitmaps[j];
						num5++;
					}
				}
			}
			long[,] huffmanFSTable = null;
			long[,] huffmanDSTable = null;
			long[,] huffmanDTTable = null;
			long[,] huffmanRDWTable = null;
			long[,] huffmanRDHTable = null;
			long[,] huffmanRDXTable = null;
			long[,] huffmanRDYTable = null;
			long[,] huffmanRSizeTable = null;
			bool flag = this.textRegionFlags.getFlagValue("SB_HUFF") != 0;
			if (flag)
			{
				int flagValue = this.textRegionHuffmanFlags.getFlagValue("SB_HUFF_FS");
				if (flagValue == 0)
				{
					huffmanFSTable = HuffmanDecoder.huffmanTableF;
				}
				else if (flagValue == 1)
				{
					huffmanFSTable = HuffmanDecoder.huffmanTableG;
				}
				int flagValue2 = this.textRegionHuffmanFlags.getFlagValue("SB_HUFF_DS");
				if (flagValue2 == 0)
				{
					huffmanDSTable = HuffmanDecoder.huffmanTableH;
				}
				else if (flagValue2 == 1)
				{
					huffmanDSTable = HuffmanDecoder.huffmanTableI;
				}
				else if (flagValue2 == 2)
				{
					huffmanDSTable = HuffmanDecoder.huffmanTableJ;
				}
				int flagValue3 = this.textRegionHuffmanFlags.getFlagValue("SB_HUFF_DT");
				if (flagValue3 == 0)
				{
					huffmanDTTable = HuffmanDecoder.huffmanTableK;
				}
				else if (flagValue3 == 1)
				{
					huffmanDTTable = HuffmanDecoder.huffmanTableL;
				}
				else if (flagValue3 == 2)
				{
					huffmanDTTable = HuffmanDecoder.huffmanTableM;
				}
				int flagValue4 = this.textRegionHuffmanFlags.getFlagValue("SB_HUFF_RDW");
				if (flagValue4 == 0)
				{
					huffmanRDWTable = HuffmanDecoder.huffmanTableN;
				}
				else if (flagValue4 == 1)
				{
					huffmanRDWTable = HuffmanDecoder.huffmanTableO;
				}
				int flagValue5 = this.textRegionHuffmanFlags.getFlagValue("SB_HUFF_RDH");
				if (flagValue5 == 0)
				{
					huffmanRDHTable = HuffmanDecoder.huffmanTableN;
				}
				else if (flagValue5 == 1)
				{
					huffmanRDHTable = HuffmanDecoder.huffmanTableO;
				}
				int flagValue6 = this.textRegionHuffmanFlags.getFlagValue("SB_HUFF_RDX");
				if (flagValue6 == 0)
				{
					huffmanRDXTable = HuffmanDecoder.huffmanTableN;
				}
				else if (flagValue6 == 1)
				{
					huffmanRDXTable = HuffmanDecoder.huffmanTableO;
				}
				int flagValue7 = this.textRegionHuffmanFlags.getFlagValue("SB_HUFF_RDY");
				if (flagValue7 == 0)
				{
					huffmanRDYTable = HuffmanDecoder.huffmanTableN;
				}
				else if (flagValue7 == 1)
				{
					huffmanRDYTable = HuffmanDecoder.huffmanTableO;
				}
				if (this.textRegionHuffmanFlags.getFlagValue("SB_HUFF_RSIZE") == 0)
				{
					huffmanRSizeTable = HuffmanDecoder.huffmanTableA;
				}
			}
			long[][] array3 = new long[36][];
			long[][] array4 = new long[num2 + 1L][];
			if (flag)
			{
				this.decoder.consumeRemainingBits();
				int i;
				for (i = 0; i < 32; i++)
				{
					long[][] array5 = array3;
					int num6 = i;
					long[] array6 = new long[4];
					array6[0] = (long)i;
					array6[1] = (long)this.decoder.readBits(4L);
					array5[num6] = array6;
				}
				long[][] array7 = array3;
				int num7 = 32;
				long[] array8 = new long[4];
				array8[0] = 259L;
				array8[1] = (long)this.decoder.readBits(4L);
				array8[2] = 2L;
				array7[num7] = array8;
				long[][] array9 = array3;
				int num8 = 33;
				long[] array10 = new long[4];
				array10[0] = 515L;
				array10[1] = (long)this.decoder.readBits(4L);
				array10[2] = 3L;
				array9[num8] = array10;
				long[][] array11 = array3;
				int num9 = 34;
				long[] array12 = new long[4];
				array12[0] = 523L;
				array12[1] = (long)this.decoder.readBits(4L);
				array12[2] = 7L;
				array11[num9] = array12;
				array3[35] = new long[]
				{
					0L,
					0L,
					HuffmanDecoder.jbig2HuffmanEOT
				};
				array3 = HuffmanDecoder.buildTable(array3, 35);
				i = 0;
				while ((long)i < num2)
				{
					long[][] array13 = array4;
					int num10 = i;
					long[] array14 = new long[4];
					array14[0] = (long)i;
					array13[num10] = array14;
					i++;
				}
				i = 0;
				while ((long)i < num2)
				{
					long num11 = this.huffmanDecoder.decodeInt(array3).intResult();
					if (num11 > 512L)
					{
						for (num11 -= 512L; num11 != 0L; num11 -= 1L)
						{
							if ((long)i >= num2)
							{
								break;
							}
							array4[i++][1] = 0L;
						}
					}
					else if (num11 > 256L)
					{
						for (num11 -= 256L; num11 != 0L; num11 -= 1L)
						{
							if ((long)i >= num2)
							{
								break;
							}
							array4[i][1] = array4[i - 1][1];
							i++;
						}
					}
					else
					{
						array4[i++][1] = num11;
					}
				}
				checked
				{
					array4[(int)((IntPtr)num2)][1] = 0L;
					array4[(int)((IntPtr)num2)][2] = HuffmanDecoder.jbig2HuffmanEOT;
				}
				array4 = HuffmanDecoder.buildTable(array4, (int)num2);
				this.decoder.consumeRemainingBits();
			}
			else
			{
				array4 = null;
				this.arithmeticDecoder.resetIntStats((int)num3);
				this.arithmeticDecoder.start();
			}
			bool flag2 = this.textRegionFlags.getFlagValue("SB_REFINE") != 0;
			long logStrips = (long)this.textRegionFlags.getFlagValue("LOG_SB_STRIPES");
			int flagValue8 = this.textRegionFlags.getFlagValue("SB_DEF_PIXEL");
			int flagValue9 = this.textRegionFlags.getFlagValue("SB_COMB_OP");
			bool transposed = this.textRegionFlags.getFlagValue("TRANSPOSED") != 0;
			int flagValue10 = this.textRegionFlags.getFlagValue("REF_CORNER");
			int flagValue11 = this.textRegionFlags.getFlagValue("SB_DS_OFFSET");
			int flagValue12 = this.textRegionFlags.getFlagValue("SB_R_TEMPLATE");
			if (flag2)
			{
				this.arithmeticDecoder.resetRefinementStats(flagValue12, null);
			}
			JBIG2Bitmap jbig2Bitmap = new JBIG2Bitmap((long)this.regionBitmapWidth, (long)this.regionBitmapHeight, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
			jbig2Bitmap.readTextRegion2(flag, flag2, num, logStrips, num2, array4, num3, array2, flagValue8, flagValue9, transposed, flagValue10, flagValue11, huffmanFSTable, huffmanDSTable, huffmanDTTable, huffmanRDWTable, huffmanRDHTable, huffmanRDXTable, huffmanRDYTable, huffmanRSizeTable, flagValue12, this.symbolRegionAdaptiveTemplateX, this.symbolRegionAdaptiveTemplateY, this.decoder);
			if (this.inlineImage)
			{
				PageInformationSegment pageInformationSegment = this.decoder.findPageSegement(this.segmentHeader.getPageAssociation());
				JBIG2Bitmap pageBitmap = pageInformationSegment.getPageBitmap();
				if (JBIG2StreamDecoder.debug)
				{
					Console.WriteLine(pageBitmap + " " + jbig2Bitmap);
				}
				int flagValue13 = this.regionFlags.getFlagValue(RegionFlags.EXTERNAL_COMBINATION_OPERATOR);
				pageBitmap.combine(jbig2Bitmap, (long)this.regionBitmapXLocation, (long)this.regionBitmapYLocation, (long)flagValue13);
			}
			else
			{
				jbig2Bitmap.setBitmapNumber(base.getSegmentHeader().getSegmentNumber());
				this.decoder.appendBitmap(jbig2Bitmap);
			}
			this.decoder.consumeRemainingBits();
		}

		void readTextRegionFlags()
		{
			short[] array = new short[2];
			this.decoder.readbyte(array);
			int @int = BinaryOperation.getInt16(array);
			this.textRegionFlags.setFlags(@int);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("text region Segment flags = " + @int);
			}
			bool flag = this.textRegionFlags.getFlagValue("SB_HUFF") != 0;
			if (flag)
			{
				short[] array2 = new short[2];
				this.decoder.readbyte(array2);
				@int = BinaryOperation.getInt16(array2);
				this.textRegionHuffmanFlags.setFlags(@int);
				if (JBIG2StreamDecoder.debug)
				{
					Console.WriteLine("text region segment Huffman flags = " + @int);
				}
			}
			bool flag2 = this.textRegionFlags.getFlagValue("SB_REFINE") != 0;
			int flagValue = this.textRegionFlags.getFlagValue("SB_R_TEMPLATE");
			if (flag2 && flagValue == 0)
			{
				this.symbolRegionAdaptiveTemplateX[0] = base.readATValue();
				this.symbolRegionAdaptiveTemplateY[0] = base.readATValue();
				this.symbolRegionAdaptiveTemplateX[1] = base.readATValue();
				this.symbolRegionAdaptiveTemplateY[1] = base.readATValue();
			}
		}

		public TextRegionFlags getTextRegionFlags()
		{
			return this.textRegionFlags;
		}

		public TextRegionHuffmanFlags getTextRegionHuffmanFlags()
		{
			return this.textRegionHuffmanFlags;
		}

		TextRegionFlags textRegionFlags = new TextRegionFlags();

		TextRegionHuffmanFlags textRegionHuffmanFlags = new TextRegionHuffmanFlags();

		bool inlineImage;

		short[] symbolRegionAdaptiveTemplateX = new short[2];

		short[] symbolRegionAdaptiveTemplateY = new short[2];
	}
}
