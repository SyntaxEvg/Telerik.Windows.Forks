using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Utils;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts
{
	class FontsManager : FontsManagerBase
	{
		public FontsManager()
		{
			this.fonts = new Dictionary<FontProperties, OpenTypeFontSource>();
			this.fallbacks = new Dictionary<FallbackRecord, OpenTypeFontSource>();
		}

		public override OpenTypeFontSource GetOpenTypeFontSource(FontProperties fontProperties)
		{
			OpenTypeFontSource fontSourceFromGlyphTypeface;
			if (!this.fonts.TryGetValue(fontProperties, out fontSourceFromGlyphTypeface))
			{
				GlyphTypeface glyphTypeface = FontsManager.GetGlyphTypeface(fontProperties.FontFamilyName, fontProperties.FontStyle, fontProperties.FontWeight);
				fontSourceFromGlyphTypeface = this.GetFontSourceFromGlyphTypeface(fontProperties.FontFamilyName, glyphTypeface);
				this.fonts[fontProperties] = fontSourceFromGlyphTypeface;
			}
			return fontSourceFromGlyphTypeface;
		}

		public OpenTypeFontSource GetFallbackFontSource(FontProperties descr, string text)
		{
			FallbackRange fallbackRange = FallbackRange.GetFallbackRange(text[0]);
			FallbackRecord key = new FallbackRecord(fallbackRange, descr);
			OpenTypeFontSource fontSourceFromGlyphTypeface;
			if (!this.fallbacks.TryGetValue(key, out fontSourceFromGlyphTypeface))
			{
				foreach (string text2 in fallbackRange.FallbackFontFamilies)
				{
					GlyphTypeface glyphTypeface = FontsManager.GetGlyphTypeface(text2, descr.FontStyle, descr.FontWeight);
					fontSourceFromGlyphTypeface = this.GetFontSourceFromGlyphTypeface(text2, glyphTypeface);
					if (fontSourceFromGlyphTypeface != null)
					{
						break;
					}
				}
				if (fontSourceFromGlyphTypeface == null)
				{
					throw new NotSupportedFontFamilyException();
				}
				this.fallbacks[key] = fontSourceFromGlyphTypeface;
			}
			return fontSourceFromGlyphTypeface;
		}

		static GlyphTypeface GetGlyphTypeface(string familyName, FontStyle style, FontWeight weight)
		{
			Typeface typeface = new Typeface(new FontFamily(familyName), style, weight, FontStretches.Normal);
			GlyphTypeface result;
			typeface.TryGetGlyphTypeface(out result);
			return result;
		}

		static IEnumerable<OpenTypeFontSourceBase> GetFontSources(Stream fontUri)
		{
			OpenTypeFontReader reader = new OpenTypeFontReader(fontUri.ReadAllBytes());
			switch (OpenTypeFontSource.GetFontType(reader))
			{
			case OpenTypeFontSourceType.TrueType:
				yield return FontsManager.ReadTrueTypeFont(reader);
				break;
			case OpenTypeFontSourceType.TrueTypeCollection:
			{
				TrueTypeCollection ttc = FontsManager.ReadTrueTypeCollection(reader);
				foreach (OpenTypeFontSourceBase fs in ttc.Fonts)
				{
					yield return fs;
				}
				break;
			}
			}
			yield break;
		}

		static FontProperties CreateFontDescriptorFromFontSource(FontSource source)
		{
			FontStyle fontStyle = (source.IsItalic ? FontStyles.Italic : FontStyles.Normal);
			FontWeight fontWeight = (source.IsBold ? FontWeights.Bold : FontWeights.Normal);
			return new FontProperties(new FontFamily(source.FontFamily), fontStyle, fontWeight);
		}

		static OpenTypeFontSourceBase ReadTrueTypeFont(OpenTypeFontReader reader)
		{
			return new OpenTypeFontSource(reader);
		}

		static TrueTypeCollection ReadTrueTypeCollection(OpenTypeFontReader reader)
		{
			TrueTypeCollection trueTypeCollection = new TrueTypeCollection(reader);
			trueTypeCollection.Initialize();
			return trueTypeCollection;
		}

		void RegisterFontSource(FontProperties descr, OpenTypeFontSource source)
		{
			this.fonts[descr] = source;
		}

		OpenTypeFontSource GetFontSourceFromGlyphTypeface(string fontFamily, GlyphTypeface gtf)
		{
			OpenTypeFontSource result = null;
			if (gtf == null)
			{
				return null;
			}
			bool flag = gtf.Weight == FontWeights.Bold && gtf.StyleSimulations != StyleSimulations.BoldItalicSimulation && gtf.StyleSimulations != StyleSimulations.BoldSimulation;
			bool flag2 = gtf.Style != FontStyles.Normal && gtf.StyleSimulations != StyleSimulations.BoldItalicSimulation && gtf.StyleSimulations != StyleSimulations.ItalicSimulation;
			foreach (OpenTypeFontSourceBase openTypeFontSourceBase in FontsManager.GetFontSources(gtf.GetFontStream()))
			{
				OpenTypeFontSource openTypeFontSource = (OpenTypeFontSource)openTypeFontSourceBase;
				if (openTypeFontSource.FontFamily == fontFamily && flag == openTypeFontSource.IsBold && flag2 == openTypeFontSource.IsItalic)
				{
					result = openTypeFontSource;
				}
				else
				{
					this.RegisterFontSource(FontsManager.CreateFontDescriptorFromFontSource(openTypeFontSource), openTypeFontSource);
				}
			}
			return result;
		}

		readonly Dictionary<FontProperties, OpenTypeFontSource> fonts;

		readonly Dictionary<FallbackRecord, OpenTypeFontSource> fallbacks;
	}
}
