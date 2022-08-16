using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class TrueTypeFont : SimpleFont
	{
		public TrueTypeFont(string name, FontSource fontSource)
			: base(name, fontSource)
		{
		}

		internal override FontType Type
		{
			get
			{
				return FontType.TrueType;
			}
		}

		internal override FontSource ActualFontSource
		{
			get
			{
				this.EnsureGlyphInitializationInfo();
				return this.initializationInfo.FontSource ?? EmptyFontSource.Instance;
			}
		}

		internal static TrueTypeGlyphInitializationInfo GetGlyphInitializationInfo(OpenTypeFontSource fontSource, IFontDescriptor fontDescriptor, ISimpleEncoding encoding)
		{
			TrueTypeGlyphInitializationInfo trueTypeGlyphInitializationInfo = new TrueTypeGlyphInitializationInfo
			{
				Platform = TrueTypeGlyphInitializationInfo.PlatformType.Unknown,
				Encoding = encoding,
				FontSource = fontSource
			};
			if (fontSource == null)
			{
				return trueTypeGlyphInitializationInfo;
			}
			bool flag = encoding != null && encoding.IsNamedEncoding && (encoding.BaseEncodingName == "MacRomanEncoding" || encoding.BaseEncodingName == "WinAnsiEncoding");
			if ((fontDescriptor != null && fontDescriptor.IsNonSymbolic) || flag)
			{
				trueTypeGlyphInitializationInfo.CMap = fontSource.CMap.GetCMapTable(3, 1);
				if (trueTypeGlyphInitializationInfo.CMap != null)
				{
					trueTypeGlyphInitializationInfo.Platform = TrueTypeGlyphInitializationInfo.PlatformType.MicrosoftNonSymbolic;
				}
				else
				{
					trueTypeGlyphInitializationInfo.CMap = fontSource.CMap.GetCMapTable(1, 0);
					if (trueTypeGlyphInitializationInfo.CMap != null)
					{
						trueTypeGlyphInitializationInfo.Platform = TrueTypeGlyphInitializationInfo.PlatformType.MacintoshNonSymbolic;
					}
				}
			}
			else if ((fontDescriptor != null && fontDescriptor.IsSymbolic) || encoding == null)
			{
				trueTypeGlyphInitializationInfo.CMap = fontSource.CMap.GetCMapTable(3, 0);
				if (trueTypeGlyphInitializationInfo.CMap != null)
				{
					byte value;
					if (TrueTypeFont.TryCalculateFirstByte(trueTypeGlyphInitializationInfo.CMap, out value))
					{
						trueTypeGlyphInitializationInfo.AppendingFirstByte = new byte?(value);
						trueTypeGlyphInitializationInfo.Platform = TrueTypeGlyphInitializationInfo.PlatformType.MicrosoftSymbolic;
					}
				}
				else
				{
					trueTypeGlyphInitializationInfo.CMap = fontSource.CMap.GetCMapTable(1, 0);
					if (trueTypeGlyphInitializationInfo.CMap != null)
					{
						trueTypeGlyphInitializationInfo.Platform = TrueTypeGlyphInitializationInfo.PlatformType.MacintoshSymbolic;
					}
				}
			}
			return trueTypeGlyphInitializationInfo;
		}

		internal static void InitializeCoreGlyph(Glyph glyphInfo, TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			switch (initializationInfo.Platform)
			{
			case TrueTypeGlyphInitializationInfo.PlatformType.MicrosoftNonSymbolic:
				TrueTypeFont.InitializeMicrosoftNonSymbolicGlyph(glyphInfo, initializationInfo, b);
				return;
			case TrueTypeGlyphInitializationInfo.PlatformType.MacintoshNonSymbolic:
				TrueTypeFont.InitializeMacintoshNonSymbolicGlyph(glyphInfo, initializationInfo, b);
				return;
			case TrueTypeGlyphInitializationInfo.PlatformType.MicrosoftSymbolic:
				TrueTypeFont.InitializeMicrosoftSymbolicGlyph(glyphInfo, initializationInfo, b);
				return;
			case TrueTypeGlyphInitializationInfo.PlatformType.MacintoshSymbolic:
				TrueTypeFont.InitializeMacintoshSymbolicGlyph(glyphInfo, initializationInfo, b);
				return;
			default:
				return;
			}
		}

		internal override void InitializeCoreGlyph(Glyph glyphInfo, CharCode charcode)
		{
			byte b = (byte)charcode.Code;
			this.EnsureGlyphInitializationInfo();
			TrueTypeFont.InitializeCoreGlyph(glyphInfo, this.initializationInfo, b);
		}

		static void InitializeMicrosoftNonSymbolicGlyph(Glyph g, TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			string name = initializationInfo.Encoding.GetName(b);
			g.Name = name;
			if (AdobeGlyphList.IsSupportedPdfName(name))
			{
				char unicode = AdobeGlyphList.GetUnicode(name);
				ushort glyphId;
				initializationInfo.CMap.TryGetGlyphId((int)unicode, out glyphId);
				g.GlyphId = glyphId;
				return;
			}
			g.GlyphId = TrueTypeFont.GetGlyphIdFromPostTable(initializationInfo.FontSource, g.Name);
		}

		static void InitializeMacintoshNonSymbolicGlyph(Glyph g, TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			string name = initializationInfo.Encoding.GetName(b);
			g.Name = name;
			if (name == ".notdef")
			{
				g.GlyphId = TrueTypeFont.GetGlyphIdFromPostTable(initializationInfo.FontSource, name);
				return;
			}
			byte charId = PredefinedSimpleEncoding.StandardMacRomanEncoding.GetCharId(name);
			ushort glyphId;
			initializationInfo.CMap.TryGetGlyphId((int)charId, out glyphId);
			g.GlyphId = glyphId;
		}

		static void InitializeMicrosoftSymbolicGlyph(Glyph g, TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			ushort unicode = TrueTypeFont.AppendByte(initializationInfo.AppendingFirstByte.Value, b);
			ushort glyphId;
			initializationInfo.CMap.TryGetGlyphId((int)unicode, out glyphId);
			g.GlyphId = glyphId;
		}

		static void InitializeMacintoshSymbolicGlyph(Glyph g, TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			ushort glyphId;
			initializationInfo.CMap.TryGetGlyphId((int)b, out glyphId);
			g.GlyphId = glyphId;
		}

		static ushort GetGlyphIdFromPostTable(OpenTypeFontSource fontSource, string name)
		{
			if (fontSource.Post == null)
			{
				return 0;
			}
			return fontSource.Post.GetGlyphId(name);
		}

		static bool TryCalculateFirstByte(CMapTable cMap, out byte firstByte)
		{
			bool result = false;
			firstByte = 0;
			try
			{
				ushort firstCode = cMap.FirstCode;
				CharCode charCode = new CharCode(firstCode);
				byte[] array = charCode.ToBytes();
				firstByte = array[0];
				result = true;
			}
			catch (NotSupportedException)
			{
			}
			return result;
		}

		static ushort AppendByte(byte firstByte, byte b)
		{
			CharCode charCode = new CharCode(new byte[] { firstByte, b });
			return (ushort)charCode.Code;
		}

		void EnsureGlyphInitializationInfo()
		{
			if (this.initializationInfo == null)
			{
				OpenTypeFontSource openTypeFontSource = base.FontSource as OpenTypeFontSource;
				if (openTypeFontSource == null)
				{
					FontProperties fontProperties = new FontProperties(new FontFamily(FixedDocumentDefaults.TrueTypeFallbackFontName));
					openTypeFontSource = FontsRepository.FontsManager.GetOpenTypeFontSource(fontProperties);
				}
				this.initializationInfo = TrueTypeFont.GetGlyphInitializationInfo(openTypeFontSource, this, base.Encoding.Value);
			}
		}

		TrueTypeGlyphInitializationInfo initializationInfo;
	}
}
