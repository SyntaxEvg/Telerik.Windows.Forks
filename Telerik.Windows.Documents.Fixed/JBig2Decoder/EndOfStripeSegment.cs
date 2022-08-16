using System;

namespace JBig2Decoder
{
	class EndOfStripeSegment : Segment
	{
		public EndOfStripeSegment(JBIG2StreamDecoder streamDecoder)
			: base(streamDecoder)
		{
		}

		public override void readSegment()
		{
			for (int i = 0; i < base.getSegmentHeader().getSegmentDataLength(); i++)
			{
				this.decoder.readbyte();
			}
		}
	}
}
