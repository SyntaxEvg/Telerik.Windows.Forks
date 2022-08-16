using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class RepeatAutoFillGenerator : IAutoFillGenerator, INamedObject
	{
		public string Name
		{
			get
			{
				return RepeatAutoFillGenerator.NAME;
			}
		}

		public void FillSeries(Worksheet worksheet, CellIndex[] indexes, int initialIndexesCount)
		{
			for (int i = 0; i < initialIndexesCount; i++)
			{
				using (WorksheetFragment worksheetFragment = worksheet.Cells[indexes[i]].Copy())
				{
					for (int j = i + initialIndexesCount; j < indexes.Length; j += initialIndexesCount)
					{
						worksheet.Cells[indexes[j]].Paste(worksheetFragment, new PasteOptions(PasteType.Formulas, false));
					}
				}
			}
		}

		static readonly string NAME = "Repeat";
	}
}
