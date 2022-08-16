using System;
using System.Windows;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class RowColumnHeadingRenderable<T> : IRenderable where T : LayoutBox
	{
		public T LayoutBox { get; set; }

		public string Text { get; set; }

		public Rect Rectangle { get; set; }

		public Size ScaleFactor { get; set; }
	}
}
