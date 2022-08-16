using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;

namespace Telerik.Windows.Documents.Core.Shapes
{
	class PathFigure
	{
		public PathFigure()
		{
			this.Segments = new List<PathSegment>();
		}

		public List<PathSegment> Segments { get; set; }

		public bool IsClosed { get; set; }

		public bool IsFilled { get; set; }

		public Point StartPoint { get; set; }

		public PathFigure Clone()
		{
			PathFigure pathFigure = new PathFigure();
			pathFigure.IsClosed = this.IsClosed;
			pathFigure.IsFilled = this.IsFilled;
			pathFigure.StartPoint = this.StartPoint;
			foreach (PathSegment pathSegment in this.Segments)
			{
				pathFigure.Segments.Add(pathSegment.Clone());
			}
			return pathFigure;
		}

		internal void Transform(Matrix transformMatrix)
		{
			this.StartPoint = transformMatrix.Transform(this.StartPoint);
			foreach (PathSegment pathSegment in this.Segments)
			{
				pathSegment.Transform(transformMatrix);
			}
		}
	}
}
