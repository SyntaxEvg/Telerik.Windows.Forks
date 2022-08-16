using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Data;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types
{
	static class HighlightColors
	{
		static HighlightColors()
		{
			HighlightColors.AvailableColors = new List<Color>();
			HighlightColors.AddColor(Colors.Transparent, "none");
			HighlightColors.AddColor(Colors.Black, "black");
			HighlightColors.AddColor(Colors.Blue, "blue");
			HighlightColors.AddColor(Colors.Cyan, "cyan");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, 0, 0, 128), "darkBlue");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, 0, 128, 128), "darkCyan");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, 128, 128, 128), "darkGray");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, 0, 128, 0), "darkGreen");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, 128, 0, 128), "darkMagenta");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, 128, 0, 0), "darkRed");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, 128, 128, 0), "darkYellow");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, 0, byte.MaxValue, 0), "green");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, 192, 192, 192), "lightGray");
			HighlightColors.AddColor(Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue), "magenta");
			HighlightColors.AddColor(Colors.Red, "red");
			HighlightColors.AddColor(Colors.White, "white");
			HighlightColors.AddColor(Colors.Yellow, "yellow");
		}

		public static List<Color> AvailableColors { get; set; }

		public static ValueMapper<string, Color> Mapper { get; set; } = new ValueMapper<string, Color>("none", Colors.Transparent);

		public static Color GetNearestHighlightingColor(Color color)
		{
			if (color == Colors.Transparent)
			{
				return color;
			}
			double num = double.MaxValue;
			Color result = Colors.Transparent;
			foreach (Color color2 in HighlightColors.AvailableColors)
			{
				if (color2 != Colors.Transparent)
				{
					double num2 = HighlightColors.ColorDistance(color, color2);
					if (num > num2)
					{
						num = num2;
						result = color2;
					}
				}
			}
			return result;
		}

		static void AddColor(Color color, string str)
		{
			HighlightColors.Mapper.AddPair(str, color);
			HighlightColors.AvailableColors.Add(color);
		}

		static double ColorDistance(Color color1, Color color2)
		{
			return Math.Sqrt(Math.Pow((double)(color1.R - color2.R), 2.0) + Math.Pow((double)(color1.G - color2.G), 2.0) + Math.Pow((double)(color1.B - color2.B), 2.0));
		}
	}
}
