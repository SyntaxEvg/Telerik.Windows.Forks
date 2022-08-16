using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Utilities
{
	static class ArcHelper
	{
		public static IEnumerable<Tuple<Point, Point, Point, Point>> GetEllipticArcCubicBezierApproximation(Point start, Point end, Size ellipseRadiuses, bool isLargeArc, SweepDirection sweepDirection, double xAxisRotationInDegrees)
		{
			Point segmentRadiusVector = end.Minus(start);
			xAxisRotationInDegrees = ArcHelper.NormalizeAngle(xAxisRotationInDegrees);
			RotateTransform vectorRotation = new RotateTransform
			{
				Angle = ArcHelper.NormalizeAngle(-xAxisRotationInDegrees)
			};
			segmentRadiusVector = vectorRotation.Transform(segmentRadiusVector);
			Point segmentVector = ArcHelper.HandleSmallCoordinates(segmentRadiusVector);
			Tuple<Point, Point> ellipseIntersections = ArcHelper.GetEllipseArcEndPoints(ellipseRadiuses, segmentVector, isLargeArc, sweepDirection);
			if (ellipseIntersections != null)
			{
				ellipseIntersections = new Tuple<Point, Point>(ArcHelper.HandleSmallCoordinates(ellipseIntersections.Item1), ArcHelper.HandleSmallCoordinates(ellipseIntersections.Item2));
				Point actualEllipseVector = ellipseIntersections.Item2.Minus(ellipseIntersections.Item1);
				double segmentLengthScaleFactor = segmentVector.Length() / actualEllipseVector.Length();
				Size ellipseToCircleScale = ArcHelper.GetScaleFactorFromEllipseToBoundingCircle(ellipseRadiuses);
				Matrix circleToTranslatedEllipseTransform = default(Matrix).ScaleMatrix(1.0 / ellipseToCircleScale.Width, 1.0 / ellipseToCircleScale.Height).TranslateMatrix(-ellipseIntersections.Item1.X, -ellipseIntersections.Item1.Y).ScaleMatrix(segmentLengthScaleFactor, segmentLengthScaleFactor)
					.RotateMatrix(xAxisRotationInDegrees)
					.TranslateMatrix(start.X, start.Y);
				Tuple<Point, Point> pointsOnCircle = ArcHelper.TranslatePointsToBoundingCircle(ellipseRadiuses, ellipseIntersections);
				foreach (Tuple<Point, Point> smallArc in ArcHelper.DivideCircleArcToSmallerArcs(pointsOnCircle, sweepDirection, isLargeArc))
				{
					Tuple<Point, Point, Point, Point> circleBezierPoints = ArcHelper.GetArcCubicBezierApproximation(smallArc, sweepDirection);
					Tuple<Point, Point, Point, Point> ellipseArcBezierPoints = new Tuple<Point, Point, Point, Point>(circleToTranslatedEllipseTransform.Transform(circleBezierPoints.Item1), circleToTranslatedEllipseTransform.Transform(circleBezierPoints.Item2), circleToTranslatedEllipseTransform.Transform(circleBezierPoints.Item3), circleToTranslatedEllipseTransform.Transform(circleBezierPoints.Item4));
					yield return ellipseArcBezierPoints;
				}
			}
			yield break;
		}

		static Tuple<Point, Point, Point, Point> GetArcCubicBezierApproximation(Tuple<Point, Point> arcPoints, SweepDirection sweepDirection)
		{
			double arcAngle = ArcHelper.GetArcAngle(arcPoints);
			double cubicBezierArcKappa = ArcHelper.GetCubicBezierArcKappa(arcAngle);
			ScaleTransform scaleTransform = new ScaleTransform
			{
				ScaleX = cubicBezierArcKappa,
				ScaleY = cubicBezierArcKappa
			};
			RotateTransform rotateTransform = new RotateTransform
			{
				Angle = (double)((sweepDirection == SweepDirection.Clockwise) ? 90 : (-90))
			};
			RotateTransform rotateTransform2 = new RotateTransform
			{
				Angle = (double)((sweepDirection == SweepDirection.Clockwise) ? (-90) : 90)
			};
			Point point = rotateTransform.Transform(arcPoints.Item1);
			point = scaleTransform.Transform(point);
			Point point2 = rotateTransform2.Transform(arcPoints.Item2);
			point2 = scaleTransform.Transform(point2);
			return new Tuple<Point, Point, Point, Point>(arcPoints.Item1, arcPoints.Item1.Plus(point), arcPoints.Item2.Plus(point2), arcPoints.Item2);
		}

		static double GetArcAngle(Tuple<Point, Point> arcPoints)
		{
			Point item = arcPoints.Item1;
			Point item2 = arcPoints.Item2;
			double num = item.MultiplyBy(item2);
			double num2 = Math.Sqrt(item.MultiplyBy(item));
			double num3 = Math.Sqrt(item2.MultiplyBy(item2));
			return Math.Acos(num / (num2 * num3));
		}

		static IEnumerable<Tuple<Point, Point>> DivideCircleArcToSmallerArcs(Tuple<Point, Point> arcPoints, SweepDirection sweepDirection, bool isLargeArc)
		{
			Point current = arcPoints.Item1;
			double radius = Math.Sqrt(current.X * current.X + current.Y * current.Y);
			ScaleTransform radiusTransform = new ScaleTransform
			{
				ScaleX = radius,
				ScaleY = radius
			};
			Point secondAxisPoint = ArcHelper.GetClosestAxisPoint(arcPoints.Item2, sweepDirection);
			bool isFirstSegmentOnLargeArc = isLargeArc;
			Point nextCurrentAxisPoint = ArcHelper.GetNextAxisPoint(current, sweepDirection);
			while (!nextCurrentAxisPoint.Equals(secondAxisPoint) || isFirstSegmentOnLargeArc)
			{
				Point scaledAxisPoint = radiusTransform.Transform(nextCurrentAxisPoint);
				yield return new Tuple<Point, Point>(current, scaledAxisPoint);
				current = scaledAxisPoint;
				nextCurrentAxisPoint = ArcHelper.GetNextAxisPoint(current, sweepDirection);
				isFirstSegmentOnLargeArc = false;
			}
			yield return new Tuple<Point, Point>(current, arcPoints.Item2);
			yield break;
		}

		static Point GetNextAxisPoint(Point point, SweepDirection sweepDirection)
		{
			Point closestAxisPoint = ArcHelper.GetClosestAxisPoint(point, sweepDirection);
			if (point.X != 0.0 && point.Y != 0.0)
			{
				return closestAxisPoint;
			}
			int num = Array.IndexOf<Point>(ArcHelper.clockwiseAxisPoints, closestAxisPoint);
			int num2 = ((sweepDirection == SweepDirection.Clockwise) ? 1 : (-1));
			num += num2;
			if (num == -1)
			{
				num = ArcHelper.clockwiseAxisPoints.Length - 1;
			}
			else if (num == ArcHelper.clockwiseAxisPoints.Length)
			{
				num = 0;
			}
			return ArcHelper.clockwiseAxisPoints[num];
		}

		static Point GetClosestAxisPoint(Point point, SweepDirection sweepDirection)
		{
			if (point == default(Point))
			{
				throw new ArgumentException("point cannot be zero!");
			}
			if (point.X > 0.0 && point.Y > 0.0)
			{
				if (sweepDirection != SweepDirection.Clockwise)
				{
					return new Point(1.0, 0.0);
				}
				return new Point(0.0, 1.0);
			}
			else if (point.X > 0.0 && point.Y < 0.0)
			{
				if (sweepDirection != SweepDirection.Clockwise)
				{
					return new Point(0.0, -1.0);
				}
				return new Point(1.0, 0.0);
			}
			else if (point.X < 0.0 && point.Y < 0.0)
			{
				if (sweepDirection != SweepDirection.Clockwise)
				{
					return new Point(-1.0, 0.0);
				}
				return new Point(0.0, -1.0);
			}
			else
			{
				if (point.X >= 0.0 || point.Y <= 0.0)
				{
					return new Point((double)Math.Sign(point.X), (double)Math.Sign(point.Y));
				}
				if (sweepDirection != SweepDirection.Clockwise)
				{
					return new Point(0.0, 1.0);
				}
				return new Point(-1.0, 0.0);
			}
		}

		static Tuple<Point, Point> TranslatePointsToBoundingCircle(Size ellipse, Tuple<Point, Point> ellipsePoints)
		{
			Size scaleFactorFromEllipseToBoundingCircle = ArcHelper.GetScaleFactorFromEllipseToBoundingCircle(ellipse);
			ScaleTransform scaleTransform = new ScaleTransform
			{
				ScaleX = scaleFactorFromEllipseToBoundingCircle.Width,
				ScaleY = scaleFactorFromEllipseToBoundingCircle.Height
			};
			return new Tuple<Point, Point>(scaleTransform.Transform(ellipsePoints.Item1), scaleTransform.Transform(ellipsePoints.Item2));
		}

		static Size GetScaleFactorFromEllipseToBoundingCircle(Size ellipse)
		{
			double width = ((ellipse.Width > ellipse.Height) ? 1.0 : (ellipse.Height / ellipse.Width));
			double height = ((ellipse.Width > ellipse.Height) ? (ellipse.Width / ellipse.Height) : 1.0);
			return new Size(width, height);
		}

		static Tuple<Point, Point> GetEllipseArcEndPoints(Size ellipse, Point segmentVector, bool isLargeArc, SweepDirection sweepDirection)
		{
			if (segmentVector.Y == 0.0)
			{
				return ArcHelper.GetHorizontalArcEndPoints(ellipse, segmentVector, isLargeArc, sweepDirection);
			}
			return ArcHelper.GetNotHorizontalArcEndPoints(ellipse, segmentVector, isLargeArc, sweepDirection);
		}

		static Tuple<Point, Point> GetHorizontalArcEndPoints(Size ellipse, Point segmentVector, bool isLargeArc, SweepDirection sweepDirection)
		{
			if (segmentVector.Y != 0.0)
			{
				throw new ArgumentException("segment vector must be horizontal!");
			}
			double x = segmentVector.X;
			double[] array;
			double yAxisIntersection;
			if (ArcHelper.TryGetYAxisIntersection(ellipse, x, out array))
			{
				yAxisIntersection = (ArcHelper.ShouldTakeTopIntersection(x, isLargeArc, sweepDirection) ? array[0] : array[1]);
			}
			else
			{
				yAxisIntersection = 0.0;
			}
			return ArcHelper.GetHorizontalCrossPoints(ellipse, x, yAxisIntersection);
		}

		static Tuple<Point, Point> GetNotHorizontalArcEndPoints(Size ellipse, Point segmentVector, bool isLargeArc, SweepDirection sweepDirection)
		{
			double[] array;
			double xAxisIntersection;
			if (ArcHelper.TryGetXAxisIntersection(ellipse, segmentVector, out array))
			{
				xAxisIntersection = (ArcHelper.ShouldTakeLeftIntersection(segmentVector, isLargeArc, sweepDirection) ? array[0] : array[1]);
			}
			else
			{
				xAxisIntersection = 0.0;
			}
			return ArcHelper.GetCrossPoints(ellipse, segmentVector, xAxisIntersection);
		}

		static bool ShouldTakeTopIntersection(double segmentVectorX, bool isLargeArc, SweepDirection sweepDirection)
		{
			if (segmentVectorX == 0.0)
			{
				throw new ArgumentException("segmentVector cannot be zero");
			}
			bool flag;
			bool flag2;
			if (segmentVectorX > 0.0)
			{
				flag = isLargeArc && sweepDirection == SweepDirection.Counterclockwise;
				flag2 = !isLargeArc && sweepDirection == SweepDirection.Clockwise;
			}
			else
			{
				flag = isLargeArc && sweepDirection == SweepDirection.Clockwise;
				flag2 = !isLargeArc && sweepDirection == SweepDirection.Counterclockwise;
			}
			return flag || flag2;
		}

		static bool ShouldTakeLeftIntersection(Point segmentVector, bool isLargeArc, SweepDirection sweepDirection)
		{
			if (segmentVector.Y == 0.0)
			{
				throw new ArgumentException("segmentVector cannot be horizontal");
			}
			bool flag;
			bool flag2;
			if (segmentVector.Y > 0.0)
			{
				flag = isLargeArc && sweepDirection == SweepDirection.Clockwise;
				flag2 = !isLargeArc && sweepDirection == SweepDirection.Counterclockwise;
			}
			else
			{
				flag = isLargeArc && sweepDirection == SweepDirection.Counterclockwise;
				flag2 = !isLargeArc && sweepDirection == SweepDirection.Clockwise;
			}
			return flag || flag2;
		}

		static double GetCubicBezierArcKappa(double arcAngleInRadians)
		{
			if (arcAngleInRadians <= 0.0 || arcAngleInRadians > 1.5707963267948966)
			{
				throw new ArgumentOutOfRangeException("arcAngleInRadians must be in the interval (0; Math.PI/2]");
			}
			double num = Math.Sin(arcAngleInRadians);
			double num2 = Math.Cos(arcAngleInRadians);
			double num3 = Math.Cos(arcAngleInRadians / 2.0);
			return (8.0 * num3 - 4.0 - 4.0 * num2) / (3.0 * num);
		}

		static bool TryGetYAxisIntersection(Size ellipse, double segmentVectorXCoordinate, out double[] yAxisIntersections)
		{
			yAxisIntersections = null;
			double width = ellipse.Width;
			double height = ellipse.Height;
			double num = 4.0 * width * width - segmentVectorXCoordinate * segmentVectorXCoordinate;
			if (num > 0.0)
			{
				double num2 = Math.Sqrt(num) * height / (2.0 * width);
				yAxisIntersections = new double[]
				{
					-num2,
					num2
				};
				return true;
			}
			return false;
		}

		static bool TryGetXAxisIntersection(Size ellipse, Point segmentVector, out double[] xAxisIntersections)
		{
			xAxisIntersections = null;
			double x = segmentVector.X;
			double y = segmentVector.Y;
			double width = ellipse.Width;
			double height = ellipse.Height;
			double num = x * x * height * height + y * y * width * width;
			double num2 = (num * num - 4.0 * num * width * width * height * height) / (4.0 * height * height * (x * x * height * height - num));
			if (num2 > 0.0)
			{
				double num3 = Math.Sqrt(num2);
				xAxisIntersections = new double[]
				{
					-num3,
					num3
				};
				return true;
			}
			return false;
		}

		static Tuple<Point, Point> GetHorizontalCrossPoints(Size ellipse, double segmentVectorX, double yAxisIntersection)
		{
			double width = ellipse.Width;
			double height = ellipse.Height;
			double num = (height * height - yAxisIntersection * yAxisIntersection) / (segmentVectorX * segmentVectorX);
			if (num <= 0.0)
			{
				return null;
			}
			double num2 = Math.Sqrt(num) * width / height;
			double num3 = -num2;
			double num4 = num2;
			return new Tuple<Point, Point>(new Point(num3 * segmentVectorX, yAxisIntersection), new Point(num4 * segmentVectorX, yAxisIntersection));
		}

		static Tuple<Point, Point> GetCrossPoints(Size ellipse, Point segmentVector, double xAxisIntersection)
		{
			double v1 = segmentVector.X;
			double v2 = segmentVector.Y;
			double width = ellipse.Width;
			double height = ellipse.Height;
			double p = xAxisIntersection;
			double num = v1 * v1 * height * height + v2 * v2 * width * width;
			double num2 = height * height * (v1 * v1 * p * p * height * height - num * p * p + num * width * width);
			if (num2 <= 0.0)
			{
				return null;
			}
			double num3 = -v1 * p * height * height;
			double num4 = Math.Sqrt(num2);
			Func<double, Point> func = (double t) => new Point(t * v1 + p, t * v2);
			double arg = (num3 - num4) / num;
			double arg2 = (num3 + num4) / num;
			return new Tuple<Point, Point>(func(arg), func(arg2));
		}

		static Point HandleSmallCoordinates(Point point)
		{
			return new Point(ArcHelper.HandleSmallNumber(point.X), ArcHelper.HandleSmallNumber(point.Y));
		}

		static double HandleSmallNumber(double number)
		{
			if (Math.Abs(number) < 1E-06)
			{
				return 0.0;
			}
			return number;
		}

		static double NormalizeAngle(double angleInDegrees)
		{
			angleInDegrees %= 360.0;
			if (angleInDegrees < 0.0)
			{
				angleInDegrees = 360.0 + angleInDegrees;
			}
			return angleInDegrees;
		}

		const double Epsilon = 1E-06;

		static readonly Point[] clockwiseAxisPoints = new Point[]
		{
			new Point(1.0, 0.0),
			new Point(0.0, 1.0),
			new Point(-1.0, 0.0),
			new Point(0.0, -1.0)
		};
	}
}
