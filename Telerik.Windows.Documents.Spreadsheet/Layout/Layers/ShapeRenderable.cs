using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class ShapeRenderable : IRenderable
	{
		public abstract FloatingShapeType ShapeType { get; }

		public abstract FloatingShapeBase ShapeBase { get; }

		public IShapeSource ShapeSource { get; set; }

		public int ZIndex { get; set; }

		public double Height { get; set; }

		public double Width { get; set; }

		public Point TopLeft { get; set; }
	}
}
