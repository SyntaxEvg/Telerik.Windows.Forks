using System;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks
{
	abstract class WorkbookElementBase : XlsxElementBase
	{
		public WorkbookElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}
	}
}
