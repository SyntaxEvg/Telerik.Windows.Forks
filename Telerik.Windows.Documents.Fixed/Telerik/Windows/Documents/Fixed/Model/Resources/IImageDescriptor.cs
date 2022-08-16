using System;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	interface IImageDescriptor
	{
		int Width { get; }

		int Height { get; }

		int BitsPerComponent { get; }

		double[] Decode { get; }

		string[] Filters { get; }

		IColorSpace ColorSpace { get; }

		IMask Mask { get; }

		IImageDescriptor SMask { get; }

		byte[] GetEncodedData();

		byte[] GetDecodedData();
	}
}
