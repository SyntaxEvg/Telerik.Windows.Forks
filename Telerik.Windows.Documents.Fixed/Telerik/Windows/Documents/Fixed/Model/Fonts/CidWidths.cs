using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class CidWidths<T> : IEnumerable<CidWidthsRange<T>>, IEnumerable
	{
		public CidWidths()
		{
			this.ranges = new List<CidWidthsRange<T>>();
		}

		public void Add(CidWidthsRange<T> range)
		{
			this.ranges.Add(range);
		}

		public IEnumerator<CidWidthsRange<T>> GetEnumerator()
		{
			foreach (CidWidthsRange<T> range in this.ranges)
			{
				yield return range;
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (CidWidthsRange<T> range in this.ranges)
			{
				yield return range;
			}
			yield break;
		}

		readonly List<CidWidthsRange<T>> ranges;
	}
}
