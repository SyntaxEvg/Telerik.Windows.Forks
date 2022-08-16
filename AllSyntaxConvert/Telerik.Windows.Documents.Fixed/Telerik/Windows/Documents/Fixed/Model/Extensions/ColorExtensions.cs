using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.Extensions
{
	public static class ColorExtensions
	{
		public static ColorBase ToColor(this Color color)
		{
			return new RgbColor(color.A, color.R, color.G, color.B);
		}
	}
}
