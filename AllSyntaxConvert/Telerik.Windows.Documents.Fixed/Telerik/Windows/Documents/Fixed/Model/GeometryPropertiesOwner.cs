using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.Model
{
	class GeometryPropertiesOwner
	{
		public GeometryPropertiesOwner()
		{
			this.Fill = FixedDocumentDefaults.Color;
			this.Stroke = FixedDocumentDefaults.Color;
			this.IsFilled = FixedDocumentDefaults.IsFilled;
			this.IsStroked = FixedDocumentDefaults.IsStroked;
			this.StrokeThickness = FixedDocumentDefaults.StrokeThickness;
			this.StrokeLineCap = FixedDocumentDefaults.StrokeLineCap;
			this.StrokeLineJoin = FixedDocumentDefaults.StrokeLineJoin;
			this.StrokeDashArray = FixedDocumentDefaults.StrokeDashArray;
			this.StrokeDashOffset = FixedDocumentDefaults.StrokeDashOffset;
			this.MiterLimit = FixedDocumentDefaults.MiterLimit;
		}

		public GeometryPropertiesOwner(GeometryPropertiesOwner other)
		{
			this.Fill = other.Fill;
			this.Stroke = other.Stroke;
			this.IsFilled = other.IsFilled;
			this.IsStroked = other.IsStroked;
			this.StrokeThickness = other.StrokeThickness;
			this.StrokeLineCap = other.StrokeLineCap;
			this.StrokeLineJoin = other.StrokeLineJoin;
			this.StrokeDashArray = other.StrokeDashArray;
			this.StrokeDashOffset = other.StrokeDashOffset;
			this.MiterLimit = other.MiterLimit;
		}

		public bool IsFilled { get; set; }

		public bool IsStroked { get; set; }

		public ColorBase Fill { get; set; }

		public ColorBase Stroke { get; set; }

		public double StrokeThickness { get; set; }

		public LineCap StrokeLineCap { get; set; }

		public LineJoin StrokeLineJoin { get; set; }

		public IEnumerable<double> StrokeDashArray { get; set; }

		public double StrokeDashOffset { get; set; }

		public double? MiterLimit { get; set; }
	}
}
