using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeOutlines;
using Telerik.Windows.Documents.Core.Fonts.Utils;
using Telerik.Windows.Documents.Core.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType
{
	class OpenTypeGlyphScaler
	{
		public OpenTypeGlyphScaler(OpenTypeFontSourceBase fontFile)
		{
			Guard.ThrowExceptionIfNull<OpenTypeFontSourceBase>(fontFile, "fontFile");
			this.fontFile = fontFile;
		}

		public double GetAdvancedWidth(ushort glyphId, double fontSize)
		{
			return this.FUnitsToPixels((int)this.fontFile.HMtx.GetAdvancedWidth((int)glyphId), fontSize);
		}

		public double FUnitsToPixels(int units, double fontSize)
		{
			return this.FUnitsToPixels((double)units, fontSize);
		}

		public double FUnitsToPixels(double units, double fontSize)
		{
			return units * 72.0 * fontSize / (72.0 * (double)this.fontFile.Head.UnitsPerEm);
		}

		public Point FUnitsOutlinePointToPixels(Point unitPoint, double fontSize)
		{
			double x = this.FUnitsToPixels(unitPoint.X, fontSize);
			double num = this.FUnitsToPixels(unitPoint.Y, fontSize);
			return new Point(x, -num);
		}

		public Point FUnitsPointToPixels(Point unitPoint, double fontSize)
		{
			double x = this.FUnitsToPixels(unitPoint.X, fontSize);
			double y = this.FUnitsToPixels(unitPoint.Y, fontSize);
			return new Point(x, y);
		}

		public void GetScaleGlyphOutlines(Glyph glyph, double fontSize)
		{
			Guard.ThrowExceptionIfNull<Glyph>(glyph, "glyph");
			switch (this.fontFile.Outlines)
			{
			case Outlines.TrueType:
				this.CreateTrueTypeOutlines(glyph, fontSize);
				return;
			case Outlines.OpenType:
				this.CreateOpenTypeOutlines(glyph, fontSize);
				return;
			default:
				return;
			}
		}

		static Point GetMidPoint(Point a, Point b)
		{
			return new Point((a.X + b.X) / 2.0, (a.Y + b.Y) / 2.0);
		}

		static LineSegment CreateLineSegment(Point point)
		{
			return new LineSegment
			{
				Point = point
			};
		}

		static QuadraticBezierSegment CreateBezierSegment(Point control, Point end)
		{
			return new QuadraticBezierSegment
			{
				Point1 = control,
				Point2 = end
			};
		}

		PathFigure CreatePathFigureFromContour(OutlinePoint[] points, double fontSize)
		{
			List<OutlinePoint> finalPoints = OpenTypeGlyphScaler.GetFinalGlyphOutlinePoints(points);
			PathFigure pathFigure = new PathFigure();
			int num = (finalPoints[0].IsOnCurve ? 0 : 1);
			pathFigure.StartPoint = this.FUnitsOutlinePointToPixels(finalPoints[num].Point, fontSize);
			Func<int, int> func = (int index) => (index + 1) % finalPoints.Count;
			int num2 = num;
			do
			{
				int num3 = func(num2);
				OutlinePoint outlinePoint = finalPoints[num3];
				if (outlinePoint.IsOnCurve)
				{
					pathFigure.Segments.Add(OpenTypeGlyphScaler.CreateLineSegment(this.FUnitsOutlinePointToPixels(outlinePoint.Point, fontSize)));
					num2 = num3;
				}
				else
				{
					int num4 = func(num3);
					OutlinePoint outlinePoint2 = finalPoints[num4];
					pathFigure.Segments.Add(OpenTypeGlyphScaler.CreateBezierSegment(this.FUnitsOutlinePointToPixels(outlinePoint.Point, fontSize), this.FUnitsOutlinePointToPixels(outlinePoint2.Point, fontSize)));
					num2 = num4;
				}
			}
			while (num2 != num);
			pathFigure.IsClosed = true;
			pathFigure.IsFilled = true;
			return pathFigure;
		}

		static List<OutlinePoint> GetFinalGlyphOutlinePoints(OutlinePoint[] points)
		{
			List<OutlinePoint> list = new List<OutlinePoint>();
			for (int i = 0; i < points.Length; i++)
			{
				OutlinePoint outlinePoint = points[i];
				OutlinePoint outlinePoint2 = points[(i + 1) % points.Length];
				list.Add(outlinePoint);
				if (!outlinePoint.IsOnCurve && !outlinePoint2.IsOnCurve)
				{
					list.Add(new OutlinePoint(1)
					{
						Point = OpenTypeGlyphScaler.GetMidPoint(outlinePoint.Point, outlinePoint2.Point)
					});
				}
			}
			return list;
		}

		void CreateOpenTypeOutlines(Glyph glyph, double fontSize)
		{
			Guard.ThrowExceptionIfNull<Glyph>(glyph, "glyph");
			this.fontFile.CFF.InitializeGlyphOutlines(glyph, fontSize);
		}

		void CreateTrueTypeOutlines(Glyph glyph, double fontSize)
		{
			Guard.ThrowExceptionIfNull<Glyph>(glyph, "glyph");
			glyph.Outlines = new GlyphOutlinesCollection();
			GlyphData glyphData = this.fontFile.GetGlyphData(glyph.GlyphId);
			foreach (OutlinePoint[] points in glyphData.Contours)
			{
				glyph.Outlines.Add(this.CreatePathFigureFromContour(points, fontSize));
			}
		}

		internal const double Dpi = 72.0;

		const double Ppi = 72.0;

		readonly OpenTypeFontSourceBase fontFile;
	}
}
