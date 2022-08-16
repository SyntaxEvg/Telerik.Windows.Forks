using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Core.DataStructures
{
	class RangeTreeCollection<TIndex, TValue> : AATree<Range<TIndex, TValue>> where TIndex : IComparable<TIndex>
	{
		void GetIntersectingRanges(AATree<Range<TIndex, TValue>>.AATreeNode node, Range<TIndex, TValue> range, ref List<Range<TIndex, TValue>> intersectingRanges)
		{
			if (node == this.sentinel)
			{
				return;
			}
			TIndex start = node.value.Start;
			if (start.CompareTo(range.Start) > 0)
			{
				this.GetIntersectingRanges(node.left, range, ref intersectingRanges);
			}
			if (node.value.IntersectsWith(range))
			{
				intersectingRanges.Add(node.value);
			}
			TIndex end = node.value.End;
			if (end.CompareTo(range.End) < 0)
			{
				this.GetIntersectingRanges(node.right, range, ref intersectingRanges);
			}
		}

		internal List<Range<TIndex, TValue>> GetIntersectingRanges(Range<TIndex, TValue> range)
		{
			List<Range<TIndex, TValue>> result = new List<Range<TIndex, TValue>>();
			this.GetIntersectingRanges(this.root, range, ref result);
			return result;
		}

		Range<TIndex, TValue> GetContainingRange(AATree<Range<TIndex, TValue>>.AATreeNode node, TIndex position)
		{
			while (node != this.sentinel)
			{
				if (position.CompareTo(node.value.Start) < 0)
				{
					node = node.left;
				}
				else
				{
					if (position.CompareTo(node.value.End) <= 0)
					{
						return node.value;
					}
					node = node.right;
				}
			}
			return null;
		}

		internal Range<TIndex, TValue> GetContainingRange(TIndex position)
		{
			return this.GetContainingRange(this.root, position);
		}
	}
}
