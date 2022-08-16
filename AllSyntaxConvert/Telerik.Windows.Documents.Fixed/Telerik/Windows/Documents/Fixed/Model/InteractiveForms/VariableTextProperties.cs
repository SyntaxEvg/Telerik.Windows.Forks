using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public class VariableTextProperties
	{
		public VariableTextProperties()
		{
			this.textPropertiesOwner = new TextPropertiesOwner();
			this.HorizontalAlignment = HorizontalAlignment.Left;
		}

		public VariableTextProperties(VariableTextProperties other)
		{
			this.textPropertiesOwner = new TextPropertiesOwner(other.textPropertiesOwner);
			this.HorizontalAlignment = other.HorizontalAlignment;
		}

		public HorizontalAlignment HorizontalAlignment { get; set; }

		public double? CharacterSpacing
		{
			get
			{
				return this.textPropertiesOwner.CharacterSpacing;
			}
			set
			{
				this.textPropertiesOwner.CharacterSpacing = value;
			}
		}

		public double? WordSpacing
		{
			get
			{
				return this.textPropertiesOwner.WordSpacing;
			}
			set
			{
				this.textPropertiesOwner.WordSpacing = value;
			}
		}

		public double? HorizontalScaling
		{
			get
			{
				return this.textPropertiesOwner.HorizontalScaling;
			}
			set
			{
				this.textPropertiesOwner.HorizontalScaling = value;
			}
		}

		public FontBase Font
		{
			get
			{
				return this.textPropertiesOwner.Font;
			}
			set
			{
				this.textPropertiesOwner.Font = value;
			}
		}

		public double FontSize
		{
			get
			{
				return this.textPropertiesOwner.FontSize;
			}
			set
			{
				this.textPropertiesOwner.FontSize = value;
			}
		}

		public double? TextRise
		{
			get
			{
				return this.textPropertiesOwner.TextRise;
			}
			set
			{
				this.textPropertiesOwner.TextRise = value;
			}
		}

		public RenderingMode RenderingMode
		{
			get
			{
				return this.textPropertiesOwner.RenderingMode;
			}
			set
			{
				this.textPropertiesOwner.RenderingMode = value;
			}
		}

		public ColorBase Fill
		{
			get
			{
				return this.textPropertiesOwner.Fill;
			}
			set
			{
				this.textPropertiesOwner.Fill = value;
			}
		}

		public ColorBase Stroke
		{
			get
			{
				return this.textPropertiesOwner.Stroke;
			}
			set
			{
				this.textPropertiesOwner.Stroke = value;
			}
		}

		public double StrokeThickness
		{
			get
			{
				return this.textPropertiesOwner.StrokeThickness;
			}
			set
			{
				this.textPropertiesOwner.StrokeThickness = value;
			}
		}

		public LineCap StrokeLineCap
		{
			get
			{
				return this.textPropertiesOwner.StrokeLineCap;
			}
			set
			{
				this.textPropertiesOwner.StrokeLineCap = value;
			}
		}

		public LineJoin StrokeLineJoin
		{
			get
			{
				return this.textPropertiesOwner.StrokeLineJoin;
			}
			set
			{
				this.textPropertiesOwner.StrokeLineJoin = value;
			}
		}

		public IEnumerable<double> StrokeDashArray
		{
			get
			{
				return this.textPropertiesOwner.StrokeDashArray;
			}
			set
			{
				this.textPropertiesOwner.StrokeDashArray = value;
			}
		}

		public double StrokeDashOffset
		{
			get
			{
				return this.textPropertiesOwner.StrokeDashOffset;
			}
			set
			{
				this.textPropertiesOwner.StrokeDashOffset = value;
			}
		}

		public double? MiterLimit
		{
			get
			{
				return this.textPropertiesOwner.MiterLimit;
			}
			set
			{
				this.textPropertiesOwner.MiterLimit = value;
			}
		}

		internal TextPropertiesOwner PropertiesOwner
		{
			get
			{
				return this.textPropertiesOwner;
			}
		}

		internal void CopyTo(Block block)
		{
			block.GraphicProperties.CopyFrom(this.PropertiesOwner.GeometryProperties);
			block.TextProperties.CopyFrom(this.PropertiesOwner);
			if (this.FontSize == 0.0)
			{
				block.TextProperties.FontSize = FixedDocumentDefaults.FontSize;
			}
			else
			{
				block.TextProperties.FontSize = Unit.PointToDip(this.FontSize);
			}
			block.HorizontalAlignment = this.HorizontalAlignment;
		}

		readonly TextPropertiesOwner textPropertiesOwner;
	}
}
