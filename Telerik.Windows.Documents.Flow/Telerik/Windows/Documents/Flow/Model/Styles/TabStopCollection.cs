using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class TabStopCollection : IEnumerable<TabStop>, IEnumerable
	{
		public TabStopCollection()
		{
			this.tabStops = new List<TabStop>();
		}

		public TabStopCollection(IEnumerable<TabStop> tabStops)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<TabStop>>(tabStops, "tabStops");
			this.tabStops = new List<TabStop>(tabStops);
		}

		public int Count
		{
			get
			{
				return this.tabStops.Count;
			}
		}

		public static bool operator ==(TabStopCollection tabStops, TabStopCollection otherTabStops)
		{
			return object.ReferenceEquals(tabStops, otherTabStops) || (tabStops != null && otherTabStops != null && tabStops.Equals(otherTabStops));
		}

		public static bool operator !=(TabStopCollection tabStops, TabStopCollection otherTabStops)
		{
			return !(tabStops == otherTabStops);
		}

		public TabStopCollection Insert(TabStop tabStop)
		{
			Guard.ThrowExceptionIfNull<TabStop>(tabStop, "tabStop");
			List<TabStop> list = this.tabStops.ToList<TabStop>();
			list.Add(tabStop);
			return new TabStopCollection(list);
		}

		public TabStopCollection Remove(TabStop tabStop)
		{
			Guard.ThrowExceptionIfNull<TabStop>(tabStop, "tabStop");
			IList<TabStop> list = this.tabStops.ToList<TabStop>();
			list.Remove(tabStop);
			return new TabStopCollection(list);
		}

		public IEnumerator<TabStop> GetEnumerator()
		{
			return this.tabStops.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public override bool Equals(object obj)
		{
			TabStopCollection tabStopCollection = obj as TabStopCollection;
			if (tabStopCollection == null || this.Count != tabStopCollection.Count)
			{
				return false;
			}
			foreach (TabStop value in this.tabStops)
			{
				if (!tabStopCollection.Contains(value))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.tabStops.GetHashCode(), base.GetHashCode());
		}

		internal TabStopCollection Merge(IEnumerable<TabStop> tabStopsToMerge)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<TabStop>>(tabStopsToMerge, "tabStopsToMerge");
			List<TabStop> list = new List<TabStop>(this.tabStops);
			list.AddRange(from ts2 in tabStopsToMerge
				where this.tabStops.All((TabStop ts1) => !ts1.Equals(ts2))
				select ts2);
			return new TabStopCollection(list);
		}

		readonly List<TabStop> tabStops;
	}
}
