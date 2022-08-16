using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public class LineSegment : PathSegment
	{
		public Point Point { get; set; }

		internal override Point LastPoint
		{
			get
			{
				return this.Point;
			}
		}

		internal override Rect GetBounds(Point previousPoint)
		{
			return new Rect(previousPoint, this.Point);
		}
	}
}
