using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	interface IGradient : IPatternColor
	{
		bool ExtendBefore { get; }

		bool ExtendAfter { get; }

		Point StartPoint { get; }

		Point EndPoint { get; }

		Color StartColor { get; }

		Color EndColor { get; }

		Color? Background { get; }

		Rect? BoundingBox { get; }

		IEnumerable<IGradientStop> GradientStops { get; }
	}
}
