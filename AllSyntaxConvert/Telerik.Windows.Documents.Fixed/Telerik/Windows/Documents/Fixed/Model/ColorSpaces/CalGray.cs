using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class CalGray : ColorSpaceBase
	{
		public override int ComponentCount
		{
			get
			{
				return 1;
			}
		}

		public CalGray()
		{
			this.WhitePoint = new double[3];
			this.BlackPoint = new double[3];
			this.Gamma = 1.0;
		}

		public override string Name
		{
			get
			{
				return "CalGray";
			}
		}

		public double[] WhitePoint { get; set; }

		public double[] BlackPoint { get; set; }

		public double Gamma { get; set; }

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceGray.GetDefaultDecodeArray();
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			return DeviceGray.GetGrayPixels(image, applyMask);
		}

		internal override ColorBase GetColor(double[] pars)
		{
			Guard.ThrowExceptionIfNull<double[]>(pars, "pars");
			double gray = pars[0];
			Color color = Color.FromGray(gray);
			return new RgbColor(color);
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(lookupData, "lookupData");
			Color color = Color.FromGray(lookupData[offset]);
			return new RgbColor(color);
		}
	}
}
