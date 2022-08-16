using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class ShapesLayer : WorksheetLayerBase
	{
		public ShapesLayer(IRenderer<ShapeRenderable> shapeRenderer, ShapeSourceFactory cache)
		{
			this.shapeRenderer = shapeRenderer;
			this.shapeRenderableFactory = new ShapeRenderableFactory(cache);
		}

		public override string Name
		{
			get
			{
				return "Shapes";
			}
		}

		internal IRenderer<ShapeRenderable> ShapeRenderer
		{
			get
			{
				return this.shapeRenderer;
			}
		}

		protected override void UpdateRenderOverride(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			foreach (FloatingShapeBase floatingShapeBase in worksheetUpdateContext.Shapes)
			{
				ShapeLayoutBox shapeLayoutBox = worksheetUpdateContext.Layout.GetShapeLayoutBox(floatingShapeBase.Shape.Id);
				foreach (ViewportPane viewportPane in worksheetUpdateContext.SheetViewport.ViewportPanes)
				{
					if (viewportPane.Rect.IntersectsWith(shapeLayoutBox.BoundingRectangle))
					{
						ShapeRenderable shapeRenderableFromFloatingShape = this.shapeRenderableFactory.GetShapeRenderableFromFloatingShape(worksheetUpdateContext.Worksheet.Shapes, floatingShapeBase);
						shapeRenderableFromFloatingShape.ShapeSource.IsLocked = true;
						base.ContainerManager.Add(shapeRenderableFromFloatingShape, viewportPane.ViewportPaneType);
					}
				}
			}
		}

		protected override void TranslateAndScale(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			foreach (ViewportPane viewportPane in worksheetUpdateContext.SheetViewport.ViewportPanes)
			{
				foreach (IRenderable renderable in from x in base.ContainerManager.GetElementsForViewportPane(viewportPane.ViewportPaneType)
					orderby ((ShapeRenderable)x).ZIndex
					select x)
				{
					ShapeRenderable shapeRenderable = (ShapeRenderable)renderable;
					Point shapeTopLeftConsideringAdjustmentForRotation = ShapesResizeHelper.GetShapeTopLeftConsideringAdjustmentForRotation(shapeRenderable.ShapeBase, worksheetUpdateContext.Layout);
					shapeRenderable.TopLeft = base.TranslateAndScale(shapeTopLeftConsideringAdjustmentForRotation, viewportPane.ViewportPaneType, worksheetUpdateContext);
					shapeRenderable.Height = shapeRenderable.ShapeBase.Height * worksheetUpdateContext.ScaleFactor.Width;
					shapeRenderable.Width = shapeRenderable.ShapeBase.Width * worksheetUpdateContext.ScaleFactor.Height;
					ImageRenderable imageRenderable = shapeRenderable as ImageRenderable;
					if (imageRenderable != null)
					{
						imageRenderable.RenderTransformOrigin = ShapesLayer.ShapesRenderTransformOrigin;
						imageRenderable.RenderTransform = ShapesLayer.GetImageRenderTransform(imageRenderable.Shape);
					}
					this.ShapeRenderer.Render(shapeRenderable, viewportPane.ViewportPaneType);
				}
			}
		}

		protected override void Clear()
		{
			foreach (object obj in Enum.GetValues(typeof(ViewportPaneType)))
			{
				ViewportPaneType viewportPaneType = (ViewportPaneType)obj;
				IEnumerable<IRenderable> elementsForViewportPane = base.ContainerManager.GetElementsForViewportPane(viewportPaneType);
				foreach (IRenderable renderable in elementsForViewportPane)
				{
					ShapeRenderable shapeRenderable = renderable as ShapeRenderable;
					if (shapeRenderable != null)
					{
						shapeRenderable.ShapeSource.IsLocked = false;
					}
				}
			}
			base.Clear();
		}

		static Transform GetImageRenderTransform(FloatingShapeBase shape)
		{
			ScaleTransform scaleTransform = new ScaleTransform();
			scaleTransform.ScaleX = (double)(shape.IsHorizontallyFlipped ? (-1) : 1);
			scaleTransform.ScaleY = (double)(shape.IsVerticallyFlipped ? (-1) : 1);
			RotateTransform rotateTransform = new RotateTransform();
			rotateTransform.Angle = shape.RotationAngle;
			return new TransformGroup
			{
				Children = { scaleTransform, rotateTransform }
			};
		}

		static readonly Point ShapesRenderTransformOrigin = new Point(0.5, 0.5);

		readonly IRenderer<ShapeRenderable> shapeRenderer;

		readonly ShapeRenderableFactory shapeRenderableFactory;
	}
}
