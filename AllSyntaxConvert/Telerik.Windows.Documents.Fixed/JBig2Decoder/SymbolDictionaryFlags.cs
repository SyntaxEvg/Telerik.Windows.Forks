using System;

namespace JBig2Decoder
{
	class SymbolDictionaryFlags : Flags
	{
		public override void setFlags(int flagsAsInt)
		{
			this.flagsAsInt = flagsAsInt;
			this.flags["SD_HUFF"] = flagsAsInt & 1;
			this.flags["SD_REF_AGG"] = (flagsAsInt >> 1) & 1;
			this.flags["SD_HUFF_DH"] = (flagsAsInt >> 2) & 3;
			this.flags["SD_HUFF_DW"] = (flagsAsInt >> 4) & 3;
			this.flags["SD_HUFF_BM_SIZE"] = (flagsAsInt >> 6) & 1;
			this.flags["SD_HUFF_AGG_INST"] = (flagsAsInt >> 7) & 1;
			this.flags["BITMAP_CC_USED"] = (flagsAsInt >> 8) & 1;
			this.flags["BITMAP_CC_RETAINED"] = (flagsAsInt >> 9) & 1;
			this.flags["SD_TEMPLATE"] = (flagsAsInt >> 10) & 3;
			this.flags["SD_R_TEMPLATE"] = (flagsAsInt >> 12) & 1;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(this.flags);
			}
		}

		public const string SD_HUFF = "SD_HUFF";

		public const string SD_REF_AGG = "SD_REF_AGG";

		public const string SD_HUFF_DH = "SD_HUFF_DH";

		public const string SD_HUFF_DW = "SD_HUFF_DW";

		public const string SD_HUFF_BM_SIZE = "SD_HUFF_BM_SIZE";

		public const string SD_HUFF_AGG_INST = "SD_HUFF_AGG_INST";

		public const string BITMAP_CC_USED = "BITMAP_CC_USED";

		public const string BITMAP_CC_RETAINED = "BITMAP_CC_RETAINED";

		public const string SD_TEMPLATE = "SD_TEMPLATE";

		public const string SD_R_TEMPLATE = "SD_R_TEMPLATE";
	}
}
