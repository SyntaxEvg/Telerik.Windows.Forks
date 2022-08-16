using System;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces
{
	class YCbCr
	{
		public static void ToRgb(ref byte c1, ref byte c2, ref byte c3)
		{
			byte b = c1;
			byte b2 = c2;
			byte b3 = c3;
			double num = (double)b + 1.402 * (double)(b3 - 128);
			double num2 = (double)b - 0.3441363 * (double)(b2 - 128) - 0.71413636 * (double)(b3 - 128);
			double num3 = (double)b + 1.772 * (double)(b2 - 128);
			c1 = ((byte)((num > 255.0) ? byte.MaxValue : ((num < 0.0) ? 0 : ((byte)num))));
			c2 = ((byte)((num2 > 255.0) ? byte.MaxValue : ((num2 < 0.0) ? 0 : ((byte)num2))));
			c3 = ((byte)((num3 > 255.0) ? byte.MaxValue : ((num3 < 0.0) ? 0 : ((byte)num3))));
		}

		public static void FromRgb(ref byte c1, ref byte c2, ref byte c3)
		{
			byte b = c1;
			byte b2 = c2;
			byte b3 = c3;
			c1 = (byte)(0.299 * (double)b + 0.587 * (double)b2 + 0.114 * (double)b3);
			c2 = (byte)(-0.168736 * (double)b - 0.331264 * (double)b2 + 0.5 * (double)b3 + 128.0);
			c3 = (byte)(0.5 * (double)b - 0.4186876 * (double)b2 - 0.08131241 * (double)b3 + 128.0);
		}
	}
}
