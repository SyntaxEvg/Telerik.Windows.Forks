using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class GlyphInfo
	{
		public GlyphInfo()
		{
			this.Ascent = 1000.0;
			this.Descent = 0.0;
		}

		public ResourceKey Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.ValidateValueChange();
				this.key = value;
			}
		}

		public CharCodeOld CharId
		{
			get
			{
				return this.charId;
			}
			set
			{
				this.ValidateValueChange();
				this.charId = value;
			}
		}

		public string ToUnicode
		{
			get
			{
				return this.toUnicode;
			}
			set
			{
				this.ValidateValueChange();
				this.toUnicode = value;
			}
		}

		public string FontFamily
		{
			get
			{
				return this.fontFamily;
			}
			set
			{
				this.ValidateValueChange();
				this.fontFamily = value;
			}
		}

		public FontStyle FontStyle
		{
			get
			{
				return this.fontStyle;
			}
			set
			{
				this.ValidateValueChange();
				this.fontStyle = value;
			}
		}

		public FontWeight FontWeight
		{
			get
			{
				return this.fontWeight;
			}
			set
			{
				this.ValidateValueChange();
				this.fontWeight = value;
			}
		}

		public bool IsBold
		{
			get
			{
				return this.isBold;
			}
			set
			{
				this.ValidateValueChange();
				this.isBold = value;
			}
		}

		public bool IsItalic
		{
			get
			{
				return this.isItalic;
			}
			set
			{
				this.ValidateValueChange();
				this.isItalic = value;
			}
		}

		public double FontSize
		{
			get
			{
				return this.fontSize;
			}
			set
			{
				this.ValidateValueChange();
				this.fontSize = value;
			}
		}

		public double Rise
		{
			get
			{
				return this.rise;
			}
			set
			{
				this.ValidateValueChange();
				this.rise = value;
			}
		}

		public double CharSpacing
		{
			get
			{
				return this.charSpacing;
			}
			set
			{
				this.ValidateValueChange();
				this.charSpacing = value;
			}
		}

		public double WordSpacing
		{
			get
			{
				return this.wordSpacing;
			}
			set
			{
				this.ValidateValueChange();
				this.wordSpacing = value;
			}
		}

		public double HorizontalScaling
		{
			get
			{
				return this.horizontalScaling;
			}
			set
			{
				this.ValidateValueChange();
				this.horizontalScaling = value;
			}
		}

		public double Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.ValidateValueChange();
				this.width = value;
			}
		}

		public double Ascent
		{
			get
			{
				return this.ascent;
			}
			set
			{
				this.ValidateValueChange();
				this.ascent = value;
			}
		}

		public double Descent
		{
			get
			{
				return this.descent;
			}
			set
			{
				this.ValidateValueChange();
				if (value > 0.0)
				{
					this.descent = -value;
					return;
				}
				this.descent = value;
			}
		}

		public Brush Fill
		{
			get
			{
				return this.fill;
			}
			set
			{
				this.ValidateValueChange();
				this.fill = value;
			}
		}

		public Brush Stroke
		{
			get
			{
				return this.stroke;
			}
			set
			{
				this.ValidateValueChange();
				this.stroke = value;
			}
		}

		public bool IsFilled
		{
			get
			{
				return this.isFilled;
			}
			set
			{
				this.ValidateValueChange();
				this.isFilled = value;
			}
		}

		public bool IsStroked
		{
			get
			{
				return this.isStroked;
			}
			set
			{
				this.ValidateValueChange();
				this.isStroked = value;
			}
		}

		public Size Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.ValidateValueChange();
				this.size = value;
			}
		}

		public bool IsSpace
		{
			get
			{
				return this.CharId.BytesCount == 1 && this.CharId.Bytes[0] == 32;
			}
		}

		public double StrokeThickness
		{
			get
			{
				return this.strokeThickness;
			}
			set
			{
				this.ValidateValueChange();
				this.strokeThickness = value;
			}
		}

		public double ScaledStrokeThickness
		{
			get
			{
				double num = ((this.FontSize == 0.0) ? 1.0 : Math.Abs(100.0 / this.FontSize));
				return this.StrokeThickness * num;
			}
		}

		public bool IsFrozen
		{
			get
			{
				return this.isFrozen;
			}
		}

		public void Freeze()
		{
			this.isFrozen = true;
		}

		public GlyphInfo Clone()
		{
			return new GlyphInfo
			{
				Ascent = this.Ascent,
				CharSpacing = this.CharSpacing,
				Fill = this.Fill,
				Stroke = this.Stroke,
				Descent = this.Descent,
				FontFamily = this.FontFamily,
				FontWeight = this.FontWeight,
				FontStyle = this.FontStyle,
				FontSize = this.FontSize,
				HorizontalScaling = this.HorizontalScaling,
				Rise = this.Rise,
				WordSpacing = this.WordSpacing,
				Width = this.Width,
				Size = this.Size,
				ToUnicode = this.ToUnicode,
				IsFilled = this.IsFilled,
				IsStroked = this.IsStroked,
				Key = this.Key
			};
		}

		public override string ToString()
		{
			return this.ToUnicode;
		}

		public override bool Equals(object obj)
		{
			bool flag = object.ReferenceEquals(this, obj);
			if (flag)
			{
				return flag;
			}
			GlyphInfo glyphInfo = obj as GlyphInfo;
			return glyphInfo != null && (this.ascent.Equals(glyphInfo.ascent) && this.descent.Equals(glyphInfo.descent) && this.isBold.Equals(glyphInfo.isBold) && this.isItalic.Equals(glyphInfo.isItalic) && this.fontSize.Equals(glyphInfo.fontSize) && this.rise.Equals(glyphInfo.rise) && this.charSpacing.Equals(glyphInfo.charSpacing) && this.horizontalScaling.Equals(glyphInfo.horizontalScaling) && this.width.Equals(glyphInfo.width) && this.isFilled.Equals(glyphInfo.isFilled) && this.isStroked.Equals(glyphInfo.isStroked) && this.strokeThickness.Equals(glyphInfo.strokeThickness) && this.wordSpacing.Equals(glyphInfo.wordSpacing) && this.IsObjectEqual<ResourceKey>(this.key, glyphInfo.key) && this.IsObjectEqual<CharCodeOld>(this.charId, glyphInfo.charId) && this.IsObjectEqual<string>(this.toUnicode, glyphInfo.toUnicode) && this.IsObjectEqual<string>(this.fontFamily, glyphInfo.fontFamily) && this.IsObjectEqual<FontStyle>(this.fontStyle, glyphInfo.fontStyle) && this.IsObjectEqual<FontWeight>(this.fontWeight, glyphInfo.fontWeight) && this.IsObjectEqual<Brush>(this.fill, glyphInfo.fill) && this.IsObjectEqual<Brush>(this.stroke, glyphInfo.stroke)) && this.IsObjectEqual<Size>(this.size, glyphInfo.size);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.ascent.GetHashCode(), this.descent.GetHashCode(), new int[]
			{
				(this.key != null) ? this.key.GetHashCode() : 0,
				this.charId.GetHashCode(),
				this.toUnicode.GetHashCode(),
				(this.fontFamily != null) ? this.fontFamily.GetHashCode() : 0,
				this.fontStyle.GetHashCode(),
				this.fontWeight.GetHashCode(),
				this.fontSize.GetHashCode(),
				this.isBold.GetHashCode(),
				this.isItalic.GetHashCode(),
				this.rise.GetHashCode(),
				this.charSpacing.GetHashCode(),
				this.horizontalScaling.GetHashCode(),
				this.width.GetHashCode(),
				(this.fill != null) ? this.fill.GetHashCode() : 0,
				(this.stroke != null) ? this.stroke.GetHashCode() : 0,
				this.isFilled.GetHashCode(),
				this.isStroked.GetHashCode(),
				this.size.GetHashCode(),
				this.strokeThickness.GetHashCode(),
				this.wordSpacing.GetHashCode()
			});
		}

		void ValidateValueChange()
		{
			if (this.IsFrozen)
			{
				throw new InvalidOperationException("The object is frozen");
			}
		}

		bool IsObjectEqual<T>(T left, T right)
		{
			bool flag = object.ReferenceEquals(left, right);
			return flag || (!object.ReferenceEquals(left, null) && left.Equals(right));
		}

		double ascent;

		double descent;

		bool isFrozen;

		ResourceKey key;

		CharCodeOld charId;

		string toUnicode;

		string fontFamily;

		FontStyle fontStyle;

		FontWeight fontWeight;

		bool isBold;

		bool isItalic;

		double fontSize;

		double rise;

		double charSpacing;

		double horizontalScaling;

		double width;

		Brush fill;

		Brush stroke;

		bool isFilled;

		bool isStroked;

		Size size;

		double strokeThickness;

		double wordSpacing;
	}
}
