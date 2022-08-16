using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export
{
	class DefaultPdfChartToImageRenderer : IPdfChartRenderer
	{
		public DefaultPdfChartToImageRenderer(IChartModelToImageConverter chartToImageConverter)
		{
			this.chartToImageConverter = chartToImageConverter;
		}

		public void RenderChart(FixedContentEditor editor, FloatingChartShape chart)
		{
			BitmapSource bitmapSourceFromFloatingChartShape = this.chartToImageConverter.GetBitmapSourceFromFloatingChartShape(chart, 300.0, 300.0);
			editor.DrawImage(SpreadsheetHelper.StreamFromBitmapSource(bitmapSourceFromFloatingChartShape), new Size(chart.Width, chart.Height));
		}

		const int DpiHorizontal = 300;

		const int DpiVertical = 300;

		readonly IChartModelToImageConverter chartToImageConverter;
	}
}
