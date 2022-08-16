using System;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Model.Drawing.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class SeriesInfo
	{
		public SeriesInfo()
		{
			this.Outline = new Outline();
		}

		public Title Title { get; set; }

		public IChartData Categories { get; set; }

		public IChartData Values { get; set; }

		public IChartData BubbleSizes { get; set; }

		public Marker Marker { get; set; }

		public bool IsSmooth { get; set; }

		public Outline Outline { get; set; }
	}
}
