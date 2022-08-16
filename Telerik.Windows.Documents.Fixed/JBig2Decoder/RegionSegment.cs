using System;

namespace JBig2Decoder
{
	abstract class RegionSegment : Segment
	{
		public RegionSegment(JBIG2StreamDecoder streamDecoder)
			: base(streamDecoder)
		{
		}

		public override void readSegment()
		{
			short[] array = new short[4];
			this.decoder.readbyte(array);
			this.regionBitmapWidth = BinaryOperation.getInt32(array);
			array = new short[4];
			this.decoder.readbyte(array);
			this.regionBitmapHeight = BinaryOperation.getInt32(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(string.Concat(new object[] { "Bitmap size = ", this.regionBitmapWidth, 'x', this.regionBitmapHeight }));
			}
			array = new short[4];
			this.decoder.readbyte(array);
			this.regionBitmapXLocation = BinaryOperation.getInt32(array);
			array = new short[4];
			this.decoder.readbyte(array);
			this.regionBitmapYLocation = BinaryOperation.getInt32(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(string.Concat(new object[] { "Bitmap location = ", this.regionBitmapXLocation, ',', this.regionBitmapYLocation }));
			}
			short num = this.decoder.readbyte();
			this.regionFlags.setFlags((int)num);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("region Segment flags = " + num);
			}
		}

		protected int regionBitmapWidth;

		protected int regionBitmapHeight;

		protected int regionBitmapXLocation;

		protected int regionBitmapYLocation;

		protected RegionFlags regionFlags = new RegionFlags();
	}
}
