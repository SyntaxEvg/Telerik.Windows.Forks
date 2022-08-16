using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class ScatterSeriesGroup : SeriesGroup<ScatterSeries>, ISupportAxes
	{
		public ScatterSeriesGroup()
			: base(SeriesType.Scatter)
		{
		}

		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Scatter;
			}
		}

		public AxisGroupName AxisGroupName
		{
			get
			{
				return this.axisGroup;
			}
			set
			{
				if (this.axisGroup != value)
				{
					this.axisGroup = value;
					this.OnSeriesGroupChanged();
				}
			}
		}

		internal ScatterStyle ScatterStyle
		{
			get
			{
				if (this.scatterStyle == null)
				{
					this.scatterStyle = this.GetScatterStyle();
				}
				return this.scatterStyle.Value;
			}
		}

		internal override int DataRangesPerSeries
		{
			get
			{
				return 2;
			}
		}

		internal override bool HasCategories
		{
			get
			{
				return false;
			}
		}

		internal override void OnSeriesGroupChanged()
		{
			this.scatterStyle = null;
			base.OnSeriesGroupChanged();
		}

		internal override void CloneProperties(SeriesGroup group)
		{
			base.CloneProperties(group);
			ScatterSeriesGroup scatterSeriesGroup = (ScatterSeriesGroup)group;
			scatterSeriesGroup.AxisGroupName = this.AxisGroupName;
		}

		ScatterStyle? GetScatterStyle()
		{
			foreach (ScatterSeries scatterSeries in base.Series)
			{
				if (scatterSeries.ScatterStyle == ScatterStyle.LineMarker)
				{
					return new ScatterStyle?(ScatterStyle.LineMarker);
				}
			}
			return new ScatterStyle?(ScatterStyle.SmoothMarker);
		}

		const SeriesType seriesType = SeriesType.Scatter;

		AxisGroupName axisGroup;

		ScatterStyle? scatterStyle;
	}
}
