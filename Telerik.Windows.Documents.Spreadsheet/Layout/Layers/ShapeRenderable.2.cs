using System;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class ShapeRenderable<T> : ShapeRenderable where T : FloatingShapeBase
	{
		public T Shape { get; set; }

		public override FloatingShapeType ShapeType
		{
			get
			{
				T shape = this.Shape;
				return shape.FloatingShapeType;
			}
		}

		public override FloatingShapeBase ShapeBase
		{
			get
			{
				return this.Shape;
			}
		}
	}
}
