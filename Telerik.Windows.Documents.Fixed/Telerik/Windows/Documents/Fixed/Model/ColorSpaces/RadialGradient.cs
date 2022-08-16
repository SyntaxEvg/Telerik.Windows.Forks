using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public class RadialGradient : Gradient, IRadialGradient, IGradient, IPatternColor
	{
		public RadialGradient(Point startPoint, Point endPoint, double startRadius, double endRadius)
			: base(startPoint, endPoint)
		{
			this.StartRadius = startRadius;
			this.EndRadius = endRadius;
		}

		public double StartRadius { get; set; }

		public double EndRadius { get; set; }

		internal override GradientType GradientType
		{
			get
			{
				return GradientType.Radial;
			}
		}

		public override bool Equals(ColorBase other)
		{
			RadialGradient radialGradient = other as RadialGradient;
			return radialGradient != null && (this.StartRadius == radialGradient.StartRadius && this.EndRadius == radialGradient.EndRadius) && base.Equals(radialGradient);
		}
	}
}
