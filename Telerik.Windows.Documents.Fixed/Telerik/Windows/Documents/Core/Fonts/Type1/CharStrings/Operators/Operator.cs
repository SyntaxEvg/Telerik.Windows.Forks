using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Shapes;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	abstract class Operator
	{
		internal static Point CalculatePoint(BuildChar interpreter, int dx, int dy)
		{
			interpreter.CurrentPoint = new Point(interpreter.CurrentPoint.X + (double)dx, interpreter.CurrentPoint.Y + (double)dy);
			return new Point(interpreter.CurrentPoint.X, interpreter.CurrentPoint.Y);
		}

		internal static void HLineTo(BuildChar interpreter, int dx)
		{
			Operator.LineTo(interpreter, dx, 0);
		}

		internal static void VLineTo(BuildChar interpreter, int dy)
		{
			Operator.LineTo(interpreter, 0, dy);
		}

		internal static void LineTo(BuildChar interpreter, int dx, int dy)
		{
			LineSegment lineSegment = new LineSegment();
			lineSegment.Point = Operator.CalculatePoint(interpreter, dx, dy);
			interpreter.CurrentPathFigure.Segments.Add(lineSegment);
		}

		internal static void CurveTo(BuildChar interpreter, int dxa, int dya, int dxb, int dyb, int dxc, int dyc)
		{
			BezierSegment bezierSegment = new BezierSegment();
			bezierSegment.Point1 = Operator.CalculatePoint(interpreter, dxa, dya);
			bezierSegment.Point2 = Operator.CalculatePoint(interpreter, dxb, dyb);
			bezierSegment.Point3 = Operator.CalculatePoint(interpreter, dxc, dyc);
			interpreter.CurrentPathFigure.Segments.Add(bezierSegment);
		}

		internal static void MoveTo(BuildChar interpreter, int dx, int dy)
		{
			interpreter.CurrentPathFigure = new PathFigure();
			interpreter.CurrentPathFigure.IsClosed = true;
			interpreter.CurrentPathFigure.IsFilled = true;
			interpreter.CurrentPathFigure.StartPoint = Operator.CalculatePoint(interpreter, dx, dy);
			interpreter.GlyphOutlines.Add(interpreter.CurrentPathFigure);
		}

		internal static void ReadWidth(BuildChar interpreter, int operands)
		{
			if (interpreter.Width != null)
			{
				return;
			}
			if (interpreter.Operands.Count == operands + 1)
			{
				interpreter.Width = new int?(interpreter.Operands.GetFirstAsInt());
				return;
			}
			interpreter.Width = new int?(0);
		}

		public abstract void Execute(BuildChar buildChar);
	}
}
