using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class LinearGradientBrush : GradientBrush
	{
		public LinearGradientBrush(Color startColor, Color endColor, Point startPoint, Point endPoint, bool extendBefore, bool extendAfter, Color? background, Matrix transform, Rect? boundingBox)
			: base(startColor, endColor, startPoint, endPoint, extendBefore, extendAfter, background, transform, boundingBox)
		{
		}

		protected override GradientBrush CloneOverride()
		{
			return new LinearGradientBrush(base.StartColor, base.EndColor, base.StartPoint, base.EndPoint, base.ExtendBefore, base.ExtendAfter, base.Background, base.Transform, base.BoundingBox);
		}
	}
}
