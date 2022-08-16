using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Media
{
	class ColorsHelper
	{
		public static Color HexStringToColor(string hexColor)
		{
			if (hexColor.StartsWith("#"))
			{
				hexColor = hexColor.Substring(1, hexColor.Length - 1);
			}
			byte a = byte.MaxValue;
			int num = 0;
			if (hexColor.Length == 8)
			{
				a = byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
				num = 2;
			}
			if (hexColor.Length == 3)
			{
				hexColor = string.Format("{0}{0}{1}{1}{2}{2}", hexColor[0], hexColor[1], hexColor[2]);
			}
			byte r = byte.Parse(hexColor.Substring(num, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hexColor.Substring(num + 2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hexColor.Substring(num + 4, 2), NumberStyles.HexNumber);
			return Color.FromArgb(a, r, g, b);
		}

		public static ColorShadeType? GetColorShadeTypeForColorAndTint(Color color, double tint)
		{
			HlsaColor color2 = HlsaColor.FromColor(color);
			double[] tintValuesForHlsaColor = ColorsHelper.GetTintValuesForHlsaColor(color2);
			double f = Math.Round(tint, 2);
			for (int i = 0; i < tintValuesForHlsaColor.Length; i++)
			{
				double f2 = Math.Round(tintValuesForHlsaColor[i], 2);
				if (ColorsHelper.AreEqual(f2, f))
				{
					return new ColorShadeType?((ColorShadeType)i);
				}
			}
			return null;
		}

		public static double GetTintAndShadeForColorAndIndex(Color color, ColorShadeType colorShadeType)
		{
			HlsaColor color2 = HlsaColor.FromColor(color);
			double[] tintValuesForHlsaColor = ColorsHelper.GetTintValuesForHlsaColor(color2);
			return tintValuesForHlsaColor[(int)colorShadeType];
		}

		public static Color UpdateTint(Color color, double tint)
		{
			if (tint == 0.0)
			{
				return color;
			}
			HlsaColor hlsaColor = HlsaColor.FromColor(color);
			int num = (int)(hlsaColor.Luminance * 240.0);
			if (tint <= 0.0)
			{
				double num2 = (double)num * -tint;
				return ColorsHelper.ColorFromHlsa(hlsaColor.Hue, ((double)num - num2) / 240.0, hlsaColor.Saturation);
			}
			int num3 = 240 - num;
			return ColorsHelper.ColorFromHlsa(hlsaColor.Hue, ((double)num + (double)num3 * tint) / 240.0, hlsaColor.Saturation);
		}

		public static Color ColorFromHlsa(double hue, double luminosity, double saturation)
		{
			return HlsaColor.ToColor(new HlsaColor(hue, luminosity, saturation, 1.0));
		}

		public static Color FromGray(byte gray)
		{
			return Color.FromArgb(byte.MaxValue, gray, gray, gray);
		}

		public static Color FromCmyk(byte cyan, byte magenta, byte yellow, byte black)
		{
			byte r = (byte)(255 - Math.Min(255, (int)(cyan + black)));
			byte g = (byte)(255 - Math.Min(255, (int)(magenta + black)));
			byte b = (byte)(255 - Math.Min(255, (int)(yellow + black)));
			return Color.FromArgb(byte.MaxValue, r, g, b);
		}

		public static void GenerateThemeGradients(List<Color> themeColors, out List<Color> mainPaletteColors, out List<ThemableColor> mainPaletteThemableColors)
		{
			mainPaletteColors = new List<Color>();
			mainPaletteThemableColors = new List<ThemableColor>();
			for (int i = 0; i < themeColors.Count; i++)
			{
				Color color = themeColors[i];
				HlsaColor color2 = HlsaColor.FromColor(color);
				double[] tintValuesForHlsaColor = ColorsHelper.GetTintValuesForHlsaColor(color2);
				for (int j = 0; j < tintValuesForHlsaColor.Length; j++)
				{
					double num = tintValuesForHlsaColor[j];
					Color item = ColorsHelper.UpdateTint(color, (double)((float)num));
					mainPaletteColors.Add(item);
					mainPaletteThemableColors.Add(new ThemableColor((ThemeColorType)i, new ColorShadeType?((ColorShadeType)j)));
				}
			}
		}

		public static int ToPixel(Color color)
		{
			return ((int)color.A << 24) | ((int)color.R << 16) | ((int)color.G << 8) | (int)color.B;
		}

		public static int GetBrightness(Color color)
		{
			return (int)Math.Sqrt((double)(color.R * color.R) * 0.241 + (double)(color.G * color.G) * 0.691 + (double)(color.B * color.B) * 0.068);
		}

		public static void GetLuminanceModulationAndOffsetFromTint(double tint, out double lumModulation, out double lumOffset)
		{
			if (tint < 0.0)
			{
				lumModulation = 1.0 + tint;
				lumOffset = 0.0;
				return;
			}
			lumModulation = 1.0 - tint;
			lumOffset = tint;
		}

		public static double GetTintFromLuminanceModulationAndOffset(double lumModulation, double lumOffset)
		{
			double result;
			if (lumOffset != 0.0)
			{
				result = lumOffset;
			}
			else
			{
				result = lumModulation - 1.0;
			}
			return result;
		}

		static bool AreEqual(double f1, double f2)
		{
			return Math.Abs(f1 - f2) < 0.001;
		}

		static double[] GetTintValuesForHlsaColor(HlsaColor color)
		{
			int num = (int)(color.Luminance * 100.0);
			foreach (ColorsHelper.TintValues tintValues in ColorsHelper.tintValuesList)
			{
				if (num >= tintValues.Start && num <= tintValues.End)
				{
					return tintValues.TintValuesArray;
				}
			}
			return null;
		}

		static readonly List<ColorsHelper.TintValues> tintValuesList = new List<ColorsHelper.TintValues>
		{
			new ColorsHelper.TintValues(0, 0, new double[] { 0.5, 0.35, 0.25, 0.15, 0.05 }),
			new ColorsHelper.TintValues(1, 19, new double[] { 0.9, 0.75, 0.5, 0.25, 0.1 }),
			new ColorsHelper.TintValues(20, 79, new double[] { 0.8, 0.6, 0.4, -0.25, -0.5 }),
			new ColorsHelper.TintValues(80, 99, new double[] { -0.1, -0.25, -0.5, -0.75, -0.9 }),
			new ColorsHelper.TintValues(100, 100, new double[] { -0.05, -0.15, -0.25, -0.35, -0.5 })
		};

		class TintValues
		{
			public TintValues(int start, int end, double[] tintValues)
			{
				this.start = start;
				this.end = end;
				this.tintValues = tintValues;
			}

			public int Start
			{
				get
				{
					return this.start;
				}
			}

			public int End
			{
				get
				{
					return this.end;
				}
			}

			public double[] TintValuesArray
			{
				get
				{
					return this.tintValues;
				}
			}

			readonly int start;

			readonly int end;

			readonly double[] tintValues;
		}
	}
}
