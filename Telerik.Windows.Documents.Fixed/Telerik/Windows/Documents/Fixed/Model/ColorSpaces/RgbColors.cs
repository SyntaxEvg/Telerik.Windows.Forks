using System;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public static class RgbColors
	{
		public static RgbColor Black
		{
			get
			{
				return new RgbColor();
			}
		}

		public static RgbColor White
		{
			get
			{
				return new RgbColor(byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		public static RgbColor Transparent
		{
			get
			{
				return new RgbColor(0, 0, 0, 0);
			}
		}
	}
}
