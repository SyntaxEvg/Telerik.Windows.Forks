using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Core.Shapes;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	class GraphicsStateOld
	{
		public GraphicsStateOld(Rect clip)
		{
			this.Brush = new SolidColorBrush(GraphicsStateOld.DefaultColor);
			this.StrokeBrush = new SolidColorBrush(GraphicsStateOld.DefaultColor);
			this.ClippingContainer = new Container();
			this.ClippingContainer.Clip = this.CreateGeometryFromRect(clip);
			this.LineWidth = 1.0;
			this.Ctm = Matrix.Identity;
			this.LineCap = 0;
			this.LineJoin = 0;
			this.MiterLimit = 1.0;
			this.DashPattern = new double[0];
			this.DashOffset = 0.0;
			this.StrokeAlphaConstant = 1.0;
			this.AlphaConstant = 1.0;
		}

		public GraphicsStateOld(GraphicsStateOld other)
		{
			this.AlphaConstant = other.AlphaConstant;
			this.AlphaSource = other.AlphaSource;
			this.BlackGeneration = other.BlackGeneration;
			this.BlendMode = other.BlendMode;
			this.Ctm = other.Ctm;
			this.Brush = other.Brush;
			this.ColorSpace = other.ColorSpace;
			this.StrokeBrush = other.StrokeBrush;
			this.StrokeColorSpace = other.StrokeColorSpace;
			this.ClippingContainer = other.ClippingContainer;
			this.DashOffset = other.DashOffset;
			this.DashPattern = other.DashPattern;
			this.Flatness = other.Flatness;
			this.Halftone = other.Halftone;
			this.LineCap = other.LineCap;
			this.LineJoin = other.LineJoin;
			this.LineWidth = other.LineWidth;
			this.MiterLimit = other.MiterLimit;
			this.Overprint = other.Overprint;
			this.OverprintMode = other.OverprintMode;
			this.RenderingIntent = other.RenderingIntent;
			this.Smoothness = other.Smoothness;
			this.SoftMask = other.SoftMask;
			this.StrokeAdjustment = other.StrokeAdjustment;
			this.StrokeAlphaConstant = other.StrokeAlphaConstant;
			this.StrokeOverprint = other.StrokeOverprint;
			this.Transfer = other.Transfer;
			this.UndercolorRemoval = other.UndercolorRemoval;
		}

		public Matrix Ctm { get; set; }

		public Container ClippingContainer { get; set; }

		public ColorSpaceOld ColorSpace { get; set; }

		public Brush Brush { get; set; }

		public ColorSpaceOld StrokeColorSpace { get; set; }

		public Brush StrokeBrush { get; set; }

		public double LineWidth { get; set; }

		public int LineCap { get; set; }

		public int LineJoin { get; set; }

		public double MiterLimit { get; set; }

		public double[] DashPattern { get; set; }

		public double DashOffset { get; set; }

		public string RenderingIntent { get; set; }

		public bool StrokeAdjustment { get; set; }

		public object BlendMode { get; set; }

		public object SoftMask { get; set; }

		public double StrokeAlphaConstant { get; set; }

		public double AlphaConstant { get; set; }

		public bool AlphaSource { get; set; }

		public bool Overprint { get; set; }

		public bool StrokeOverprint { get; set; }

		public int OverprintMode { get; set; }

		public object BlackGeneration { get; set; }

		public object UndercolorRemoval { get; set; }

		public object Transfer { get; set; }

		public object Halftone { get; set; }

		public double Flatness
		{
			get
			{
				return this.flatness;
			}
			set
			{
				if (this.flatness != value)
				{
					if (value < 0.0)
					{
						this.flatness = 0.0;
						return;
					}
					if (value > 100.0)
					{
						this.flatness = 100.0;
						return;
					}
					this.flatness = value;
				}
			}
		}

		public double Smoothness { get; set; }

		public PenLineCap GetLineCap()
		{
			return (PenLineCap)this.LineCap;
		}

		public PenLineJoin GetLineJoin()
		{
			return (PenLineJoin)this.LineJoin;
		}

		public void SetProps(ExtGStateOld props)
		{
			if (props.LineWidth != null)
			{
				this.LineWidth = props.LineWidth.Value;
			}
			if (props.LineCap != null)
			{
				this.LineCap = props.LineCap.Value;
			}
			if (props.LineJoin != null)
			{
				this.LineJoin = props.LineJoin.Value;
			}
			if (props.MitterLimit != null)
			{
				this.MiterLimit = props.MitterLimit.Value;
			}
			if (props.DashPattern != null)
			{
				this.DashPattern = props.DashPattern.ToDashPattern();
			}
			if (props.RenderingIndent != null)
			{
				this.RenderingIntent = props.RenderingIndent.Value;
			}
			if (props.OverprintAll != null)
			{
				this.Overprint = props.OverprintAll.Value;
				this.StrokeOverprint = props.OverprintAll.Value;
			}
			if (props.Overprint != null)
			{
				this.Overprint = props.Overprint.Value;
			}
			if (props.OverprintMode != null)
			{
				this.OverprintMode = props.OverprintMode.Value;
			}
			if (props.StrokeAlphaConstant != null)
			{
				this.StrokeAlphaConstant = props.StrokeAlphaConstant.Value;
			}
			if (props.AlphaConstant != null)
			{
				this.AlphaConstant = props.AlphaConstant.Value;
			}
			if (props.AlphaSource != null)
			{
				this.AlphaSource = props.AlphaSource.Value;
			}
		}

		public Brush GetBrushWithAlpha()
		{
			byte alpha = Color.ConvertColorComponentToByte(this.AlphaConstant);
			return BrushFactory.GetBrushWithDifferentAlpha(this.Brush, alpha);
		}

		public Brush GetStrokeBrushWithAlpha()
		{
			byte alpha = Color.ConvertColorComponentToByte(this.StrokeAlphaConstant);
			return BrushFactory.GetBrushWithDifferentAlpha(this.StrokeBrush, alpha);
		}

		PathGeometry CreateGeometryFromRect(Rect rect)
		{
			PathGeometry pathGeometry = new PathGeometry();
			PathFigure pathFigure = new PathFigure();
			pathFigure.IsClosed = true;
			pathFigure.IsFilled = true;
			pathFigure.StartPoint = new Point(rect.Left, rect.Top);
			pathFigure.Segments.Add(new LineSegment
			{
				Point = new Point(rect.Right, rect.Top)
			});
			pathFigure.Segments.Add(new LineSegment
			{
				Point = new Point(rect.Right, rect.Bottom)
			});
			pathFigure.Segments.Add(new LineSegment
			{
				Point = new Point(rect.Left, rect.Bottom)
			});
			pathGeometry.Figures.Add(pathFigure);
			pathGeometry.TransformMatrix = Matrix.Identity;
			return pathGeometry;
		}

		public static readonly Color DefaultColor = Color.Black;

		double flatness;
	}
}
