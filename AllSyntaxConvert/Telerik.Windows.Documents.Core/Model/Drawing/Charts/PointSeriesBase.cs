using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public abstract class PointSeriesBase : SeriesBase
	{
		public IChartData XValues
		{
			get
			{
				return this.xValues;
			}
			set
			{
				if (this.xValues != value)
				{
					this.xValues = value;
					base.OnSeriesChanged();
				}
			}
		}

		public IChartData YValues
		{
			get
			{
				return this.yValues;
			}
			set
			{
				if (this.yValues != value)
				{
					this.yValues = value;
					base.OnSeriesChanged();
				}
			}
		}

		internal sealed override IChartData VerticalSeriesData
		{
			get
			{
				return this.YValues;
			}
			set
			{
				this.YValues = value;
			}
		}

		internal sealed override IChartData HorizontalSeriesData
		{
			get
			{
				return this.XValues;
			}
			set
			{
				this.XValues = value;
			}
		}

		internal override void CloneProperties(SeriesBase series)
		{
			base.CloneProperties(series);
			PointSeriesBase pointSeriesBase = (PointSeriesBase)series;
			if (this.xValues != null)
			{
				pointSeriesBase.xValues = this.xValues.Clone();
			}
			if (this.yValues != null)
			{
				pointSeriesBase.yValues = this.yValues.Clone();
			}
		}

		IChartData xValues;

		IChartData yValues;
	}
}
