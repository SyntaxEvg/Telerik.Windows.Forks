using System;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Export
{
	interface IOpenXmlExportContext
	{
		DocumentTheme Theme { get; }

		ResourceManager Resources { get; }

		string GetRelationshipIdByResource(IResource resource);

		void RegisterResource(string relationshipId, IResource resource);

		ChartPart GetChartPartForChart(ChartShape chart);

		ChartShape GetChartForChartPart(ChartPart chartPart);
	}
}
