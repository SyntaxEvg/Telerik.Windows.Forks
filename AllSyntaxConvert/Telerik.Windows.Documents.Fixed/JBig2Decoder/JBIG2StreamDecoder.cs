using System;
using System.Collections.Generic;
using System.Linq;

namespace JBig2Decoder
{
	class JBIG2StreamDecoder
	{
		public void movePointer(int i)
		{
			this.reader.movePointer(i);
		}

		public void setGlobalData(byte[] data)
		{
			this.globalData = data;
		}

		public byte[] decodeJBIG2(byte[] data)
		{
			this.reader = new Big2StreamReader(data);
			this.resetDecoder();
			bool flag = this.checkHeader();
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("validFile = " + flag);
			}
			if (!flag)
			{
				this.noOfPagesKnown = true;
				this.randomAccessOrganisation = false;
				this.noOfPages = 1;
				if (this.globalData != null)
				{
					this.reader = new Big2StreamReader(this.globalData);
					this.huffmanDecoder = new HuffmanDecoder(this.reader);
					this.mmrDecoder = new MMRDecoder(this.reader);
					this.arithmeticDecoder = new ArithmeticDecoder(this.reader);
					this.readSegments();
					this.reader = new Big2StreamReader(data);
				}
				else
				{
					this.reader.movePointer(-8);
				}
			}
			else
			{
				if (JBIG2StreamDecoder.debug)
				{
					Console.WriteLine("==== File Header ====");
				}
				this.setFileHeaderFlags();
				if (JBIG2StreamDecoder.debug)
				{
					Console.WriteLine("randomAccessOrganisation = " + this.randomAccessOrganisation);
					Console.WriteLine("noOfPagesKnown = " + this.noOfPagesKnown);
				}
				if (this.noOfPagesKnown)
				{
					this.noOfPages = this.getNoOfPages();
					if (JBIG2StreamDecoder.debug)
					{
						Console.WriteLine("noOfPages = " + this.noOfPages);
					}
				}
			}
			this.huffmanDecoder = new HuffmanDecoder(this.reader);
			this.mmrDecoder = new MMRDecoder(this.reader);
			this.arithmeticDecoder = new ArithmeticDecoder(this.reader);
			this.readSegments();
			JBIG2Bitmap pageBitmap = this.findPageSegement(1).getPageBitmap();
			pageBitmap.getWidth();
			pageBitmap.getHeight();
			return pageBitmap.getData(true);
		}

		public HuffmanDecoder getHuffmanDecoder()
		{
			return this.huffmanDecoder;
		}

		public MMRDecoder getMMRDecoder()
		{
			return this.mmrDecoder;
		}

		public ArithmeticDecoder getArithmeticDecoder()
		{
			return this.arithmeticDecoder;
		}

		void resetDecoder()
		{
			this.noOfPagesKnown = false;
			this.randomAccessOrganisation = false;
			this.noOfPages = -1;
			this.segments.Clear();
			this.bitmaps.Clear();
		}

		void readSegments()
		{
			bool flag = false;
			while (!this.reader.isFinished() && !flag)
			{
				SegmentHeader segmentHeader = new SegmentHeader();
				this.readSegmentHeader(segmentHeader);
				Segment segment = null;
				int segmentType = segmentHeader.getSegmentType();
				int[] referredToSegments = segmentHeader.getReferredToSegments();
				int referredToSegmentCount = segmentHeader.getReferredToSegmentCount();
				int num = segmentType;
				if (num <= 16)
				{
					if (num != 0)
					{
						switch (num)
						{
						case 4:
							segment = new TextRegionSegment(this, false);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 5:
							break;
						case 6:
							segment = new TextRegionSegment(this, true);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 7:
							segment = new TextRegionSegment(this, true);
							segment.setSegmentHeader(segmentHeader);
							break;
						default:
							if (num == 16)
							{
								segment = new PatternDictionarySegment(this);
								segment.setSegmentHeader(segmentHeader);
							}
							break;
						}
					}
					else
					{
						segment = new SymbolDictionarySegment(this);
						segment.setSegmentHeader(segmentHeader);
					}
				}
				else
				{
					switch (num)
					{
					case 20:
						segment = new HalftoneRegionSegment(this, false);
						segment.setSegmentHeader(segmentHeader);
						break;
					case 21:
						break;
					case 22:
						segment = new HalftoneRegionSegment(this, true);
						segment.setSegmentHeader(segmentHeader);
						break;
					case 23:
						segment = new HalftoneRegionSegment(this, true);
						segment.setSegmentHeader(segmentHeader);
						break;
					default:
						switch (num)
						{
						case 36:
							segment = new GenericRegionSegment(this, false);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 37:
						case 41:
						case 44:
						case 45:
						case 46:
						case 47:
						case 52:
						case 53:
							break;
						case 38:
							segment = new GenericRegionSegment(this, true);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 39:
							segment = new GenericRegionSegment(this, true);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 40:
							segment = new RefinementRegionSegment(this, false, referredToSegments, referredToSegmentCount);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 42:
							segment = new RefinementRegionSegment(this, true, referredToSegments, referredToSegmentCount);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 43:
							segment = new RefinementRegionSegment(this, true, referredToSegments, referredToSegmentCount);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 48:
							segment = new PageInformationSegment(this);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 49:
							continue;
						case 50:
							segment = new EndOfStripeSegment(this);
							segment.setSegmentHeader(segmentHeader);
							break;
						case 51:
							flag = true;
							continue;
						default:
							if (num == 62)
							{
								segment = new ExtensionSegment(this);
								segment.setSegmentHeader(segmentHeader);
							}
							break;
						}
						break;
					}
				}
				if (!this.randomAccessOrganisation)
				{
					segment.readSegment();
				}
				this.segments.Add(segment);
			}
			if (this.randomAccessOrganisation)
			{
				foreach (Segment segment2 in this.segments)
				{
					segment2.readSegment();
				}
			}
		}

		public PageInformationSegment findPageSegement(int page)
		{
			foreach (Segment segment in this.segments)
			{
				SegmentHeader segmentHeader = segment.getSegmentHeader();
				if (segmentHeader.getSegmentType() == 48 && segmentHeader.getPageAssociation() == page)
				{
					return (PageInformationSegment)segment;
				}
			}
			return null;
		}

		public Segment findSegment(int segmentNumber)
		{
			foreach (Segment segment in this.segments)
			{
				if (segment.getSegmentHeader().getSegmentNumber() == segmentNumber)
				{
					return segment;
				}
			}
			return null;
		}

		void readSegmentHeader(SegmentHeader segmentHeader)
		{
			this.handleSegmentNumber(segmentHeader);
			this.handleSegmentHeaderFlags(segmentHeader);
			this.handleSegmentReferredToCountAndRententionFlags(segmentHeader);
			this.handleReferedToSegmentNumbers(segmentHeader);
			this.handlePageAssociation(segmentHeader);
			if (segmentHeader.getSegmentType() != 51)
			{
				this.handleSegmentDataLength(segmentHeader);
			}
		}

		void handlePageAssociation(SegmentHeader segmentHeader)
		{
			bool flag = segmentHeader.isPageAssociationSizeSet();
			int num;
			if (flag)
			{
				short[] array = new short[4];
				this.reader.readbyte(array);
				num = BinaryOperation.getInt32(array);
			}
			else
			{
				num = (int)this.reader.readbyte();
			}
			segmentHeader.setPageAssociation(num);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("pageAssociation = " + num);
			}
		}

		void handleSegmentNumber(SegmentHeader segmentHeader)
		{
			short[] array = new short[4];
			this.reader.readbyte(array);
			int @int = BinaryOperation.getInt32(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("SegmentNumber = " + @int);
			}
			segmentHeader.setSegmentNumber(@int);
		}

		void handleSegmentHeaderFlags(SegmentHeader segmentHeader)
		{
			short segmentHeaderFlags = this.reader.readbyte();
			segmentHeader.setSegmentHeaderFlags(segmentHeaderFlags);
		}

		void handleSegmentReferredToCountAndRententionFlags(SegmentHeader segmentHeader)
		{
			short num = this.reader.readbyte();
			int num2 = (num & 224) >> 5;
			short[] array = null;
			short num3 = (short)(num & 31);
			if (num2 <= 4)
			{
				array = new short[] { num3 };
			}
			else if (num2 == 7)
			{
				short[] array2 = new short[4];
				array2[0] = num3;
				for (int i = 1; i < 4; i++)
				{
					array2[i] = this.reader.readbyte();
				}
				num2 = BinaryOperation.getInt32(array2);
				int num4 = (int)Math.Ceiling(4.0 + (double)(num2 + 1) / 8.0);
				int num5 = num4 - 4;
				array = new short[num5];
				this.reader.readbyte(array);
			}
			segmentHeader.setReferredToSegmentCount(num2);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("referredToSegmentCount = " + num2);
			}
			segmentHeader.setRententionFlags(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("retentionFlags = ");
			}
			if (JBIG2StreamDecoder.debug)
			{
				for (int j = 0; j < array.Length; j++)
				{
					Console.WriteLine(array[j] + " ");
				}
				Console.WriteLine("");
			}
		}

		void handleReferedToSegmentNumbers(SegmentHeader segmentHeader)
		{
			int referredToSegmentCount = segmentHeader.getReferredToSegmentCount();
			int[] array = new int[referredToSegmentCount];
			int segmentNumber = segmentHeader.getSegmentNumber();
			if (segmentNumber <= 256)
			{
				for (int i = 0; i < referredToSegmentCount; i++)
				{
					array[i] = (int)this.reader.readbyte();
				}
			}
			else if (segmentNumber <= 65536)
			{
				short[] array2 = new short[2];
				for (int j = 0; j < referredToSegmentCount; j++)
				{
					this.reader.readbyte(array2);
					array[j] = BinaryOperation.getInt16(array2);
				}
			}
			else
			{
				short[] array3 = new short[4];
				for (int k = 0; k < referredToSegmentCount; k++)
				{
					this.reader.readbyte(array3);
					array[k] = BinaryOperation.getInt32(array3);
				}
			}
			segmentHeader.setReferredToSegments(array);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("referredToSegments = ");
				for (int l = 0; l < array.Length; l++)
				{
					Console.WriteLine(array[l] + " ");
				}
				Console.WriteLine("");
			}
		}

		int getNoOfPages()
		{
			short[] array = new short[4];
			this.reader.readbyte(array);
			return BinaryOperation.getInt32(array);
		}

		void handleSegmentDataLength(SegmentHeader segmentHeader)
		{
			short[] array = new short[4];
			this.reader.readbyte(array);
			int @int = BinaryOperation.getInt32(array);
			segmentHeader.setDataLength(@int);
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine("dateLength = " + @int);
			}
		}

		void setFileHeaderFlags()
		{
			short num = this.reader.readbyte();
			if ((num & 252) != 0)
			{
				Console.WriteLine("Warning, reserved bits (2-7) of file header flags are not zero " + num);
			}
			int num2 = (int)(num & 1);
			this.randomAccessOrganisation = num2 == 0;
			int num3 = (int)(num & 2);
			this.noOfPagesKnown = num3 == 0;
		}

		bool checkHeader()
		{
			short[] first = new short[] { 151, 74, 66, 50, 13, 10, 26, 10 };
			short[] array = new short[8];
			this.reader.readbyte(array);
			return first.SequenceEqual(array);
		}

		public int readBits(long num)
		{
			return this.reader.readBits(num);
		}

		public int readBit()
		{
			return this.reader.readBit();
		}

		public void readbyte(short[] buff)
		{
			this.reader.readbyte(buff);
		}

		public void consumeRemainingBits()
		{
			this.reader.consumeRemainingBits();
		}

		public short readbyte()
		{
			return this.reader.readbyte();
		}

		public void appendBitmap(JBIG2Bitmap bitmap)
		{
			this.bitmaps.Add(bitmap);
		}

		public JBIG2Bitmap findBitmap(int bitmapNumber)
		{
			foreach (JBIG2Bitmap jbig2Bitmap in this.bitmaps)
			{
				if (jbig2Bitmap.getBitmapNumber() == bitmapNumber)
				{
					return jbig2Bitmap;
				}
			}
			return null;
		}

		public JBIG2Bitmap getPageAsJBIG2Bitmap(int i)
		{
			return this.findPageSegement(1).getPageBitmap();
		}

		public bool isNumberOfPagesKnown()
		{
			return this.noOfPagesKnown;
		}

		public int getNumberOfPages()
		{
			return this.noOfPages;
		}

		public bool isRandomAccessOrganisationUsed()
		{
			return this.randomAccessOrganisation;
		}

		public List<Segment> getAllSegments()
		{
			return this.segments;
		}

		public static bool debug;

		Big2StreamReader reader;

		ArithmeticDecoder arithmeticDecoder;

		HuffmanDecoder huffmanDecoder;

		MMRDecoder mmrDecoder;

		bool noOfPagesKnown;

		bool randomAccessOrganisation;

		int noOfPages = -1;

		List<Segment> segments = new List<Segment>();

		List<JBIG2Bitmap> bitmaps = new List<JBIG2Bitmap>();

		byte[] globalData;
	}
}
