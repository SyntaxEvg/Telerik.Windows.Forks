using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	interface IFixedContentRenderer
	{
		IDisposable PushTransform(Matrix matrix);

		IDisposable PushClipping(GeometryBase geometry);

		void RenderText(TextRenderingContext renderingContext);

		void RenderGlyph(GlyphRenderingContext renderingContext);

		void RenderGeometry(GeometryRenderingContext renderingContext);

		void RenderImage(ImageRenderingContext renderingContext);
	}
}
