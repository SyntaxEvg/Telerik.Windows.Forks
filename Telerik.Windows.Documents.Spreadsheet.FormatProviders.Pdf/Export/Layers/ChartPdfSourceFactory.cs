using System;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class ChartPdfSourceFactory : IShapeSourceFactory<FloatingChartShape>
	{
		public IShapeSource GetShapeSource(FloatingChartShape shape)
		{
			return new ChartPdfSource(shape);
		}
	}
}
