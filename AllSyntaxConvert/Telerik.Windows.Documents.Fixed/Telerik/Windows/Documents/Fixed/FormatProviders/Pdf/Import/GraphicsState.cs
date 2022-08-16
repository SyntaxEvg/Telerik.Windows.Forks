using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class GraphicsState
	{
		public GraphicsState()
		{
			this.FillColor = FixedDocumentDefaults.DefaultFormatProviderColorspace.DefaultColor;
			this.StrokeColor = FixedDocumentDefaults.DefaultFormatProviderColorspace.DefaultColor;
			this.StrokeLineWidth = FixedDocumentDefaults.StrokeThickness;
			this.Matrix = default(Matrix);
		}

		public GraphicsState(GraphicsState other)
		{
			Guard.ThrowExceptionIfNull<GraphicsState>(other, "other");
			this.fillColorSpace = other.fillColorSpace;
			this.strokeColorSpace = other.strokeColorSpace;
			this.alphaIsShape = other.alphaIsShape;
			this.FillColor = other.FillColor;
			this.StrokeColor = other.StrokeColor;
			this.StrokeLineWidth = other.StrokeLineWidth;
			this.StrokeLineCap = other.StrokeLineCap;
			this.StrokeLineJoin = other.StrokeLineJoin;
			this.Clipping = other.Clipping;
			this.StrokeDashArray = other.StrokeDashArray;
			this.StrokeDashOffset = other.StrokeDashOffset;
			this.MiterLimit = other.MiterLimit;
			this.Matrix = other.Matrix;
			this.AlphaConstant = other.AlphaConstant;
			this.StrokeAlphaConstant = other.StrokeAlphaConstant;
		}

		public double? StrokeAlphaConstant { get; set; }

		public double? AlphaConstant { get; set; }

		public Clipping Clipping { get; set; }

		public double? MiterLimit { get; set; }

		public IEnumerable<double> StrokeDashArray { get; set; }

		public double StrokeDashOffset { get; set; }

		public ColorObjectBase FillColor { get; set; }

		public ColorObjectBase StrokeColor { get; set; }

		public double StrokeLineWidth { get; set; }

		public LineJoin StrokeLineJoin { get; set; }

		public LineCap StrokeLineCap { get; set; }

		public Matrix Matrix { get; set; }

		public bool TryGetStrokeColorSpace(out ColorSpaceObject colorSpace)
		{
			colorSpace = this.strokeColorSpace;
			return colorSpace != null;
		}

		public bool TryGetFillColorSpace(out ColorSpaceObject colorSpace)
		{
			colorSpace = this.fillColorSpace;
			return colorSpace != null;
		}

		public void SetFillColorSpace(ColorSpaceObject colorSpace)
		{
			Guard.ThrowExceptionIfNull<ColorSpaceObject>(colorSpace, "value");
			this.fillColorSpace = colorSpace;
			this.FillColor = colorSpace.DefaultColor;
		}

		public void SetStrokeColorSpace(ColorSpaceObject colorSpace)
		{
			Guard.ThrowExceptionIfNull<ColorSpaceObject>(colorSpace, "value");
			this.strokeColorSpace = colorSpace;
			this.StrokeColor = colorSpace.DefaultColor;
		}

		public void UpdateProperties(ExtGState state)
		{
			if (state.StrokeAlphaConstant != null)
			{
				this.StrokeAlphaConstant = state.StrokeAlphaConstant;
			}
			if (state.AlphaConstant != null)
			{
				this.AlphaConstant = state.AlphaConstant;
			}
			if (state.AlphaSource != null)
			{
				this.alphaIsShape = state.AlphaSource.Value;
			}
		}

		public ColorBase CalculateFillColor(PostScriptReader reader, IPdfContentImportContext context)
		{
			RgbColorObject rgbColorObject = this.FillColor as RgbColorObject;
			if (rgbColorObject != null)
			{
				RgbColor rgbColor = rgbColorObject.ToColor(reader, context) as RgbColor;
				rgbColor.A = this.GetAlphaComponent();
				return rgbColor;
			}
			return this.FillColor.ToColor(reader, context);
		}

		public ColorBase CalculateStrokeColor(PostScriptReader reader, IPdfContentImportContext context)
		{
			RgbColorObject rgbColorObject = this.StrokeColor as RgbColorObject;
			if (rgbColorObject != null)
			{
				RgbColor rgbColor = rgbColorObject.ToColor(reader, context) as RgbColor;
				rgbColor.A = this.GetStrokeAlphaComponent();
				return rgbColor;
			}
			return this.StrokeColor.ToColor(reader, context);
		}

		public ColorBase CalculateStencilColor(PostScriptReader reader, IPdfContentImportContext context)
		{
			ColorBase colorBase = this.CalculateFillColor(reader, context);
			if (colorBase is SimpleColor)
			{
				return colorBase;
			}
			return RgbColors.Black;
		}

		public byte GetAlphaComponent()
		{
			if (this.AlphaConstant != null && !this.alphaIsShape)
			{
				return GraphicsState.GetComponentByteValue(this.AlphaConstant.Value);
			}
			return FixedDocumentDefaults.DefaultAlphaComponent;
		}

		public byte GetStrokeAlphaComponent()
		{
			if (this.StrokeAlphaConstant != null && !this.alphaIsShape)
			{
				return GraphicsState.GetComponentByteValue(this.StrokeAlphaConstant.Value);
			}
			return FixedDocumentDefaults.DefaultAlphaComponent;
		}

		public void ConcatenateMatrix(Matrix matrix)
		{
			this.Matrix = matrix.MultiplyBy(this.Matrix);
		}

		public void IntersectClipping(Clipping clipping)
		{
			if (this.Clipping != null)
			{
				clipping.Clipping = this.Clipping;
			}
			this.Clipping = clipping;
		}

		static byte GetComponentByteValue(double value)
		{
			return (byte)(value * 255.0);
		}

		ColorSpaceObject fillColorSpace;

		ColorSpaceObject strokeColorSpace;

		bool alphaIsShape;
	}
}
