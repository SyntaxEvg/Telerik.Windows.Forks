using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.Model.Text
{
	public class TextFragment : PositionContentElement
	{
		public TextFragment()
		{
			this.textPropertiesOwner = new TextPropertiesOwner();
		}

		public TextFragment(string text)
			: this()
		{
			this.Text = text;
		}

		internal TextFragment(TextFragment other, string text)
			: this(other)
		{
			this.Text = text;
		}

		internal TextFragment(TextFragment other)
			: this(other.TextProperties)
		{
			base.Clipping = other.Clipping;
			base.Position = other.Position;
		}

		internal TextFragment(TextPropertiesOwner otherProperties)
		{
			this.textPropertiesOwner = new TextPropertiesOwner(otherProperties);
		}

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

		public string Text
		{
			get
			{
				return this.TextCollection.Text;
			}
			set
			{
				this.TextCollection.Text = value;
			}
		}

		internal TextCollection TextCollection
		{
			get
			{
				return this.textPropertiesOwner.TextCollection;
			}
		}

		internal Matrix TextMatrix
		{
			get
			{
				return this.textPropertiesOwner.TextMatrix;
			}
			set
			{
				this.textPropertiesOwner.TextMatrix = value;
			}
		}

		internal TextPropertiesOwner TextProperties
		{
			get
			{
				return this.textPropertiesOwner;
			}
		}

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.TextFragment;
			}
		}

		public override string ToString()
		{
			return this.Text;
		}

		internal override PositionContentElement CreateClonedInstance()
		{
			return new TextFragment(this);
		}

		readonly TextPropertiesOwner textPropertiesOwner;
	}
}
