using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public class PathFigure
	{
		public PathFigure()
		{
			this.segments = new PathSegmentCollection();
		}

		public PathSegmentCollection Segments
		{
			get
			{
				return this.segments;
			}
		}

		public Point StartPoint { get; set; }

		public bool IsClosed { get; set; }

		internal Rect GetBounds()
		{
			double x = this.StartPoint.X;
			double x2 = this.StartPoint.X;
			double y = this.StartPoint.Y;
			double y2 = this.StartPoint.Y;
			Point lastPoint = this.StartPoint;
			foreach (PathSegment pathSegment in this.Segments)
			{
				PathGeometry.AppendBounds(pathSegment.GetBounds(lastPoint), ref x, ref x2, ref y, ref y2);
				lastPoint = pathSegment.LastPoint;
			}
			return new Rect(new Point(x, y), new Point(x2, y2));
		}

		readonly PathSegmentCollection segments;
	}
}
