using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class Type1Font : SimpleFont
	{
		public Type1Font(string name, FontSource fontSource)
			: base(name, fontSource)
		{
		}

		internal override FontType Type
		{
			get
			{
				return FontType.Type1;
			}
		}

		internal override FontSource ActualFontSource
		{
			get
			{
				if (base.FontSource == EmptyFontSource.Instance)
				{
					this.EnsureFallbackFontSource();
					return this.fallbackFontSource;
				}
				return base.FontSource;
			}
		}

		internal override void InitializeCoreGlyph(Glyph glyph, CharCode charcode)
		{
			byte b = (byte)charcode.Code;
			FontSource actualFontSource = this.ActualFontSource;
			CFFFontSource cfffontSource = actualFontSource as CFFFontSource;
			if (cfffontSource != null)
			{
				Type1Font.InitializeCoreGlyph(glyph, cfffontSource, base.Encoding.Value, b);
				return;
			}
			Type1FontSource fontSource = (Type1FontSource)actualFontSource;
			Type1Font.InitializeCoreGlyph(glyph, fontSource, base.Encoding.Value, b);
		}

		internal static void InitializeCoreGlyph(Glyph glyph, CFFFontSource fontSource, ISimpleEncoding encoding, byte b)
		{
			string name = ((encoding == null) ? fontSource.GetGlyphName((ushort)b) : encoding.GetName(b));
			glyph.Name = name;
			ushort glyphId;
			fontSource.TryGetGlyphId(name, out glyphId);
			glyph.GlyphId = glyphId;
		}

		internal static void InitializeCoreGlyph(Glyph glyph, Type1FontSource fontSource, ISimpleEncoding encoding, byte b)
		{
			string name = ((encoding == null) ? fontSource.GetGlyphName((ushort)b) : encoding.GetName(b));
			glyph.Name = name;
			glyph.GlyphId = (ushort)b;
		}

		void EnsureFallbackFontSource()
		{
			if (this.fallbackFontSource == null)
			{
				string name = base.Name;
				if (FontsManagerBase.IsStandardFontName(name))
				{
					this.fallbackFontSource = FontsRepository.FontsManager.GetStandardFontSource(name);
					return;
				}
				this.fallbackFontSource = FontsRepository.FontsManager.GetType1FallbackFontSource(name);
			}
		}

		Type1FontSource fallbackFontSource;
	}
}
