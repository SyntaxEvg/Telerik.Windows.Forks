using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	interface IMulticomponentColorSpace : IColorSpace
	{
		Color GetColor(double[] components);
	}
}
