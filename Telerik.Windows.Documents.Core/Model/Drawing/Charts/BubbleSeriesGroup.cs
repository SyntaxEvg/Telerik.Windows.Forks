using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class BubbleSeriesGroup : SeriesGroup<BubbleSeries>, ISupportAxes
	{
		public BubbleSeriesGroup()
			: base(SeriesType.Bubble)
		{
		}

		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Bubble;
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

		internal override int DataRangesPerSeries
		{
			get
			{
				return 3;
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
			base.OnSeriesGroupChanged();
		}

		internal override void CloneProperties(SeriesGroup group)
		{
			base.CloneProperties(group);
			BubbleSeriesGroup bubbleSeriesGroup = (BubbleSeriesGroup)group;
			bubbleSeriesGroup.AxisGroupName = this.AxisGroupName;
		}

		const SeriesType seriesType = SeriesType.Bubble;

		AxisGroupName axisGroup;
	}
}
