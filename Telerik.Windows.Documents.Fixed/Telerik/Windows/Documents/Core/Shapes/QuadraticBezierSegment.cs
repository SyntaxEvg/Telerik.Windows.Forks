using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;

namespace Telerik.Windows.Documents.Core.Shapes
{
	class QuadraticBezierSegment : PathSegment
	{
		public Point Point1 { get; set; }

		public Point Point2 { get; set; }

		public override PathSegment Clone()
		{
			return new QuadraticBezierSegment
			{
				Point1 = this.Point1,
				Point2 = this.Point2
			};
		}

		public override void Transform(Matrix transformMatrix)
		{
			this.Point1 = transformMatrix.Transform(this.Point1);
			this.Point2 = transformMatrix.Transform(this.Point2);
		}
	}
}
