using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public abstract class SeriesGroup
	{
		public abstract SeriesType SeriesType { get; }

		public SeriesCollection Series
		{
			get
			{
				return this.SeriesInternal;
			}
		}

		internal abstract SeriesCollection SeriesInternal { get; }

		internal virtual int DataRangesPerSeries
		{
			get
			{
				return 1;
			}
		}

		internal virtual bool HasCategories
		{
			get
			{
				return true;
			}
		}

		public SeriesGroup Clone()
		{
			SeriesGroup seriesGroup = (SeriesGroup)Activator.CreateInstance(base.GetType());
			this.CloneProperties(seriesGroup);
			return seriesGroup;
		}

		internal virtual void CloneProperties(SeriesGroup group)
		{
			foreach (SeriesBase seriesBase in this.Series)
			{
				group.Series.Add(seriesBase.Clone());
			}
		}

		internal abstract event EventHandler SeriesGroupChanged;
	}
}
