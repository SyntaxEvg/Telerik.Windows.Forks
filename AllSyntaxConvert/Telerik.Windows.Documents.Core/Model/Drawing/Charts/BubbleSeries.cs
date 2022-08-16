using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class BubbleSeries : PointSeriesBase
	{
		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Bubble;
			}
		}

		public IChartData BubbleSizes
		{
			get
			{
				return this.bubbleSizes;
			}
			set
			{
				if (this.bubbleSizes != value)
				{
					this.bubbleSizes = value;
					base.OnSeriesChanged();
				}
			}
		}

		internal override void CloneProperties(SeriesBase series)
		{
			base.CloneProperties(series);
			BubbleSeries bubbleSeries = (BubbleSeries)series;
			bubbleSeries.bubbleSizes = this.bubbleSizes.Clone();
		}

		IChartData bubbleSizes;
	}
}
