using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public abstract class SeriesCollection : IEnumerable<SeriesBase>, IEnumerable
	{
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IEnumerator<SeriesBase> GetEnumerator()
		{
			return this.GetEnumeratorOverride();
		}

		internal abstract IEnumerator<SeriesBase> GetEnumeratorOverride();

		public SeriesBase Add()
		{
			return this.AddOverride();
		}

		public SeriesBase AddBubble(IChartData xValuesData, IChartData yValuesData, IChartData bubbleSizesData, Title title = null)
		{
			return this.AddOverride(xValuesData, yValuesData, bubbleSizesData, title);
		}

		public SeriesBase AddScatter(IChartData xValuesData, IChartData yValuesData, Title title = null)
		{
			return this.AddOverride(xValuesData, yValuesData, null, title);
		}

		public SeriesBase Add(IChartData categoriesData, IChartData valuesData, Title title = null)
		{
			return this.AddOverride(categoriesData, valuesData, null, title);
		}

		internal abstract SeriesBase AddOverride();

		internal abstract SeriesBase AddOverride(IChartData xValuesData, IChartData valuesData, IChartData bubbleSizesData, Title title = null);

		public void Add(SeriesBase series)
		{
			this.AddOverride(series);
		}

		internal abstract void AddOverride(SeriesBase series);

		public void Remove(SeriesBase series)
		{
			this.RemoveOverride(series);
		}

		internal abstract void RemoveOverride(SeriesBase series);

		internal abstract event EventHandler CollectionChanged;

		internal abstract event EventHandler SeriesChanged;
	}
}
