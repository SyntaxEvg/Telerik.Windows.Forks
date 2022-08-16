using System;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.Model.Drawing.Shapes
{
	class ChartShape : ShapeBase
	{
		public DocumentChart Chart { get; set; }

		public ChartShape()
		{
		}

		public ChartShape(ChartShape other)
			: base(other)
		{
			this.Chart = other.Chart.Clone();
		}
	}
}
