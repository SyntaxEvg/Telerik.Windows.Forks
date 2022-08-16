using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	interface IIndexedColorSpace : IColorSpace
	{
		int HiVal { get; }

		Color GetColor(int index);

		Color[] GetAllColors();
	}
}
