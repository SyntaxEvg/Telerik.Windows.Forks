using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	interface IGradientStop
	{
		Color Color { get; }

		double Offset { get; }
	}
}
