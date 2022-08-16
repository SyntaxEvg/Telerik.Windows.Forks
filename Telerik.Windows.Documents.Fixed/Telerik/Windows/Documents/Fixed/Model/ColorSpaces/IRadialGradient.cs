using System;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	interface IRadialGradient : IGradient, IPatternColor
	{
		double StartRadius { get; }

		double EndRadius { get; }
	}
}
