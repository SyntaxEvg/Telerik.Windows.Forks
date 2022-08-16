using System;

namespace Telerik.Windows.Documents.Flow.Model.BorderEvaluation.GridItems
{
	class CrossBorderGridItem : BorderGridItemBase
	{
		public double HorizontalSize { get; set; }

		public double VerticalSize { get; set; }

		public CrossBorderDirection CrossBorderDirection { get; set; }
	}
}
