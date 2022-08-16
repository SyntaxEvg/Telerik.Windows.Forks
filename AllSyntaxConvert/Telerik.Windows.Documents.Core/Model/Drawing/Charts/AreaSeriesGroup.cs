using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class AreaSeriesGroup : SeriesGroup<AreaSeries>, ISupportAxes, ISupportGrouping
	{
		public AreaSeriesGroup()
			: base(SeriesType.Area)
		{
		}

		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Area;
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

		internal override void CloneProperties(SeriesGroup group)
		{
			base.CloneProperties(group);
			AreaSeriesGroup areaSeriesGroup = (AreaSeriesGroup)group;
			areaSeriesGroup.Grouping = this.Grouping;
			areaSeriesGroup.AxisGroupName = this.AxisGroupName;
		}

		const SeriesType areaSeriesType = SeriesType.Area;

		SeriesGrouping grouping;

		AxisGroupName axisGroup;
	}
}
