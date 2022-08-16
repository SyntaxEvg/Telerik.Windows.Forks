using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Fixed.Utilities.Rendering.Annotations;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	class FixedPageContentRenderingHelper
	{
		public FixedPageContentRenderingHelper(RadFixedPage page)
		{
			this.page = page;
			this.contentRootRenderingHelper = new ContentRootRenderingHelper();
			this.appearanceProvider = new WidgetAppearanceProvider(new DefaultAppearanceProvider(null));
		}

		public void RenderContentElements(IFixedContentRenderer renderer)
		{
			Rect viewport = PageLayoutHelper.CalculatePageBoundingRectangle(this.page);
			this.RenderContentElements(renderer, viewport);
		}

		public void RenderContentElements(IFixedContentRenderer renderer, Rect viewport)
		{
			Rect visibleContentBox = PageLayoutHelper.GetVisibleContentBox(this.page);
			Matrix matrix = PageLayoutHelper.CalculateVisibibleContentTransformation(this.page);
			using (this.contentRootRenderingHelper.EnsureCleanBoundsCache())
			{
				using (renderer.PushTransform(matrix))
				{
					using (renderer.PushClipping(new Telerik.Windows.Documents.Fixed.Model.Graphics.RectangleGeometry(visibleContentBox)))
					{
						this.contentRootRenderingHelper.RenderContentElements(renderer, viewport, this.page, visibleContentBox);
					}
				}
			}
		}

		public void RenderTiling(IFixedContentRenderer renderer, TilingBase tiling)
		{
			using (renderer.PushClipping(new Telerik.Windows.Documents.Fixed.Model.Graphics.RectangleGeometry(tiling.BoundingBox)))
			{
				this.contentRootRenderingHelper.RenderContentElements(renderer, tiling.BoundingBox, tiling, tiling.BoundingBox);
			}
		}

		public void RenderNormalAnnotationAppearance(IFixedContentRenderer renderer, Annotation annotation)
		{
			FormSource appearance;
			if (this.appearanceProvider.TryProvideAppearance(annotation, AnnotationAppearanceMode.Normal, out appearance))
			{
				this.RenderAnnotationAppearance(renderer, annotation, appearance);
			}
		}

		public void RenderDownAnnotationAppearance(IFixedContentRenderer renderer, Annotation annotation)
		{
			FormSource appearance;
			if (this.appearanceProvider.TryProvideAppearance(annotation, AnnotationAppearanceMode.Down, out appearance))
			{
				this.RenderAnnotationAppearance(renderer, annotation, appearance);
				return;
			}
			this.RenderNormalAnnotationAppearance(renderer, annotation);
		}

		void RenderAnnotationAppearance(IFixedContentRenderer renderer, Annotation annotation, FormSource appearance)
		{
			double m = ((appearance.Size.Width != 0.0) ? (annotation.Rect.Width / appearance.Size.Width) : 1.0);
			double m2 = ((appearance.Size.Height != 0.0) ? (annotation.Rect.Height / appearance.Size.Height) : 1.0);
			Matrix m3 = new Matrix(m, 0.0, 0.0, m2, annotation.Rect.X, annotation.Rect.Y);
			Matrix matrix = appearance.Matrix.MultiplyBy(m3);
			Matrix m4 = PageLayoutHelper.CalculateVisibibleContentTransformation(this.page);
			matrix = matrix.MultiplyBy(m4);
			using (this.contentRootRenderingHelper.EnsureCleanBoundsCache())
			{
				using (renderer.PushTransform(matrix))
				{
					using (renderer.PushClipping(new Telerik.Windows.Documents.Fixed.Model.Graphics.RectangleGeometry(appearance.BoundingBox)))
					{
						this.contentRootRenderingHelper.RenderContentElements(renderer, appearance.BoundingBox, appearance, appearance.BoundingBox);
					}
				}
			}
		}

		readonly RadFixedPage page;

		readonly ContentRootRenderingHelper contentRootRenderingHelper;

		readonly BaseAppearanceProvider appearanceProvider;
	}
}
