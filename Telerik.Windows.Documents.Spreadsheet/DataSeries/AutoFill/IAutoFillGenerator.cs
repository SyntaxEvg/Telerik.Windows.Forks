using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	interface IAutoFillGenerator : INamedObject
	{
		void FillSeries(Worksheet worksheet, CellIndex[] indexes, int initialIndexesCount);
	}
}
