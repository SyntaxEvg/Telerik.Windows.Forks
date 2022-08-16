using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import
{
	class XlsxImporter : OpenXmlImporter<XlsxPartsManager>
	{
		protected override XlsxPartsManager CreatePartsManager()
		{
			return new XlsxPartsManager();
		}
	}
}
