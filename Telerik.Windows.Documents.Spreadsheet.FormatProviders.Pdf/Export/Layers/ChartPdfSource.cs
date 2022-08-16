using System;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class ChartPdfSource : IShapeSource
	{
		public ChartPdfSource(FloatingChartShape chartShape)
		{
			this.chartShape = chartShape;
		}

		public FloatingChartShape ChartShape
		{
			get
			{
				return this.chartShape;
			}
		}

		bool IShapeSource.IsLocked { get; set; }

		readonly FloatingChartShape chartShape;
	}
}
