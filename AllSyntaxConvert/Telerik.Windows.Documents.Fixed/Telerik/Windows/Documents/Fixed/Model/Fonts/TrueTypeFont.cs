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
	class TrueTypeFont : global::Telerik.Windows.Documents.Fixed.Model.Fonts.SimpleFont
	{
		public TrueTypeFont(string name, global::Telerik.Windows.Documents.Core.Fonts.FontSource fontSource)
			: base(name, fontSource)
		{
		}

		internal override global::Telerik.Windows.Documents.Fixed.Model.Fonts.FontType Type
		{
			get
			{
				return global::Telerik.Windows.Documents.Fixed.Model.Fonts.FontType.TrueType;
			}
		}

		internal override global::Telerik.Windows.Documents.Core.Fonts.FontSource ActualFontSource
		{
			get
			{
				this.EnsureGlyphInitializationInfo();
				return this.initializationInfo.FontSource;
				//return this.initializationInfo.FontSource ?? global::Telerik.Windows.Documents.Core.Fonts.EmptyFontSource.Instance;
			}
		}

		internal static global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo GetGlyphInitializationInfo(global::Telerik.Windows.Documents.Core.Fonts.OpenType.OpenTypeFontSource fontSource, global::Telerik.Windows.Documents.Fixed.Model.Fonts.IFontDescriptor fontDescriptor, global::Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding.ISimpleEncoding encoding)
		{
			global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo trueTypeGlyphInitializationInfo = new global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo
			{
				Platform = global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo.PlatformType.Unknown,
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
					trueTypeGlyphInitializationInfo.Platform = global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo.PlatformType.MicrosoftNonSymbolic;
				}
				else
				{
					trueTypeGlyphInitializationInfo.CMap = fontSource.CMap.GetCMapTable(1, 0);
					if (trueTypeGlyphInitializationInfo.CMap != null)
					{
						trueTypeGlyphInitializationInfo.Platform = global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo.PlatformType.MacintoshNonSymbolic;
					}
				}
			}
			else if ((fontDescriptor != null && fontDescriptor.IsSymbolic) || encoding == null)
			{
				trueTypeGlyphInitializationInfo.CMap = fontSource.CMap.GetCMapTable(3, 0);
				if (trueTypeGlyphInitializationInfo.CMap != null)
				{
					byte value;
					if (global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.TryCalculateFirstByte(trueTypeGlyphInitializationInfo.CMap, out value))
					{
						trueTypeGlyphInitializationInfo.AppendingFirstByte = new byte?(value);
						trueTypeGlyphInitializationInfo.Platform = global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo.PlatformType.MicrosoftSymbolic;
					}
				}
				else
				{
					trueTypeGlyphInitializationInfo.CMap = fontSource.CMap.GetCMapTable(1, 0);
					if (trueTypeGlyphInitializationInfo.CMap != null)
					{
						trueTypeGlyphInitializationInfo.Platform = global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo.PlatformType.MacintoshSymbolic;
					}
				}
			}
			return trueTypeGlyphInitializationInfo;
		}

		internal static void InitializeCoreGlyph(global::Telerik.Windows.Documents.Core.Fonts.Glyphs.Glyph glyphInfo, global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			switch (initializationInfo.Platform)
			{
				case global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo.PlatformType.MicrosoftNonSymbolic:
					global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.InitializeMicrosoftNonSymbolicGlyph(glyphInfo, initializationInfo, b);
					return;
				case global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo.PlatformType.MacintoshNonSymbolic:
					global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.InitializeMacintoshNonSymbolicGlyph(glyphInfo, initializationInfo, b);
					return;
				case global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo.PlatformType.MicrosoftSymbolic:
					global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.InitializeMicrosoftSymbolicGlyph(glyphInfo, initializationInfo, b);
					return;
				case global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo.PlatformType.MacintoshSymbolic:
					global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.InitializeMacintoshSymbolicGlyph(glyphInfo, initializationInfo, b);
					return;
				default:
					return;
			}
		}

		internal override void InitializeCoreGlyph(global::Telerik.Windows.Documents.Core.Fonts.Glyphs.Glyph glyphInfo, global::Telerik.Windows.Documents.Fixed.Model.Data.CharCode charcode)
		{
			byte b = (byte)charcode.Code;
			this.EnsureGlyphInitializationInfo();
			global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.InitializeCoreGlyph(glyphInfo, this.initializationInfo, b);
		}

		private static void InitializeMicrosoftNonSymbolicGlyph(global::Telerik.Windows.Documents.Core.Fonts.Glyphs.Glyph g, global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			string name = initializationInfo.Encoding.GetName(b);
			g.Name = name;
			if (global::Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding.AdobeGlyphList.IsSupportedPdfName(name))
			{
				char unicode = global::Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding.AdobeGlyphList.GetUnicode(name);
				ushort glyphId;
				initializationInfo.CMap.TryGetGlyphId((int)unicode, out glyphId);
				g.GlyphId = glyphId;
				return;
			}
			g.GlyphId = global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.GetGlyphIdFromPostTable(initializationInfo.FontSource, g.Name);
		}

		private static void InitializeMacintoshNonSymbolicGlyph(global::Telerik.Windows.Documents.Core.Fonts.Glyphs.Glyph g, global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			string name = initializationInfo.Encoding.GetName(b);
			g.Name = name;
			if (name == ".notdef")
			{
				g.GlyphId = global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.GetGlyphIdFromPostTable(initializationInfo.FontSource, name);
				return;
			}
			byte charId = global::Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding.PredefinedSimpleEncoding.StandardMacRomanEncoding.GetCharId(name);
			ushort glyphId;
			initializationInfo.CMap.TryGetGlyphId((int)charId, out glyphId);
			g.GlyphId = glyphId;
		}

		private static void InitializeMicrosoftSymbolicGlyph(global::Telerik.Windows.Documents.Core.Fonts.Glyphs.Glyph g, global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			ushort unicode = global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.AppendByte(initializationInfo.AppendingFirstByte.Value, b);
			ushort glyphId;
			initializationInfo.CMap.TryGetGlyphId((int)unicode, out glyphId);
			g.GlyphId = glyphId;
		}

		private static void InitializeMacintoshSymbolicGlyph(global::Telerik.Windows.Documents.Core.Fonts.Glyphs.Glyph g, global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo initializationInfo, byte b)
		{
			ushort glyphId;
			initializationInfo.CMap.TryGetGlyphId((int)b, out glyphId);
			g.GlyphId = glyphId;
		}

		private static ushort GetGlyphIdFromPostTable(global::Telerik.Windows.Documents.Core.Fonts.OpenType.OpenTypeFontSource fontSource, string name)
		{
			if (fontSource.Post == null)
			{
				return 0;
			}
			return fontSource.Post.GetGlyphId(name);
		}

		private static bool TryCalculateFirstByte(global::Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.CMapTable cMap, out byte firstByte)
		{
			bool result = false;
			firstByte = 0;
			try
			{
				ushort firstCode = cMap.FirstCode;
				global::Telerik.Windows.Documents.Fixed.Model.Data.CharCode charCode = new global::Telerik.Windows.Documents.Fixed.Model.Data.CharCode(firstCode);
				byte[] array = charCode.ToBytes();
				firstByte = array[0];
				result = true;
			}
			catch (global::System.NotSupportedException)
			{
			}
			return result;
		}

		private static ushort AppendByte(byte firstByte, byte b)
		{
			global::Telerik.Windows.Documents.Fixed.Model.Data.CharCode charCode = new global::Telerik.Windows.Documents.Fixed.Model.Data.CharCode(new byte[] { firstByte, b });
			return (ushort)charCode.Code;
		}

		private void EnsureGlyphInitializationInfo()
		{
			if (this.initializationInfo == null)
			{
				global::Telerik.Windows.Documents.Core.Fonts.OpenType.OpenTypeFontSource openTypeFontSource = base.FontSource as global::Telerik.Windows.Documents.Core.Fonts.OpenType.OpenTypeFontSource;
				if (openTypeFontSource == null)
				{
					global::Telerik.Windows.Documents.Core.Fonts.FontProperties fontProperties = new global::Telerik.Windows.Documents.Core.Fonts.FontProperties(new global::System.Windows.Media.FontFamily(global::Telerik.Windows.Documents.Fixed.Model.FixedDocumentDefaults.TrueTypeFallbackFontName));
					openTypeFontSource = global::Telerik.Windows.Documents.Fixed.Model.Fonts.FontsRepository.FontsManager.GetOpenTypeFontSource(fontProperties);
				}
				this.initializationInfo = global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeFont.GetGlyphInitializationInfo(openTypeFontSource, this, base.Encoding.Value);
			}
		}

		private global::Telerik.Windows.Documents.Fixed.Model.Fonts.TrueTypeGlyphInitializationInfo initializationInfo;
	}
}
