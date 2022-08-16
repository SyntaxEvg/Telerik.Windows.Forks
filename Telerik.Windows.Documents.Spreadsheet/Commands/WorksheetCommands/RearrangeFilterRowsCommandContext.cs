using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class RearrangeFilterRowsCommandContext : WorksheetCommandContextBase
	{
		public int Delta
		{
			get
			{
				return this.delta;
			}
		}

		public int AbsoluteIndex
		{
			get
			{
				return this.absoluteIndex;
			}
		}

		public ShiftType ShiftType
		{
			get
			{
				return this.shiftType;
			}
		}

		public override bool ShouldBringActiveCellIntoView
		{
			get
			{
				return false;
			}
		}

		public RearrangeFilterRowsCommandContext(Worksheet worksheet, int delta, int absoluteIndex, ShiftType shiftType)
			: base(worksheet)
		{
			if (shiftType != ShiftType.Down && shiftType != ShiftType.Up)
			{
				throw new ArgumentException("The shift type must be ShiftType.Down or ShiftType.Up", "shiftType");
			}
			this.delta = delta;
			this.shiftType = shiftType;
			this.absoluteIndex = absoluteIndex;
		}

		readonly int delta;

		readonly int absoluteIndex;

		readonly ShiftType shiftType;
	}
}
