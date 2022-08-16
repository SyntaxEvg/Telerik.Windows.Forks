using System;

namespace JBig2Decoder
{
	class TextRegionHuffmanFlags : Flags
	{
		public override void setFlags(int flagsAsInt)
		{
			this.flagsAsInt = flagsAsInt;
			this.flags["SB_HUFF_FS"] = flagsAsInt & 3;
			this.flags["SB_HUFF_DS"] = (flagsAsInt >> 2) & 3;
			this.flags["SB_HUFF_DT"] = (flagsAsInt >> 4) & 3;
			this.flags["SB_HUFF_RDW"] = (flagsAsInt >> 6) & 3;
			this.flags["SB_HUFF_RDH"] = (flagsAsInt >> 8) & 3;
			this.flags["SB_HUFF_RDX"] = (flagsAsInt >> 10) & 3;
			this.flags["SB_HUFF_RDY"] = (flagsAsInt >> 12) & 3;
			this.flags["SB_HUFF_RSIZE"] = (flagsAsInt >> 14) & 1;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(this.flags);
			}
		}

		public const string SB_HUFF_FS = "SB_HUFF_FS";

		public const string SB_HUFF_DS = "SB_HUFF_DS";

		public const string SB_HUFF_DT = "SB_HUFF_DT";

		public const string SB_HUFF_RDW = "SB_HUFF_RDW";

		public const string SB_HUFF_RDH = "SB_HUFF_RDH";

		public const string SB_HUFF_RDX = "SB_HUFF_RDX";

		public const string SB_HUFF_RDY = "SB_HUFF_RDY";

		public const string SB_HUFF_RSIZE = "SB_HUFF_RSIZE";
	}
}
