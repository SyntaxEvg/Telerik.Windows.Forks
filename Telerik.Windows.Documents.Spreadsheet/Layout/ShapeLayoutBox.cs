using System;
using System.Windows;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	class ShapeLayoutBox : LayoutBox
	{
		public ShapeLayoutBox(int shapeId, Rect rect)
			: base(rect)
		{
			this.shapeId = shapeId;
		}

		public int ShapeId
		{
			get
			{
				return this.shapeId;
			}
		}

		readonly int shapeId;
	}
}
