using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class ImageRenderable : ShapeRenderable<FloatingImage>
	{
		public Point RenderTransformOrigin { get; set; }

		public Transform RenderTransform { get; set; }
	}
}
