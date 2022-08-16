using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeLockAspectRatioCommandContext : WorksheetCommandContextBase
	{
		public bool LockAspectRatio
		{
			get
			{
				return this.lockAspectRatio;
			}
		}

		public bool OldLockAspectRatio
		{
			get
			{
				return this.oldLockAspectRatio;
			}
			set
			{
				this.oldLockAspectRatio = value;
			}
		}

		public FloatingShapeBase Shape
		{
			get
			{
				return this.shape;
			}
		}

		public SetShapeLockAspectRatioCommandContext(Worksheet worksheet, FloatingShapeBase shape, bool lockAspectRatio)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			this.lockAspectRatio = lockAspectRatio;
			this.shape = shape;
		}

		readonly bool lockAspectRatio;

		bool oldLockAspectRatio;

		readonly FloatingShapeBase shape;
	}
}
