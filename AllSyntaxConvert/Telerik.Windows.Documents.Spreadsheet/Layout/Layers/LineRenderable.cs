using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class LineRenderable : IRenderable
	{
		public double X1 { get; set; }

		public double Y1 { get; set; }

		public double X2 { get; set; }

		public double Y2 { get; set; }

		public Color Stroke { get; set; }

		public double StrokeThickness { get; set; }

		public CellBorderStyle CellBorderStyle { get; set; }

		public int ZIndex { get; set; }

		public override string ToString()
		{
			return string.Format("LineRenderable: X1={0}; Y1={1}; X2={2}; Y2={3};", new object[] { this.X1, this.Y1, this.X2, this.Y2 });
		}
	}
}
