using System;

namespace Telerik.Windows.Documents.Core.Imaging
{
	struct Color
	{
		public Color(byte a, byte r, byte g, byte b)
		{
			this.a = a;
			this.r = r;
			this.g = g;
			this.b = b;
		}

		public Color(byte r, byte g, byte b)
		{
			this = new Color(byte.MaxValue, r, g, b);
		}

		public static Color Transparent
		{
			get
			{
				return Color.FromArgb(0, 0, 0, 0);
			}
		}

		public static Color Black
		{
			get
			{
				return Color.FromArgb(byte.MaxValue, 0, 0, 0);
			}
		}

		public static Color White
		{
			get
			{
				return Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		public byte A
		{
			get
			{
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		public byte R
		{
			get
			{
				return this.r;
			}
			set
			{
				this.r = value;
			}
		}

		public byte G
		{
			get
			{
				return this.g;
			}
			set
			{
				this.g = value;
			}
		}

		public byte B
		{
			get
			{
				return this.b;
			}
			set
			{
				this.b = value;
			}
		}

		public static byte ConvertColorComponentToByte(double component)
		{
			if (component > 1.0)
			{
				return byte.MaxValue;
			}
			if (component < 0.0)
			{
				return 0;
			}
			return (byte)(component * 255.0);
		}

		public static Color FromColor(byte alpha, Color c)
		{
			return Color.FromArgb(alpha, c.r, c.g, c.b);
		}

		public static Color FromArgb(int alpha, int red, int green, int blue, int bits)
		{
			byte alpha2 = Color.ConvertColorComponentToByte(alpha, bits);
			byte red2 = Color.ConvertColorComponentToByte(red, bits);
			byte green2 = Color.ConvertColorComponentToByte(green, bits);
			byte blue2 = Color.ConvertColorComponentToByte(blue, bits);
			return Color.FromArgb(alpha2, red2, green2, blue2);
		}

		public static Color FromArgb(double alpha, double red, double green, double blue)
		{
			byte alpha2 = Color.ConvertColorComponentToByte(alpha);
			byte red2 = Color.ConvertColorComponentToByte(red);
			byte green2 = Color.ConvertColorComponentToByte(green);
			byte blue2 = Color.ConvertColorComponentToByte(blue);
			return Color.FromArgb(alpha2, red2, green2, blue2);
		}

		public static Color FromArgb(byte alpha, byte red, byte green, byte blue)
		{
			return new Color(alpha, red, green, blue);
		}

		public static Color FromGray(double gray)
		{
			return Color.FromGray(Color.ConvertColorComponentToByte(gray));
		}

		public static Color FromGray(int gray, int bits)
		{
			return Color.FromGray(Color.ConvertColorComponentToByte(gray, bits));
		}

		public static Color FromGray(byte gray)
		{
			return new Color(gray, gray, gray);
		}

		public static Color FromCmyk(int cyan, int magenta, int yellow, int black, int bits)
		{
			double num = (double)((1 << bits) - 1);
			double cyan2 = (double)cyan / num;
			double magenta2 = (double)magenta / num;
			double yellow2 = (double)yellow / num;
			double black2 = (double)black / num;
			return Color.FromCmyk(cyan2, magenta2, yellow2, black2);
		}

		public static Color FromCmyk(byte cyan, byte magenta, byte yellow, byte black)
		{
			double cyan2 = (double)cyan / 255.0;
			double magenta2 = (double)magenta / 255.0;
			double yellow2 = (double)yellow / 255.0;
			double black2 = (double)black / 255.0;
			return Color.FromCmyk(cyan2, magenta2, yellow2, black2);
		}

		public static Color FromCmyk(double cyan, double magenta, double yellow, double black)
		{
			double num = Color.RestrictComponentToDoubleLimits(cyan);
			double num2 = Color.RestrictComponentToDoubleLimits(magenta);
			double num3 = Color.RestrictComponentToDoubleLimits(yellow);
			double num4 = Color.RestrictComponentToDoubleLimits(black);
			double component = num * (-4.387332384609988 * num + 54.48615194189176 * num2 + 18.82290502165302 * num3 + 212.25662451639585 * num4 + -285.2331026137004) + num2 * (1.7149763477362134 * num2 - 5.6096736904047315 * num3 + -17.873870861415444 * num4 - 5.497006427196366) + num3 * (-2.5217340131683033 * num3 - 21.248923337353073 * num4 + 17.5119270841813) + num4 * (-21.86122147463605 * num4 - 189.48180835922747) + 255.0;
			double component2 = num * (8.841041422036149 * num + 60.118027045597366 * num2 + 6.871425592049007 * num3 + 31.159100130055922 * num4 + -79.2970844816548) + num2 * (-15.310361306967817 * num2 + 17.575251261109482 * num3 + 131.35250912493976 * num4 - 190.9453302588951) + num3 * (4.444339102852739 * num3 + 9.8632861493405 * num4 - 24.86741582555878) + num4 * (-20.737325471181034 * num4 - 187.80453709719578) + 255.0;
			double component3 = num * (0.8842522430003296 * num + 8.078677503112928 * num2 + 30.89978309703729 * num3 - 0.23883238689178934 * num4 + -14.183576799673286) + num2 * (10.49593273432072 * num2 + 63.02378494754052 * num3 + 50.606957656360734 * num4 - 112.23884253719248) + num3 * (0.03296041114873217 * num3 + 115.60384449646641 * num4 + -193.58209356861505) + num4 * (-22.33816807309886 * num4 - 180.12613974708367) + 255.0;
			byte b = Color.RestrictComponentToByteLimits(component);
			byte b2 = Color.RestrictComponentToByteLimits(component2);
			byte b3 = Color.RestrictComponentToByteLimits(component3);
			return new Color(b, b2, b3);
		}

		public static double RestrictComponentToDoubleLimits(double component)
		{
			if (component > 1.0)
			{
				return 1.0;
			}
			if (component < 0.0)
			{
				return 0.0;
			}
			return component;
		}

		public static Color FromPixel(int pixel)
		{
			byte alpha;
			byte red;
			byte green;
			byte blue;
			Color.GetComponentsFromPixel(pixel, out alpha, out red, out green, out blue);
			return Color.FromArgb(alpha, red, green, blue);
		}

		public static void GetComponentsFromPixel(int pixel, out byte a, out byte r, out byte g, out byte b)
		{
			b = (byte)(pixel & 255);
			g = (byte)((pixel >> 8) & 255);
			r = (byte)((pixel >> 16) & 255);
			a = (byte)((pixel >> 24) & 255);
		}

		public static int GetPixelFromComponents(byte r, byte g, byte b)
		{
			byte maxValue = byte.MaxValue;
			int num = (int)b | ((int)g << 8);
			num |= (int)r << 16;
			return num | ((int)maxValue << 24);
		}

		public static bool operator ==(Color a, Color b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Color a, Color b)
		{
			return !(a == b);
		}

		public byte GetGrayComponent()
		{
			return this.r;
		}

		public int ToPixel()
		{
			return ((int)this.a << 24) | ((int)this.r << 16) | ((int)this.g << 8) | (int)this.b;
		}

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.a.GetHashCode();
			num = num * 23 + this.r.GetHashCode();
			num = num * 23 + this.g.GetHashCode();
			return num * 23 + this.b.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is Color && this.Equals((Color)obj);
		}

		public bool Equals(Color value)
		{
			return this.a == value.a && this.r == value.r && this.g == value.g && this.b == value.b;
		}

		static byte ConvertColorComponentToByte(int component, int bits)
		{
			double num = (double)((1 << bits) - 1);
			return Color.ConvertColorComponentToByte((double)component / num);
		}

		static byte RestrictComponentToByteLimits(double component)
		{
			if (component > 255.0)
			{
				return byte.MaxValue;
			}
			if (component < 0.0)
			{
				return 0;
			}
			return (byte)component;
		}

		public override string ToString()
		{
			return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[] { this.A, this.R, this.G, this.B });
		}

		byte a;

		byte r;

		byte g;

		byte b;
	}
}
