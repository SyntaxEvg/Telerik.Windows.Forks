using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class CidType0Font : CidFontBase
	{
		public CidType0Font(string name, FontSource fontSource)
			: base(name, fontSource)
		{
		}

		internal override FontType Type
		{
			get
			{
				return FontType.CidType0Font;
			}
		}

		internal static void InitializeCoreGlyph(Glyph g, CFFFontSource fontSource, int charCode)
		{
			if (fontSource.UsesCIDFontOperators)
			{
				ushort glyphId;
				fontSource.TryGetGlyphId((int)((ushort)charCode), out glyphId);
				g.GlyphId = glyphId;
				return;
			}
			g.GlyphId = (ushort)charCode;
		}

		internal override void InitializeCoreGlyph(Glyph glyphInfo, CharCode charcode)
		{
			CFFFontSource cfffontSource = this.ActualFontSource as CFFFontSource;
			if (cfffontSource != null)
			{
				CidType0Font.InitializeCoreGlyph(glyphInfo, cfffontSource, charcode.Code);
			}
		}
	}
}
