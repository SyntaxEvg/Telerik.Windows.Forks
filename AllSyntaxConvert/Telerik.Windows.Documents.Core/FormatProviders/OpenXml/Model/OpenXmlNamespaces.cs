using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model
{
	static class OpenXmlNamespaces
	{
		const string DefaultNamespaceName = "xmlns";

		public static readonly OpenXmlNamespace ContentTypesNamespace = new OpenXmlNamespace("http://schemas.openxmlformats.org/package/2006/content-types");

		public static readonly OpenXmlNamespace RelationshipsNamespace = new OpenXmlNamespace("http://schemas.openxmlformats.org/package/2006/relationships");

		public static readonly OpenXmlNamespace SpreadsheetMLNamespace = new OpenXmlNamespace("http://schemas.openxmlformats.org/spreadsheetml/2006/main");

		public static readonly OpenXmlNamespace OfficeDocumentRelationshipsNamespace = new OpenXmlNamespace("xmlns", "r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

		public static readonly OpenXmlNamespace DrawingMLNamespace = new OpenXmlNamespace("xmlns", "a", "http://schemas.openxmlformats.org/drawingml/2006/main");

		public static readonly OpenXmlNamespace WordprocessingMLNamespace = new OpenXmlNamespace("xmlns", "w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

		public static readonly OpenXmlNamespace SpreadsheetDrawingMLNamespace = new OpenXmlNamespace("xmlns", "xdr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");

		public static readonly OpenXmlNamespace WordprocessingDrawingMLNamespace = new OpenXmlNamespace("xmlns", "wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");

		public static readonly OpenXmlNamespace PictureDrawingMLNamespace = new OpenXmlNamespace("xmlns", "pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");

		public static readonly OpenXmlNamespace ChartDrawingMLNamespace = new OpenXmlNamespace("xmlns", "c", "http://schemas.openxmlformats.org/drawingml/2006/chart");

		public static readonly OpenXmlNamespace VectorMLNamespace = new OpenXmlNamespace("xmlns", "v", "urn:schemas-microsoft-com:vml");
	}
}
