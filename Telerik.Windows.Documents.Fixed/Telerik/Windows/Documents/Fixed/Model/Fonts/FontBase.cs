using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	public abstract class FontBase : IFontDescriptor
	{
		internal FontBase(string name, FontSource fontSource)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNull<FontSource>(fontSource, "fontSource");
			this.name = name;
			this.fontSource = fontSource;
			this.glyphsCache = new Dictionary<CharCode, Glyph>();
			this.fontFamily = new PdfProperty<string>(new Func<string>(this.GetFontSourceFontFamily));
			this.fontStretch = new PdfProperty<string>();
			this.charSet = new PdfProperty<string>();
			this.fontWeight = new PdfProperty<double>();
			this.fontBBox = new PdfProperty<Rect>(new Func<Rect>(this.GetFontSourceBoundingBox));
			this.italicAngle = new PdfProperty<double>(new Func<double>(this.GetFontSourceItalicAngle));
			this.ascent = new PdfProperty<double>(new Func<double>(this.GetFontSourceAscent));
			this.descent = new PdfProperty<double>(new Func<double>(this.GetFontSourceDescent));
			this.leading = new PdfProperty<double>();
			this.capHeight = new PdfProperty<double>(new Func<double>(this.GetFontSourceCapHeight));
			this.xHeight = new PdfProperty<double>();
			this.stemV = new PdfProperty<double>(new Func<double>(this.GetFontSourceStemV));
			this.stemH = new PdfProperty<double>();
			this.avgWidth = new PdfProperty<double>();
			this.maxWidth = new PdfProperty<double>();
			this.missingWidth = new PdfProperty<double>();
			this.toUnicode = new PdfProperty<CMapEncoding>();
			this.fontFileInfo = new PdfProperty<FontFileInfo>();
			this.InitializeFlagsFromFontSource();
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		internal virtual string FontName
		{
			get
			{
				return this.Name;
			}
		}

		internal abstract FontType Type { get; }

		internal double UnderlinePosition
		{
			get
			{
				return this.FontSource.UnderlinePosition / 1000.0;
			}
		}

		internal double UnderlineThickness
		{
			get
			{
				return this.FontSource.UnderlineThickness / 1000.0;
			}
		}

		internal double Height
		{
			get
			{
				return Math.Abs(this.Ascent.Value) + Math.Abs(this.Descent.Value);
			}
		}

		internal virtual double LineGap
		{
			get
			{
				return this.FontSource.LineGap / 1000.0;
			}
		}

		internal virtual bool IsBold
		{
			get
			{
				return this.FontSource.IsBold;
			}
		}

		internal virtual bool HasFontDescriptorProperties
		{
			get
			{
				return this.Ascent.HasValue || this.Descent.HasValue || this.FontFamily.HasValue || this.FontStretch.HasValue || this.FontWeight.HasValue || this.BoundingBox.HasValue || this.ItalicAngle.HasValue || this.Leading.HasValue || this.CapHeight.HasValue || this.XHeight.HasValue || this.StemH.HasValue || this.StemV.HasValue || this.AvgWidth.HasValue || this.MaxWidth.HasValue || this.MissingWidth.HasValue || this.FontFileInfo.HasValue || this.CharSet.HasValue;
			}
		}

		internal PdfProperty<double> Ascent
		{
			get
			{
				return this.ascent;
			}
		}

		internal PdfProperty<double> Descent
		{
			get
			{
				return this.descent;
			}
		}

		internal PdfProperty<double> ItalicAngle
		{
			get
			{
				return this.italicAngle;
			}
		}

		internal PdfProperty<string> FontStretch
		{
			get
			{
				return this.fontStretch;
			}
		}

		internal PdfProperty<string> CharSet
		{
			get
			{
				return this.charSet;
			}
		}

		internal PdfProperty<double> FontWeight
		{
			get
			{
				return this.fontWeight;
			}
		}

		internal PdfProperty<double> CapHeight
		{
			get
			{
				return this.capHeight;
			}
		}

		internal PdfProperty<double> StemV
		{
			get
			{
				return this.stemV;
			}
		}

		internal PdfProperty<Rect> BoundingBox
		{
			get
			{
				return this.fontBBox;
			}
		}

		internal PdfProperty<CMapEncoding> ToUnicode
		{
			get
			{
				return this.toUnicode;
			}
		}

		internal PdfProperty<string> FontFamily
		{
			get
			{
				return this.fontFamily;
			}
		}

		internal PdfProperty<double> Leading
		{
			get
			{
				return this.leading;
			}
		}

		internal PdfProperty<double> XHeight
		{
			get
			{
				return this.xHeight;
			}
		}

		internal PdfProperty<double> StemH
		{
			get
			{
				return this.stemH;
			}
		}

		internal PdfProperty<double> AvgWidth
		{
			get
			{
				return this.avgWidth;
			}
		}

		internal PdfProperty<double> MaxWidth
		{
			get
			{
				return this.maxWidth;
			}
		}

		internal PdfProperty<double> MissingWidth
		{
			get
			{
				return this.missingWidth;
			}
		}

		internal PdfProperty<FontFileInfo> FontFileInfo
		{
			get
			{
				return this.fontFileInfo;
			}
		}

		internal bool IsFixedPitch { get; set; }

		internal bool IsSerif { get; set; }

		internal bool IsSymbolic { get; set; }

		internal bool IsScript { get; set; }

		internal bool IsNonSymbolic { get; set; }

		internal bool IsItalic { get; set; }

		internal bool IsAllCap { get; set; }

		internal bool IsSmallCap { get; set; }

		internal bool IsForcingBold { get; set; }

		internal FontSource FontSource
		{
			get
			{
				return this.fontSource;
			}
		}

		internal abstract FontSource ActualFontSource { get; }

		bool IFontDescriptor.IsSymbolic
		{
			get
			{
				return this.IsSymbolic;
			}
		}

		bool IFontDescriptor.IsNonSymbolic
		{
			get
			{
				return this.IsNonSymbolic;
			}
		}

		internal bool TryGetGlyphId(int unicode, out ushort glyphId)
		{
			return this.FontSource.TryGetGlyphId(unicode, out glyphId);
		}

		internal virtual IEnumerable<CharInfo> CalculateCharacters(string text)
		{
			Guard.ThrowExceptionIfNull<string>(text, "text");
			foreach (char ch in text)
			{
				int charId;
				if (this.FontSource.TryGetCharCode((int)ch, out charId))
				{
					char c = ch;
					yield return new CharInfo(c.ToString(), new CharCode((byte)charId));
				}
			}
			yield break;
		}

		internal abstract void InitializeCoreGlyph(Glyph glyph, CharCode charcode);

		internal GlyphOutlinesCollection GetCachedOutlines(CharCode charCode)
		{
			Glyph cachedGlyph = this.GetCachedGlyph(charCode);
			if (cachedGlyph.Outlines == null)
			{
				this.ActualFontSource.InitializeGlyphOutlines(cachedGlyph, 100.0);
			}
			return cachedGlyph.Outlines;
		}

		internal virtual double GetWidth(int charCode)
		{
			return this.FontSource.GetAdvancedWidth(charCode);
		}

		internal virtual byte[] ComputeSubset(IEnumerable<CharInfo> usedCharacters)
		{
			return this.FontSource.Data;
		}

		double GetFontSourceStemV()
		{
			return this.FontSource.StemV;
		}

		double GetFontSourceCapHeight()
		{
			return this.FontSource.CapHeight;
		}

		double GetFontSourceDescent()
		{
			return this.FontSource.Descent / 1000.0;
		}

		double GetFontSourceAscent()
		{
			return this.FontSource.Ascent / 1000.0;
		}

		double GetFontSourceItalicAngle()
		{
			return this.FontSource.ItalicAngle;
		}

		string GetFontSourceFontFamily()
		{
			return this.FontSource.FontFamily;
		}

		Rect GetFontSourceBoundingBox()
		{
			double x = this.FontSource.BoundingBox.X;
			double y = this.Descent.Value * 1000.0;
			double width = this.FontSource.BoundingBox.Width;
			double height = (this.Ascent.Value + Math.Abs(this.Descent.Value)) * 1000.0;
			Rect result = new Rect(x, y, width, height);
			return result;
		}

		Glyph GetCachedGlyph(CharCode charCode)
		{
			Glyph glyph;
			if (!this.glyphsCache.TryGetValue(charCode, out glyph))
			{
				glyph = new Glyph();
				this.InitializeCoreGlyph(glyph, charCode);
				this.glyphsCache.Add(charCode, glyph);
			}
			return glyph;
		}

		void InitializeFlagsFromFontSource()
		{
			foreach (FontFlag fontFlag in this.FontSource.Flags)
			{
				FontFlag fontFlag2 = fontFlag;
				switch (fontFlag2)
				{
				case FontFlag.FixedPitch:
					this.IsFixedPitch = true;
					continue;
				case FontFlag.Serif:
					this.IsSerif = true;
					continue;
				case FontFlag.Symbolic:
					this.IsSymbolic = true;
					continue;
				case FontFlag.Script:
					this.IsScript = true;
					continue;
				case (FontFlag)5:
					break;
				case FontFlag.Nonsymbolic:
					this.IsNonSymbolic = true;
					continue;
				case FontFlag.Italic:
					this.IsItalic = true;
					continue;
				default:
					switch (fontFlag2)
					{
					case FontFlag.AllCap:
						this.IsAllCap = true;
						continue;
					case FontFlag.SmallCap:
						this.IsSmallCap = true;
						continue;
					case FontFlag.ForceBold:
						this.IsForcingBold = true;
						continue;
					}
					break;
				}
				throw new NotSupportedException(string.Format("Not supported font flag: {0}", fontFlag));
			}
		}

		internal const double FontRatio = 1000.0;

		internal const double GlyphOutlinesCacheFontSize = 100.0;

		readonly string name;

		readonly FontSource fontSource;

		readonly PdfProperty<string> fontFamily;

		readonly PdfProperty<string> fontStretch;

		readonly PdfProperty<string> charSet;

		readonly PdfProperty<double> fontWeight;

		readonly PdfProperty<Rect> fontBBox;

		readonly PdfProperty<double> italicAngle;

		readonly PdfProperty<double> ascent;

		readonly PdfProperty<double> descent;

		readonly PdfProperty<double> leading;

		readonly PdfProperty<double> capHeight;

		readonly PdfProperty<double> xHeight;

		readonly PdfProperty<double> stemV;

		readonly PdfProperty<double> stemH;

		readonly PdfProperty<double> avgWidth;

		readonly PdfProperty<double> maxWidth;

		readonly PdfProperty<double> missingWidth;

		readonly PdfProperty<CMapEncoding> toUnicode;

		readonly PdfProperty<FontFileInfo> fontFileInfo;

		readonly Dictionary<CharCode, Glyph> glyphsCache;
	}
}
