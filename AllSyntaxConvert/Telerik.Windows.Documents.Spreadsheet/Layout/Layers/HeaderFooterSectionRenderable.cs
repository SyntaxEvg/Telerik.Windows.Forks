using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class HeaderFooterSectionRenderable : IRenderable
	{
		public HeaderFooterSection Section { get; set; }

		public TextAlignment Alignment { get; set; }

		public double BlockWidth { get; set; }

		public double ScaleFactor { get; set; }

		public double Left { get; set; }

		public HeaderFooterRenderContext HeaderFooterContext { get; set; }
	}
}
