using System;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Import
{
	interface IOpenXmlImportContext
	{
		DocumentTheme Theme { get; set; }

		ResourceManager Resources { get; }

		bool IsImportSuspended { get; }

		void BeginImport();

		void EndImport();

		IResource GetResourceByResourceKey(string resourceKey);

		void RegisterResource(string resourceKey, IResource resource);

		ChartShape GetChartForChartPart(ChartPart chartPart);

		void RegisterChartPartForChart(ChartShape chartShape, ChartPart chartPart);

		FormulaChartData GetFormulaChartData(string formula);

		void RegisterSeriesGroupAwaitingAxisGroupName(ISupportAxes seriesGroup, int catAxisId, int valAxisId);

		void RegisterAxisGroup(AxisGroupName axisGroupName, int thisId, int otherId);

		void PairSeriesGroupsWithAxes();
	}
}
