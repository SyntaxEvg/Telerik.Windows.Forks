using System;

namespace Telerik.Documents.SpreadsheetStreaming.Core
{
	static class PartPaths
	{
		public static readonly string WorkbookPartPath = "xl/workbook.xml";

		public static readonly string WorkbookRelationshipsPartPath = "xl/_rels/workbook.xml.rels";

		public static readonly string StylesPartPath = "xl/styles.xml";

		public static readonly string WorksheetPartPath = "xl/worksheets/sheet{0}.xml";

		public static readonly string ContentTypesPath = "[Content_Types].xml";
	}
}
