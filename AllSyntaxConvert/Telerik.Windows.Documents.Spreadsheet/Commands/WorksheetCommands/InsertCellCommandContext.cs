using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class InsertCellCommandContext : WorksheetCommandContextBase
	{
		public CellRange CellRange
		{
			get
			{
				return this.cellRange;
			}
		}

		public InsertShiftType ShiftType
		{
			get
			{
				return this.shiftType;
			}
		}

		public InsertCellCommandContext(Worksheet worksheet, CellRange cellRange, InsertShiftType shiftType)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			this.cellRange = cellRange;
			this.shiftType = shiftType;
		}

		readonly CellRange cellRange;

		readonly InsertShiftType shiftType;
	}
}
