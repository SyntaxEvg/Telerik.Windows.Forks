using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	class HlsaColor
	{
		public HlsaColor(double hue, double luminance, double saturation, double alpha)
		{
			this.hue = MathUtilities.Clamp(hue, 0.0, 360.0, 5);
			this.luminance = MathUtilities.Clamp(luminance, 0.0, 1.0, 5);
			this.saturation = MathUtilities.Clamp(saturation, 0.0, 1.0, 5);
			this.alpha = MathUtilities.Clamp(alpha, 0.0, 1.0, 5);
		}

		public double Alpha
		{
			get
			{
				return this.alpha;
			}
		}

		public double Hue
		{
			get
			{
				return this.hue;
			}
		}

		public double Luminance
		{
			get
			{
				return this.luminance;
			}
		}

		public double Saturation
		{
			get
			{
				return this.saturation;
			}
		}

		public static HlsaColor FromColor(SpreadColor color)
		{
			double num = (double)color.R / 255.0;
			double num2 = (double)color.G / 255.0;
			double num3 = (double)color.B / 255.0;
			double num4 = (double)color.A;
			double num5 = Math.Max(num, Math.Max(num2, num3));
			double num6 = System.Math.Min(num, Math.Min(num2, num3));
			double num7 = (num6 + num5) / 2.0;
			if (num6 == num5)
			{
				return new HlsaColor(0.0, num7, 0.0, num4);
			}
			double num8;
			if (num7 <= 0.5)
			{
				num8 = (num5 - num6) / (num5 + num6);
			}
			else
			{
				num8 = (num5 - num6) / (2.0 - num5 - num6);
			}
			double num9 = (num5 - num) / (num5 - num6);
			double num10 = (num5 - num2) / (num5 - num6);
			double num11 = (num5 - num3) / (num5 - num6);
			double num12;
			if (num == num5)
			{
				num12 = num11 - num10;
			}
			else if (num2 == num5)
			{
				num12 = 2.0 + num9 - num11;
			}
			else
			{
				num12 = 4.0 + num10 - num9;
			}
			num12 /= 6.0;
			num12 = MathUtilities.CustomMod(num12);
			return new HlsaColor(num12 * 360.0, num7, num8, num4);
		}

		public static SpreadColor ToColor(HlsaColor color)
		{
			double num = color.Hue / 360.0;
			double num2 = color.Luminance;
			double num3 = color.Saturation;
			double num4 = color.Alpha;
			if (num3 == 0.0)
			{
				return new SpreadColor((byte)MathUtilities.Clamp(num4 * 255.0, 0.0, 255.0, 5), (byte)MathUtilities.Clamp(num2 * 255.0, 0.0, 255.0, 5), (byte)MathUtilities.Clamp(num2 * 255.0, 0.0, 255.0, 5), (byte)MathUtilities.Clamp(num2 * 255.0, 0.0, 255.0, 5));
			}
			double num5;
			if (num2 <= 0.5)
			{
				num5 = num2 * (1.0 + num3);
			}
			else
			{
				num5 = num2 + num3 - num2 * num3;
			}
			double m = 2.0 * num2 - num5;
			double num6 = HlsaColor.ConvertValue(m, num5, num + 0.3333333333333333);
			double num7 = HlsaColor.ConvertValue(m, num5, num);
			double num8 = HlsaColor.ConvertValue(m, num5, num - 0.3333333333333333);
			return new SpreadColor((byte)MathUtilities.Clamp(num4 * 255.0, 0.0, 255.0, 5), (byte)MathUtilities.Clamp(num6 * 255.0, 0.0, 255.0, 5), (byte)MathUtilities.Clamp(num7 * 255.0, 0.0, 255.0, 5), (byte)MathUtilities.Clamp(num8 * 255.0, 0.0, 255.0, 5));
		}

		public static bool operator ==(HlsaColor instance1, HlsaColor instance2)
		{
			if (object.ReferenceEquals((object)instance1, (object)instance2))
				return true;
			return (object)instance1 != null && (object)instance2 != null && HlsaColor.AreEqual(instance1, instance2);
		}

		public static bool operator !=(HlsaColor instance1, HlsaColor instance2)
		{
			return !(instance1 == instance2);
		}

		public override bool Equals(object obj)
		{
			HlsaColor hlsaColor = obj as HlsaColor;
			return obj != null && !(hlsaColor == null) && HlsaColor.AreEqual(this, hlsaColor);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes((int)this.Hue, (int)this.Luminance, (int)this.Saturation, (int)this.Alpha);
		}

		static bool AreEqual(HlsaColor instance1, HlsaColor instance2)
		{
			return !(instance1 == null) && !(instance2 == null) && (instance1.Hue == instance2.Hue && instance1.Luminance == instance2.Luminance && instance1.Saturation == instance2.Saturation) && instance1.Alpha == instance2.Alpha;
		}

		static double ConvertValue(double m1, double m2, double hue)
		{
			hue = MathUtilities.CustomMod(hue);
			if (hue < 0.16666666666666666)
			{
				return m1 + (m2 - m1) * hue * 6.0;
			}
			if (hue < 0.5)
			{
				return m2;
			}
			if (hue < 0.6666666666666666)
			{
				return m1 + (m2 - m1) * (0.6666666666666666 - hue) * 6.0;
			}
			return m1;
		}

		const double OneSixth = 0.16666666666666666;

		const double OneThird = 0.3333333333333333;

		const double TwoThird = 0.6666666666666666;

		readonly double alpha;

		readonly double hue;

		readonly double luminance;

		readonly double saturation;
	}
}
