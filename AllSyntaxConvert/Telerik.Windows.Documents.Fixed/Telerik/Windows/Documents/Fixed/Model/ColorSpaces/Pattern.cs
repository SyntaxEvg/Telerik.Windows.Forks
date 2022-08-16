using System;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class Pattern : ColorSpaceBase
	{
		public override int ComponentCount
		{
			get
			{
				if (this.UnderlyingColorSpace != null)
				{
					return this.UnderlyingColorSpace.ComponentCount;
				}
				return 0;
			}
		}

		public override string Name
		{
			get
			{
				return "Pattern";
			}
		}

		public ColorSpaceBase UnderlyingColorSpace { get; set; }

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			throw new NotSupportedException("This method is not supported.");
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			throw new NotSupportedException("This method is not supported.");
		}

		internal override ColorBase GetColor(double[] pars)
		{
			throw new NotSupportedException("This method is not supported.");
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			throw new NotSupportedException("This method is not supported.");
		}
	}
}
