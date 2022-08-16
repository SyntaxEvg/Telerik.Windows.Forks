using System;

namespace JBig2Decoder
{
	class RefinementRegionFlags : Flags
	{
		public override void setFlags(int flagsAsInt)
		{
			this.flagsAsInt = flagsAsInt;
			this.flags["GR_TEMPLATE"] = flagsAsInt & 1;
			this.flags["TPGDON"] = (flagsAsInt >> 1) & 1;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(this.flags);
			}
		}

		public const string GR_TEMPLATE = "GR_TEMPLATE";

		public const string TPGDON = "TPGDON";
	}
}
