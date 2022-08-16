using System;

namespace JBig2Decoder
{
	class PatternDictionarySegment : Segment
	{
		public PatternDictionarySegment(JBIG2StreamDecoder streamDecoder)
			: base(streamDecoder)
		{
		}

		public override void readSegment()
		{
			this.readPatternDictionaryFlags();
			this.width = (int)this.decoder.readbyte();
			this.height = (int)this.decoder.readbyte();
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(string.Concat(new object[] { "pattern dictionary size = ", this.width, " , ", this.height }));
			}
			short[] array = new short[4];
			this.decoder.readbyte(array);
			this.grayMax = BinaryOperation.getInt32(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("grey max = " + this.grayMax);
			}
			bool flag = this.patternDictionaryFlags.getFlagValue("HD_MMR") == 1;
			int flagValue = this.patternDictionaryFlags.getFlagValue("HD_TEMPLATE");
			if (!flag)
			{
				this.arithmeticDecoder.resetGenericStats(flagValue, null);
				this.arithmeticDecoder.start();
			}
			short[] array2 = new short[4];
			short[] array3 = new short[4];
			array2[0] = (short)(-(short)this.width);
			array3[0] = 0;
			array2[1] = -3;
			array3[1] = -1;
			array2[2] = 2;
			array3[2] = -2;
			array2[3] = -2;
			array3[3] = -2;
			this.size = this.grayMax + 1;
			JBIG2Bitmap jbig2Bitmap = new JBIG2Bitmap((long)(this.size * this.width), (long)this.height, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
			jbig2Bitmap.clear(0);
			jbig2Bitmap.readBitmap(flag, flagValue, false, false, null, array2, array3, (long)(this.segmentHeader.getSegmentDataLength() - 7));
			JBIG2Bitmap[] array4 = new JBIG2Bitmap[this.size];
			int num = 0;
			for (int i = 0; i < this.size; i++)
			{
				array4[i] = jbig2Bitmap.getSlice((long)num, 0L, (long)this.width, (long)this.height);
				num += this.width;
			}
			this.bitmaps = array4;
		}

		public JBIG2Bitmap[] getBitmaps()
		{
			return this.bitmaps;
		}

		void readPatternDictionaryFlags()
		{
			short num = this.decoder.readbyte();
			this.patternDictionaryFlags.setFlags((int)num);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("pattern Dictionary flags = " + num);
			}
		}

		public PatternDictionaryFlags getPatternDictionaryFlags()
		{
			return this.patternDictionaryFlags;
		}

		public int getSize()
		{
			return this.size;
		}

		PatternDictionaryFlags patternDictionaryFlags = new PatternDictionaryFlags();

		int width;

		int height;

		int grayMax;

		JBIG2Bitmap[] bitmaps;

		int size;
	}
}
