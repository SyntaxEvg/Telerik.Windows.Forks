using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class BarSeriesGroup : SeriesGroup<BarSeries>, ISupportAxes, ISupportGrouping
	{
		public BarSeriesGroup()
			: base(SeriesType.Bar)
		{
		}

		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Bar;
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

		public SeriesGrouping Grouping
		{
			get
			{
				return this.grouping;
			}
			set
			{
				if (this.grouping != value)
				{
					this.grouping = value;
					this.OnSeriesGroupChanged();
				}
			}
		}

		public BarDirection BarDirection
		{
			get
			{
				return this.barDirection;
			}
			set
			{
				if (this.barDirection != value)
				{
					this.barDirection = value;
					this.OnSeriesGroupChanged();
				}
			}
		}

		internal override void CloneProperties(SeriesGroup group)
		{
			base.CloneProperties(group);
			BarSeriesGroup barSeriesGroup = (BarSeriesGroup)group;
			barSeriesGroup.BarDirection = this.BarDirection;
			barSeriesGroup.Grouping = this.Grouping;
			barSeriesGroup.AxisGroupName = this.AxisGroupName;
		}

		const SeriesType barSeriesType = SeriesType.Bar;

		AxisGroupName axisGroup;

		BarDirection barDirection;

		SeriesGrouping grouping;
	}
}
