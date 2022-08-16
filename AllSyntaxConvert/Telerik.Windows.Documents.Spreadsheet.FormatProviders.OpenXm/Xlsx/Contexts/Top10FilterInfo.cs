using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class Top10FilterInfo : IFilterInfo
	{
		public double Value { get; set; }

		public bool Percent { get; set; }

		public bool Top { get; set; }
	}
}
