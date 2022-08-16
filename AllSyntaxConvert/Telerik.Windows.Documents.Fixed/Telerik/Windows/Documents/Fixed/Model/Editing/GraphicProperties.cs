using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	public class GraphicProperties : PropertiesBase<GraphicProperties>
	{
		public GraphicProperties()
		{
			this.FillColor = FixedDocumentDefaults.Color;
			this.StrokeColor = FixedDocumentDefaults.Color;
			this.StrokeThickness = FixedDocumentDefaults.StrokeThickness;
			this.IsFilled = true;
			this.IsStroked = true;
		}

		public bool IsFilled { get; set; }

		public bool IsStroked { get; set; }

		public ColorBase FillColor { get; set; }

		public ColorBase StrokeColor { get; set; }

		public double StrokeThickness { get; set; }

		public double? MiterLimit { get; set; }

		public double StrokeDashOffset { get; set; }

		public IEnumerable<double> StrokeDashArray { get; set; }

		public LineJoin StrokeLineJoin { get; set; }

		public LineCap StrokeLineCap { get; set; }

		public override void CopyFrom(GraphicProperties graphicProperties)
		{
			this.FillColor = graphicProperties.FillColor;
			this.StrokeColor = graphicProperties.StrokeColor;
			this.StrokeThickness = graphicProperties.StrokeThickness;
			this.MiterLimit = graphicProperties.MiterLimit;
			this.StrokeDashOffset = graphicProperties.StrokeDashOffset;
			this.StrokeDashArray = graphicProperties.StrokeDashArray;
			this.StrokeLineJoin = graphicProperties.StrokeLineJoin;
			this.StrokeLineCap = graphicProperties.StrokeLineCap;
			this.IsFilled = graphicProperties.IsFilled;
			this.IsStroked = graphicProperties.IsStroked;
		}

		internal void CopyFrom(GeometryPropertiesOwner geometry)
		{
			this.FillColor = geometry.Fill;
			this.StrokeColor = geometry.Stroke;
			this.StrokeThickness = geometry.StrokeThickness;
			this.MiterLimit = geometry.MiterLimit;
			this.StrokeDashOffset = geometry.StrokeDashOffset;
			this.StrokeDashArray = geometry.StrokeDashArray;
			this.StrokeLineJoin = geometry.StrokeLineJoin;
			this.StrokeLineCap = geometry.StrokeLineCap;
			this.IsFilled = geometry.IsFilled;
			this.IsStroked = geometry.IsStroked;
		}

		internal void CopyTo(GeometryPropertiesOwner geometry)
		{
			geometry.Fill = this.FillColor;
			geometry.Stroke = this.StrokeColor;
			geometry.StrokeThickness = this.StrokeThickness;
			geometry.MiterLimit = this.MiterLimit;
			geometry.StrokeDashOffset = this.StrokeDashOffset;
			geometry.StrokeDashArray = this.StrokeDashArray;
			geometry.StrokeLineJoin = this.StrokeLineJoin;
			geometry.StrokeLineCap = this.StrokeLineCap;
			geometry.IsFilled = this.IsFilled;
			geometry.IsStroked = this.IsStroked;
		}
	}
}
