using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class DocumentChart
	{
		public DocumentChart()
		{
			this.seriesGroups = new SeriesGroupCollection();
			this.seriesGroups.CollectionChanged += this.SeriesGroups_CollectionChanged;
			this.seriesGroups.SeriesGroupsChanged += this.SeriesGroupss_SeriesGroupChanged;
		}

		public SeriesGroupCollection SeriesGroups
		{
			get
			{
				return this.seriesGroups;
			}
		}

		public AxisGroup PrimaryAxes
		{
			get
			{
				return this.primaryAxes;
			}
			set
			{
				if (this.primaryAxes != value)
				{
					if (this.primaryAxes != null)
					{
						this.primaryAxes.CollectionChanged -= this.Axes_CollectionChanged;
						this.primaryAxes.AxesChanged -= this.Axes_AxesChanged;
					}
					this.primaryAxes = value;
					if (this.primaryAxes != null)
					{
						this.primaryAxes.CollectionChanged += this.Axes_CollectionChanged;
						this.primaryAxes.AxesChanged += this.Axes_AxesChanged;
					}
				}
			}
		}

		public AxisGroup SecondaryAxes
		{
			get
			{
				return this.secondaryAxes;
			}
			set
			{
				if (this.secondaryAxes != null)
				{
					this.secondaryAxes.CollectionChanged -= this.Axes_CollectionChanged;
					this.secondaryAxes.AxesChanged -= this.Axes_AxesChanged;
				}
				this.secondaryAxes = value;
				if (this.secondaryAxes != null)
				{
					this.secondaryAxes.CollectionChanged += this.Axes_CollectionChanged;
					this.secondaryAxes.AxesChanged += this.Axes_AxesChanged;
				}
			}
		}

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
					this.OnChartChanged();
				}
			}
		}

		public Legend Legend
		{
			get
			{
				return this.legend;
			}
			set
			{
				if (this.legend != value)
				{
					if (this.legend != null)
					{
						this.legend.LegendChanged -= this.Legend_LegendChanged;
					}
					this.legend = value;
					if (this.legend != null)
					{
						this.legend.LegendChanged += this.Legend_LegendChanged;
					}
					this.OnChartChanged();
				}
			}
		}

		public DocumentChart Clone()
		{
			DocumentChart documentChart = new DocumentChart();
			foreach (SeriesGroup seriesGroup in this.SeriesGroups)
			{
				documentChart.SeriesGroups.Add(seriesGroup.Clone());
			}
			documentChart.PrimaryAxes = ((this.PrimaryAxes != null) ? this.PrimaryAxes.Clone() : null);
			documentChart.SecondaryAxes = ((this.SecondaryAxes != null) ? this.SecondaryAxes.Clone() : null);
			documentChart.Title = ((this.Title != null) ? this.Title.Clone() : null);
			documentChart.Legend = ((this.Legend != null) ? this.Legend.Clone() : null);
			return documentChart;
		}

		void Axes_AxesChanged(object sender, EventArgs e)
		{
			this.OnChartChanged();
		}

		void Axes_CollectionChanged(object sender, EventArgs e)
		{
			this.OnChartChanged();
		}

		void SeriesGroupss_SeriesGroupChanged(object sender, EventArgs e)
		{
			this.OnChartChanged();
		}

		void SeriesGroups_CollectionChanged(object sender, EventArgs e)
		{
			this.OnChartChanged();
		}

		void Legend_LegendChanged(object sender, EventArgs e)
		{
			this.OnChartChanged();
		}

		internal event EventHandler ChartChanged;

		void OnChartChanged()
		{
			if (this.ChartChanged != null)
			{
				this.ChartChanged(this, EventArgs.Empty);
			}
		}

		readonly SeriesGroupCollection seriesGroups;

		AxisGroup primaryAxes;

		AxisGroup secondaryAxes;

		Legend legend;

		Title title;
	}
}
