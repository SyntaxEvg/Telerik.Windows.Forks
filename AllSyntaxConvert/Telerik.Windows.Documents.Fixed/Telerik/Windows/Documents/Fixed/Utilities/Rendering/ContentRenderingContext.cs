using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	class ContentRenderingContext : IContentRenderingContext
	{
		protected ContentRenderingContext(PositionContentElement renderedElement, Rect currentContainerBounds)
		{
			this.renderedElementMatrix = renderedElement.Position.Matrix;
			this.currentContainerBounds = currentContainerBounds;
		}

		Rect IContentRenderingContext.CurrentContainerBounds
		{
			get
			{
				return this.currentContainerBounds;
			}
		}

		Matrix IContentRenderingContext.RenderedElementPositionMatrix
		{
			get
			{
				return this.renderedElementMatrix;
			}
		}

		readonly Matrix renderedElementMatrix;

		readonly Rect currentContainerBounds;
	}
}
