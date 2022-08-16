using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class BarSeries : CategorySeriesBase
	{
		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Bar;
			}
		}
	}
}
