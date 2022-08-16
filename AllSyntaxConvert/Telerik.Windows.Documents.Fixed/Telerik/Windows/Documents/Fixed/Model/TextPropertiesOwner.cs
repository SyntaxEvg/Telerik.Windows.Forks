using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.Model
{
	class TextPropertiesOwner
	{
		public TextPropertiesOwner()
		{
			this.textCollection = new TextCollection();
			this.geometryProperties = new GeometryPropertiesOwner();
			this.RenderingMode = FixedDocumentDefaults.TextRenderingMode;
			this.FontSize = FixedDocumentDefaults.FontSize;
			this.Font = FixedDocumentDefaults.Font;
			this.TextMatrix = Matrix.Identity;
		}

		public TextPropertiesOwner(TextPropertiesOwner other)
		{
			this.textCollection = new TextCollection(other.TextCollection);
			this.geometryProperties = new GeometryPropertiesOwner(other.GeometryProperties);
			this.CharacterSpacing = other.CharacterSpacing;
			this.WordSpacing = other.WordSpacing;
			this.HorizontalScaling = other.HorizontalScaling;
			this.TextRise = other.TextRise;
			this.FontSize = other.FontSize;
			this.RenderingMode = other.RenderingMode;
			this.Font = other.Font;
			this.TextMatrix = other.TextMatrix;
		}

		public double? CharacterSpacing { get; set; }

		public double? WordSpacing { get; set; }

		public double? HorizontalScaling { get; set; }

		public double? TextRise { get; set; }

		public double FontSize { get; set; }

		public RenderingMode RenderingMode
		{
			get
			{
				return this.renderingMode;
			}
			set
			{
				this.renderingMode = value;
				this.UpdateIsFilled(value);
				this.UpdateIsStroked(value);
			}
		}

		public FontBase Font
		{
			get
			{
				return this.TextCollection.Font;
			}
			set
			{
				this.TextCollection.Font = value;
			}
		}

		public ColorBase Fill
		{
			get
			{
				return this.GeometryProperties.Fill;
			}
			set
			{
				this.GeometryProperties.Fill = value;
			}
		}

		public ColorBase Stroke
		{
			get
			{
				return this.GeometryProperties.Stroke;
			}
			set
			{
				this.GeometryProperties.Stroke = value;
			}
		}

		public double StrokeThickness
		{
			get
			{
				return this.GeometryProperties.StrokeThickness;
			}
			set
			{
				this.GeometryProperties.StrokeThickness = value;
			}
		}

		public LineCap StrokeLineCap
		{
			get
			{
				return this.GeometryProperties.StrokeLineCap;
			}
			set
			{
				this.GeometryProperties.StrokeLineCap = value;
			}
		}

		public LineJoin StrokeLineJoin
		{
			get
			{
				return this.GeometryProperties.StrokeLineJoin;
			}
			set
			{
				this.GeometryProperties.StrokeLineJoin = value;
			}
		}

		public IEnumerable<double> StrokeDashArray
		{
			get
			{
				return this.GeometryProperties.StrokeDashArray;
			}
			set
			{
				this.GeometryProperties.StrokeDashArray = value;
			}
		}

		public double StrokeDashOffset
		{
			get
			{
				return this.GeometryProperties.StrokeDashOffset;
			}
			set
			{
				this.GeometryProperties.StrokeDashOffset = value;
			}
		}

		public double? MiterLimit
		{
			get
			{
				return this.GeometryProperties.MiterLimit;
			}
			set
			{
				this.GeometryProperties.MiterLimit = value;
			}
		}

		internal Matrix TextMatrix { get; set; }

		internal GeometryPropertiesOwner GeometryProperties
		{
			get
			{
				return this.geometryProperties;
			}
		}

		internal TextCollection TextCollection
		{
			get
			{
				return this.textCollection;
			}
		}

		void UpdateIsFilled(RenderingMode renderingMode)
		{
			this.GeometryProperties.IsFilled = renderingMode == RenderingMode.Fill || renderingMode == RenderingMode.FillAndAddToClippingPath || renderingMode == RenderingMode.FillAndStroke || renderingMode == RenderingMode.FillStrokeAndAddToClippingPath;
		}

		void UpdateIsStroked(RenderingMode renderingMode)
		{
			this.GeometryProperties.IsStroked = renderingMode == RenderingMode.Stroke || renderingMode == RenderingMode.StrokeAndAddToClippingPath || renderingMode == RenderingMode.FillAndStroke || renderingMode == RenderingMode.FillStrokeAndAddToClippingPath;
		}

		readonly TextCollection textCollection;

		readonly GeometryPropertiesOwner geometryProperties;

		RenderingMode renderingMode;
	}
}
