using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public class Path : PositionContentElement
	{
		public Path()
		{
			this.geometryProperties = new GeometryPropertiesOwner();
		}

		Path(Path other)
		{
			this.geometryProperties = new GeometryPropertiesOwner(other.geometryProperties);
			this.Geometry = other.Geometry;
		}

		public GeometryBase Geometry { get; set; }

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

		public bool IsFilled
		{
			get
			{
				return this.GeometryProperties.IsFilled;
			}
			set
			{
				this.GeometryProperties.IsFilled = value;
			}
		}

		public bool IsStroked
		{
			get
			{
				return this.GeometryProperties.IsStroked;
			}
			set
			{
				this.GeometryProperties.IsStroked = value;
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

		internal GeometryPropertiesOwner GeometryProperties
		{
			get
			{
				return this.geometryProperties;
			}
		}

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.Path;
			}
		}

		internal override PositionContentElement CreateClonedInstance()
		{
			return new Path(this);
		}

		readonly GeometryPropertiesOwner geometryProperties;
	}
}
