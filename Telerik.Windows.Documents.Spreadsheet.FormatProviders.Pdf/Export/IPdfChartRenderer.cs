using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export
{
	public interface IPdfChartRenderer
	{
		void RenderChart(FixedContentEditor editor, FloatingChartShape chart);
	}
}
