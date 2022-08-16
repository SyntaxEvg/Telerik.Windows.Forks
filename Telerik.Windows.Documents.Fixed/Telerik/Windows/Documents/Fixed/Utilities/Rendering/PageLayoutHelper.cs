using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	public static class PageLayoutHelper
	{
		public static Rect GetVisibleContentBox(IFixedPage page)
		{
			Rect mediaBox = page.MediaBox;
			mediaBox.Intersect(page.CropBox);
			return (mediaBox != Rect.Empty) ? mediaBox : new Rect(0.0, 0.0, 0.0, 0.0);
		}

		public static double GetActualWidth(IFixedPage page)
		{
			Rect visibleContentBox = PageLayoutHelper.GetVisibleContentBox(page);
			if (page.Rotation == Rotation.Rotate0 || page.Rotation == Rotation.Rotate180)
			{
				return visibleContentBox.Width;
			}
			return visibleContentBox.Height;
		}

		public static double GetActualHeight(IFixedPage page)
		{
			Rect visibleContentBox = PageLayoutHelper.GetVisibleContentBox(page);
			if (page.Rotation == Rotation.Rotate0 || page.Rotation == Rotation.Rotate180)
			{
				return visibleContentBox.Height;
			}
			return visibleContentBox.Width;
		}

		internal static Rect CalculatePageBoundingRectangle(IFixedPage page)
		{
			Matrix matrix = PageLayoutHelper.CalculateRotatedPageTransformationInDip(page);
			Rect visibleContentBox = PageLayoutHelper.GetVisibleContentBox(page);
			Rect rect = new Rect(0.0, 0.0, visibleContentBox.Width, visibleContentBox.Height);
			return matrix.Transform(rect);
		}

		internal static Matrix CalculateRotatedPageTransformationInDip(IFixedPage page)
		{
			Rect visibleContentBox = PageLayoutHelper.GetVisibleContentBox(page);
			Rotation rotation = page.Rotation;
			double angle;
			Point point;
			if (rotation != Rotation.Rotate90)
			{
				if (rotation != Rotation.Rotate180)
				{
					if (rotation != Rotation.Rotate270)
					{
						angle = 0.0;
						point = default(Point);
					}
					else
					{
						angle = 270.0;
						point = new Point(visibleContentBox.Width / 2.0, visibleContentBox.Width / 2.0);
					}
				}
				else
				{
					angle = 180.0;
					point = new Point(visibleContentBox.Width / 2.0, visibleContentBox.Height / 2.0);
				}
			}
			else
			{
				angle = 90.0;
				point = new Point(visibleContentBox.Height / 2.0, visibleContentBox.Height / 2.0);
			}
			Matrix result = default(Matrix);
			result.RotateAt(angle, point.X, point.Y);
			return result;
		}

		internal static Matrix CalculateVisibibleContentTransformation(IFixedPage page)
		{
			Rect visibleContentBox = PageLayoutHelper.GetVisibleContentBox(page);
			Matrix m = PageLayoutHelper.CalculateRotatedPageTransformationInDip(page);
			return new Matrix(1.0, 0.0, 0.0, 1.0, -visibleContentBox.X, -visibleContentBox.Y).MultiplyBy(m);
		}

		internal static Matrix CalculatePointToDipPageTransformation(IFixedPage page)
		{
			Matrix result = default(Matrix);
			double num = Unit.PointToDip(1.0);
			double scaleX = num;
			double scaleY = -num;
			double centerX = 0.0;
			double centerY = page.MediaBox.Height / (1.0 + num);
			Matrix matrix = PageLayoutHelper.CalculateRotatedPageTransformationInDip(page);
			result.ScaleAt(scaleX, scaleY, centerX, centerY);
			result.Append(matrix);
			return result;
		}
	}
}
