using System;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces
{
	class YCbCrK
	{
		public static void ToCmyk(ref byte c1, ref byte c2, ref byte c3, ref byte c4)
		{
			byte b = c1;
			byte b2 = c2;
			byte b3 = c3;
			byte b4 = c4;
			YCbCr.ToRgb(ref b, ref b2, ref b3);
			c1 = (byte)(byte.MaxValue - b);
			c2 = (byte)(byte.MaxValue - b2);
			c3 = (byte)(byte.MaxValue - b3);
			c4 = b4;
		}

		public static void FromCmyk(ref byte c1, ref byte c2, ref byte c3, ref byte c4)
		{
			byte b = (byte)(byte.MaxValue - c1);
			byte b2 = (byte)(byte.MaxValue - c2);
			byte b3 = (byte)(byte.MaxValue - c3);
			byte b4 = c4;
			YCbCr.FromRgb(ref b, ref b2, ref b3);
			c1 = b;
			c2 = b2;
			c3 = b3;
			c4 = b4;
		}
	}
}
