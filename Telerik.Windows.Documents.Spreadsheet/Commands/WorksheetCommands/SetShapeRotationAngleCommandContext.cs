using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeRotationAngleCommandContext : WorksheetCommandContextBase
	{
		public FloatingShapeBase FloatingShape
		{
			get
			{
				return this.shape;
			}
		}

		public double Angle
		{
			get
			{
				return this.angle;
			}
		}

		public double OldAngle
		{
			get
			{
				return this.oldAngle;
			}
			set
			{
				Guard.ThrowExceptionIfOutOfRange<double>((double)SetShapeRotationAngleCommandContext.ExcelMinRotationAngle, (double)SetShapeRotationAngleCommandContext.ExcelMaxRotationAngle, value, "value");
				this.oldAngle = value;
			}
		}

		public bool AdjustCellindex
		{
			get
			{
				return this.adjustCellindex;
			}
		}

		public SetShapeRotationAngleCommandContext(Worksheet worksheet, FloatingShapeBase shape, double angle, bool adjustCellindex)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			Guard.ThrowExceptionIfOutOfRange<double>((double)SetShapeRotationAngleCommandContext.ExcelMinRotationAngle, (double)SetShapeRotationAngleCommandContext.ExcelMaxRotationAngle, angle, "angle");
			this.shape = shape;
			this.angle = angle;
			this.adjustCellindex = adjustCellindex;
		}

		static readonly int ExcelMinRotationAngle = -35791;

		static readonly int ExcelMaxRotationAngle = 35791;

		readonly FloatingShapeBase shape;

		readonly double angle;

		double oldAngle;

		readonly bool adjustCellindex;
	}
}
