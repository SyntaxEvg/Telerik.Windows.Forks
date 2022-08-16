using System;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class Lab : ColorSpaceBase
	{
		public override int ComponentCount
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override string Name
		{
			get
			{
				return "Lab";
			}
		}

		public double[] WhitePoint { get; set; }

		public double[] BlackPoint { get; set; }

		public double[] Range { get; set; }

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			throw new NotImplementedException();
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			throw new NotImplementedException();
		}

		internal override ColorBase GetColor(double[] pars)
		{
			throw new NotImplementedException();
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			throw new NotImplementedException();
		}
	}
}
