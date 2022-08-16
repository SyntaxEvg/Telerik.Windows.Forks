using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class LinearGradientBox
	{
		public Rect BoundingBox { get; set; }

		public PathGeometry Clip { get; set; }

		public LinearGradientBrush Fill { get; set; }
	}
}
