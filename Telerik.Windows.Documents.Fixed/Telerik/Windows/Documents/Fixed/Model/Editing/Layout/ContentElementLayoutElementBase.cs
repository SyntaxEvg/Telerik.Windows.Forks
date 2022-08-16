using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class ContentElementLayoutElementBase<T> : LayoutElementBase where T : PositionContentElement
	{
		internal ContentElementLayoutElementBase(T element, double width, double trimmedWidth, double baselineOffset, FontBase font, double fontSize, RenderingMode renderingMode, ColorBase underlineColor, UnderlinePattern underlinePattern, ColorBase highlightColor)
			: base(width, trimmedWidth, baselineOffset, font, fontSize)
		{
			Guard.ThrowExceptionIfNull<T>(element, "element");
			this.element = element;
			this.underlineColor = underlineColor;
			this.underlinePattern = underlinePattern;
			this.renderingMode = renderingMode;
			this.highlightColor = highlightColor;
		}

		internal ContentElementLayoutElementBase(T element, double width, double trimmedWidth, double baselineOffset, ContentElementLayoutElementBase<T> other)
			: this(element, width, trimmedWidth, baselineOffset, other.Font, other.FontSize, other.RenderingMode, other.UnderlineColor, other.UnderlinePattern, other.HighlightColor)
		{
		}

		public T Element
		{
			get
			{
				return this.element;
			}
		}

		internal RenderingMode RenderingMode
		{
			get
			{
				return this.renderingMode;
			}
		}

		internal ColorBase UnderlineColor
		{
			get
			{
				return this.underlineColor;
			}
		}

		internal UnderlinePattern UnderlinePattern
		{
			get
			{
				return this.underlinePattern;
			}
		}

		internal ColorBase HighlightColor
		{
			get
			{
				return this.highlightColor;
			}
		}

		internal override LineInfo CompleteLine(double maxLineWidth, double currentLineOffsetX, List<LayoutElementBase> elementsInCurrentLine, out IEnumerable<LayoutElementBase> pendingLayoutElements)
		{
			LineInfo lineInfo = new LineInfo();
			lineInfo.AddRange(elementsInCurrentLine);
			if (elementsInCurrentLine.Count == 0)
			{
				lineInfo.Add(this);
				pendingLayoutElements = Enumerable.Empty<LayoutElementBase>();
			}
			else
			{
				pendingLayoutElements = new LayoutElementBase[] { this };
			}
			return lineInfo;
		}

		internal override void Draw(DrawLayoutElementContext context)
		{
			Guard.ThrowExceptionIfNull<DrawLayoutElementContext>(context, "context");
			if (this.HighlightColor != null)
			{
				Path path = this.DrawHighlights(context);
				Matrix matrix = default(Matrix).TranslateMatrix(context.OffsetX, context.OffsetY);
				path.Position = new MatrixPosition(matrix);
				context.Editor.Draw(path);
			}
			Matrix matrix2 = default(Matrix).TranslateMatrix(context.OffsetX, context.OffsetY);
			matrix2 = this.Transform(context, matrix2);
			T t = this.element;
			t.Position = new MatrixPosition(matrix2);
			context.Editor.Draw(this.element);
			if (this.RenderingMode != RenderingMode.None && this.UnderlinePattern != UnderlinePattern.None)
			{
				Path path2 = this.DrawUnderline(context.Line);
				Matrix matrix3 = default(Matrix).TranslateMatrix(context.OffsetX, context.OffsetY + context.Line.BaselineOffset + context.Line.UnderlinePosition + context.Line.UnderlineThickness / 2.0);
				path2.Position = new MatrixPosition(matrix3);
				context.Editor.Draw(path2);
			}
		}

		internal override bool CanFit(double maxTotalWidth, double currentLineOffsetX)
		{
			return maxTotalWidth.IsGreaterThanOrEqualTo(currentLineOffsetX + base.TrimmedWidth, 0.1);
		}

		protected virtual Matrix Transform(DrawLayoutElementContext context, Matrix transform)
		{
			Guard.ThrowExceptionIfNull<DrawLayoutElementContext>(context, "context");
			return transform;
		}

		Path DrawHighlights(DrawLayoutElementContext context)
		{
			Guard.ThrowExceptionIfNull<DrawLayoutElementContext>(context, "context");
			double height;
			switch (context.Block.LineSpacingType)
			{
			case HeightType.AtLeast:
			case HeightType.Exact:
				height = context.Block.GetActualLineHeight(context.Line);
				break;
			default:
				height = context.Line.BaselineOffset + context.Line.Descent;
				break;
			}
			Rect rect = new Rect(0.0, 0.0, base.GetWidthInLine(context.Line), height);
			return new Path
			{
				Geometry = new Telerik.Windows.Documents.Fixed.Model.Graphics.RectangleGeometry(rect),
				IsFilled = true,
				IsStroked = false,
				Fill = this.HighlightColor
			};
		}

		Path DrawUnderline(LineInfo line)
		{
			Guard.ThrowExceptionIfNull<LineInfo>(line, "line");
			Guard.ThrowExceptionIfNull<T>(this.element, "element");
			Point startPoint = new Point(0.0, 0.0);
			Point point = new Point(base.GetWidthInLine(line), 0.0);
			Path path = new Path();
			Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry pathGeometry = new Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry();
			Telerik.Windows.Documents.Fixed.Model.Graphics.PathFigure pathFigure = pathGeometry.Figures.AddPathFigure();
			pathFigure.IsClosed = false;
			pathFigure.StartPoint = startPoint;
			pathFigure.Segments.AddLineSegment(point);
			path.Geometry = pathGeometry;
			path.Stroke = this.UnderlineColor;
			path.IsFilled = false;
			path.IsStroked = true;
			path.StrokeThickness = line.UnderlineThickness;
			return path;
		}

		readonly RenderingMode renderingMode;

		readonly ColorBase underlineColor;

		readonly UnderlinePattern underlinePattern;

		readonly ColorBase highlightColor;

		readonly T element;
	}
}
