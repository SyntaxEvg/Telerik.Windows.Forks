using System;

namespace JBig2Decoder
{
	class PageInformationFlags : Flags
	{
		public override void setFlags(int flagsAsInt)
		{
			this.flagsAsInt = flagsAsInt;
			this.flags[PageInformationFlags.DEFAULT_PIXEL_VALUE] = (flagsAsInt >> 2) & 1;
			this.flags[PageInformationFlags.DEFAULT_COMBINATION_OPERATOR] = (flagsAsInt >> 3) & 3;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(this.flags);
			}
		}

		public static string DEFAULT_PIXEL_VALUE = "DEFAULT_PIXEL_VALUE";

		public static string DEFAULT_COMBINATION_OPERATOR = "DEFAULT_COMBINATION_OPERATOR";
	}
}
