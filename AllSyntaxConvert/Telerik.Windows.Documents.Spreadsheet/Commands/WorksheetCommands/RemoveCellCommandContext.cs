using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RemoveCellCommandContext : WorksheetCommandContextBase
	{
		public RemoveShiftType ShiftType
		{
			get
			{
				return this.shiftType;
			}
		}

		public CellRange CellRange
		{
			get
			{
				return this.cellRange;
			}
		}

		internal CellsPropertyBag OldCellProperties { get; set; }

		public RemoveCellCommandContext(Worksheet worksheet, CellRange cellRange, RemoveShiftType shiftType)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			this.cellRange = cellRange;
			this.shiftType = shiftType;
		}

		readonly CellRange cellRange;

		readonly RemoveShiftType shiftType;
	}
}
