using System;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class ShapeRenderableFactory
	{
		public ShapeRenderableFactory(ShapeSourceFactory shapeSourceFactory)
		{
			this.shapeSourceFactory = shapeSourceFactory;
		}

		public ShapeRenderable GetShapeRenderableFromFloatingShape(ShapeCollection shapes, FloatingShapeBase shape)
		{
			FloatingImage floatingImage = shape as FloatingImage;
			FloatingChartShape floatingChartShape = shape as FloatingChartShape;
			ShapeRenderable result;
			if (floatingImage != null)
			{
				result = new ImageRenderable
				{
					Shape = floatingImage,
					ShapeSource = this.shapeSourceFactory.GetImageSource(floatingImage),
					ZIndex = shapes.GetZIndexById(shape.Id)
				};
			}
			else
			{
				if (floatingChartShape == null)
				{
					throw new NotSupportedException();
				}
				result = new ChartRenderable
				{
					Shape = floatingChartShape,
					ShapeSource = this.shapeSourceFactory.GetChartSource(floatingChartShape),
					ZIndex = shapes.GetZIndexById(shape.Id)
				};
			}
			return result;
		}

		readonly ShapeSourceFactory shapeSourceFactory;
	}
}
