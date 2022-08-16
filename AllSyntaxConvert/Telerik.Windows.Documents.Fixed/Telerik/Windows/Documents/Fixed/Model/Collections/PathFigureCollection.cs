using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.Model.Collections
{
	public sealed class PathFigureCollection : List<PathFigure>
	{
		public PathFigure AddPathFigure()
		{
			PathFigure pathFigure = new PathFigure();
			base.Add(pathFigure);
			return pathFigure;
		}
	}
}
