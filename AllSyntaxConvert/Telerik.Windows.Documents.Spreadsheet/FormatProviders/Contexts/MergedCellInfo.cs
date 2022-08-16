using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts
{
	public class MergedCellInfo
	{
		public CellRange CellRange
		{
			get
			{
				return this.cellRange;
			}
			set
			{
				Guard.ThrowExceptionIfNull<CellRange>(value, "value");
				this.cellRange = value;
			}
		}

		public MergedCellInfo()
		{
		}

		public MergedCellInfo(CellRange cellRange)
		{
			this.CellRange = cellRange;
		}

		CellRange cellRange;
	}
}
