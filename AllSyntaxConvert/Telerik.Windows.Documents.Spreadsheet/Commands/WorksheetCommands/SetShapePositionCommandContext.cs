using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapePositionCommandContext : WorksheetCommandContextBase
	{
		public FloatingShapeBase Shape
		{
			get
			{
				return this.shape;
			}
		}

		public CellIndex NewCellIndex
		{
			get
			{
				return this.newCellIndex;
			}
		}

		public double NewOffsetX
		{
			get
			{
				return this.newOffsetX;
			}
		}

		public double NewOffsetY
		{
			get
			{
				return this.newOffsetY;
			}
		}

		public CellIndex OldCellIndex
		{
			get
			{
				return this.oldCellIndex;
			}
			set
			{
				this.oldCellIndex = value;
			}
		}

		public double OldOffsetX
		{
			get
			{
				return this.oldOffsetX;
			}
			set
			{
				this.oldOffsetX = value;
			}
		}

		public double OldOffsetY
		{
			get
			{
				return this.oldOffsetY;
			}
			set
			{
				this.oldOffsetY = value;
			}
		}

		public SetShapePositionCommandContext(Worksheet worksheet, FloatingShapeBase shape, CellIndex newCellIndex, double newOffsetX, double newOffsetY)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			Guard.ThrowExceptionIfNull<CellIndex>(newCellIndex, "newCellIndex");
			this.shape = shape;
			this.newCellIndex = newCellIndex;
			this.newOffsetX = newOffsetX;
			this.newOffsetY = newOffsetY;
		}

		readonly FloatingShapeBase shape;

		readonly CellIndex newCellIndex;

		readonly double newOffsetX;

		readonly double newOffsetY;

		CellIndex oldCellIndex;

		double oldOffsetX;

		double oldOffsetY;
	}
}
