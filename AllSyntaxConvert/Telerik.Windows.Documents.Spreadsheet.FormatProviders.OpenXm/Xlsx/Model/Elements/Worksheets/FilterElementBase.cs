using System;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	abstract class FilterElementBase : WorksheetElementBase
	{
		public FilterElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		internal abstract IFilterInfo GetInfo();

		internal abstract void CopyPropertiesFrom(IXlsxWorksheetExportContext context, int columnId);
	}
}
