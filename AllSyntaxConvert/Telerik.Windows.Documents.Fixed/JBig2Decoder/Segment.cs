using System;

namespace JBig2Decoder
{
	abstract class Segment
	{
		public Segment(JBIG2StreamDecoder streamDecoder)
		{
			this.decoder = streamDecoder;
			this.huffmanDecoder = this.decoder.getHuffmanDecoder();
			this.arithmeticDecoder = this.decoder.getArithmeticDecoder();
			this.mmrDecoder = this.decoder.getMMRDecoder();
		}

		protected short readATValue()
		{
			short num2;
			short num = (num2 = this.decoder.readbyte());
			if ((num2 & 128) != 0)
			{
				num |= -256;
			}
			return num;
		}

		public SegmentHeader getSegmentHeader()
		{
			return this.segmentHeader;
		}

		public void setSegmentHeader(SegmentHeader segmentHeader)
		{
			this.segmentHeader = segmentHeader;
		}

		public abstract void readSegment();

		public const int SYMBOL_DICTIONARY = 0;

		public const int INTERMEDIATE_TEXT_REGION = 4;

		public const int IMMEDIATE_TEXT_REGION = 6;

		public const int IMMEDIATE_LOSSLESS_TEXT_REGION = 7;

		public const int PATTERN_DICTIONARY = 16;

		public const int INTERMEDIATE_HALFTONE_REGION = 20;

		public const int IMMEDIATE_HALFTONE_REGION = 22;

		public const int IMMEDIATE_LOSSLESS_HALFTONE_REGION = 23;

		public const int INTERMEDIATE_GENERIC_REGION = 36;

		public const int IMMEDIATE_GENERIC_REGION = 38;

		public const int IMMEDIATE_LOSSLESS_GENERIC_REGION = 39;

		public const int INTERMEDIATE_GENERIC_REFINEMENT_REGION = 40;

		public const int IMMEDIATE_GENERIC_REFINEMENT_REGION = 42;

		public const int IMMEDIATE_LOSSLESS_GENERIC_REFINEMENT_REGION = 43;

		public const int PAGE_INFORMATION = 48;

		public const int END_OF_PAGE = 49;

		public const int END_OF_STRIPE = 50;

		public const int END_OF_FILE = 51;

		public const int PROFILES = 52;

		public const int TABLES = 53;

		public const int EXTENSION = 62;

		public const int BITMAP = 70;

		protected SegmentHeader segmentHeader;

		protected HuffmanDecoder huffmanDecoder;

		protected ArithmeticDecoder arithmeticDecoder;

		protected MMRDecoder mmrDecoder;

		protected JBIG2StreamDecoder decoder;
	}
}
