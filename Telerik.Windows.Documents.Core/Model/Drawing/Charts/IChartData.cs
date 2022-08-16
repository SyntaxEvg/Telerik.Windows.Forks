using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public interface IChartData
	{
		ChartDataType ChartDataType { get; }

		IChartData Clone();
	}
}
