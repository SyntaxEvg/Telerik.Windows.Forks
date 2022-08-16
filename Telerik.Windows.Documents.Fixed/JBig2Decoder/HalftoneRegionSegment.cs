using System;

namespace JBig2Decoder
{
	class HalftoneRegionSegment : RegionSegment
	{
		public HalftoneRegionSegment(JBIG2StreamDecoder streamDecoder, bool inlineImage)
			: base(streamDecoder)
		{
			this.inlineImage = inlineImage;
		}

		public override void readSegment()
		{
			base.readSegment();
			this.readHalftoneRegionFlags();
			short[] array = new short[4];
			this.decoder.readbyte(array);
			int @int = BinaryOperation.getInt32(array);
			array = new short[4];
			this.decoder.readbyte(array);
			int int2 = BinaryOperation.getInt32(array);
			array = new short[4];
			this.decoder.readbyte(array);
			int int3 = BinaryOperation.getInt32(array);
			array = new short[4];
			this.decoder.readbyte(array);
			int int4 = BinaryOperation.getInt32(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(string.Concat(new object[] { "grid pos and size = ", int3, ',', int4, ' ', @int, ',', int2 }));
			}
			array = new short[2];
			this.decoder.readbyte(array);
			int int5 = BinaryOperation.getInt16(array);
			array = new short[2];
			this.decoder.readbyte(array);
			int int6 = BinaryOperation.getInt16(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(string.Concat(new object[] { "step size = ", int5, ',', int6 }));
			}
			int[] referredToSegments = this.segmentHeader.getReferredToSegments();
			if (referredToSegments.Length != 1)
			{
				Console.WriteLine("Error in halftone Segment. refSegs should == 1");
			}
			Segment segment = this.decoder.findSegment(referredToSegments[0]);
			if (segment.getSegmentHeader().getSegmentType() != 16 && JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("Error in halftone Segment. bad symbol dictionary reference");
			}
			PatternDictionarySegment patternDictionarySegment = (PatternDictionarySegment)segment;
			int num = 0;
			int i;
			for (i = 1; i < patternDictionarySegment.getSize(); i <<= 1)
			{
				num++;
			}
			JBIG2Bitmap jbig2Bitmap = patternDictionarySegment.getBitmaps()[0];
			long width = jbig2Bitmap.getWidth();
			long height = jbig2Bitmap.getHeight();
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(string.Concat(new object[] { "pattern size = ", width, ',', height }));
			}
			bool flag = this.halftoneRegionFlags.getFlagValue("H_MMR") != 0;
			int flagValue = this.halftoneRegionFlags.getFlagValue("H_TEMPLATE");
			if (!flag)
			{
				this.arithmeticDecoder.resetGenericStats(flagValue, null);
				this.arithmeticDecoder.start();
			}
			int flagValue2 = this.halftoneRegionFlags.getFlagValue("H_DEF_PIXEL");
			jbig2Bitmap = new JBIG2Bitmap((long)this.regionBitmapWidth, (long)this.regionBitmapHeight, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
			jbig2Bitmap.clear(flagValue2);
			bool flag2 = this.halftoneRegionFlags.getFlagValue("H_ENABLE_SKIP") != 0;
			JBIG2Bitmap jbig2Bitmap2 = null;
			if (flag2)
			{
				jbig2Bitmap2 = new JBIG2Bitmap((long)@int, (long)int2, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
				jbig2Bitmap2.clear(0);
				for (int j = 0; j < int2; j++)
				{
					for (int k = 0; k < @int; k++)
					{
						int num2 = int3 + j * int6 + k * int5;
						int num3 = int4 + j * int5 - k * int6;
						if ((long)num2 + width >> 8 <= 0L || num2 >> 8 >= this.regionBitmapWidth || (long)num3 + height >> 8 <= 0L || num3 >> 8 >= this.regionBitmapHeight)
						{
							jbig2Bitmap2.setPixel((long)j, (long)k, 1L);
						}
					}
				}
			}
			int[] array2 = new int[@int * int2];
			short[] array3 = new short[4];
			short[] array4 = new short[4];
			array3[0] = ((flagValue <= 1) ? 3 : 2);
			array4[0] = -1;
			array3[1] = -3;
			array4[1] = -1;
			array3[2] = 2;
			array4[2] = -2;
			array3[3] = -2;
			array4[3] = -2;
			for (int l = num - 1; l >= 0; l--)
			{
				JBIG2Bitmap jbig2Bitmap3 = new JBIG2Bitmap((long)@int, (long)int2, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
				jbig2Bitmap3.readBitmap(flag, flagValue, false, flag2, jbig2Bitmap2, array3, array4, -1L);
				i = 0;
				for (int m = 0; m < int2; m++)
				{
					for (int n = 0; n < @int; n++)
					{
						int num4 = jbig2Bitmap3.getPixel(n, m) ^ (array2[i] & 1);
						array2[i] = (array2[i] << 1) | num4;
						i++;
					}
				}
			}
			int flagValue3 = this.halftoneRegionFlags.getFlagValue("H_COMB_OP");
			i = 0;
			for (int num5 = 0; num5 < int2; num5++)
			{
				int num6 = int3 + num5 * int6;
				int num7 = int4 + num5 * int5;
				for (int num8 = 0; num8 < @int; num8++)
				{
					if (!flag2 || jbig2Bitmap2.getPixel(num5, num8) != 1)
					{
						JBIG2Bitmap bitmap = patternDictionarySegment.getBitmaps()[array2[i]];
						jbig2Bitmap.combine(bitmap, (long)(num6 >> 8), (long)(num7 >> 8), (long)flagValue3);
					}
					num6 += int5;
					num7 -= int6;
					i++;
				}
			}
			if (this.inlineImage)
			{
				PageInformationSegment pageInformationSegment = this.decoder.findPageSegement(this.segmentHeader.getPageAssociation());
				JBIG2Bitmap pageBitmap = pageInformationSegment.getPageBitmap();
				int flagValue4 = this.regionFlags.getFlagValue(RegionFlags.EXTERNAL_COMBINATION_OPERATOR);
				pageBitmap.combine(jbig2Bitmap, (long)this.regionBitmapXLocation, (long)this.regionBitmapYLocation, (long)flagValue4);
				return;
			}
			jbig2Bitmap.setBitmapNumber(base.getSegmentHeader().getSegmentNumber());
			this.decoder.appendBitmap(jbig2Bitmap);
		}

		void readHalftoneRegionFlags()
		{
			short num = this.decoder.readbyte();
			this.halftoneRegionFlags.setFlags((int)num);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("generic region Segment flags = " + num);
			}
		}

		public HalftoneRegionFlags getHalftoneRegionFlags()
		{
			return this.halftoneRegionFlags;
		}

		HalftoneRegionFlags halftoneRegionFlags = new HalftoneRegionFlags();

		bool inlineImage;
	}
}
