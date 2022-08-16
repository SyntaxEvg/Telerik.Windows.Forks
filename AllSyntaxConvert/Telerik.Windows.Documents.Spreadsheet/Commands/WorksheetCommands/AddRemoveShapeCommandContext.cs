using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class AddRemoveShapeCommandContext : WorksheetCommandContextBase
	{
		public FloatingShapeBase Shape
		{
			get
			{
				return this.shape;
			}
		}

		public AddRemoveShapeCommandContext(Worksheet worksheet, FloatingShapeBase shape)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			this.shape = shape;
		}

		readonly FloatingShapeBase shape;
	}
}
