using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public class PathGeometry : GeometryBase
	{
		public PathGeometry()
		{
			this.figures = new PathFigureCollection();
			this.FillRule = FixedDocumentDefaults.FillRule;
		}

		public PathFigureCollection Figures
		{
			get
			{
				return this.figures;
			}
		}

		public FillRule FillRule { get; set; }

		internal static void AppendBounds(Rect rect, ref double minX, ref double maxX, ref double minY, ref double maxY)
		{
			minX = System.Math.Min(minX, rect.X);
			maxX = Math.Max(maxX, rect.X + rect.Width);
			minY = System.Math.Min(minY, rect.Y);
			maxY = Math.Max(maxY, rect.Y + rect.Height);
		}

		internal static void AppendBounds(Point point, ref double minX, ref double maxX, ref double minY, ref double maxY)
		{
			minX = System.Math.Min(minX, point.X);
			maxX = Math.Max(maxX, point.X);
			minY = System.Math.Min(minY, point.Y);
			maxY = Math.Max(maxY, point.Y);
		}

		protected override Rect GetBounds()
		{
			double maxValue = double.MaxValue;
			double minValue = double.MinValue;
			double maxValue2 = double.MaxValue;
			double minValue2 = double.MinValue;
			foreach (PathFigure pathFigure in this.Figures)
			{
				PathGeometry.AppendBounds(pathFigure.GetBounds(), ref maxValue, ref minValue, ref maxValue2, ref minValue2);
			}
			return new Rect(new Point(maxValue, maxValue2), new Point(minValue, minValue2));
		}

		readonly PathFigureCollection figures;
	}
}
