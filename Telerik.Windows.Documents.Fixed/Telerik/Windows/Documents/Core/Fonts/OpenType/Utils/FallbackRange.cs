using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Utils
{
	class FallbackRange
	{
		static void InitializeFallbackRanges()
		{
			FallbackRange.fallbackRanges = new List<FallbackRange>();
			FallbackRange.fallbackRanges.Add(new FallbackRange(new Range[]
			{
				new Range(0, 591),
				new Range(1024, 1327)
			}, new string[] { "Times New Roman" }));
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
			this.FallbackFontFamilies = fallbackFontFamilies;
		}

		public Range[] Ranges { get; set; }

		public string[] FallbackFontFamilies { get; set; }

		public bool FallsInRange(char unicode)
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

		static List<FallbackRange> fallbackRanges;
	}
}
