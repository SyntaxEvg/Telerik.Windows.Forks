using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;

namespace Telerik.Windows.Documents.Core.Fonts
{
	class EmptyFontSource : FontSource
	{
		EmptyFontSource()
		{
		}

		public override string GetFontFamily()
		{
			return string.Empty;
		}

		public override double GetUnderlineThickness()
		{
			return 0.0;
		}

		public override double GetUnderlinePosition()
		{
			return 0.0;
		}

		public override bool GetIsBold()
		{
			return false;
		}

		public override double GetAscent()
		{
			return 0.0;
		}

		public override double GetDescent()
		{
			return 0.0;
		}

		public override double GetLineGap()
		{
			return 0.0;
		}

		public override double GetCapHeight()
		{
			return 0.0;
		}

		public override double GetStemV()
		{
			return 0.0;
		}

		public override double GetItalicAngle()
		{
			return 0.0;
		}

		public override bool GetIsItalic()
		{
			return false;
		}

		public override Rect GetBoundingBox()
		{
			return default(Rect);
		}

		public override byte[] GetData()
		{
			return new byte[0];
		}

		public override bool TryGetCharCode(int unicode, out int charCode)
		{
			charCode = 0;
			return false;
		}

		public override bool TryGetGlyphId(int unicode, out ushort glyphId)
		{
			glyphId = 0;
			return false;
		}

		public override double GetAdvancedWidthOverride(int charCode)
		{
			return 0.0;
		}

		public override void GetAdvancedWidthOverride(Glyph glyph)
		{
		}

		public override void InitializeGlyphOutlinesOverride(Glyph glyph, double fontSize)
		{
		}

		public static EmptyFontSource Instance = new EmptyFontSource();
	}
}
