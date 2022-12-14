using System;

namespace BitMiracle.LibTiff.Classic
{
	enum Compression
	{
		NONE = 1,
		CCITTRLE,
		CCITTFAX3,
		CCITT_T4 = 3,
		CCITTFAX4,
		CCITT_T6 = 4,
		LZW,
		OJPEG,
		JPEG,
		NEXT = 32766,
		CCITTRLEW = 32771,
		PACKBITS = 32773,
		THUNDERSCAN = 32809,
		IT8CTPAD = 32895,
		IT8LW,
		IT8MP,
		IT8BL,
		PIXARFILM = 32908,
		PIXARLOG,
		DEFLATE = 32946,
		ADOBE_DEFLATE = 8,
		DCS = 32947,
		JBIG = 34661,
		SGILOG = 34676,
		SGILOG24,
		JP2000 = 34712
	}
}
