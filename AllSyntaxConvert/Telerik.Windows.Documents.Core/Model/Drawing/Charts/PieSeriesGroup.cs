using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class PieSeriesGroup : SeriesGroup<PieSeries>
	{
		public PieSeriesGroup()
			: base(SeriesType.Pie)
		{
		}

		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Pie;
			}
		}

		const SeriesType pieSeriesType = SeriesType.Pie;
	}
}
