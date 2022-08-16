using System;

namespace BitMiracle.LibTiff.Classic
{
	enum TiffType : short
	{
		NOTYPE,
		ANY = 0,
		BYTE,
		ASCII,
		SHORT,
		LONG,
		RATIONAL,
		SBYTE,
		UNDEFINED,
		SSHORT,
		SLONG,
		SRATIONAL,
		FLOAT,
		DOUBLE,
		IFD
	}
}
