using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	static class OpenXmlNamespaces
	{
		const string DefaultNamespaceName = "xmlns";

		public static readonly OpenXmlNamespace ContentTypesNamespace = new OpenXmlNamespace("http://schemas.openxmlformats.org/package/2006/content-types");

		public static readonly OpenXmlNamespace SpreadsheetMLNamespace = new OpenXmlNamespace("http://schemas.openxmlformats.org/spreadsheetml/2006/main");

		public static readonly OpenXmlNamespace OfficeDocumentRelationshipsNamespace = new OpenXmlNamespace("xmlns", "r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

		public static readonly OpenXmlNamespace RelationshipsNamespace = new OpenXmlNamespace("http://schemas.openxmlformats.org/package/2006/relationships");
	}
}
