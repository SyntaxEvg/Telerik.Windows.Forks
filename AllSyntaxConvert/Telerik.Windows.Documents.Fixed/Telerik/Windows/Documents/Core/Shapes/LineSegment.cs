using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;

namespace Telerik.Windows.Documents.Core.Shapes
{
	class LineSegment : PathSegment
	{
		public Point Point { get; set; }

		public override PathSegment Clone()
		{
			return new LineSegment
			{
				Point = this.Point
			};
		}

		public override void Transform(Matrix transformMatrix)
		{
			this.Point = transformMatrix.Transform(this.Point);
		}
	}
}
