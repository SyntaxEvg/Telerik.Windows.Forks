using System;
using System.Collections.Generic;

namespace Telerik.Documents.SpreadsheetStreaming
{
	interface IRowImporter
	{
		int RowIndex { get; }

		int OutlineLevel { get; }

		bool IsCustomHeight { get; }

		double HeightInPixels { get; }

		double HeightInPoints { get; }

		bool IsHidden { get; }

		IEnumerable<ICellImporter> Cells { get; }
	}
}
