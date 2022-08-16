using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding
{
	class DifferencesRanges : IEnumerable<DifferencesRange>, IEnumerable
	{
		public DifferencesRanges()
		{
			this.ranges = new List<DifferencesRange>();
		}

		public DifferencesRange AddDifferencesRange(byte startCharCode)
		{
			DifferencesRange differencesRange = new DifferencesRange(startCharCode);
			this.ranges.Add(differencesRange);
			return differencesRange;
		}

		public IEnumerator GetEnumerator()
		{
			foreach (DifferencesRange range in this.ranges)
			{
				yield return range;
			}
			yield break;
		}

		IEnumerator<DifferencesRange> IEnumerable<DifferencesRange>.GetEnumerator()
		{
			foreach (DifferencesRange range in this.ranges)
			{
				yield return range;
			}
			yield break;
		}

		readonly List<DifferencesRange> ranges;
	}
}
