using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public abstract class CategorySeriesBase : SeriesBase
	{
		public IChartData Values
		{
			get
			{
				return this.values;
			}
			set
			{
				if (this.values != value)
				{
					this.values = value;
					base.OnSeriesChanged();
				}
			}
		}

		public IChartData Categories
		{
			get
			{
				return this.categories;
			}
			set
			{
				if (this.categories != value)
				{
					this.categories = value;
					base.OnSeriesChanged();
				}
			}
		}

		internal sealed override IChartData VerticalSeriesData
		{
			get
			{
				return this.Values;
			}
			set
			{
				this.Values = value;
			}
		}

		internal sealed override IChartData HorizontalSeriesData
		{
			get
			{
				return this.Categories;
			}
			set
			{
				this.Categories = value;
			}
		}

		internal override void CloneProperties(SeriesBase series)
		{
			base.CloneProperties(series);
			CategorySeriesBase categorySeriesBase = (CategorySeriesBase)series;
			if (this.values != null)
			{
				categorySeriesBase.values = this.values.Clone();
			}
			if (this.categories != null)
			{
				categorySeriesBase.categories = this.categories.Clone();
			}
		}

		IChartData values;

		IChartData categories;
	}
}
