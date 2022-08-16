using System;

namespace JBig2Decoder
{
	class PatternDictionaryFlags : Flags
	{
		public override void setFlags(int flagsAsInt)
		{
			this.flagsAsInt = flagsAsInt;
			this.flags["HD_MMR"] = flagsAsInt & 1;
			this.flags["HD_TEMPLATE"] = (flagsAsInt >> 1) & 3;
			if (JBIG2StreamDecoder.debug)
			{
				Console.WriteLine(this.flags);
			}
		}

		public const string HD_MMR = "HD_MMR";

		public const string HD_TEMPLATE = "HD_TEMPLATE";
	}
}
