using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class PieSeries : CategorySeriesBase
	{
		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Pie;
			}
		}
	}
}
