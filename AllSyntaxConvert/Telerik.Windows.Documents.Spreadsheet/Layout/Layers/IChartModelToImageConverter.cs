using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	interface IChartModelToImageConverter
	{
		BitmapSource GetBitmapSourceFromFloatingChartShape(FloatingChartShape floatingChartShape, double dpiH, double dpiV);

		BitmapSource GetBitmapSourceFromFloatingChartShape(FloatingChartShape floatingChartShape);

		BitmapSource GetBitmapSourceFromFloatingChartShape(FloatingChartShape floatingChartShape, double dpiH, double dpiV, Size scale);

		BitmapSource GetBitmapSourceFromFloatingChartShape(FloatingChartShape floatingChartShape, Size scale);
	}
}
