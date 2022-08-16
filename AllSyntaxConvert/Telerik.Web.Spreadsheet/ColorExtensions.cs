using System;
using System.Windows.Media;

namespace Telerik.Web.Spreadsheet
{
	public static class ColorExtensions
	{
		public static string ToHex(this Color color)
		{
			return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}

		public static Color ToColor(this string hex)
		{
			return (Color)ColorConverter.ConvertFromString(hex);
		}
	}
}
