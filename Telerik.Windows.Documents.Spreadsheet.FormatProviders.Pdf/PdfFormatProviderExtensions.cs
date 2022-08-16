using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf
{
	static class PdfFormatProviderExtensions
	{
		public static RgbColor ToPdfRgbColor(this Color color)
		{
			return new RgbColor(color.A, color.R, color.G, color.B);
		}

		public static Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop ToPdfGradientStop(this System.Windows.Media.GradientStop gradientStop)
		{
			return new Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop(gradientStop.Color.ToPdfRgbColor(), gradientStop.Offset);
		}

		public static Gradient ToPdfGradient(this LinearGradientBrush linearGradientBrush, Matrix matrix)
		{
			Point startPoint = matrix.Transform(linearGradientBrush.StartPoint);
			Point endPoint = matrix.Transform(linearGradientBrush.EndPoint);
			LinearGradient linearGradient = new LinearGradient(startPoint, endPoint);
			IEnumerable<System.Windows.Media.GradientStop> enumerable = from stop in linearGradientBrush.GradientStops
				orderby stop.Offset
				select stop;
			foreach (System.Windows.Media.GradientStop gradientStop in enumerable)
			{
				linearGradient.GradientStops.Add(gradientStop.ToPdfGradientStop());
			}
			return linearGradient;
		}

		public static Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment ToPdfHorizontalTextAlignment(this TextAlignment textAlignment)
		{
			switch (textAlignment)
			{
			case TextAlignment.Left:
				return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left;
			case TextAlignment.Right:
				return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Right;
			default:
				return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
			}
		}

		public static Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment ToPdfBaselineAlignment(this System.Windows.BaselineAlignment baselineAlignment)
		{
			switch (baselineAlignment)
			{
			case System.Windows.BaselineAlignment.Subscript:
				return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Subscript;
			case System.Windows.BaselineAlignment.Superscript:
				return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Superscript;
			default:
				return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Baseline;
			}
		}

		public static Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry TransformRectangle(this Matrix matrix, Rect rectangle)
		{
			Point startPoint = matrix.Transform(new Point(rectangle.Left, rectangle.Top));
			Point point = matrix.Transform(new Point(rectangle.Right, rectangle.Top));
			Point point2 = matrix.Transform(new Point(rectangle.Right, rectangle.Bottom));
			Point point3 = matrix.Transform(new Point(rectangle.Left, rectangle.Bottom));
			Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry pathGeometry = new Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry();
			Telerik.Windows.Documents.Fixed.Model.Graphics.PathFigure pathFigure = pathGeometry.Figures.AddPathFigure();
			pathFigure.IsClosed = true;
			pathFigure.StartPoint = startPoint;
			pathFigure.Segments.AddLineSegment().Point = point;
			pathFigure.Segments.AddLineSegment().Point = point2;
			pathFigure.Segments.AddLineSegment().Point = point3;
			return pathGeometry;
		}

		public static UnderlinePattern ToPdfUnderlinePattern(this UnderlineType underlineType)
		{
			if (underlineType == UnderlineType.None)
			{
				return UnderlinePattern.None;
			}
			return UnderlinePattern.Single;
		}

		public static UnderlinePattern ToPdfTextDecorations(this TextDecorationCollection textDecorations)
		{
			if (textDecorations == TextDecorations.Underline)
			{
				return UnderlinePattern.Single;
			}
			return UnderlinePattern.None;
		}

		public static void PushTransformedClipping(this FixedContentEditor editor, Rect rect)
		{
			editor.PushClipping(editor.Position.Matrix.TransformRectangle(rect));
		}

		public static Matrix PreserveStates(this FixedContentEditor editor)
		{
			Matrix matrix = editor.Position.Matrix;
			editor.SaveProperties();
			return matrix;
		}

		public static void RestoreStates(this FixedContentEditor editor, Matrix oldEditorMatrix)
		{
			editor.RestoreProperties();
			editor.Position = new MatrixPosition(oldEditorMatrix);
		}

		public static Size MeasureText(this FixedContentEditor editor, string text, FontProperties fontProperties, double? wrappingWidth)
		{
			Block block = new Block();
			block.TextProperties.CopyFrom(editor.TextProperties);
			block.TextProperties.TrySetFont(fontProperties.FontFamily, fontProperties.FontStyle, fontProperties.FontWeight);
			block.TextProperties.FontSize = fontProperties.FontSize;
			block.InsertText(text);
			return block.Measure(new Size((wrappingWidth != null) ? wrappingWidth.Value : double.PositiveInfinity, double.PositiveInfinity));
		}

		public static Size MeasureText(this FixedContentEditor editor, string text, double? wrappingWidth)
		{
			Block block = new Block();
			block.TextProperties.CopyFrom(editor.TextProperties);
			block.InsertText(text);
			return block.Measure(new Size((wrappingWidth != null) ? wrappingWidth.Value : double.PositiveInfinity, double.PositiveInfinity));
		}

		public static void DrawTextBlock(this FixedContentEditor editor, string text, double width, double height, Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment horizontalAlignment, Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment verticalAlignment)
		{
			Block block = new Block();
			block.HorizontalAlignment = horizontalAlignment;
			block.VerticalAlignment = verticalAlignment;
			block.TextProperties.CopyFrom(editor.TextProperties);
			block.GraphicProperties.CopyFrom(editor.GraphicProperties);
			block.InsertText(text);
			editor.DrawBlock(block, new Size(width, height));
		}
	}
}
