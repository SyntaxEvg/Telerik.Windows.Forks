using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	interface ITiling : IPatternColor
	{
		Rect BoundingBox { get; }
	}
}
