using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class RadialGradientBrush : GradientBrush, IRadialGradient, IGradient, IPatternColor
	{
		public RadialGradientBrush(Color startColor, Color endColor, Point startPoint, Point endPoint, double r1, double r2, bool extendBefore, bool extendAfter, Color? background, Matrix transform, Rect? boundingBox)
			: base(startColor, endColor, startPoint, endPoint, extendBefore, extendAfter, background, transform, boundingBox)
		{
			this.r1 = r1;
			this.r2 = r2;
		}

		public double Radius1
		{
			get
			{
				return this.r1;
			}
		}

		public double Radius2
		{
			get
			{
				return this.r2;
			}
		}

		double IRadialGradient.StartRadius
		{
			get
			{
				return this.Radius1;
			}
		}

		double IRadialGradient.EndRadius
		{
			get
			{
				return this.Radius2;
			}
		}

		protected override GradientBrush CloneOverride()
		{
			return new RadialGradientBrush(base.StartColor, base.EndColor, base.StartPoint, base.EndPoint, this.Radius1, this.Radius2, base.ExtendBefore, base.ExtendAfter, base.Background, base.Transform, base.BoundingBox);
		}

		readonly double r1;

		readonly double r2;
	}
}
