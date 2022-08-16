using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeFlipCommandContext : WorksheetCommandContextBase
	{
		public FloatingShapeBase Shape
		{
			get
			{
				return this.shape;
			}
		}

		public bool HorizontalFlip
		{
			get
			{
				return this.horizontalFlip;
			}
		}

		public bool VerticalFlip
		{
			get
			{
				return this.verticalFlip;
			}
		}

		public bool OldHorizontalFlip
		{
			get
			{
				return this.oldHorizontalFlip;
			}
			set
			{
				this.oldHorizontalFlip = value;
			}
		}

		public bool OldVerticalFlip
		{
			get
			{
				return this.oldVerticalFlip;
			}
			set
			{
				this.oldVerticalFlip = value;
			}
		}

		public SetShapeFlipCommandContext(Worksheet worksheet, FloatingShapeBase shape, bool horizontalFlip, bool verticalFlip)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			this.shape = shape;
			this.horizontalFlip = horizontalFlip;
			this.verticalFlip = verticalFlip;
		}

		readonly FloatingShapeBase shape;

		readonly bool horizontalFlip;

		readonly bool verticalFlip;

		bool oldHorizontalFlip;

		bool oldVerticalFlip;
	}
}
