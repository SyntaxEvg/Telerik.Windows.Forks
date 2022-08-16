using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Core.Fonts
{
	abstract class FontSource
	{
		public string FontFamily
		{
			get
			{
				string result;
				lock (FontSource.lockObject)
				{
					if (string.IsNullOrEmpty(this.fontFamily))
					{
						lock (FontSource.lockObject)
						{
							this.fontFamily = this.GetFontFamily();
						}
					}
					result = this.fontFamily;
				}
				return result;
			}
		}

		public double UnderlineThickness
		{
			get
			{
				double value;
				lock (FontSource.lockObject)
				{
					if (this.underlineThickness == null)
					{
						lock (FontSource.lockObject)
						{
							this.underlineThickness = new double?(this.GetUnderlineThickness());
						}
					}
					value = this.underlineThickness.Value;
				}
				return value;
			}
		}

		public double UnderlinePosition
		{
			get
			{
				double value;
				lock (FontSource.lockObject)
				{
					if (this.underlinePosition == null)
					{
						lock (FontSource.lockObject)
						{
							this.underlinePosition = new double?(this.GetUnderlinePosition());
						}
					}
					value = this.underlinePosition.Value;
				}
				return value;
			}
		}

		public bool IsBold
		{
			get
			{
				bool value;
				lock (FontSource.lockObject)
				{
					if (this.isBold == null)
					{
						lock (FontSource.lockObject)
						{
							this.isBold = new bool?(this.GetIsBold());
						}
					}
					value = this.isBold.Value;
				}
				return value;
			}
		}

		public bool IsItalic
		{
			get
			{
				bool value;
				lock (FontSource.lockObject)
				{
					if (this.isItalic == null)
					{
						lock (FontSource.lockObject)
						{
							this.isItalic = new bool?(this.GetIsItalic());
						}
					}
					value = this.isItalic.Value;
				}
				return value;
			}
		}

		public double Ascent
		{
			get
			{
				double value;
				lock (FontSource.lockObject)
				{
					if (this.ascent == null)
					{
						lock (FontSource.lockObject)
						{
							this.ascent = new double?(this.GetAscent());
						}
					}
					value = this.ascent.Value;
				}
				return value;
			}
		}

		public double Descent
		{
			get
			{
				double value;
				lock (FontSource.lockObject)
				{
					if (this.descent == null)
					{
						lock (FontSource.lockObject)
						{
							this.descent = new double?(this.GetDescent());
						}
					}
					value = this.descent.Value;
				}
				return value;
			}
		}

		public double LineGap
		{
			get
			{
				double value;
				lock (FontSource.lockObject)
				{
					if (this.lineGap == null)
					{
						lock (FontSource.lockObject)
						{
							this.lineGap = new double?(this.GetLineGap());
						}
					}
					value = this.lineGap.Value;
				}
				return value;
			}
		}

		public double CapHeight
		{
			get
			{
				double value;
				lock (FontSource.lockObject)
				{
					if (this.capHeight == null)
					{
						lock (FontSource.lockObject)
						{
							this.capHeight = new double?(this.GetCapHeight());
						}
					}
					value = this.capHeight.Value;
				}
				return value;
			}
		}

		public double StemV
		{
			get
			{
				double value;
				lock (FontSource.lockObject)
				{
					if (this.stemV == null)
					{
						lock (FontSource.lockObject)
						{
							this.stemV = new double?(this.GetStemV());
						}
					}
					value = this.stemV.Value;
				}
				return value;
			}
		}

		public double ItalicAngle
		{
			get
			{
				double value;
				lock (FontSource.lockObject)
				{
					if (this.italicAngle == null)
					{
						lock (FontSource.lockObject)
						{
							this.italicAngle = new double?(this.GetItalicAngle());
						}
					}
					value = this.italicAngle.Value;
				}
				return value;
			}
		}

		public Rect BoundingBox
		{
			get
			{
				Rect value;
				lock (FontSource.lockObject)
				{
					if (this.boundingBox == null)
					{
						lock (FontSource.lockObject)
						{
							this.boundingBox = new Rect?(this.GetBoundingBox());
						}
					}
					value = this.boundingBox.Value;
				}
				return value;
			}
		}

		public byte[] Data
		{
			get
			{
				byte[] result;
				lock (FontSource.lockObject)
				{
					if (this.data == null)
					{
						lock (FontSource.lockObject)
						{
							this.data = this.GetData();
						}
					}
					result = this.data;
				}
				return result;
			}
		}

		public IEnumerable<FontFlag> Flags
		{
			get
			{
				IEnumerable<FontFlag> result;
				lock (FontSource.lockObject)
				{
					if (this.flags == null)
					{
						lock (FontSource.lockObject)
						{
							this.flags = this.GetFlags();
						}
					}
					result = this.flags;
				}
				return result;
			}
		}

		public virtual IEnumerable<FontFlag> GetFlags()
		{
			if (this.IsItalic)
			{
				yield return FontFlag.Italic;
			}
			yield break;
		}

		public abstract bool GetIsItalic();

		public abstract bool GetIsBold();

		public abstract string GetFontFamily();

		public abstract double GetUnderlineThickness();

		public abstract double GetUnderlinePosition();

		public abstract double GetAscent();

		public abstract double GetDescent();

		public abstract double GetLineGap();

		public abstract double GetCapHeight();

		public abstract double GetStemV();

		public abstract double GetItalicAngle();

		public abstract Rect GetBoundingBox();

		public abstract byte[] GetData();

		public abstract bool TryGetCharCode(int unicode, out int charCode);

		public abstract bool TryGetGlyphId(int unicode, out ushort glyphId);

		public double GetAdvancedWidth(int charCode)
		{
			double result;
			lock (FontSource.lockObject)
			{
				double advancedWidthOverride = this.GetAdvancedWidthOverride(charCode);
				result = advancedWidthOverride;
			}
			return result;
		}

		public abstract double GetAdvancedWidthOverride(int charCode);

		public void GetAdvancedWidth(Glyph glyph)
		{
			lock (FontSource.lockObject)
			{
				this.GetAdvancedWidthOverride(glyph);
			}
		}

		public abstract void GetAdvancedWidthOverride(Glyph glyph);

		public void InitializeGlyphOutlines(Glyph glyph, double fontSize)
		{
			lock (FontSource.lockObject)
			{
				this.InitializeGlyphOutlinesOverride(glyph, fontSize);
			}
		}

		public abstract void InitializeGlyphOutlinesOverride(Glyph glyph, double fontSize);

		static readonly object lockObject = new object();

		string fontFamily;

		double? underlineThickness;

		double? underlinePosition;

		bool? isBold;

		bool? isItalic;

		double? ascent;

		double? descent;

		double? lineGap;

		double? capHeight;

		double? stemV;

		double? italicAngle;

		Rect? boundingBox;

		byte[] data;

		IEnumerable<FontFlag> flags;
	}
}
