using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class LineMeasureContext
	{
		public LineMeasureContext(IEnumerable<LayoutElementBase> elements, double remainingLineWidth)
		{
			this.PendingElements = elements;
			this.RemainingLineWidth = remainingLineWidth;
		}

		public IEnumerable<LayoutElementBase> PendingElements { get; set; }

		public double RemainingLineWidth { get; set; }
	}
}
