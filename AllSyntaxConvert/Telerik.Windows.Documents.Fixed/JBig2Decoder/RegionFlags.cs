using System;

namespace JBig2Decoder
{
	class RegionFlags : Flags
	{
		public override void setFlags(int flagsAsInt)
		{
			this.flagsAsInt = flagsAsInt;
			this.flags[RegionFlags.EXTERNAL_COMBINATION_OPERATOR] = flagsAsInt & 7;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(this.flags);
			}
		}

		public static string EXTERNAL_COMBINATION_OPERATOR = "EXTERNAL_COMBINATION_OPERATOR";
	}
}
