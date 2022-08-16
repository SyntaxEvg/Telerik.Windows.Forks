using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Utils
{
	class UnicodeRange
	{
		static int UnicodeRangeComparison(UnicodeRange range, char unicode)
		{
			if ((int)unicode < range.Range.RangeStart)
			{
				return -1;
			}
			if (range.Range.RangeStart <= (int)unicode && (int)unicode <= range.Range.RangeEnd)
			{
				return 0;
			}
			return 1;
		}

		static int UnicodeRangeComparison(UnicodeRange left, UnicodeRange range)
		{
			return left.Range.RangeStart.CompareTo(range.Range.RangeStart);
		}

		static void InitializeUnicodeRanges()
		{
			UnicodeRange.unicodeRanges = new List<UnicodeRange>();
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("arab", 1536, 1791, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("armn", 1328, 1423, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("avst", 68352, 68415, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("bali", 6912, 7039, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("bamu", 42656, 42751, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("batk", 7104, 7167, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("beng", 2432, 2559, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("bopo", 12544, 12799, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("brah", 69632, 69759, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("brai", 10240, 10495, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("bugi", 6656, 6687, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("buhd", 5952, 5983, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("byzm", 118784, 119039, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("cans", 5120, 5759, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("cari", 66208, 4319, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("cakm", 69888, 69967, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("cham", 43520, 43615, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("cher", 5024, 5119, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("hani", 19968, 40959, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("copt", 11392, 11519, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("cprt", 67584, 67647, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("cyrl", 1024, 1279, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("dsrt", 66560, 66639, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("deva", 2304, 2431, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("egyp", 77824, 78895, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("ethi", 4608, 4991, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("geor", 4256, 4351, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("glag", 11264, 11359, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("goth", 66352, 66383, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("grek", 880, 1023, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("gujr", 2688, 2815, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("guru", 2560, 2687, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("hang", 44032, 55215, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("jamo", 4352, 4607, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("hano", 5920, 5951, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("hebr", 1424, 1535, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("kana", 12352, 12447, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("armi", 67648, 67679, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("phli", 68448, 68479, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("prti", 68416, 265055, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("java", 43392, 43487, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("kthi", 69760, 69839, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("knda", 3200, 3327, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("kana", 12448, 12543, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("kali", 43264, 43311, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("khar", 68096, 68191, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("khmr", 6016, 6143, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("lao ", 3712, 3839, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("latn", 0, 127, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("lepc", 7168, 7247, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("limb", 6400, 6479, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("linb", 65536, 65663, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("lisu", 42192, 42239, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("lyci", 66176, 66207, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("lydi", 67872, 67903, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("mlym", 3328, 3455, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("mlm2", 3328, 3455, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("mand", 2112, 2143, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("math", 119808, 120831, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("mtei", 43968, 44031, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("merc", 68000, 68095, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("mero", 67968, 67999, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("mong", 6144, 6319, TextDirection.TopToBottom));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("musc", 119040, 119295, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("mymr", 4096, 4255, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("talu", 6528, 6623, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("nko ", 1984, 1999, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("orya", 2816, 2943, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("ogam", 5760, 5791, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("olck", 7248, 7295, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("ital", 66304, 66351, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("xpeo", 66464, 66527, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("sarb", 68192, 68223, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("orkh", 68608, 68687, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("osma", 66688, 66735, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("phag", 43072, 43135, TextDirection.TopToBottom));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("phnx", 67840, 67871, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("rjng", 43312, 43359, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("runr", 5792, 5887, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("samr", 2048, 2111, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("saur", 43136, 43231, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("shrd", 70016, 70111, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("shaw", 66640, 66687, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("sinh", 3456, 3583, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("sora", 69840, 69887, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("sund", 7040, 7103, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("sylo", 43008, 43055, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("syrc", 1792, 1871, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("tglg", 5888, 5919, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("tagb", 5984, 6015, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("tale", 6480, 6527, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("lana", 6688, 6831, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("tavt", 43648, 43743, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("takr", 71296, 71375, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("taml", 2944, 3071, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("telu", 3072, 3199, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("thaa", 1920, 1983, TextDirection.RightToLeft));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("thai", 3584, 3711, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("tibt", 3840, 4095, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("tfng", 11568, 11647, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("ugar", 66432, 66463, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("vai ", 42240, 42559, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Add(new UnicodeRange("yi  ", 40960, 42191, TextDirection.LeftToRight));
			UnicodeRange.unicodeRanges.Sort(new Comparison<UnicodeRange>(UnicodeRange.UnicodeRangeComparison));
		}

		internal static UnicodeRange GetUnicodeRange(char unicode)
		{
			UnicodeRange unicodeRange = CollectionsExtensions.FindElement<UnicodeRange, char>(UnicodeRange.unicodeRanges, unicode, new CompareDelegate<UnicodeRange, char>(UnicodeRange.UnicodeRangeComparison));
			if (unicodeRange.Range.RangeStart <= (int)unicode && (int)unicode <= unicodeRange.Range.RangeEnd)
			{
				return unicodeRange;
			}
			return UnicodeRange.defaultRange;
		}

		static UnicodeRange()
		{
			UnicodeRange.InitializeUnicodeRanges();
		}

		public UnicodeRange(string tag)
		{
			this.Tag = Tags.GetTagFromString(tag);
			this.Direction = TextDirection.LeftToRight;
			this.Range = new Range();
		}

		public UnicodeRange(string tag, int rangeStart, int rangeEnd, TextDirection direction)
		{
			this.Tag = Tags.GetTagFromString(tag);
			this.Range = new Range(rangeStart, rangeEnd);
			this.Direction = direction;
		}

		public Range Range { get; set; }

		public uint Tag { get; set; }

		public TextDirection Direction { get; set; }

		public override string ToString()
		{
			return Tags.GetStringFromTag(this.Tag);
		}

		static List<UnicodeRange> unicodeRanges;

		static UnicodeRange defaultRange = new UnicodeRange("DFLT");
	}
}
