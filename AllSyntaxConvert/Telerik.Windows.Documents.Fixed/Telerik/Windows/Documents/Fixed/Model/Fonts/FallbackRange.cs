using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class FallbackRange
	{
		public static FallbackRange DefaultRange
		{
			get
			{
				return new FallbackRange(new Range[]
				{
					new Range(8192, 8238),
					new Range(8240, 8399),
					new Range(8448, 9215),
					new Range(9312, 10175)
				}, new string[] { "Verdana", "Segoe UI Symbol", "SimSun", "MS Gothic", "MingLiU", "Tahoma", "Lucida Sans", "Unicode", "Arial", "Arial Unicode MS" });
			}
		}

		static void InitializeFallbackRanges()
		{
			FallbackRange.fallbackRanges = new List<FallbackRange>();
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(1536, 1791),
				new Range(1872, 1919),
				new Range(64336, 64975),
				new Range(65008, 65023),
				new Range(65136, 65278)
			}, new string[] { "Microsoft Uighur", "Segoe UI", "Tahoma", "Simplified Arabic", "Traditional Arabic", "Arial", "Arial Unicode MS" }));
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(0, 1327),
				new Range(1424, 1791),
				new Range(1872, 1919),
				new Range(7424, 8191),
				new Range(11360, 11391),
				new Range(64256, 64271),
				new Range(64285, 64511)
			}, new string[] { "Segoe UI", "Tahoma", "Arial", "Arial Unicode MS", "Microsoft Sans Serif", "Lucida Sans Unicode" }));
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(64512, 64975),
				new Range(65008, 65023),
				new Range(65056, 65071),
				new Range(65136, 65278)
			}, new string[] { "Tahoma", "Simplified Arabic", "Traditional Arabic", "Arial Unicode MS" }));
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(1328, 1423),
				new Range(4256, 4303),
				new Range(4304, 4351),
				new Range(64272, 64284)
			}, new string[] { "Sylfaen", "Arial Unicode MS" }));
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(4352, 4607),
				new Range(12592, 12687),
				new Range(12800, 12831),
				new Range(12896, 12927),
				new Range(44032, 55215)
			}, new string[] { "Malgun Gothic", "Gulim" }));
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(8192, 8238),
				new Range(8240, 8399),
				new Range(8448, 9215),
				new Range(9312, 10175)
			}, new string[]
			{
				"Microsoft YaHei", "Meiryo", "Segoe UI", "Segoe UI Symbol", "SimSun", "MS Gothic", "MingLiU", "Tahoma", "Lucida Sans", "Unicode",
				"Arial", "Arial Unicode MS"
			}));
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(10176, 10623),
				new Range(10752, 11263)
			}, new string[] { "Segoe UI Symbol", "Cambria Math" }));
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(11904, 12031)
			}, new string[] { "Microsoft YaHei", "SimSun", "Meiryo", "MingLiu" }));
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(12288, 12543),
				new Range(12784, 12799)
			}, new string[] { "Meiryo", "Segoe UI", "Microsoft JhengHei", "MingLiu", "MS Gothic", "Arial Unicode MS" }));
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(13312, 19903),
				new Range(19968, 40959)
			}, new string[] { "Meiryo", "Segoe UI", "Microsoft YaHei", "SimSun", "SimSun-18030", "SimSun-ExtB", "MingLiU" }));
		}

		internal static FallbackRange GetFallbackRange(char unicode)
		{
			foreach (FallbackRange fallbackRange in FallbackRange.fallbackRanges)
			{
				if (fallbackRange.FallsInRange(unicode))
				{
					return fallbackRange;
				}
			}
			return null;
		}

		static FallbackRange()
		{
			FallbackRange.InitializeFallbackRanges();
		}

		public FallbackRange(Range[] ranges, string[] fallbackFontFamilies)
		{
			this.Ranges = ranges;
			this.fonts = new Dictionary<FontProperties, FontBase>();
			this.fallbackFontFamilies = fallbackFontFamilies;
		}

		public Range[] Ranges { get; set; }

		FontBase GetFont(string fontFamily, FontStyle fontStyle, FontWeight fontWeight)
		{
			FontProperties key = new FontProperties(new FontFamily(fontFamily), fontStyle, fontWeight);
			FontBase fontBase;
			if (!this.fonts.TryGetValue(key, out fontBase))
			{
				lock (this.lockObject)
				{
					if (!this.fonts.TryGetValue(key, out fontBase) && FontsRepository.TryCreateFont(new FontFamily(fontFamily), fontStyle, fontWeight, out fontBase))
					{
						this.fonts[key] = fontBase;
					}
				}
			}
			return fontBase;
		}

		public FontBase GetFallbackFont(FontBase original, char ch)
		{
			FontStyle fontStyle = (original.IsItalic ? FontStyles.Italic : FontStyles.Normal);
			FontWeight fontWeight = (original.IsBold ? FontWeights.Bold : FontWeights.Normal);
			foreach (string fontFamily in this.fallbackFontFamilies)
			{
				FontBase font = this.GetFont(fontFamily, fontStyle, fontWeight);
				ushort num;
				if (font != null && font.TryGetGlyphId((int)ch, out num))
				{
					return font;
				}
			}
			return this.GetFont("Arial", fontStyle, fontWeight);
		}

		bool FallsInRange(char unicode)
		{
			foreach (Range range in this.Ranges)
			{
				if (range.IsInRange((int)unicode))
				{
					return true;
				}
			}
			return false;
		}

		const string DefaultFallbackFont = "Arial";

		readonly object lockObject = new object();

		readonly Dictionary<FontProperties, FontBase> fonts;

		readonly string[] fallbackFontFamilies;

		static List<FallbackRange> fallbackRanges;
	}
}
