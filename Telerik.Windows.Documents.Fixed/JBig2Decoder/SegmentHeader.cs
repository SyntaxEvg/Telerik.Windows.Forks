using System;

namespace JBig2Decoder
{
	class SegmentHeader
	{
		public void setSegmentNumber(int SegmentNumber)
		{
			this.segmentNumber = SegmentNumber;
		}

		public void setSegmentHeaderFlags(short SegmentHeaderFlags)
		{
			this.segmentType = (int)(SegmentHeaderFlags & 63);
			this.pageAssociationSizeSet = (SegmentHeaderFlags & 64) == 64;
			this.deferredNonRetainSet = (SegmentHeaderFlags & 80) == 80;
		}

		public void setReferredToSegmentCount(int referredToSegmentCount)
		{
			this.referredToSegmentCount = referredToSegmentCount;
		}

		public void setRententionFlags(short[] rententionFlags)
		{
			this.rententionFlags = rententionFlags;
		}

		public void setReferredToSegments(int[] referredToSegments)
		{
			this.referredToSegments = referredToSegments;
		}

		public int[] getReferredToSegments()
		{
			return this.referredToSegments;
		}

		public int getSegmentType()
		{
			return this.segmentType;
		}

		public int getSegmentNumber()
		{
			return this.segmentNumber;
		}

		public bool isPageAssociationSizeSet()
		{
			return this.pageAssociationSizeSet;
		}

		public bool isDeferredNonRetainSet()
		{
			return this.deferredNonRetainSet;
		}

		public int getReferredToSegmentCount()
		{
			return this.referredToSegmentCount;
		}

		public short[] getRententionFlags()
		{
			return this.rententionFlags;
		}

		public int getPageAssociation()
		{
			return this.pageAssociation;
		}

		public void setPageAssociation(int pageAssociation)
		{
			this.pageAssociation = pageAssociation;
		}

		public void setDataLength(int dataLength)
		{
			this.dataLength = dataLength;
		}

		public void setSegmentType(int type)
		{
			this.segmentType = type;
		}

		public int getSegmentDataLength()
		{
			return this.dataLength;
		}

		int segmentNumber;

		int segmentType;

		bool pageAssociationSizeSet;

		bool deferredNonRetainSet;

		int referredToSegmentCount;

		short[] rententionFlags;

		int[] referredToSegments;

		int pageAssociation;

		int dataLength;
	}
}
