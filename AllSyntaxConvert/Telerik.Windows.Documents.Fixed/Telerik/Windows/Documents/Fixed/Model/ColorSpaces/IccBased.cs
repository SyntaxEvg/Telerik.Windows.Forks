using System;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class IccBased : ColorSpaceBase
	{
		public override int ComponentCount
		{
			get
			{
				return this.N;
			}
		}

		public string[] Filters { get; set; }

		public override string Name
		{
			get
			{
				return "ICCBased";
			}
		}

		public byte[] Data { get; set; }

		public ColorSpaceBase Alternate { get; set; }

		public int N { get; set; }

		public double[] Range { get; set; }

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			ColorSpaceBase colorSpaceBase = this.GetCachedAlternateColorSpace();
			return colorSpaceBase.GetDefaultDecodeArray(bitsPerComponent);
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			ColorSpaceBase colorSpaceBase = this.GetCachedAlternateColorSpace();
			return colorSpaceBase.GetPixels(image, applyMask);
		}

		internal override ColorBase GetColor(double[] pars)
		{
			Guard.ThrowExceptionIfNull<double[]>(pars, "pars");
			ColorSpaceBase colorSpaceBase = this.GetCachedAlternateColorSpace();
			return colorSpaceBase.GetColor(pars);
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(lookupData, "lookupData");
			ColorSpaceBase colorSpaceBase = this.GetCachedAlternateColorSpace();
			return colorSpaceBase.GetColorFromLookup(lookupData, offset);
		}

		internal ColorSpaceBase GetCachedAlternateColorSpace()
		{
			if (this.cachedAlternateColorSpace == null)
			{
				if (this.Alternate != null)
				{
					this.cachedAlternateColorSpace = this.Alternate;
				}
				else
				{
					this.cachedAlternateColorSpace = this.CalculateAlternateColorSpaceFromN();
				}
			}
			return this.cachedAlternateColorSpace;
		}

		ColorSpaceBase CalculateAlternateColorSpaceFromN()
		{
			switch (this.N)
			{
			case 1:
				return new DeviceGray();
			case 3:
				return new DeviceRgb();
			case 4:
				return new DeviceCmyk();
			}
			throw new NotSupportedException();
		}

		ColorSpaceBase cachedAlternateColorSpace;
	}
}
