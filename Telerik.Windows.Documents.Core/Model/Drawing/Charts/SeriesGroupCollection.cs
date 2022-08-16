using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class SeriesGroupCollection : IEnumerable<SeriesGroup>, IEnumerable
	{
		public SeriesGroupCollection()
		{
			this.innerList = new List<SeriesGroup>();
		}

		public void Add(SeriesGroup seriesGroup)
		{
			if (seriesGroup == null)
			{
				throw new ArgumentNullException("seriesGroup");
			}
			this.innerList.Add(seriesGroup);
			seriesGroup.SeriesGroupChanged += this.SeriesGroup_SeriesGroupChanged;
			this.OnCollectionChanged();
		}

		public void Remove(SeriesGroup seriesGroup)
		{
			if (this.innerList.Contains(seriesGroup))
			{
				seriesGroup.SeriesGroupChanged -= this.SeriesGroup_SeriesGroupChanged;
				this.innerList.Remove(seriesGroup);
				this.OnCollectionChanged();
			}
		}

		public IEnumerator<SeriesGroup> GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		void SeriesGroup_SeriesGroupChanged(object sender, EventArgs e)
		{
			this.OnSeriesGroupsChanged();
		}

		internal event EventHandler CollectionChanged;

		void OnCollectionChanged()
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, EventArgs.Empty);
			}
		}

		internal event EventHandler SeriesGroupsChanged;

		void OnSeriesGroupsChanged()
		{
			if (this.SeriesGroupsChanged != null)
			{
				this.SeriesGroupsChanged(this, EventArgs.Empty);
			}
		}

		readonly List<SeriesGroup> innerList;
	}
}
