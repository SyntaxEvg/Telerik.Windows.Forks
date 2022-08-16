using System;

namespace BitMiracle.LibTiff.Classic.Internal
{
	struct TiffHeader
	{
		public const int TIFF_MAGIC_SIZE = 2;

		public const int TIFF_VERSION_SIZE = 2;

		public const int TIFF_DIROFFSET_SIZE = 4;

		public const int SizeInBytes = 8;

		public short tiff_magic;

		public short tiff_version;

		public uint tiff_diroff;
	}
}
