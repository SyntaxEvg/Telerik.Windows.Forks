using System;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public static class RgbColors
	{
		public static global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor Black
		{
			get
			{
				return new global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor();
			}
		}

		public static global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor White
		{
			get
			{
				return new global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor(byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		public static global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor Transparent
		{
			get
			{
				return new global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor(0, 0, 0, 0);
			}
		}
	}
}
