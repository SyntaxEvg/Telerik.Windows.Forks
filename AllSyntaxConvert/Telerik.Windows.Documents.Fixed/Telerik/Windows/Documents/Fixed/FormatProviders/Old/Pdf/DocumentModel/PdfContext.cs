using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Core.Shapes;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Enums;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel
{
	class PdfContext
	{
		public PdfContext(PdfContentManager contentManager, Rect clip)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			this.contentManager = contentManager;
			this.GraphicsState = new GraphicsStateOld(clip);
			this.GraphicsState.ColorSpace = new DeviceGrayOld(contentManager);
			this.GraphicsState.StrokeColorSpace = new DeviceGrayOld(contentManager);
			this.TextState = new TextStateOld();
			this.CurrentPathGeometry = new PathGeometry();
			this.graphicsStateStack = new Stack<GraphicsStateOld>();
			this.textStateStack = new Stack<TextStateOld>();
		}

		public TextStateOld TextState { get; set; }

		public GraphicsStateOld GraphicsState { get; set; }

		public PathGeometry CurrentPathGeometry { get; set; }

		public PathFigure CurrentPathFigure { get; set; }

		public Point CurrentPoint { get; set; }

		public Color StencilColor
		{
			get
			{
				SolidColorBrush solidColorBrush = this.GraphicsState.GetBrushWithAlpha() as SolidColorBrush;
				if (solidColorBrush == null)
				{
					return GraphicsStateOld.DefaultColor;
				}
				return solidColorBrush.Color;
			}
		}

		public PdfResourceManager ResourcesManager
		{
			get
			{
				return this.contentManager.ResourceManager;
			}
		}

		public void SaveGraphicState()
		{
			this.graphicsStateStack.Push(new GraphicsStateOld(this.GraphicsState));
			this.textStateStack.Push(new TextStateOld(this.TextState));
		}

		public void RestoreGraphicState()
		{
			this.GraphicsState = this.graphicsStateStack.Pop();
			this.TextState = this.textStateStack.Pop();
		}

		public void SetMatrix(double a, double b, double c, double d, double e, double f)
		{
			this.GraphicsState.Ctm = new Matrix(a, b, c, d, e, f) * this.GraphicsState.Ctm;
		}

		public void SetLineWidth(double lineWidth)
		{
			if (lineWidth <= 1E-05)
			{
				this.GraphicsState.LineWidth = 0.5;
				return;
			}
			this.GraphicsState.LineWidth = lineWidth;
		}

		public void SetLineCap(int lineCap)
		{
			this.GraphicsState.LineCap = lineCap;
		}

		public void SetLineJoin(int lineJoin)
		{
			this.GraphicsState.LineJoin = lineJoin;
		}

		public void SetMitterLimit(double miterLimit)
		{
			this.GraphicsState.MiterLimit = miterLimit;
		}

		public void SetDashPattern(PdfArrayOld dashPattern, double dashOffset)
		{
			Guard.ThrowExceptionIfNull<PdfArrayOld>(dashPattern, "dashPattern");
			this.GraphicsState.DashPattern = dashPattern.ToDashPattern();
			this.GraphicsState.DashOffset = dashOffset;
		}

		public void SetRenderingIntent(string intent)
		{
			this.GraphicsState.RenderingIntent = intent;
		}

		public void SetFlatness(double flatness)
		{
			this.GraphicsState.Flatness = flatness;
		}

		public void SetGraphicState(ExtGStateOld props)
		{
			Guard.ThrowExceptionIfNull<ExtGStateOld>(props, "props");
			this.GraphicsState.SetProps(props);
		}

		public void MoveTo(double x, double y)
		{
			if (this.CurrentPathFigure != null && this.CurrentPathFigure.Segments.Count == 0)
			{
				this.CurrentPathGeometry.Figures.Remove(this.CurrentPathFigure);
			}
			this.CurrentPathFigure = new PathFigure();
			this.CurrentPoint = new Point(x, y);
			this.CurrentPathFigure.StartPoint = this.CurrentPoint;
			this.CurrentPathGeometry.Figures.Add(this.CurrentPathFigure);
		}

		public void LineTo(double x, double y)
		{
			this.CurrentPoint = new Point(x, y);
			this.CurrentPathFigure.Segments.Add(new LineSegment
			{
				Point = this.CurrentPoint
			});
		}

		public void CurveTo(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			BezierSegment bezierSegment = new BezierSegment();
			bezierSegment.Point1 = new Point(x1, y1);
			bezierSegment.Point2 = new Point(x2, y2);
			bezierSegment.Point3 = new Point(x3, y3);
			this.CurrentPathFigure.Segments.Add(bezierSegment);
			this.CurrentPoint = bezierSegment.Point3;
		}

		public void HCurveTo(double x2, double y2, double x3, double y3)
		{
			BezierSegment bezierSegment = new BezierSegment();
			bezierSegment.Point1 = this.CurrentPoint;
			bezierSegment.Point2 = new Point(x2, y2);
			bezierSegment.Point3 = new Point(x3, y3);
			this.CurrentPathFigure.Segments.Add(bezierSegment);
			this.CurrentPoint = bezierSegment.Point3;
		}

		public void VCurveTo(double x1, double y1, double x3, double y3)
		{
			BezierSegment bezierSegment = new BezierSegment();
			bezierSegment.Point1 = new Point(x1, y1);
			bezierSegment.Point2 = new Point(x3, y3);
			bezierSegment.Point3 = new Point(x3, y3);
			this.CurrentPathFigure.Segments.Add(bezierSegment);
			this.CurrentPoint = bezierSegment.Point3;
		}

		public void Close()
		{
			if (this.CurrentPathFigure == null)
			{
				return;
			}
			this.CurrentPathFigure.IsClosed = true;
			this.MoveTo(this.CurrentPathFigure.StartPoint.X, this.CurrentPathFigure.StartPoint.Y);
		}

		public void Rectangle(double x, double y, double width, double height)
		{
			this.MoveTo(x, y);
			this.LineTo(x + width, y);
			this.LineTo(x + width, y + height);
			this.LineTo(x, y + height);
			this.Close();
		}

		public Path Stroke()
		{
			Path path = this.CreatePath();
			this.Stroke(path);
			this.CurrentPathGeometry = new PathGeometry();
			this.CurrentPathFigure = null;
			return path;
		}

		public Path CloseAndStroke()
		{
			this.Close();
			return this.Stroke();
		}

		public Path Fill()
		{
			return this.Fill(FillRule.Nonzero);
		}

		public Path Fill(FillRule rule)
		{
			Path path = this.CreatePath();
			this.Fill(path, rule);
			this.CurrentPathGeometry = new PathGeometry();
			this.CurrentPathFigure = null;
			return path;
		}

		public Path FillAndStroke()
		{
			return this.FillAndStroke(FillRule.Nonzero);
		}

		public Path FillAndStroke(FillRule rule)
		{
			Path path = this.CreatePath();
			this.Fill(path, rule);
			this.Stroke(path);
			this.CurrentPathGeometry = new PathGeometry();
			this.CurrentPathFigure = null;
			return path;
		}

		public Path CloseFillAndStroke()
		{
			return this.CloseFillAndStroke(FillRule.Nonzero);
		}

		public Path CloseFillAndStroke(FillRule rule)
		{
			this.Close();
			Path result = this.FillAndStroke(rule);
			this.CurrentPathGeometry = new PathGeometry();
			this.CurrentPathFigure = null;
			return result;
		}

		public void EndPath()
		{
			this.CurrentPathGeometry = new PathGeometry();
			this.CurrentPathFigure = null;
		}

		public void SetClippingPath()
		{
			this.SetClippingPath(FillRule.Nonzero);
		}

		public void SetClippingPath(FillRule rule)
		{
			if (!this.CurrentPathGeometry.IsEmpty)
			{
				Container container = new Container();
				container.Clip = this.CurrentPathGeometry.Clone();
				PdfContext.Fill(container.Clip, rule);
				container.Clip.TransformMatrix = this.GraphicsState.Ctm;
				this.GraphicsState.ClippingContainer.Content.AddChild(container);
				this.GraphicsState.ClippingContainer = container;
			}
		}

		public void SetStrokeColorSpace(ColorSpaceOld space)
		{
			this.GraphicsState.StrokeColorSpace = space;
			this.GraphicsState.StrokeBrush = space.DefaultBrush;
		}

		public void SetColorSpace(ColorSpaceOld space)
		{
			this.GraphicsState.ColorSpace = space;
			this.GraphicsState.Brush = space.DefaultBrush;
		}

		public void SetStrokeColor(Brush brush)
		{
			this.GraphicsState.StrokeBrush = brush;
		}

		public void SetColor(Brush brush)
		{
			this.GraphicsState.Brush = brush;
		}

		public void SetGrayStrokeColor(double color)
		{
			this.SetStrokeColorSpace(new DeviceGrayOld(this.contentManager));
			this.SetStrokeColor(new SolidColorBrush(Color.FromGray(color)));
		}

		public void SetGrayColor(double color)
		{
			this.SetColorSpace(new DeviceGrayOld(this.contentManager));
			this.SetColor(new SolidColorBrush(Color.FromGray(color)));
		}

		public void SetRgbStrokeColor(Color color)
		{
			this.SetStrokeColorSpace(new DeviceRgbOld(this.contentManager));
			this.SetStrokeColor(new SolidColorBrush(color));
		}

		public void SetRgbColor(Color color)
		{
			this.SetColorSpace(new DeviceRgbOld(this.contentManager));
			this.SetColor(new SolidColorBrush(color));
		}

		public void SetCmykStrokeColor(Color color)
		{
			this.SetStrokeColorSpace(new DeviceCmykOld(this.contentManager));
			this.SetStrokeColor(new SolidColorBrush(color));
		}

		public void SetCmykColor(Color color)
		{
			this.SetColorSpace(new DeviceCmykOld(this.contentManager));
			this.SetColor(new SolidColorBrush(color));
		}

		public Image CreateXImage(XImage image)
		{
			Guard.ThrowExceptionIfNull<XImage>(image, "image");
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(image.Reference, "reference");
			return new Image
			{
				ImageSourceKey = this.contentManager.ResourceManager.RegisterXImage(image, this.StencilColor),
				TransformMatrix = this.GraphicsState.Ctm
			};
		}

		public Container CreateXForm(PdfResourceOld baseResources, XForm form, bool skipNonTextRelatedOffset)
		{
			this.SaveGraphicState();
			Matrix matrix = form.Matrix.ToMatrix();
			Container container = new Container();
			container.TransformMatrix = matrix * this.GraphicsState.Ctm;
			foreach (Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement contentElement in this.ResourcesManager.GetXFormContent(baseResources, form, skipNonTextRelatedOffset))
			{
				container.Content.AddChild(contentElement.Clone());
			}
			this.RestoreGraphicState();
			return container;
		}

		public void SetCharSpacing(double charSpace)
		{
			this.TextState.CharSpacing = charSpace;
		}

		public void SetWordSpacing(double wordSpace)
		{
			this.TextState.WordSpacing = wordSpace;
		}

		public void SetHorizontalScaling(double scale)
		{
			this.TextState.HorizontalScaling = scale;
		}

		public void SetTextLeading(double leading)
		{
			this.TextState.Leading = -leading;
		}

		public void SetFont(FontBaseOld font, double size)
		{
			Guard.ThrowExceptionIfNull<FontBaseOld>(font, "font");
			this.TextState.Font = font;
			this.TextState.FontSize = size;
		}

		public void SetRenderingMode(RenderingMode mode)
		{
			this.TextState.RenderingMode = mode;
		}

		public void SetRise(double rise)
		{
			this.TextState.Rise = rise;
		}

		public void MoveToNextLine(double tx, double ty)
		{
			Matrix matrix = new Matrix
			{
				M11 = 1.0,
				M12 = 0.0,
				OffsetX = tx,
				M21 = 0.0,
				M22 = 1.0,
				OffsetY = ty
			};
			this.TextState.TextMatrix = (this.TextState.TextLineMatrix = matrix * this.TextState.TextLineMatrix);
		}

		public void MoveToNextLineWithTextLeading(double tx, double ty)
		{
			this.SetTextLeading(-ty);
			this.MoveToNextLine(tx, ty);
		}

		public void SetTextMatrix(double a, double b, double c, double d, double e, double f)
		{
			this.TextState.TextMatrix = (this.TextState.TextLineMatrix = new Matrix
			{
				M11 = a,
				M12 = b,
				M21 = c,
				M22 = d,
				OffsetX = e,
				OffsetY = f
			});
		}

		public void MoveToNextLineWithCurrentTextLeading()
		{
			this.MoveToNextLine(0.0, this.TextState.Leading);
		}

		public void BeginText()
		{
			if (this.TextState.IsInTextMode)
			{
				throw new InvalidOperationException("Already in text mode.");
			}
			this.TextState.IsInTextMode = true;
			this.TextState.TextMatrix = (this.TextState.TextLineMatrix = Matrix.Identity);
		}

		public void EndText()
		{
			this.TextState.IsInTextMode = false;
		}

		public GlyphsLayoutBox DrawText(PdfStringOld text)
		{
			Guard.ThrowExceptionIfNull<PdfStringOld>(text, "text");
			FontBaseOld font = this.TextState.Font;
			GlyphsLayoutBox glyphsLayoutBox = new GlyphsLayoutBox();
			glyphsLayoutBox.Glyphs.AddRange(font.RenderGlyphs(this, text));
			return glyphsLayoutBox;
		}

		public GlyphsLayoutBox MoveToNextLineAndDrawText(PdfStringOld text)
		{
			this.MoveToNextLineWithCurrentTextLeading();
			return this.DrawText(text);
		}

		public GlyphsLayoutBox MoveToNextLineAndDrawText(PdfStringOld str, double w, double c)
		{
			this.SetWordSpacing(w);
			this.SetCharSpacing(c);
			return this.MoveToNextLineAndDrawText(str);
		}

		public GlyphsLayoutBox DrawText(PdfArrayOld arr)
		{
			Guard.ThrowExceptionIfNull<PdfArrayOld>(arr, "arr");
			FontBaseOld font = this.TextState.Font;
			GlyphsLayoutBox glyphsLayoutBox = new GlyphsLayoutBox();
			for (int i = 0; i < arr.Count; i++)
			{
				PdfStringOld pdfStringOld = arr[i] as PdfStringOld;
				if (pdfStringOld != null)
				{
					glyphsLayoutBox.Glyphs.AddRange(font.RenderGlyphs(this, pdfStringOld));
				}
				else
				{
					double tj;
					arr.TryGetReal(i, out tj);
					this.UpdateTextMatrix(tj);
				}
			}
			return glyphsLayoutBox;
		}

		public Matrix GetTextRenderingMatrix()
		{
			return new Matrix
			{
				M11 = this.TextState.FontSize * (this.TextState.HorizontalScaling / 100.0),
				M22 = -this.TextState.FontSize,
				OffsetY = this.TextState.FontSize + this.TextState.Rise
			} * this.TextState.TextMatrix * this.GraphicsState.Ctm;
		}

		public void UpdateTextMatrix(GlyphOld glyph)
		{
			this.TextState.TextMatrix = Helper.CalculateTextMatrix(this.TextState.TextMatrix, glyph);
		}

		public Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement ApplyShading(ShadingOld shading)
		{
			return new Path
			{
				Fill = shading.CreateBrush(this.GraphicsState.Ctm, null),
				Data = this.GraphicsState.ClippingContainer.Clip.Clone()
			};
		}

		static void Fill(PathGeometry geometry, FillRule rule)
		{
			foreach (PathFigure pathFigure in geometry.Figures)
			{
				pathFigure.IsClosed = true;
				pathFigure.IsFilled = true;
			}
			geometry.FillRule = rule;
		}

		void UpdateTextMatrix(double tj)
		{
			double x = tj * 0.001 * this.TextState.FontSize * this.TextState.HorizontalScaling / 100.0;
			Point point = this.TextState.TextMatrix.Transform(new Point(0.0, 0.0));
			Point point2 = this.TextState.TextMatrix.Transform(new Point(x, 0.0));
			this.TextState.TextMatrix = this.TextState.TextMatrix.Translate(-(point2.X - point.X), -(point2.Y - point.Y));
		}

		Path CreatePath()
		{
			return new Path
			{
				Data = this.CurrentPathGeometry,
				TransformMatrix = this.GraphicsState.Ctm
			};
		}

		void Fill(Path path, FillRule rule)
		{
			PdfContext.Fill(path.Data, rule);
			path.Fill = this.GraphicsState.GetBrushWithAlpha();
		}

		void Stroke(Path path)
		{
			path.StrokeThickness = this.GraphicsState.LineWidth;
			path.StrokeStartLineCap = this.GraphicsState.GetLineCap();
			path.StrokeEndLineCap = this.GraphicsState.GetLineCap();
			path.StrokeLineJoin = this.GraphicsState.GetLineJoin();
			path.StrokeMiterLimit = this.GraphicsState.MiterLimit;
			path.StrokeDashOffset = this.GraphicsState.DashOffset;
			path.StrokeDashArray = from pat in this.GraphicsState.DashPattern
				select pat / path.StrokeThickness;
			path.Stroke = this.GraphicsState.GetStrokeBrushWithAlpha();
		}

		const double Epsylon = 1E-05;

		readonly Stack<GraphicsStateOld> graphicsStateStack;

		readonly Stack<TextStateOld> textStateStack;

		readonly PdfContentManager contentManager;
	}
}
