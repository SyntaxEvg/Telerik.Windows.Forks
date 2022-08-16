using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public class RgbColor : global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.SimpleColor
	{
		public RgbColor()
		{
			this.A = byte.MaxValue;
		}

		public RgbColor(byte r, byte g, byte b)
			: this(byte.MaxValue, r, g, b)
		{
		}

		public RgbColor(byte a, byte r, byte g, byte b)
		{
			this.A = a;
			this.R = r;
			this.G = g;
			this.B = b;
		}

		internal RgbColor(global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor other)
			: this(other.A, other.R, other.G, other.B)
		{
		}

		internal RgbColor(global::Telerik.Windows.Documents.Core.Imaging.Color color)
			: this(color.A, color.R, color.G, color.B)
		{
		}

		public byte A { get; set; }

		public byte R { get; set; }

		public byte G { get; set; }

		public byte B { get; set; }

		internal override global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.ColorSpaceBase ColorSpace
		{
			get
			{
				return global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor.DeviceRgbColorSpace;
			}
		}

		internal override bool IsTransparent
		{
			get
			{
				return this.A == 0;
			}
		}

		public override int GetHashCode()
		{
			return ((int)this.A << 24) | ((int)this.R << 16) | ((int)this.G << 8) | (int)this.B;
		}

		public override bool Equals(object obj)
		{
			return obj != null && this.Equals(obj as global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.ColorBase);
		}

		public override bool Equals(global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.ColorBase other)
		{
			if (other == null)
			{
				return false;
			}
			global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor rgbColor = other as global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor;
			return this.R == rgbColor.R && this.G == rgbColor.G && this.B == rgbColor.B && this.A == rgbColor.A;
		}

		public override string ToString()
		{
			return string.Format("<A={0} R={1} G={2} B={3}>", new object[] { this.A, this.R, this.G, this.B });
		}

		internal override int[] GetColorComponents()
		{
			return new int[]
			{
				(int)this.R,
				(int)this.G,
				(int)this.B
			};
		}

		internal override global::Telerik.Windows.Documents.Core.Imaging.Color ToColor()
		{
			return new global::Telerik.Windows.Documents.Core.Imaging.Color(this.A, this.R, this.G, this.B);
		}

		internal const byte DefaultAlphaValue = 255;

		private static readonly global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.DeviceRgb DeviceRgbColorSpace = new global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.DeviceRgb();
	}
}
