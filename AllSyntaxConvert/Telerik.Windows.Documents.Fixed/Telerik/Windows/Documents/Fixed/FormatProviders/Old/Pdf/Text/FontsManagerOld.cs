using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	class FontsManagerOld
	{
		public FontsManagerOld()
		{
			this.encodings = new Dictionary<FontBaseOld, EncodingOld>();
			this.type0FontsEncodings = new Dictionary<Type0FontOld, CMapOld>();
			this.unicodes = new Dictionary<FontBaseOld, CMapOld>();
		}

		public FontsManager SystemFontsManager
		{
			get
			{
				if (this.systemFontsManager == null)
				{
					this.systemFontsManager = new FontsManager();
				}
				return this.systemFontsManager;
			}
		}

		public void SetEncoding(FontBaseOld font, EncodingOld encoding)
		{
			Guard.ThrowExceptionIfNull<FontBaseOld>(font, "font");
			this.encodings[font] = encoding;
		}

		public CMapOld GetType0FontEncoding(Type0FontOld font)
		{
			Guard.ThrowExceptionIfNull<Type0FontOld>(font, "font");
			CMapOld cmapOld;
			if (!this.type0FontsEncodings.TryGetValue(font, out cmapOld))
			{
				if (font.Encoding == null)
				{
					cmapOld = null;
				}
				else if (font.Encoding.IsIdentityH || font.Encoding.IsIdentityV)
				{
					cmapOld = CMapOld.Identity;
				}
				else
				{
					CMapStreamParser cmapStreamParser = new CMapStreamParser(font.ContentManager, font.Encoding);
					cmapOld = cmapStreamParser.ParseCMap();
				}
				this.type0FontsEncodings[font] = cmapOld;
			}
			return cmapOld;
		}

		public CMapOld GetFontToUnicode(FontBaseOld font)
		{
			Guard.ThrowExceptionIfNull<FontBaseOld>(font, "font");
			CMapOld cmapOld = null;
			if (!this.unicodes.TryGetValue(font, out cmapOld))
			{
				if (font.ToUnicode != null)
				{
					if (font.ToUnicode.IsIdentityH || font.ToUnicode.IsIdentityV)
					{
						cmapOld = CMapOld.Identity;
					}
					else
					{
						CMapStreamParser cmapStreamParser = new CMapStreamParser(font.ContentManager, font.ToUnicode);
						cmapOld = cmapStreamParser.ParseCMap();
					}
				}
				this.unicodes[font] = cmapOld;
			}
			return cmapOld;
		}

		public OpenTypeFontSource GetTrueTypeFontSource(string fontFamily, FontStyle style, FontWeight weight)
		{
			FontProperties fontProperties = new FontProperties(new FontFamily(fontFamily), style, weight);
			return this.SystemFontsManager.GetOpenTypeFontSource(fontProperties);
		}

		public OpenTypeFontSource GetTrueTypeFontSource(string fontName)
		{
			string fontFamily;
			FontWeight weight;
			FontStyle style;
			FontsHelper.GetFontFamily(fontName, out fontFamily, out weight, out style);
			return this.GetTrueTypeFontSource(fontFamily, style, weight);
		}

		readonly Dictionary<FontBaseOld, EncodingOld> encodings;

		readonly Dictionary<Type0FontOld, CMapOld> type0FontsEncodings;

		readonly Dictionary<FontBaseOld, CMapOld> unicodes;

		FontsManager systemFontsManager;
	}
}
