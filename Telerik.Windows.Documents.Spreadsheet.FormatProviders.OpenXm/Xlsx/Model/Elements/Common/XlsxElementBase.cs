using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common
{
	abstract class XlsxElementBase : OpenXmlElementBase<IXlsxWorkbookImportContext, IXlsxWorkbookExportContext, XlsxPartsManager>
	{
		public XlsxElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}
	}
}
