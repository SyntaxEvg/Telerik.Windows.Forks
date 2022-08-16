using System;

namespace JBig2Decoder
{
	class TextRegionFlags : Flags
	{
		public override void setFlags(int flagsAsInt)
		{
			this.flagsAsInt = flagsAsInt;
			this.flags["SB_HUFF"] = flagsAsInt & 1;
			this.flags["SB_REFINE"] = (flagsAsInt >> 1) & 1;
			this.flags["LOG_SB_STRIPES"] = (flagsAsInt >> 2) & 3;
			this.flags["REF_CORNER"] = (flagsAsInt >> 4) & 3;
			this.flags["TRANSPOSED"] = (flagsAsInt >> 6) & 1;
			this.flags["SB_COMB_OP"] = (flagsAsInt >> 7) & 3;
			this.flags["SB_DEF_PIXEL"] = (flagsAsInt >> 9) & 1;
			int num = (flagsAsInt >> 10) & 31;
			if ((num & 16) != 0)
			{
				num |= -16;
			}
			this.flags["SB_DS_OFFSET"] = num;
			this.flags["SB_R_TEMPLATE"] = (flagsAsInt >> 15) & 1;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(this.flags);
			}
		}

		public const string SB_HUFF = "SB_HUFF";

		public const string SB_REFINE = "SB_REFINE";

		public const string LOG_SB_STRIPES = "LOG_SB_STRIPES";

		public const string REF_CORNER = "REF_CORNER";

		public const string TRANSPOSED = "TRANSPOSED";

		public const string SB_COMB_OP = "SB_COMB_OP";

		public const string SB_DEF_PIXEL = "SB_DEF_PIXEL";

		public const string SB_DS_OFFSET = "SB_DS_OFFSET";

		public const string SB_R_TEMPLATE = "SB_R_TEMPLATE";
	}
}
