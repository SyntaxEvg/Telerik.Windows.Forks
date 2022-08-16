using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	interface IContentRenderingContext
	{
		Matrix RenderedElementPositionMatrix { get; }

		Rect CurrentContainerBounds { get; }
	}
}
