using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Editing.Layout;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	class ContentRootRenderingHelper
	{
		internal ContentRootRenderingHelper()
		{
			this.contentElementsGraphCache = new ContentElementsGraphCache();
			this.containersBoundsStack = new Stack<Rect>();
		}

		internal void RenderContentElements(IFixedContentRenderer renderer, Rect viewport, IContentRootElement element, Rect elementBounds)
		{
			using (this.PushContainerBounds(elementBounds))
			{
				ContentElementsGraph graph = this.contentElementsGraphCache.GetGraph(element);
				foreach (ContentElementBase parent in graph.GetRootElements())
				{
					this.RenderContentElementHierarchy(renderer, viewport, graph, parent);
				}
			}
		}

		internal IDisposable EnsureCleanBoundsCache()
		{
			return new DisposableObject(new Action(this.ClearBoundsCache));
		}

		void RenderContentElementHierarchy(IFixedContentRenderer renderer, Rect viewport, ContentElementsGraph contentElementsGraph, ContentElementBase parent)
		{
			Clipping clipping = parent as Clipping;
			if (clipping != null)
			{
				this.RenderClippedElements(renderer, viewport, contentElementsGraph, clipping);
				return;
			}
			this.RenderContentElement(renderer, viewport, parent);
		}

		void RenderClippedElements(IFixedContentRenderer renderer, Rect viewport, ContentElementsGraph contentElementsGraph, Clipping clipping)
		{
			using (renderer.PushTransform(clipping.Transform))
			{
				using (renderer.PushClipping(clipping.Clip))
				{
					IEnumerable<ContentElementBase> clippingChildren = contentElementsGraph.GetClippingChildren(clipping);
					foreach (ContentElementBase parent in clippingChildren)
					{
						using (renderer.PushTransform(clipping.Transform.InverseMatrix()))
						{
							this.RenderContentElementHierarchy(renderer, viewport, contentElementsGraph, parent);
						}
					}
				}
			}
		}

		void RenderContentElement(IFixedContentRenderer renderer, Rect viewport, ContentElementBase element)
		{
			if (this.IsInViewport(viewport, element))
			{
				switch (element.ElementType)
				{
				case FixedDocumentElementType.Path:
				{
					Path path = (Path)element;
					this.RenderPath(renderer, path);
					return;
				}
				case FixedDocumentElementType.Image:
				{
					Image image = (Image)element;
					this.RenderImage(renderer, image);
					return;
				}
				case FixedDocumentElementType.TextFragment:
				{
					TextFragment textFragment = (TextFragment)element;
					this.RenderTextFragment(renderer, textFragment);
					return;
				}
				case FixedDocumentElementType.Form:
				{
					Form form = (Form)element;
					this.RenderForm(renderer, form);
					break;
				}
				default:
					return;
				}
			}
		}

		void RenderTextFragment(IFixedContentRenderer renderer, TextFragment textFragment)
		{
			Rect currentContainerBounds = this.PeekContainerBounds();
			if (textFragment.Font.ActualFontSource != EmptyFontSource.Instance)
			{
				double num = textFragment.FontSize / 100.0;
				double? horizontalScaling = textFragment.HorizontalScaling;
				double num2 = ((horizontalScaling != null) ? horizontalScaling.GetValueOrDefault() : 1.0);
				double? textRise = textFragment.TextRise;
				double offsetY = ((textRise != null) ? textRise.GetValueOrDefault() : 0.0);
				Matrix trans = new Matrix(num * num2, 0.0, 0.0, num, 0.0, offsetY) * ShowText.FlipTextTransformation;
				Matrix matrix = (ShowText.FlipTextTransformation * textFragment.TextMatrix).InverseMatrix() * textFragment.Position.Matrix;
				Matrix matrix2 = textFragment.TextMatrix;
				GeometryPropertiesOwner geometryProperties = textFragment.TextProperties.GeometryProperties;
				using (renderer.PushTransform(matrix))
				{
					foreach (CharInfo charInfo in textFragment.TextCollection.Characters)
					{
						FontBase font = textFragment.TextCollection.Font;
						GlyphOutlinesCollection cachedOutlines = font.GetCachedOutlines(charInfo.CharCode);
						if (cachedOutlines != null)
						{
							Matrix outlinesTransformation = trans * matrix2;
							GlyphRenderingContext renderingContext = new GlyphRenderingContext(textFragment, cachedOutlines, outlinesTransformation, currentContainerBounds);
							renderer.RenderGlyph(renderingContext);
						}
						double tx = TextFragmentLayoutElement.MeasureWidth(charInfo.CharCode, font, textFragment.FontSize, textFragment.WordSpacing, textFragment.CharacterSpacing, textFragment.HorizontalScaling);
						matrix2 = TranslateText.GetTranslatedTextMatrix(matrix2, tx);
					}
					return;
				}
			}
			if (!string.IsNullOrEmpty(textFragment.Text))
			{
				using (renderer.PushTransform(textFragment.Position.Matrix))
				{
					TextRenderingContext renderingContext2 = new TextRenderingContext(textFragment, currentContainerBounds);
					renderer.RenderText(renderingContext2);
				}
			}
		}

		void RenderPath(IFixedContentRenderer renderer, Path path)
		{
			if (path.Geometry != null)
			{
				using (renderer.PushTransform(path.Position.Matrix))
				{
					Rect currentContainerBounds = this.PeekContainerBounds();
					GeometryRenderingContext renderingContext = new GeometryRenderingContext(path, currentContainerBounds);
					renderer.RenderGeometry(renderingContext);
				}
			}
		}

		void RenderImage(IFixedContentRenderer renderer, Image image)
		{
			if (image.ImageSource != null && image.Width > 0.0 && image.Height > 0.0)
			{
				using (renderer.PushTransform(image.Position.Matrix))
				{
					Rect currentContainerBounds = this.PeekContainerBounds();
					ImageRenderingContext renderingContext = new ImageRenderingContext(image, currentContainerBounds);
					renderer.RenderImage(renderingContext);
				}
			}
		}

		void RenderForm(IFixedContentRenderer renderer, Form form)
		{
			if (form.FormSource != null && form.Width > 0.0 && form.Height > 0.0)
			{
				Rect boundingBox = form.FormSource.BoundingBox;
				bool applyTopLeftTransformation = false;
				Matrix scaledPosition = form.GetScaledPosition(applyTopLeftTransformation);
				using (renderer.PushTransform(scaledPosition))
				{
					using (renderer.PushClipping(new Telerik.Windows.Documents.Fixed.Model.Graphics.RectangleGeometry(boundingBox)))
					{
						this.RenderContentElements(renderer, boundingBox, form.FormSource, boundingBox);
					}
				}
			}
		}

		bool IsInViewport(Rect viewport, ContentElementBase element)
		{
			return true;
		}

		IDisposable PushContainerBounds(Rect bounds)
		{
			this.containersBoundsStack.Push(bounds);
			return new DisposableObject(new Action(this.PopContainerBounds));
		}

		void PopContainerBounds()
		{
			this.containersBoundsStack.Pop();
		}

		Rect PeekContainerBounds()
		{
			return this.containersBoundsStack.Peek();
		}

		void ClearBoundsCache()
		{
			this.containersBoundsStack.Clear();
		}

		readonly ContentElementsGraphCache contentElementsGraphCache;

		readonly Stack<Rect> containersBoundsStack;
	}
}
