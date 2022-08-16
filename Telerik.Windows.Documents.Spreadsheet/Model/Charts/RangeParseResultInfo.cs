using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Charts
{
	class RangeParseResultInfo
	{
		public CellRange DataRange { get; set; }

		public CellRange CategoriesRange { get; set; }

		public CellRange SeriesTitlesRange { get; set; }

		public bool SeriesRangesAreVertical { get; set; }
	}
}
