using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeSizeCommandContext : WorksheetCommandContextBase
	{
		public double NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		public double OldValue
		{
			get
			{
				return this.oldValue;
			}
			set
			{
				this.oldValue = value;
			}
		}

		public bool RespectLockAspectRatio
		{
			get
			{
				return this.respectLockAspectRatio;
			}
		}

		public FloatingShapeBase Shape
		{
			get
			{
				return this.shape;
			}
		}

		public bool AdjustCellIndex
		{
			get
			{
				return this.adjustCellIndex;
			}
		}

		public SetShapeSizeCommandContext(Worksheet worksheet, FloatingShapeBase shape, double newValue, bool respectLockAspectRatio, bool adjustCellIndex)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			this.shape = shape;
			this.newValue = newValue;
			this.respectLockAspectRatio = respectLockAspectRatio;
			this.adjustCellIndex = adjustCellIndex;
		}

		readonly FloatingShapeBase shape;

		readonly double newValue;

		double oldValue;

		readonly bool respectLockAspectRatio;

		readonly bool adjustCellIndex;
	}
}
