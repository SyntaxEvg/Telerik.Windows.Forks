using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class CidType2Font : CidFontBase
	{
		internal override string FontName
		{
			get
			{
				return this.prefix + base.FontName;
			}
		}

		internal GlyphTypeface GlyphTypeface
		{
			get
			{
				return this.glyphTypeface;
			}
			set
			{
				if (this.glyphTypeface != value)
				{
					this.glyphTypeface = value;
					this.prefix = ((this.glyphTypeface == null) ? string.Empty : this.CreateEmbeddedFontSubsetName());
				}
			}
		}

		internal override byte[] ComputeSubset(IEnumerable<CharInfo> usedCharacters)
		{
			if (!usedCharacters.Any<CharInfo>())
			{
				return new byte[0];
			}
			byte[] array = null;
			if (this.GlyphTypeface != null)
			{
				try
				{
					array = this.GlyphTypeface.ComputeSubset((from g in usedCharacters
						select (ushort)g.CharCode.Code).ToArray<ushort>());
				}
				catch (FileFormatException)
				{
				}
			}
			if (array == null)
			{
				array = base.ComputeSubset(usedCharacters);
			}
			return array;
		}

		string CreateEmbeddedFontSubsetName()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			byte[] array = Guid.NewGuid().ToByteArray();
			for (int i = 0; i < 6; i++)
			{
				stringBuilder.Append((char)(65 + array[i] % 26));
			}
			stringBuilder.Append('+');
			return stringBuilder.ToString();
		}

		public CidType2Font(string name, FontSource fontSource)
			: base(name, fontSource)
		{
		}

		internal override FontType Type
		{
			get
			{
				return FontType.CidType2Font;
			}
		}

		internal override IEnumerable<CharInfo> CalculateCharacters(string text)
		{
			foreach (char ch in text)
			{
				ushort glyphId;
				if (base.TryGetGlyphId((int)ch, out glyphId))
				{
					char c = ch;
					yield return new CharInfo(c.ToString(), new CharCode(glyphId));
				}
			}
			yield break;
		}

		internal static void InitializeCoreGlyph(Glyph g, ICidToGidMap mapping, int charCode)
		{
			if (mapping != null)
			{
				g.GlyphId = mapping.GetGlyphId(charCode);
				return;
			}
			g.GlyphId = (ushort)charCode;
		}

		internal override void InitializeCoreGlyph(Glyph glyphInfo, CharCode charcode)
		{
			CidType2Font.InitializeCoreGlyph(glyphInfo, base.CharIdToGlyphIdMapping.Value, charcode.Code);
		}

		GlyphTypeface glyphTypeface;

		string prefix;
	}
}
