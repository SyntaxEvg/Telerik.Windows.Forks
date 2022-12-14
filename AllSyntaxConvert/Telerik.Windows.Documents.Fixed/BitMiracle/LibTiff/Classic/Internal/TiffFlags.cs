using System;

namespace BitMiracle.LibTiff.Classic.Internal
{
	[Flags]
	enum TiffFlags
	{
		MSB2LSB = 1,
		LSB2MSB = 2,
		FILLORDER = 3,
		DIRTYDIRECT = 8,
		BUFFERSETUP = 16,
		CODERSETUP = 32,
		BEENWRITING = 64,
		SWAB = 128,
		NOBITREV = 256,
		MYBUFFER = 512,
		ISTILED = 1024,
		POSTENCODE = 4096,
		INSUBIFD = 8192,
		UPSAMPLED = 16384,
		STRIPCHOP = 32768,
		HEADERONLY = 65536,
		NOREADRAW = 131072
	}
}
