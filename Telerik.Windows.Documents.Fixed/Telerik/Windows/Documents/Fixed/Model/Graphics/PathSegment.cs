using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public abstract class PathSegment
	{
		internal abstract Point LastPoint { get; }

		internal abstract Rect GetBounds(Point lastPoint);
	}
}
