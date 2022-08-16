using System;

namespace JBig2Decoder
{
	class HalftoneRegionFlags : Flags
	{
		public override void setFlags(int flagsAsInt)
		{
			this.flagsAsInt = flagsAsInt;
			this.flags["H_MMR"] = flagsAsInt & 1;
			this.flags["H_TEMPLATE"] = (flagsAsInt >> 1) & 3;
			this.flags["H_ENABLE_SKIP"] = (flagsAsInt >> 3) & 1;
			this.flags["H_COMB_OP"] = (flagsAsInt >> 4) & 7;
			this.flags["H_DEF_PIXEL"] = (flagsAsInt >> 7) & 1;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(this.flags);
			}
		}

		public const string H_MMR = "H_MMR";

		public const string H_TEMPLATE = "H_TEMPLATE";

		public const string H_ENABLE_SKIP = "H_ENABLE_SKIP";

		public const string H_COMB_OP = "H_COMB_OP";

		public const string H_DEF_PIXEL = "H_DEF_PIXEL";
	}
}
