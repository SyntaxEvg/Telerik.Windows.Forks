using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class AreaSeries : CategorySeriesBase
	{
		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Area;
			}
		}
	}
}
