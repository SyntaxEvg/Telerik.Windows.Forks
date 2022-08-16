using System;

namespace JBig2Decoder
{
	class GenericRegionSegment : RegionSegment
	{
		public GenericRegionSegment(JBIG2StreamDecoder streamDecoder, bool inlineImage)
			: base(streamDecoder)
		{
			this.inlineImage = inlineImage;
		}

		public override void readSegment()
		{
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("==== Reading Immediate Generic Region ====");
			}
			base.readSegment();
			this.readGenericRegionFlags();
			bool flag = this.genericRegionFlags.getFlagValue("MMR") != 0;
			int flagValue = this.genericRegionFlags.getFlagValue("GB_TEMPLATE");
			short[] array = new short[4];
			short[] array2 = new short[4];
			if (!flag)
			{
				if (flagValue == 0)
				{
					array[0] = base.readATValue();
					array2[0] = base.readATValue();
					array[1] = base.readATValue();
					array2[1] = base.readATValue();
					array[2] = base.readATValue();
					array2[2] = base.readATValue();
					array[3] = base.readATValue();
					array2[3] = base.readATValue();
				}
				else
				{
					array[0] = base.readATValue();
					array2[0] = base.readATValue();
				}
				this.arithmeticDecoder.resetGenericStats(flagValue, null);
				this.arithmeticDecoder.start();
			}
			bool typicalPredictionGenericDecodingOn = this.genericRegionFlags.getFlagValue("TPGDON") != 0;
			int num = this.segmentHeader.getSegmentDataLength();
			if (num == -1)
			{
				this.unknownLength = true;
				short num2;
				short num3;
				if (flag)
				{
					num2 = 0;
					num3 = 0;
				}
				else
				{
					num2 = 255;
					num3 = 172;
				}
				int num4 = 0;
				for (;;)
				{
					short num5 = this.decoder.readbyte();
					num4++;
					if (num5 == num2)
					{
						short num6 = this.decoder.readbyte();
						num4++;
						if (num6 == num3)
						{
							break;
						}
					}
				}
				num = num4 - 2;
				this.decoder.movePointer(-num4);
			}
			JBIG2Bitmap jbig2Bitmap = new JBIG2Bitmap((long)this.regionBitmapWidth, (long)this.regionBitmapHeight, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
			jbig2Bitmap.clear(0);
			jbig2Bitmap.readBitmap(flag, flagValue, typicalPredictionGenericDecodingOn, false, null, array, array2, (long)(flag ? 0 : (num - 18)));
			if (this.inlineImage)
			{
				PageInformationSegment pageInformationSegment = this.decoder.findPageSegement(this.segmentHeader.getPageAssociation());
				JBIG2Bitmap pageBitmap = pageInformationSegment.getPageBitmap();
				int flagValue2 = this.regionFlags.getFlagValue(RegionFlags.EXTERNAL_COMBINATION_OPERATOR);
				if (pageInformationSegment.getPageBitmapHeight() == -1 && (long)(this.regionBitmapYLocation + this.regionBitmapHeight) > pageBitmap.getHeight())
				{
					pageBitmap.expand(this.regionBitmapYLocation + this.regionBitmapHeight, pageInformationSegment.getPageInformationFlags().getFlagValue(PageInformationFlags.DEFAULT_PIXEL_VALUE));
				}
				pageBitmap.combine(jbig2Bitmap, (long)this.regionBitmapXLocation, (long)this.regionBitmapYLocation, (long)flagValue2);
			}
			else
			{
				jbig2Bitmap.setBitmapNumber(base.getSegmentHeader().getSegmentNumber());
				this.decoder.appendBitmap(jbig2Bitmap);
			}
			if (this.unknownLength)
			{
				this.decoder.movePointer(4);
			}
		}

		void readGenericRegionFlags()
		{
			short num = this.decoder.readbyte();
			this.genericRegionFlags.setFlags((int)num);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("generic region Segment flags = " + num);
			}
		}

		public GenericRegionFlags getGenericRegionFlags()
		{
			return this.genericRegionFlags;
		}

		GenericRegionFlags genericRegionFlags = new GenericRegionFlags();

		bool inlineImage;

		bool unknownLength;
	}
}
