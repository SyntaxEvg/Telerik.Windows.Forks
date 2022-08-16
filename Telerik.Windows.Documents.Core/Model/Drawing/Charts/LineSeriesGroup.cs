using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class LineSeriesGroup : SeriesGroup<LineSeries>, ISupportAxes, ISupportGrouping
	{
		public LineSeriesGroup()
			: base(SeriesType.Line)
		{
		}

		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Line;
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
			LineSeriesGroup lineSeriesGroup = (LineSeriesGroup)group;
			lineSeriesGroup.Grouping = this.Grouping;
			lineSeriesGroup.AxisGroupName = this.AxisGroupName;
		}

		const SeriesType lineSeriesType = SeriesType.Line;

		SeriesGrouping grouping;

		AxisGroupName axisGroup;
	}
}
