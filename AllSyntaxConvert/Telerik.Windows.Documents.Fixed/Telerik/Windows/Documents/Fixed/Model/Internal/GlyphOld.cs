using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Shapes;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class GlyphOld : Glyph, IContentElement
	{
		public GlyphOld(GlyphInfo glyphInfo)
		{
			this.TransformMatrix = Matrix.Identity;
			this.glyphInfo = glyphInfo;
		}

		public GlyphOld()
			: this(new GlyphInfo())
		{
		}

		public GlyphInfo GlyphInfo
		{
			get
			{
				return this.glyphInfo;
			}
		}

		public ResourceKey Key
		{
			get
			{
				return this.glyphInfo.Key;
			}
			set
			{
				this.glyphInfo.Key = value;
			}
		}

		public CharCodeOld CharId
		{
			get
			{
				return this.glyphInfo.CharId;
			}
			set
			{
				this.glyphInfo.CharId = value;
			}
		}

		public string ToUnicode
		{
			get
			{
				return this.glyphInfo.ToUnicode;
			}
			set
			{
				this.glyphInfo.ToUnicode = value;
			}
		}

		public string FontFamily
		{
			get
			{
				return this.glyphInfo.FontFamily;
			}
			set
			{
				this.glyphInfo.FontFamily = value;
			}
		}

		public FontStyle FontStyle
		{
			get
			{
				return this.glyphInfo.FontStyle;
			}
			set
			{
				this.glyphInfo.FontStyle = value;
			}
		}

		public FontWeight FontWeight
		{
			get
			{
				return this.glyphInfo.FontWeight;
			}
			set
			{
				this.glyphInfo.FontWeight = value;
			}
		}

		public bool IsBold
		{
			get
			{
				return this.glyphInfo.IsBold;
			}
			set
			{
				this.glyphInfo.IsBold = value;
			}
		}

		public bool IsItalic
		{
			get
			{
				return this.glyphInfo.IsItalic;
			}
			set
			{
				this.glyphInfo.IsItalic = value;
			}
		}

		public double FontSize
		{
			get
			{
				return this.glyphInfo.FontSize;
			}
			set
			{
				this.glyphInfo.FontSize = value;
			}
		}

		public double Rise
		{
			get
			{
				return this.glyphInfo.Rise;
			}
			set
			{
				this.glyphInfo.Rise = value;
			}
		}

		public double CharSpacing
		{
			get
			{
				return this.glyphInfo.CharSpacing;
			}
			set
			{
				this.glyphInfo.CharSpacing = value;
			}
		}

		public double WordSpacing
		{
			get
			{
				return this.glyphInfo.WordSpacing;
			}
			set
			{
				this.glyphInfo.WordSpacing = value;
			}
		}

		public double HorizontalScaling
		{
			get
			{
				return this.glyphInfo.HorizontalScaling;
			}
			set
			{
				this.glyphInfo.HorizontalScaling = value;
			}
		}

		public double Width
		{
			get
			{
				return this.glyphInfo.Width;
			}
			set
			{
				this.glyphInfo.Width = value;
			}
		}

		public double Ascent
		{
			get
			{
				return this.glyphInfo.Ascent;
			}
			set
			{
				this.glyphInfo.Ascent = value;
			}
		}

		public double Descent
		{
			get
			{
				return this.glyphInfo.Descent;
			}
			set
			{
				this.glyphInfo.Descent = value;
			}
		}

		public Brush Fill
		{
			get
			{
				return this.glyphInfo.Fill;
			}
			set
			{
				this.glyphInfo.Fill = value;
			}
		}

		public Brush Stroke
		{
			get
			{
				return this.glyphInfo.Stroke;
			}
			set
			{
				this.glyphInfo.Stroke = value;
			}
		}

		public bool IsFilled
		{
			get
			{
				return this.glyphInfo.IsFilled;
			}
			set
			{
				this.glyphInfo.IsFilled = value;
			}
		}

		public bool IsStroked
		{
			get
			{
				return this.glyphInfo.IsStroked;
			}
			set
			{
				this.glyphInfo.IsStroked = value;
			}
		}

		public IEnumerable<Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement> Contents { get; set; }

		public Matrix TransformMatrix { get; set; }

		public Size Size
		{
			get
			{
				return this.glyphInfo.Size;
			}
			set
			{
				this.glyphInfo.Size = value;
			}
		}

		public Rect BoundingRect { get; set; }

		public bool IsSpace
		{
			get
			{
				return this.glyphInfo.IsSpace;
			}
		}

		public int ZIndex { get; set; }

		public PathGeometry Clip { get; set; }

		public bool HasChildren
		{
			get
			{
				return false;
			}
		}

		public IEnumerable<IContentElement> Children
		{
			get
			{
				return null;
			}
		}

		public double StrokeThickness
		{
			get
			{
				return this.glyphInfo.StrokeThickness;
			}
			set
			{
				this.glyphInfo.StrokeThickness = value;
			}
		}

		public double ScaledStrokeThickness
		{
			get
			{
				return this.glyphInfo.ScaledStrokeThickness;
			}
		}

		public ContentElementTypeOld Type
		{
			get
			{
				return ContentElementTypeOld.Glyph;
			}
		}

		public IContentElement Clone()
		{
			GlyphInfo glyphInfo = this.GlyphInfo.Clone();
			GlyphOld glyphOld = new GlyphOld(glyphInfo);
			glyphOld.Name = base.Name;
			glyphOld.TransformMatrix = this.TransformMatrix;
			glyphOld.ZIndex = this.ZIndex;
			glyphOld.GlyphId = base.GlyphId;
			if (base.Outlines != null)
			{
				glyphOld.Outlines = base.Outlines.Clone();
			}
			return glyphOld;
		}

		public Rect Arrange(Matrix transformMatrix)
		{
			this.BoundingRect = Helper.GetBoundingRect(new Rect(new Point(0.0, 0.0), new Size(this.Width, Math.Max(1.0, (this.Ascent - 2.0 * this.Descent) / 1000.0))), this.TransformMatrix * transformMatrix);
			if (this.Contents != null)
			{
				foreach (Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement contentElement in this.Contents)
				{
					contentElement.Arrange(transformMatrix * this.TransformMatrix);
				}
			}
			return this.BoundingRect;
		}

		public void FreezeGlyphInfo()
		{
			if (!this.GlyphInfo.IsFrozen)
			{
				this.GlyphInfo.Freeze();
			}
		}

		public override string ToString()
		{
			return this.ToUnicode;
		}

		GlyphInfo glyphInfo;
	}
}
