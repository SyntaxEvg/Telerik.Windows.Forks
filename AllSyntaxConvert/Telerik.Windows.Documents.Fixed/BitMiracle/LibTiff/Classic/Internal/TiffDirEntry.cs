using System;
using System.Globalization;

namespace BitMiracle.LibTiff.Classic.Internal
{
	class TiffDirEntry
	{
		public new string ToString()
		{
			return string.Concat(new string[]
			{
				this.tdir_tag.ToString(),
				", ",
				this.tdir_type.ToString(),
				" ",
				this.tdir_offset.ToString(CultureInfo.InvariantCulture)
			});
		}

		public const int SizeInBytes = 12;

		public TiffTag tdir_tag;

		public TiffType tdir_type;

		public int tdir_count;

		public uint tdir_offset;
	}
}
