using System;

namespace JBig2Decoder
{
	class RefinementRegionSegment : RegionSegment
	{
		public RefinementRegionSegment(JBIG2StreamDecoder streamDecoder, bool inlineImage, int[] referedToSegments, int noOfReferedToSegments)
			: base(streamDecoder)
		{
			this.inlineImage = inlineImage;
			this.referedToSegments = referedToSegments;
			this.noOfReferedToSegments = noOfReferedToSegments;
		}

		public override void readSegment()
		{
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("==== Reading Generic Refinement Region ====");
			}
			base.readSegment();
			this.readGenericRegionFlags();
			short[] array = new short[2];
			short[] array2 = new short[2];
			int flagValue = this.refinementRegionFlags.getFlagValue("GR_TEMPLATE");
			if (flagValue == 0)
			{
				array[0] = base.readATValue();
				array2[0] = base.readATValue();
				array[1] = base.readATValue();
				array2[1] = base.readATValue();
			}
			if (this.noOfReferedToSegments == 0 || this.inlineImage)
			{
				PageInformationSegment pageInformationSegment = this.decoder.findPageSegement(this.segmentHeader.getPageAssociation());
				JBIG2Bitmap pageBitmap = pageInformationSegment.getPageBitmap();
				if (pageInformationSegment.getPageBitmapHeight() == -1 && (long)(this.regionBitmapYLocation + this.regionBitmapHeight) > pageBitmap.getHeight())
				{
					pageBitmap.expand(this.regionBitmapYLocation + this.regionBitmapHeight, pageInformationSegment.getPageInformationFlags().getFlagValue(PageInformationFlags.DEFAULT_PIXEL_VALUE));
				}
			}
			if (this.noOfReferedToSegments > 1)
			{
				if (JBIG2StreamDecoder.debug)
				{
					Console.WriteLine("Bad reference in JBIG2 generic refinement Segment");
				}
				return;
			}
			JBIG2Bitmap referredToBitmap;
			if (this.noOfReferedToSegments == 1)
			{
				referredToBitmap = this.decoder.findBitmap(this.referedToSegments[0]);
			}
			else
			{
				PageInformationSegment pageInformationSegment2 = this.decoder.findPageSegement(this.segmentHeader.getPageAssociation());
				JBIG2Bitmap pageBitmap2 = pageInformationSegment2.getPageBitmap();
				referredToBitmap = pageBitmap2.getSlice((long)this.regionBitmapXLocation, (long)this.regionBitmapYLocation, (long)this.regionBitmapWidth, (long)this.regionBitmapHeight);
			}
			this.arithmeticDecoder.resetRefinementStats(flagValue, null);
			this.arithmeticDecoder.start();
			bool typicalPredictionGenericRefinementOn = this.refinementRegionFlags.getFlagValue("TPGDON") != 0;
			JBIG2Bitmap jbig2Bitmap = new JBIG2Bitmap((long)this.regionBitmapWidth, (long)this.regionBitmapHeight, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
			jbig2Bitmap.readGenericRefinementRegion((long)flagValue, typicalPredictionGenericRefinementOn, referredToBitmap, 0L, 0L, array, array2);
			if (this.inlineImage)
			{
				PageInformationSegment pageInformationSegment3 = this.decoder.findPageSegement(this.segmentHeader.getPageAssociation());
				JBIG2Bitmap pageBitmap3 = pageInformationSegment3.getPageBitmap();
				int flagValue2 = this.regionFlags.getFlagValue(RegionFlags.EXTERNAL_COMBINATION_OPERATOR);
				pageBitmap3.combine(jbig2Bitmap, (long)this.regionBitmapXLocation, (long)this.regionBitmapYLocation, (long)flagValue2);
				return;
			}
			jbig2Bitmap.setBitmapNumber(base.getSegmentHeader().getSegmentNumber());
			this.decoder.appendBitmap(jbig2Bitmap);
		}

		void readGenericRegionFlags()
		{
			short num = this.decoder.readbyte();
			this.refinementRegionFlags.setFlags((int)num);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("generic region Segment flags = " + num);
			}
		}

		public RefinementRegionFlags getGenericRegionFlags()
		{
			return this.refinementRegionFlags;
		}

		RefinementRegionFlags refinementRegionFlags = new RefinementRegionFlags();

		bool inlineImage;

		int noOfReferedToSegments;

		int[] referedToSegments;
	}
}
