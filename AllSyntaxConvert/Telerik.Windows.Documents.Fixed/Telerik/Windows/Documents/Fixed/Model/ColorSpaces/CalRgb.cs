using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class CalRgb : ColorSpaceBase
	{
		public override int ComponentCount
		{
			get
			{
				return 3;
			}
		}

		public CalRgb()
		{
			this.WhitePoint = new double[3];
			this.BlackPoint = new double[3];
			this.Gamma = new double[] { 1.0, 1.0, 1.0 };
			this.Matrix = new double[] { 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 };
		}

		public override string Name
		{
			get
			{
				return "CalRGB";
			}
		}

		public double[] WhitePoint { get; set; }

		public double[] BlackPoint { get; set; }

		public double[] Gamma { get; set; }

		public double[] Matrix { get; set; }

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceRgb.GetDefaultDecodeArray();
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			return DeviceRgb.GetRgbPixels(image, applyMask);
		}

		internal override ColorBase GetColor(double[] pars)
		{
			Guard.ThrowExceptionIfNull<double[]>(pars, "pars");
			int num = pars.Length;
			byte r = Color.ConvertColorComponentToByte(pars[num - 3]);
			byte g = Color.ConvertColorComponentToByte(pars[num - 2]);
			byte b = Color.ConvertColorComponentToByte(pars[num - 1]);
			return new RgbColor(r, g, b);
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(lookupData, "lookupData");
			Color color = Color.FromArgb(byte.MaxValue, lookupData[offset], lookupData[offset + 1], lookupData[offset + 2]);
			return new RgbColor(color);
		}
	}
}
