using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class ShapesResizeHelper
	{
		public static Point DetermineNewTopLeftWhenResized(Rect shapeCurrentRectangle, double rotationAngle, double newWidth, double newHeight, bool isLeftResizing = false, bool isTopResizing = false)
		{
			double x = (isLeftResizing ? (shapeCurrentRectangle.Width - newWidth / 2.0) : (newWidth / 2.0));
			double y = (isTopResizing ? (shapeCurrentRectangle.Height - newHeight / 2.0) : (newHeight / 2.0));
			Point point = new Point(x, y);
			Matrix m = default(Matrix).RotateMatrix(rotationAngle);
			Point point2 = new Point(shapeCurrentRectangle.X, shapeCurrentRectangle.Y);
			Point anchorPoint = point2.Add(new Point(shapeCurrentRectangle.Width / 2.0, shapeCurrentRectangle.Height / 2.0));
			Point point3 = point2.Rotate(anchorPoint, rotationAngle);
			Point point4 = m.TranslateMatrix(point3.X, point3.Y).Transform(point);
			Point result = new Point(point4.X - Math.Abs(newWidth / 2.0), point4.Y - Math.Abs(newHeight / 2.0));
			return result;
		}

		public static Size GetLockAspectRatioScaleFactor(double widthHeightAspectRatio, double proposedWidth, double proposedHeight)
		{
			double num = Math.Abs(proposedWidth / proposedHeight);
			if (num <= widthHeightAspectRatio)
			{
				return new Size(widthHeightAspectRatio / num, 1.0);
			}
			return new Size(1.0, num / widthHeightAspectRatio);
		}

		public static Point AdjustBoundingRectangleWhenOutOfSpreadsheet(RadWorksheetLayout layout, Rect boundingRectangle, Point topLeftCornerCoordinates)
		{
			Guard.ThrowExceptionIfNull<RadWorksheetLayout>(layout, "layout");
			double x = topLeftCornerCoordinates.X;
			double y = topLeftCornerCoordinates.Y;
			if (boundingRectangle.Left < 0.0)
			{
				x = topLeftCornerCoordinates.X + Math.Abs(boundingRectangle.X);
			}
			if (boundingRectangle.Right >= layout.Width)
			{
				x = topLeftCornerCoordinates.X - (boundingRectangle.Right - layout.Width) - 0.1;
			}
			if (boundingRectangle.Top < 0.0)
			{
				y = topLeftCornerCoordinates.Y + Math.Abs(boundingRectangle.Y);
			}
			if (boundingRectangle.Bottom >= layout.Height)
			{
				y = topLeftCornerCoordinates.Y - (boundingRectangle.Bottom - layout.Height) - 0.1;
			}
			return new Point(x, y);
		}

		public static Point AdjustPointWhenOutOfSpreadsheet(RadWorksheetLayout layout, Point point)
		{
			double num = point.X;
			double num2 = point.Y;
			num = Math.Max(0.0, num);
			num2 = Math.Max(0.0, num2);
			num = System.Math.Min(layout.Width - 0.1, num);
			num2 = System.Math.Min(layout.Height - 0.1, num2);
			return new Point(num, num2);
		}

		public static Point AdjustTopLeftForRotation(Point point, double width, double height)
		{
			Matrix identity = Matrix.Identity;
			Point result = identity.RotateMatrixAt(90.0, point.X + width / 2.0, point.Y + height / 2.0).Transform(point);
			result.X -= height;
			return result;
		}

		public static Point ReverseAdjustmentForRotation(Point point, double width, double height)
		{
			Matrix identity = Matrix.Identity;
			Point result = identity.RotateMatrixAt(-90.0, point.X + height / 2.0, point.Y + width / 2.0).Transform(point);
			result.Y -= height;
			return result;
		}

		public static Point GetShapeTopLeftConsideringAdjustmentForRotation(FloatingShapeBase shape, RadWorksheetLayout layout)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			Point shapeTopLeft = layout.GetShapeTopLeft(shape);
			if (shape.DoesRotationAngleRequireCellIndexChange())
			{
				return ShapesResizeHelper.ReverseAdjustmentForRotation(shapeTopLeft, shape.Width, shape.Height);
			}
			return shapeTopLeft;
		}

		public static Point GetDocumentCoordinateFromRotatedPoint(Point point, double rotationAngle, Point topLeftCoordinate, double width, double height)
		{
			return ShapesResizeHelper.GetRotatedToDocumentTransoformMatrix(rotationAngle, topLeftCoordinate, width, height).Transform(point);
		}

		public static Point GetRotatedCoordinateFromDocumentPoint(Point point, double rotationAngle, Point topLeftCoordinate, double width, double height)
		{
			Matrix rotatedToDocumentTransoformMatrix = ShapesResizeHelper.GetRotatedToDocumentTransoformMatrix(rotationAngle, topLeftCoordinate, width, height);
			return rotatedToDocumentTransoformMatrix.InverseMatrix().Transform(point);
		}

		static Matrix GetRotatedToDocumentTransoformMatrix(double rotationAngle, Point topLeftCoordinate, double width, double height)
		{
			Matrix m = default(Matrix).RotateMatrix(rotationAngle);
			Point anchorPoint = topLeftCoordinate.Add(new Point(width / 2.0, height / 2.0));
			Point point = topLeftCoordinate.Rotate(anchorPoint, rotationAngle);
			return m.TranslateMatrix(point.X, point.Y);
		}
	}
}
