using System;

namespace BitMiracle.LibTiff.Classic
{
	[Flags]
	enum TiffPrintFlags
	{
		NONE = 0,
		STRIPS = 1,
		CURVES = 2,
		COLORMAP = 4,
		JPEGQTABLES = 256,
		JPEGACTABLES = 512,
		JPEGDCTABLES = 512
	}
}
