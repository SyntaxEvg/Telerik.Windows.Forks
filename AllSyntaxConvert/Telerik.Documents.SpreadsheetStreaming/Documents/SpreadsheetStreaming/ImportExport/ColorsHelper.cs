using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport
{
	class ColorsHelper
	{
		public static SpreadColor HexStringToColor(string hexColor)
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
			return new SpreadColor(a, r, g, b);
		}

		public static SpreadColorShadeType? GetColorShadeTypeForColorAndTint(SpreadColor color, double tint)
		{
			HlsaColor color2 = HlsaColor.FromColor(color);
			double[] tintValuesForHlsaColor = ColorsHelper.GetTintValuesForHlsaColor(color2);
			double d = Math.Round(tint, 2);
			for (int i = 0; i < tintValuesForHlsaColor.Length; i++)
			{
				double d2 = Math.Round(tintValuesForHlsaColor[i], 2);
				if (d2.EqualsDouble(d))
				{
					return new SpreadColorShadeType?((SpreadColorShadeType)i);
				}
			}
			return null;
		}

		public static double GetTintAndShadeForColorAndIndex(SpreadColor color, SpreadColorShadeType colorShadeType)
		{
			HlsaColor color2 = HlsaColor.FromColor(color);
			double[] tintValuesForHlsaColor = ColorsHelper.GetTintValuesForHlsaColor(color2);
			return tintValuesForHlsaColor[(int)colorShadeType];
		}

		public static SpreadColor UpdateTint(SpreadColor color, double tint)
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

		public static SpreadColor ColorFromHlsa(double hue, double luminosity, double saturation)
		{
			return HlsaColor.ToColor(new HlsaColor(hue, luminosity, saturation, 1.0));
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
