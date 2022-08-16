using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model
{
	static class XlsxPartNames
	{
		public const string ContentTypesPartName = "/[Content_Types].xml";

		public const string WorkbookPartName = "/xl/workbook.xml";

		public const string WorksheetPartName = "/xl/worksheets/sheet{0}.xml";

		public const string StylesPartName = "/xl/styles.xml";

		public const string SharedStringsPartName = "/xl/sharedStrings.xml";

		public const string ThemePartName = "/xl/theme/theme1.xml";

		public const string DrawingPartName = "/xl/drawings/drawing{0}.xml";

		public const string ResourcePartName = "/xl/media/{0}";

		public const string ChartPartName = "/xl/charts/chart{0}.xml";
	}
}
