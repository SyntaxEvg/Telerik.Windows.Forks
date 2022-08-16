using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class TwoCellAnchorInfo
	{
		public CellIndex FromIndex { get; set; }

		public CellIndex ToIndex { get; set; }

		public double FromOffsetX { get; set; }

		public double FromOffsetY { get; set; }

		public double ToOffsetX { get; set; }

		public double ToOffsetY { get; set; }
	}
}
