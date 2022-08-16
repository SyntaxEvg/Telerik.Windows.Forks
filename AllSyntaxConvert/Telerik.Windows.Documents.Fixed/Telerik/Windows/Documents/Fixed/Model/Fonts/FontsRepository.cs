using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	public static class FontsRepository
	{
		private const double HelveticaDescent = -211.0;

		private const double HelveticaAscent = 905.0;

		private const double HelveticaLineGap = 32.0;

		private const double TimesDescent = -216.0;

		private const double TimesAscent = 891.0;

		private const double TimesLineGap = 42.0;

		private const double CourierDescent = -300.0;

		private const double CourierAscent = 832.0;

		private const double CourierLineGap = 0.0;

		private const double SymbolDescent = -219.0;

		private const double SymbolAscent = 1005.0;

		private const double SymbolLineGap = 0.0;

		private const double ZapfDingbatsDescent = -220.0;

		private const double ZapfDingbatsAscent = 1000.0;

		private const double ZapfDingbatsLineGap = 0.0;

		private static readonly StandardFont helvetica;

		private static readonly StandardFont helveticaBold;

		private static readonly StandardFont helveticaOblique;

		private static readonly StandardFont helveticaBoldOblique;

		private static readonly StandardFont courier;

		private static readonly StandardFont courierBold;

		private static readonly StandardFont courierOblique;

		private static readonly StandardFont courierBoldOblique;

		private static readonly StandardFont timesRoman;

		private static readonly StandardFont timesBold;

		private static readonly StandardFont timesItalic;

		private static readonly StandardFont timesBoldItalic;

		private static readonly StandardFont symbol;

		private static readonly StandardFont zapfDingbats;


		private static readonly object lockObject = new object();

		private static  Dictionary<FontProperties, FontBase> fontPropertiesToFont = new Dictionary<FontProperties, FontBase>();

		private static  Dictionary<string, Func<StandardFont>> standardFontCreators = new Dictionary<string, Func<StandardFont>>();
		public static bool TryCreateFont(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, out FontBase font)
		{
			FontProperties fontProperties = new FontProperties(fontFamily, fontStyle, fontWeight);
			if (!FontsRepository.fontPropertiesToFont.TryGetValue(fontProperties, out font))
			{
				lock (FontsRepository.lockObject)
				{
					if (!FontsRepository.fontPropertiesToFont.TryGetValue(fontProperties, out font))
					{
						Typeface typeface = new Typeface(fontProperties.FontFamily, fontProperties.FontStyle, fontProperties.FontWeight, FontStretches.Normal);
						GlyphTypeface glyphTypeface;
						if (typeface.TryGetGlyphTypeface(out glyphTypeface))
						{
							Stream fontStream = glyphTypeface.GetFontStream();
							font = FontsRepository.CreateFont(fontProperties, fontStream.ReadAllBytes());
						}
						if (font != null)
						{
							if (font.Type == FontType.CidType2Font)
							{
								((CidType2Font)font).GlyphTypeface = glyphTypeface;
							}
							FontsRepository.RegisterFont(fontProperties, font);
						}
						return font != null;
					}
				}
				return true;
			}
			return true;
		}

		public static bool TryCreateFont(FontFamily fontFamily, out FontBase font)
		{
			return FontsRepository.TryCreateFont(fontFamily, FontStyles.Normal, FontWeights.Normal, out font);
		}

		static FontsRepository()
		{
			FontsRepository.RegisterStandartFontCreators();
			FontsRepository.helvetica = FontsRepository.CreateStandardFontByName("Helvetica");
			FontsRepository.helveticaBold = FontsRepository.CreateStandardFontByName("Helvetica-Bold");
			FontsRepository.helveticaOblique = FontsRepository.CreateStandardFontByName("Helvetica-Oblique");
			FontsRepository.helveticaBoldOblique = FontsRepository.CreateStandardFontByName("Helvetica-BoldOblique");
			FontsRepository.courier = FontsRepository.CreateStandardFontByName("Courier");
			FontsRepository.courierBold = FontsRepository.CreateStandardFontByName("Courier-Bold");
			FontsRepository.courierOblique = FontsRepository.CreateStandardFontByName("Courier-Oblique");
			FontsRepository.courierBoldOblique = FontsRepository.CreateStandardFontByName("Courier-BoldOblique");
			FontsRepository.timesRoman = FontsRepository.CreateStandardFontByName("Times-Roman");
			FontsRepository.timesBold = FontsRepository.CreateStandardFontByName("Times-Bold");
			FontsRepository.timesItalic = FontsRepository.CreateStandardFontByName("Times-Italic");
			FontsRepository.timesBoldItalic = FontsRepository.CreateStandardFontByName("Times-BoldItalic");
			FontsRepository.symbol = FontsRepository.CreateStandardFontByName("Symbol");
			FontsRepository.zapfDingbats = FontsRepository.CreateStandardFontByName("ZapfDingbats");
			FontsRepository.RegisterStandartFonts();
		}

		public static FontBase Symbol
		{
			get
			{
				return FontsRepository.symbol;
			}
		}

		public static FontBase ZapfDingbats
		{
			get
			{
				return FontsRepository.zapfDingbats;
			}
		}

		public static FontBase TimesRoman
		{
			get
			{
				return FontsRepository.timesRoman;
			}
		}

		public static FontBase TimesBold
		{
			get
			{
				return FontsRepository.timesBold;
			}
		}

		public static FontBase TimesBoldItalic
		{
			get
			{
				return FontsRepository.timesBoldItalic;
			}
		}

		public static FontBase TimesItalic
		{
			get
			{
				return FontsRepository.timesItalic;
			}
		}

		public static FontBase Helvetica
		{
			get
			{
				return FontsRepository.helvetica;
			}
		}

		public static FontBase HelveticaBold
		{
			get
			{
				return FontsRepository.helveticaBold;
			}
		}

		public static FontBase HelveticaOblique
		{
			get
			{
				return FontsRepository.helveticaOblique;
			}
		}

		public static FontBase HelveticaBoldOblique
		{
			get
			{
				return FontsRepository.helveticaBoldOblique;
			}
		}

		public static FontBase Courier
		{
			get
			{
				return FontsRepository.courier;
			}
		}

		public static FontBase CourierBold
		{
			get
			{
				return FontsRepository.courierBold;
			}
		}

		public static FontBase CourierOblique
		{
			get
			{
				return FontsRepository.courierOblique;
			}
		}

		public static FontBase CourierBoldOblique
		{
			get
			{
				return FontsRepository.courierBoldOblique;
			}
		}

		internal static FontsManager FontsManager { get; set; } = new FontsManager();

		public static void RegisterFont(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, byte[] data)
		{
			Guard.ThrowExceptionIfNull<FontFamily>(fontFamily, "fontFamily");
			Guard.ThrowExceptionIfNull<FontStyle>(fontStyle, "fontStyle");
			Guard.ThrowExceptionIfNull<FontWeight>(fontWeight, "fontWeight");
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			FontProperties fontProperties = new FontProperties(fontFamily, fontStyle, fontWeight);
			FontsRepository.RegisterFont(fontProperties, FontsRepository.CreateFont(fontProperties, data));
		}

		internal static bool TryCreateStandardFont(string fontName, out StandardFont font)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			Func<StandardFont> func;
			if (FontsRepository.standardFontCreators.TryGetValue(fontName, out func))
			{
				font = func();
				return true;
			}
			font = null;
			return false;
		}

		private static void RegisterStandartFontCreators()
		{
			FontsRepository.RegisterStandardFontCreator("Helvetica", 905.0, -211.0, 32.0, false, false);
			FontsRepository.RegisterStandardFontCreator("Helvetica-Bold", 905.0, -211.0, 32.0, true, false);
			FontsRepository.RegisterStandardFontCreator("Helvetica-Oblique", 905.0, -211.0, 32.0, false, true);
			FontsRepository.RegisterStandardFontCreator("Helvetica-BoldOblique", 905.0, -211.0, 32.0, true, true);
			FontsRepository.RegisterStandardFontCreator("Courier", 832.0, -300.0, 0.0, false, false);
			FontsRepository.RegisterStandardFontCreator("Courier-Bold", 832.0, -300.0, 0.0, true, false);
			FontsRepository.RegisterStandardFontCreator("Courier-Oblique", 832.0, -300.0, 0.0, false, true);
			FontsRepository.RegisterStandardFontCreator("Courier-BoldOblique", 832.0, -300.0, 0.0, true, true);
			FontsRepository.RegisterStandardFontCreator("Times-Roman", 891.0, -216.0, 42.0, false, false);
			FontsRepository.RegisterStandardFontCreator("Times-Bold", 891.0, -216.0, 42.0, true, false);
			FontsRepository.RegisterStandardFontCreator("Times-Italic", 891.0, -216.0, 42.0, false, true);
			FontsRepository.RegisterStandardFontCreator("Times-BoldItalic", 891.0, -216.0, 42.0, true, true);
			FontsRepository.RegisterStandardFontCreator("Symbol", 1005.0, -219.0, 0.0, false, false);
			FontsRepository.RegisterStandardFontCreator("ZapfDingbats", 1000.0, -220.0, 0.0, false, false);
		}

		private static void RegisterStandartFonts()
		{
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Helvetica")), FontsRepository.helvetica);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Helvetica"), FontStyles.Normal, FontWeights.Bold), FontsRepository.helveticaBold);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Helvetica"), FontStyles.Italic, FontWeights.Normal), FontsRepository.helveticaOblique);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Helvetica"), FontStyles.Italic, FontWeights.Bold), FontsRepository.helveticaBoldOblique);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Courier"), FontStyles.Normal, FontWeights.Normal), FontsRepository.courier);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Courier"), FontStyles.Normal, FontWeights.Bold), FontsRepository.courierBold);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Courier"), FontStyles.Italic, FontWeights.Normal), FontsRepository.courierOblique);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Courier"), FontStyles.Italic, FontWeights.Bold), FontsRepository.courierBoldOblique);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Times-Roman"), FontStyles.Normal, FontWeights.Normal), FontsRepository.timesRoman);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Times-Roman"), FontStyles.Normal, FontWeights.Bold), FontsRepository.timesBold);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Times-Roman"), FontStyles.Italic, FontWeights.Normal), FontsRepository.timesItalic);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Times-Roman"), FontStyles.Italic, FontWeights.Bold), FontsRepository.timesBoldItalic);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("Symbol"), FontStyles.Normal, FontWeights.Normal), FontsRepository.symbol);
			FontsRepository.RegisterFont(new FontProperties(new FontFamily("ZapfDingbats"), FontStyles.Normal, FontWeights.Normal), FontsRepository.zapfDingbats);
		}

		private static StandardFont CreateStandardFont(string fontName, double ascent, double descent, double lineGap, bool isBold, bool isItalic)
		{
			return new StandardFont(fontName, ascent, descent, lineGap, isBold, isItalic, FontsRepository.FontsManager.GetStandardFontSource(fontName));
		}

		private static void RegisterFont(FontProperties fontProperties, FontBase font)
		{
			Guard.ThrowExceptionIfNull<FontProperties>(fontProperties, "fontProperties");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			FontsRepository.fontPropertiesToFont[fontProperties] = font;
		}

		private static void RegisterStandardFontCreator(string fontName, double ascent, double descent, double lineGap, bool isBold, bool isItalic)
		{
			FontsRepository.standardFontCreators[fontName] = () => FontsRepository.CreateStandardFont(fontName, ascent, descent, lineGap, isBold, isItalic);
		}

		private static StandardFont CreateStandardFontByName(string fontName)
		{
			var t =FontsRepository.standardFontCreators[fontName];
			return t();
		}

		private static FontBase CreateFont(FontProperties fontProperties, byte[] data)
		{
			OpenTypeFontReader reader = new OpenTypeFontReader(data);
			FontBase result;
			switch (OpenTypeFontSource.GetFontType(reader))
			{
				case OpenTypeFontSourceType.TrueType:
					result = FontsRepository.CreateFontFromOpenTypeFontSource(new OpenTypeFontSource(fontProperties, reader));
					break;
				case OpenTypeFontSourceType.TrueTypeCollection:
					result = FontsRepository.CreateFontFromTrueTypeCollection(fontProperties, new TrueTypeCollection(reader));
					break;
				default:
					result = null;
					break;
			}
			return result;
		}

		private static FontBase CreateFontFromTrueTypeCollection(FontProperties fontProperties, TrueTypeCollection trueTypeCollection)
		{
			trueTypeCollection.Initialize();
			foreach (OpenTypeFontSourceBase openTypeFontSourceBase in trueTypeCollection.Fonts)
			{
				OpenTypeFontSource openTypeFontSource = (OpenTypeFontSource)openTypeFontSourceBase;
				if (openTypeFontSource.FontFamily == fontProperties.FontFamilyName)
				{
					return FontsRepository.CreateFontFromOpenTypeFontSource(openTypeFontSource);
				}
			}
			return null;
		}

		private static FontBase CreateFontFromOpenTypeFontSource(OpenTypeFontSource fontSource)
		{
			return new CidType2Font(FontsRepository.CreateFontName(fontSource), fontSource);
		}

		private static string CreateFontName(OpenTypeFontSource fontSource)
		{
			string text = fontSource.FontFamily;
			if (fontSource.IsBold)
			{
				if (fontSource.IsItalic)
				{
					text += ",BoldItalic";
				}
				else
				{
					text += ",Bold";
				}
			}
			else if (fontSource.IsItalic)
			{
				text += ",Italic";
			}
			return text;
		}

	}
}
