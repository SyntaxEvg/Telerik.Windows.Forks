using System;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	interface IColorSpace
	{
		int ComponentCount { get; }

		PixelContainer GetPixels(IImageDescriptor image, bool applyMask);

		double[] GetDefaultDecodeArray(int bitsPerComponent);
	}
}
