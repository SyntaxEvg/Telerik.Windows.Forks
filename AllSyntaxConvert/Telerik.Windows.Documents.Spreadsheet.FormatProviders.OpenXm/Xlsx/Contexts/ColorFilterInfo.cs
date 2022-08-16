using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class ColorFilterInfo : IFilterInfo
	{
		public bool CellColor { get; set; }

		public int DxfId { get; set; }
	}
}
