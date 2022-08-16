using System;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class ShapeSourceFactory
	{
		public ShapeSourceFactory(IShapeSourceFactory<FloatingImage> imageSourceCache, IShapeSourceFactory<FloatingChartShape> chartSourceCache)
		{
			this.imageSourceCache = imageSourceCache;
			this.chartSourceCache = chartSourceCache;
		}

		public IShapeSource GetImageSource(FloatingImage image)
		{
			return this.imageSourceCache.GetShapeSource(image);
		}

		public IShapeSource GetChartSource(FloatingChartShape chart)
		{
			return this.chartSourceCache.GetShapeSource(chart);
		}

		readonly IShapeSourceFactory<FloatingImage> imageSourceCache;

		readonly IShapeSourceFactory<FloatingChartShape> chartSourceCache;
	}
}
