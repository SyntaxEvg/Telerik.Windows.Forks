using System;

namespace JBig2Decoder
{
	class PageInformationSegment : Segment
	{
		public PageInformationSegment(JBIG2StreamDecoder streamDecoder)
			: base(streamDecoder)
		{
		}

		public PageInformationFlags getPageInformationFlags()
		{
			return this.pageInformationFlags;
		}

		public JBIG2Bitmap getPageBitmap()
		{
			return this.pageBitmap;
		}

		public override void readSegment()
		{
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("==== Reading Page Information Dictionary ====");
			}
			short[] array = new short[4];
			this.decoder.readbyte(array);
			this.pageBitmapWidth = BinaryOperation.getInt32(array);
			array = new short[4];
			this.decoder.readbyte(array);
			this.pageBitmapHeight = BinaryOperation.getInt32(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(string.Concat(new object[] { "Bitmap size = ", this.pageBitmapWidth, 'x', this.pageBitmapHeight }));
			}
			array = new short[4];
			this.decoder.readbyte(array);
			this.xResolution = BinaryOperation.getInt32(array);
			array = new short[4];
			this.decoder.readbyte(array);
			this.yResolution = BinaryOperation.getInt32(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(string.Concat(new object[] { "Resolution = ", this.xResolution, 'x', this.yResolution }));
			}
			short num = this.decoder.readbyte();
			this.pageInformationFlags.setFlags((int)num);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("symbolDictionaryFlags = " + num);
			}
			array = new short[2];
			this.decoder.readbyte(array);
			this.pageStriping = BinaryOperation.getInt16(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("Page Striping = " + this.pageStriping);
			}
			int flagValue = this.pageInformationFlags.getFlagValue(PageInformationFlags.DEFAULT_PIXEL_VALUE);
			int num2;
			if (this.pageBitmapHeight == -1)
			{
				num2 = this.pageStriping & 32767;
			}
			else
			{
				num2 = this.pageBitmapHeight;
			}
			this.pageBitmap = new JBIG2Bitmap((long)this.pageBitmapWidth, (long)num2, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
			this.pageBitmap.clear(flagValue);
		}

		public int getPageBitmapHeight()
		{
			return this.pageBitmapHeight;
		}

		int pageBitmapHeight;

		int pageBitmapWidth;

		int yResolution;

		int xResolution;

		PageInformationFlags pageInformationFlags = new PageInformationFlags();

		int pageStriping;

		JBIG2Bitmap pageBitmap;
	}
}
