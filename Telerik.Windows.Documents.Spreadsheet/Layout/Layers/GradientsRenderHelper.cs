using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	static class GradientsRenderHelper
	{
		public static IEnumerable<LinearGradientBox> GenerateGradientBoxes(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>> func = GradientsRenderHelper.gradientToGradientBoxes[fill.GradientType];
			return func(fill, colorScheme, containerSize);
		}

		static IEnumerable<LinearGradientBox> GenerateHorizontalGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(0.0, 1.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateHorizontalReversedGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 1.0), new Point(0.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateHorizontalRepeatedGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 0.5;
			double gradientStopOffsetColor1Repeat = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateRepeatedGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, gradientStopOffsetColor1Repeat, new Point(0.0, 0.0), new Point(0.0, 1.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateVerticalGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(1.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateVerticalReversedGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(1.0, 0.0), new Point(0.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateVerticalRepeatedGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 0.5;
			double gradientStopOffsetColor1Repeat = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateRepeatedGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, gradientStopOffsetColor1Repeat, new Point(0.0, 0.0), new Point(1.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateDiagonalUpGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(1.0, 1.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateDiagonalUpReversedGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(1.0, 1.0), new Point(0.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateDiagonalUpRepeatedGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 0.5;
			double gradientStopOffsetColor1Repeat = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateRepeatedGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, gradientStopOffsetColor1Repeat, new Point(0.0, 0.0), new Point(1.0, 1.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateDiagonalDownGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(1.0, 0.0), new Point(0.0, 1.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateDiagonalDownReversedGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 1.0), new Point(1.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateDiagonalDownRepeatedGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.0;
			double gradientStopOffsetColor2 = 0.5;
			double gradientStopOffsetColor1Repeat = 1.0;
			LinearGradientBox gradient = GradientsRenderHelper.GenerateRepeatedGradientRectangleWithRectangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, gradientStopOffsetColor1Repeat, new Point(0.0, 1.0), new Point(1.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return gradient;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateFromTopLeftCornerGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.2;
			double gradientStopOffsetColor2 = 1.0;
			double width = containerSize.Width;
			double height = containerSize.Height;
			LinearGradientBox topGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(width, height), new Point(width, 0.0), new Point(0.0, 0.0), new Point(1.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			LinearGradientBox bottomGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(width, height), new Point(0.0, height), new Point(0.0, 0.0), new Point(0.0, 1.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return topGradientBox;
			yield return bottomGradientBox;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateFromTopRightCornerGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.2;
			double gradientStopOffsetColor2 = 1.0;
			double width = containerSize.Width;
			double height = containerSize.Height;
			LinearGradientBox topGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(width, 0.0), new Point(0.0, height), new Point(1.0, 0.0), new Point(0.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			LinearGradientBox bottomGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(width, height), new Point(width, 0.0), new Point(0.0, height), new Point(0.0, 0.0), new Point(0.0, 1.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return topGradientBox;
			yield return bottomGradientBox;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateFromBottomLeftCornerGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.2;
			double gradientStopOffsetColor2 = 1.0;
			double width = containerSize.Width;
			double height = containerSize.Height;
			LinearGradientBox topGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(width, 0.0), new Point(0.0, height), new Point(0.0, 1.0), new Point(0.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			LinearGradientBox bottomGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(width, height), new Point(width, 0.0), new Point(0.0, height), new Point(0.0, 0.0), new Point(1.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return topGradientBox;
			yield return bottomGradientBox;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateFromBottomRightCornerGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.2;
			double gradientStopOffsetColor2 = 1.0;
			double width = containerSize.Width;
			double height = containerSize.Height;
			LinearGradientBox topGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(width, height), new Point(width, 0.0), new Point(0.0, 1.0), new Point(0.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			LinearGradientBox bottomGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(width, height), new Point(0.0, height), new Point(1.0, 0.0), new Point(0.0, 0.0), new Rect(new Point(0.0, 0.0), containerSize));
			yield return topGradientBox;
			yield return bottomGradientBox;
			yield break;
		}

		static IEnumerable<LinearGradientBox> GenerateFromCenterGradient(GradientFill fill, ThemeColorScheme colorScheme, Size containerSize)
		{
			double gradientStopOffsetColor = 0.75;
			double gradientStopOffsetColor2 = 0.0;
			double width = containerSize.Width;
			double height = containerSize.Height;
			LinearGradientBox leftGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(0.0, height), new Point(width / 2.0, height / 2.0), new Point(0.0, 0.0), new Point(1.0, 0.0), new Rect(0.0, 0.0, width / 2.0, height));
			LinearGradientBox rightGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(width / 2.0, 0.0), new Point(width / 2.0, height), new Point(0.0, height / 2.0), new Point(1.0, 0.0), new Point(0.0, 0.0), new Rect(width / 2.0, 0.0, width / 2.0, height));
			LinearGradientBox topGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, 0.0), new Point(width / 2.0, height / 2.0), new Point(width, 0.0), new Point(0.0, 0.0), new Point(0.0, 1.0), new Rect(0.0, 0.0, width, height / 2.0));
			LinearGradientBox bottomGradientBox = GradientsRenderHelper.GenerateGradientRectangleWithTriangleClip(fill, colorScheme, gradientStopOffsetColor, gradientStopOffsetColor2, new Point(0.0, height / 2.0), new Point(width, height / 2.0), new Point(width / 2.0, 0.0), new Point(0.0, 1.0), new Point(0.0, 0.0), new Rect(0.0, height / 2.0, width, height / 2.0));
			yield return leftGradientBox;
			yield return rightGradientBox;
			yield return topGradientBox;
			yield return bottomGradientBox;
			yield break;
		}

		static LinearGradientBox GenerateGradientRectangleWithTriangleClip(GradientFill fill, ThemeColorScheme colorScheme, double gradientStopOffsetColor1, double gradientStopOffsetColor2, Point clipFirstPoint, Point clipSecondPoint, Point clipThirdPoint, Point gradientBrushStartPoint, Point gradientBrushEndPoint, Rect boundingBox)
		{
			return GradientsRenderHelper.GenerateGradientRectangle(fill, colorScheme, gradientStopOffsetColor1, gradientStopOffsetColor2, gradientBrushStartPoint, gradientBrushEndPoint, new PathFigure
			{
				Segments = 
				{
					new LineSegment
					{
						Point = clipSecondPoint
					},
					new LineSegment
					{
						Point = clipThirdPoint
					}
				},
				StartPoint = clipFirstPoint
			}, boundingBox);
		}

		static LinearGradientBox GenerateGradientRectangleWithRectangleClip(GradientFill fill, ThemeColorScheme colorScheme, double gradientStopOffsetColor1, double gradientStopOffsetColor2, Point gradientBrushStartPoint, Point gradientBrushEndPoint, Rect boundingBox)
		{
			return GradientsRenderHelper.GenerateGradientRectangle(fill, colorScheme, gradientStopOffsetColor1, gradientStopOffsetColor2, gradientBrushStartPoint, gradientBrushEndPoint, new PathFigure
			{
				Segments = 
				{
					new LineSegment
					{
						Point = new Point(0.0, boundingBox.Height)
					},
					new LineSegment
					{
						Point = new Point(boundingBox.Width, boundingBox.Height)
					},
					new LineSegment
					{
						Point = new Point(boundingBox.Width, 0.0)
					}
				},
				StartPoint = new Point(0.0, 0.0)
			}, boundingBox);
		}

		static LinearGradientBox GenerateRepeatedGradientRectangleWithRectangleClip(GradientFill fill, ThemeColorScheme colorScheme, double gradientStopOffsetColor1, double gradientStopOffsetColor2, double gradientStopOffsetColor1Repeat, Point gradientBrushStartPoint, Point gradientBrushEndPoint, Rect boundingBox)
		{
			return GradientsRenderHelper.GenerateRepeatedGradientRectangle(fill, colorScheme, gradientStopOffsetColor1, gradientStopOffsetColor2, gradientStopOffsetColor1Repeat, gradientBrushStartPoint, gradientBrushEndPoint, new PathFigure
			{
				Segments = 
				{
					new LineSegment
					{
						Point = new Point(0.0, boundingBox.Height)
					},
					new LineSegment
					{
						Point = new Point(boundingBox.Width, boundingBox.Height)
					},
					new LineSegment
					{
						Point = new Point(boundingBox.Width, 0.0)
					}
				},
				StartPoint = new Point(0.0, 0.0)
			}, boundingBox);
		}

		static LinearGradientBox GenerateGradientRectangle(GradientFill fill, ThemeColorScheme colorScheme, double gradientStopOffsetColor1, double gradientStopOffsetColor2, Point gradientBrushStartPoint, Point gradientBrushEndPoint, PathFigure rectClipPathFigure, Rect boundingBox)
		{
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new GradientStopCollection
			{
				new GradientStop
				{
					Color = fill.Color1.GetActualValue(colorScheme),
					Offset = gradientStopOffsetColor1
				},
				new GradientStop
				{
					Color = fill.Color2.GetActualValue(colorScheme),
					Offset = gradientStopOffsetColor2
				}
			}, 0.0);
			linearGradientBrush.StartPoint = gradientBrushStartPoint;
			linearGradientBrush.EndPoint = gradientBrushEndPoint;
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures.Add(rectClipPathFigure);
			return new LinearGradientBox
			{
				BoundingBox = boundingBox,
				Fill = linearGradientBrush,
				Clip = pathGeometry
			};
		}

		static LinearGradientBox GenerateRepeatedGradientRectangle(GradientFill fill, ThemeColorScheme colorScheme, double gradientStopOffsetColor1, double gradientStopOffsetColor2, double gradientStopOffsetColor1Repeat, Point gradientBrushStartPoint, Point gradientBrushEndPoint, PathFigure rectClipPathFigure, Rect boundingBox)
		{
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new GradientStopCollection
			{
				new GradientStop
				{
					Color = fill.Color1.GetActualValue(colorScheme),
					Offset = gradientStopOffsetColor1
				},
				new GradientStop
				{
					Color = fill.Color2.GetActualValue(colorScheme),
					Offset = gradientStopOffsetColor2
				},
				new GradientStop
				{
					Color = fill.Color1.GetActualValue(colorScheme),
					Offset = gradientStopOffsetColor1Repeat
				}
			}, 0.0);
			linearGradientBrush.StartPoint = gradientBrushStartPoint;
			linearGradientBrush.EndPoint = gradientBrushEndPoint;
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures.Add(rectClipPathFigure);
			return new LinearGradientBox
			{
				BoundingBox = boundingBox,
				Fill = linearGradientBrush,
				Clip = pathGeometry
			};
		}

		static readonly Dictionary<GradientType, Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>> gradientToGradientBoxes = new Dictionary<GradientType, Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>>
		{
			{
				GradientType.Horizontal,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateHorizontalGradient)
			},
			{
				GradientType.HorizontalReversed,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateHorizontalReversedGradient)
			},
			{
				GradientType.HorizontalRepeated,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateHorizontalRepeatedGradient)
			},
			{
				GradientType.Vertical,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateVerticalGradient)
			},
			{
				GradientType.VerticalReversed,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateVerticalReversedGradient)
			},
			{
				GradientType.VerticalRepeated,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateVerticalRepeatedGradient)
			},
			{
				GradientType.DiagonalUp,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateDiagonalUpGradient)
			},
			{
				GradientType.DiagonalUpReversed,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateDiagonalUpReversedGradient)
			},
			{
				GradientType.DiagonalUpRepeated,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateDiagonalUpRepeatedGradient)
			},
			{
				GradientType.DiagonalDown,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateDiagonalDownGradient)
			},
			{
				GradientType.DiagonalDownReversed,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateDiagonalDownReversedGradient)
			},
			{
				GradientType.DiagonalDownRepeated,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateDiagonalDownRepeatedGradient)
			},
			{
				GradientType.FromTopLeftCorner,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateFromTopLeftCornerGradient)
			},
			{
				GradientType.FromTopRightCorner,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateFromTopRightCornerGradient)
			},
			{
				GradientType.FromBottomLeftCorner,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateFromBottomLeftCornerGradient)
			},
			{
				GradientType.FromBottomRightCorner,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateFromBottomRightCornerGradient)
			},
			{
				GradientType.FromCenter,
				new Func<GradientFill, ThemeColorScheme, Size, IEnumerable<LinearGradientBox>>(GradientsRenderHelper.GenerateFromCenterGradient)
			}
		};
	}
}
