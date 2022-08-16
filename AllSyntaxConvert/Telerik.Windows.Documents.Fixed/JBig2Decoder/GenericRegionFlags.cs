using System;

namespace JBig2Decoder
{
	class GenericRegionFlags : Flags
	{
		public override void setFlags(int flagsAsInt)
		{
			this.flagsAsInt = flagsAsInt;
			this.flags["MMR"] = flagsAsInt & 1;
			this.flags["GB_TEMPLATE"] = (flagsAsInt >> 1) & 3;
			this.flags["TPGDON"] = (flagsAsInt >> 3) & 1;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(this.flags);
			}
		}

		public const string MMR = "MMR";

		public const string GB_TEMPLATE = "GB_TEMPLATE";

		public const string TPGDON = "TPGDON";
	}
}
