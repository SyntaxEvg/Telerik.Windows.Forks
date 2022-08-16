using System;
using Telerik.Windows.Documents.Model.Drawing.Theming;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public abstract class SeriesBase
	{
		protected SeriesBase()
		{
			this.outline = new Outline();
			this.outline.ShapeOutlineChanged += this.Outline_ShapeOutlineChanged;
		}

		public abstract SeriesType SeriesType { get; }

		public Title Title
		{
			get
			{
				return this.title;
			}
			set
			{
				if (this.title != value)
				{
					this.title = value;
					this.OnSeriesChanged();
				}
			}
		}

		public Outline Outline
		{
			get
			{
				return this.outline;
			}
			internal set
			{
				if (this.outline != value)
				{
					this.outline = value;
					this.OnSeriesChanged();
				}
			}
		}

		internal abstract IChartData HorizontalSeriesData { get; set; }

		internal abstract IChartData VerticalSeriesData { get; set; }

		public SeriesBase Clone()
		{
			SeriesBase seriesBase = (SeriesBase)Activator.CreateInstance(base.GetType());
			this.CloneProperties(seriesBase);
			return seriesBase;
		}

		internal virtual void CloneProperties(SeriesBase series)
		{
			if (series.title != null)
			{
				series.title = this.title.Clone();
			}
			if (series.outline != null)
			{
				series.outline = this.outline.Clone();
			}
		}

		internal event EventHandler SeriesChanged;

		internal void OnSeriesChanged()
		{
			if (this.SeriesChanged != null)
			{
				this.SeriesChanged(this, EventArgs.Empty);
			}
		}

		void Outline_ShapeOutlineChanged(object sender, EventArgs e)
		{
			this.OnSeriesChanged();
		}

		Title title;

		Outline outline;
	}
}
