using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public abstract class SeriesGroup<T> : SeriesGroup where T : SeriesBase
	{
		protected SeriesGroup(SeriesType seriesType)
		{
			this.series = new SeriesCollection<T>(seriesType);
			this.series.CollectionChanged += this.Series_CollectionChanged;
			this.series.SeriesChanged += this.Series_SeriesChanged;
		}

		public new SeriesCollection<T> Series
		{
			get
			{
				return this.SeriesInternal as SeriesCollection<T>;
			}
		}

		internal override SeriesCollection SeriesInternal
		{
			get
			{
				return this.series;
			}
		}

		void Series_SeriesChanged(object sender, EventArgs e)
		{
			this.OnSeriesGroupChanged();
		}

		void Series_CollectionChanged(object sender, EventArgs e)
		{
			this.OnSeriesGroupChanged();
		}

		internal override event EventHandler SeriesGroupChanged;

		internal virtual void OnSeriesGroupChanged()
		{
			if (this.SeriesGroupChanged != null)
			{
				this.SeriesGroupChanged(this, EventArgs.Empty);
			}
		}

		readonly SeriesCollection<T> series;
	}
}
