using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public class LinearGradient : Gradient
	{
		public LinearGradient(Point startPoint, Point endPoint)
			: base(startPoint, endPoint)
		{
		}

		internal override GradientType GradientType
		{
			get
			{
				return GradientType.Linear;
			}
		}

		public override bool Equals(ColorBase other)
		{
			return other is LinearGradient && base.Equals(other);
		}
	}
}
