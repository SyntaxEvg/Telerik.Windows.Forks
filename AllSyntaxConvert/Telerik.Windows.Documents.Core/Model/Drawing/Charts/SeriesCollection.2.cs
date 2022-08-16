using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class SeriesCollection<T> : SeriesCollection where T : SeriesBase
	{
		internal SeriesCollection(SeriesType seriesType)
		{
			this.innerList = new List<T>();
			this.seriesType = seriesType;
		}

		public new IEnumerator<T> GetEnumerator()
		{
			return this.GetEnumeratorOverride() as IEnumerator<T>;
		}

		internal override IEnumerator<SeriesBase> GetEnumeratorOverride()
		{
			return this.innerList.GetEnumerator();
		}

		public new T Add()
		{
			return this.AddOverride() as T;
		}

		internal override SeriesBase AddOverride()
		{
			SeriesBase series = SeriesFactory.GetSeries(this.seriesType);
			T t = series as T;
			this.Add(t);
			return t;
		}

		public new T AddBubble(IChartData xValuesData, IChartData yValuesData, IChartData bubbleSizesData, Title title = null)
		{
			return this.AddOverride(xValuesData, yValuesData, bubbleSizesData, title) as T;
		}

		public new T AddScatter(IChartData xValuesData, IChartData yValuesData, Title title = null)
		{
			return this.AddOverride(xValuesData, yValuesData, null, title) as T;
		}

		public new T Add(IChartData categoriesData, IChartData valuesData, Title title = null)
		{
			return this.AddOverride(categoriesData, valuesData, null, title) as T;
		}

		internal override SeriesBase AddOverride(IChartData xValuesData, IChartData valuesData, IChartData bubbleSizesData, Title title = null)
		{
			SeriesBase series = SeriesFactory.GetSeries(this.seriesType, xValuesData, valuesData, bubbleSizesData, title);
			T t = series as T;
			this.Add(t);
			return t;
		}

		public void Add(T series)
		{
			this.AddOverride(series);
		}

		internal override void AddOverride(SeriesBase series)
		{
			T t = series as T;
			if (t == null)
			{
				throw new ArgumentException("The series is of type that is incompatible with the collection", "series");
			}
			t.SeriesChanged += this.Series_SeriesChanged;
			this.innerList.Add(t);
			this.OnCollectionChanged();
		}

		public void Remove(T series)
		{
			this.RemoveOverride(series);
		}

		internal override void RemoveOverride(SeriesBase series)
		{
			T t = series as T;
			if (t == null)
			{
				throw new InvalidOperationException();
			}
			if (this.innerList.Contains(t))
			{
				series.SeriesChanged -= this.Series_SeriesChanged;
				this.innerList.Remove(t);
				this.OnCollectionChanged();
			}
		}

		void Series_SeriesChanged(object sender, EventArgs e)
		{
			this.OnSeriesChanged();
		}

		internal override event EventHandler CollectionChanged;

		void OnCollectionChanged()
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, EventArgs.Empty);
			}
		}

		internal override event EventHandler SeriesChanged;

		void OnSeriesChanged()
		{
			if (this.SeriesChanged != null)
			{
				this.SeriesChanged(this, EventArgs.Empty);
			}
		}

		readonly List<T> innerList;

		readonly SeriesType seriesType;
	}
}
