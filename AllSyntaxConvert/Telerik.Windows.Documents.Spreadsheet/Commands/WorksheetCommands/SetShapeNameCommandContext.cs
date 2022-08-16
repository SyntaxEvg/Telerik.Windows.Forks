using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetShapeNameCommandContext : WorksheetCommandContextBase
	{
		public FloatingShapeBase Shape
		{
			get
			{
				return this.shape;
			}
		}

		public string NewName
		{
			get
			{
				return this.newName;
			}
		}

		public string OldName
		{
			get
			{
				return this.oldName;
			}
			set
			{
				this.oldName = value;
			}
		}

		public SetShapeNameCommandContext(Worksheet worksheet, FloatingShapeBase shape, string newName)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			Guard.ThrowExceptionIfNull<string>(newName, "newName");
			this.shape = shape;
			this.newName = newName;
		}

		readonly FloatingShapeBase shape;

		readonly string newName;

		string oldName;
	}
}
