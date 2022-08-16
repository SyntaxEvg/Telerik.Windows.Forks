using System;
using System.Collections;

namespace BitMiracle.LibTiff.Classic.Internal
{
	class TagCompare : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			TiffFieldInfo tiffFieldInfo = x as TiffFieldInfo;
			TiffFieldInfo tiffFieldInfo2 = y as TiffFieldInfo;
			if (tiffFieldInfo.Tag != tiffFieldInfo2.Tag)
			{
				return tiffFieldInfo.Tag - tiffFieldInfo2.Tag;
			}
			if (tiffFieldInfo.Type != TiffType.NOTYPE)
			{
				return (int)(tiffFieldInfo2.Type - tiffFieldInfo.Type);
			}
			return 0;
		}
	}
}
